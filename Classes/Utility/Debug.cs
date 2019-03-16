using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace if2ktool
{
    public enum LogType : byte
    {
        Info, Warning, Error, Success
    }

    public struct LogItem
    {
        public LogType logType;
        public bool inline;
        public bool showMsgBox;
        public string message;
    }

    public static class Debug
    {
        static ConsoleColor consoleColor;

        // Logging worker
        static BackgroundWorker worker;
        static ConcurrentQueue<LogItem> logQueue;
        static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        // File logging
        static FileStream logStream;
        static StreamWriter logWriter;

        static bool logToFile;
        static bool threadedLogging;

        const string DEBUG_PREFIX_INFO      = "[INFO]   : ";
        const string DEBUG_PREFIX_WARNING   = "[WARNING]: ";
        const string DEBUG_PREFIX_ERROR     = "[ERROR]  : ";

        static Debug()
        {
            threadedLogging = Settings.Current.threadedLogging;

            // Note that we use a ConcurrentQueue as it is thread safe, unlike a standard Queue
            logQueue = new ConcurrentQueue<LogItem>();

            // Create BackgroundWorker
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();

            // Open console
            ConsoleHelper.ToggleConsole(Settings.Current.showConsole);
            consoleColor = Console.ForegroundColor;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        public static void ToggleThreadedLogging(bool enable)
        {
            if (enable == true && worker.IsBusy == false)
            {
                manualResetEvent.Set();
                threadedLogging = true;
            }
            else if (enable == false)
            {
                // Don't cancel the worker, just reset the ManualResetEvent
                manualResetEvent.Reset();

                // Clear the queue...
                logQueue = new ConcurrentQueue<LogItem>();

                // And set threadedLogging to false to prevent LogInternal from setting it
                threadedLogging = false;
            }
        }

        public static void ToggleLogToFile(bool enabled)
        {
            if (enabled == true && logToFile == false)
            {
                logToFile = true;

                try
                {
                    logStream = new FileStream("log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                    logStream.SetLength(0);
                    logWriter = new StreamWriter(logStream);

                    Debug.Log("Opened log file for writing");
                }
                catch (Exception e)
                {
                    Debug.Log("Debug : Cannot open a log file for writing");
                    Debug.Log(e.Message);
                    return;
                }
            }
            else if (enabled == false && logToFile == true)
            {
                logWriter.Close();
                logStream.Close();
                logWriter.Dispose();
                logStream.Dispose();

                logToFile = false;
            }
        }

        private static void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Using a ManualResetEvent is preferable to a while(true) loop, as it will halt the thread instead of infinitely loop (which causes 100% CPU usage)

            // WaitOne never returns until Set() is called on the ManualResetEvent, after which it will return true. Set is called in LogInternal, where we're queueing data for the worker. This effectively signals to the worker that there is data to process
            while (manualResetEvent.WaitOne())
            {
                // Here we empty the queued items
                while (logQueue.Count > 0)
                {
                    if (logQueue.TryDequeue(out LogItem item))
                        LogPerform(item);
                }

                // Once we've processed the queue, ManualResetEvent.Reset() is called, which will cause WaitOne to never return again.
                manualResetEvent.Reset();
            }
        }

        // --- Logging Methods ---
        
        public static void Log(string message)
        {
            LogInternal(message, LogType.Info, false, false);
        }

        public static void LogLine()
        {
            LogInternal(string.Empty, LogType.Info, false, false);
        }

        public static void LogInline(string message)
        {
            LogInternal(message, LogType.Info, false, true);
        }

        public static void LogSuccess(string message)
        {
            LogInternal(message, LogType.Success, false, false);
        }

        public static void LogError(string message, bool showMsgBox = false)
        {
            LogInternal(message, LogType.Error, showMsgBox, false);
        }

        public static void LogWarning(string message, bool showMsgBox = false)
        {
            LogInternal(message, LogType.Warning, showMsgBox, false);
        }

        // Only logs if Settings.Current.fullLogging is true
        public static void LogFull(string message)
        {
            if (Settings.Current.fullLogging)
            {
                Debug.Log(message);
            }
        }

        public static void LogCollection(List<LogItem> logItems)
        {
            lock (logQueue)
            {
                foreach (LogItem item in logItems)
                    LogInternal(item);
            }
        }

        public static void LogWithType(string message, LogType logType)
        {
            switch (logType)
            {
                case LogType.Info:      Log(message); break;
                case LogType.Warning:   LogWarning(message); break;
                case LogType.Error:     LogError(message); break;
                case LogType.Success:   LogSuccess(message); break;
            }
        }

        // Logs a stacktrace for an exception. This occurs on the logging thread (if enabled), so the logging itself may be delayed
        // This only logs the stacktrace, and NOT the exception or any of its members
        public static void LogStackTrace(Exception e)
        {
            // Create a StackTrace that captures filename, line number and column information.
            var st = new StackTrace(e, true);

            for (int i = 0; i < st.FrameCount; i++)
            {
                // Note that at this level, there are four stack frames, one for each method invocation.
                StackFrame sf = st.GetFrame(i);
                
                string fileName = sf.GetFileName();

                if (string.IsNullOrEmpty(fileName))
                    fileName = "<filename unknown>";

                string log = string.Format("{0} (at {1}:{2})", sf.GetMethod(), fileName, sf.GetFileLineNumber());
                Debug.Log(log);
            }
        }

        // ---

        // This either queues a message to be logged with the BackgroundWorker, or otherwise just does a direct log
        // Is also handles MessageBoxes, which should be shown as soon as it is logged, not when the logger gets to them
        static void LogInternal(string message, LogType logType, bool showMsgBox, bool inline)
        {
            if (showMsgBox) DoMessageBox(message, logType);

            // Put the log in the queue if logging is threaded
            if (Settings.Current.threadedLogging)
            {
                var logItem = new LogItem() { logType = logType, message = message, inline = inline };

                // Lock the queue before enqueue
                //lock (logQueue)
                {
                    manualResetEvent.Set();
                    logQueue.Enqueue(logItem);
                }
            }
            // Otherwise just perform the log
            else
            {
                LogPerform(message, logType, inline);
            }
        }

        static void LogInternal(LogItem logItem)
        {
            // Queue the logItem if we're logging in a thread
            if (threadedLogging)
            {
                manualResetEvent.Set();
                logQueue.Enqueue(logItem);
            }

            // Otherwise just perform the log
            else
            {
                LogPerform(logItem.message, logItem.logType, logItem.inline);
            }

            // Show message box second
            if (logItem.showMsgBox)
                DoMessageBox(logItem.message, logItem.logType);
        }

        static void DoMessageBox(string message, LogType logType)
        {
            MessageBoxIcon messageBoxIcon;

            if (logType == LogType.Info || logType == LogType.Success)
                messageBoxIcon = MessageBoxIcon.Information;
            else if (logType == LogType.Warning)
                messageBoxIcon = MessageBoxIcon.Warning;
            else if (logType == LogType.Error)
                messageBoxIcon = MessageBoxIcon.Error;
            else
                messageBoxIcon = MessageBoxIcon.None;

            MessageBox.Show(message, logType.ToString(), MessageBoxButtons.OK, messageBoxIcon);
        }

        static void LogPerform(LogItem logItem)
        {
            LogPerform(logItem.message, logItem.logType, logItem.inline);
        }

        // This is called from within the BackgroundWorker, and actually performs the logging
        static void LogPerform(string message, LogType logType, bool inline)
        {
            // Change the console color if needed
            if (logType != LogType.Info)
            {
                if (logType == LogType.Warning)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else if (logType == LogType.Error)
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (logType == LogType.Success)
                    Console.ForegroundColor = ConsoleColor.Green;
            }

            // Log to console
            if (inline)
                Console.Write(message);
            else
                Console.WriteLine(message);

            // Log to file
            if (logToFile)
            {
                // Write an empty line if the message is empty
                if (string.IsNullOrEmpty(message) && !inline)
                {
                    logWriter.WriteLine();
                }

                // Otherwise, if there is a message...
                else
                {
                    if (inline)
                        logWriter.Write(message);
                    else
                        logWriter.WriteLine(LogTypeToPrefix(logType) + message);
                }
            }

            // Reset the console color (don't bother checking)
            Console.ForegroundColor = consoleColor;
        }

        static string LogTypeToPrefix(LogType logType)
        {
            switch (logType)
            {
                case LogType.Info:      return DEBUG_PREFIX_INFO;
                case LogType.Error:     return DEBUG_PREFIX_ERROR;
                case LogType.Warning:   return DEBUG_PREFIX_WARNING;
                default:                return DEBUG_PREFIX_INFO;
            }
        }
    }
}
