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
    // This class is used to queue up a series of ordered Debug.LogItem, and have them all processed at once by calling DebugStack.Push
    // It contains most of the same static methods in the Debug class, but in a private, encapsulated object
    // LogItems that should include a messagebox are fired on Push() and not when the item is logged!
    public class DebugStack
    {
        List<LogItem> logItems;

        public DebugStack()
        {
            logItems = new List<LogItem>();
        }

        // Call this to push the DebugStack to the Debug logger
        public void Push()
        {
            if (logItems != null && logItems.Count > 0)
                Debug.LogCollection(logItems);
        }

        // --- Logging Methods ---
        
        public void Log(string message)
        {
            LogInternal(message, LogType.Info, false);
        }

        public void LogLine()
        {
            LogInternal(string.Empty, LogType.Info, false);
        }

        public void LogInline(string message)
        {
            LogInternal(message, LogType.Info, true);
        }

        public void LogSuccess(string message)
        {
            LogInternal(message, LogType.Success, false);
        }

        public void LogError(string message, bool showMsg = false)
        {
            LogInternal(message, LogType.Error);
        }

        public void LogWarning(string message, bool showMsgBox = false)
        {
            LogInternal(message, LogType.Warning);
        }

        // Only logs if Settings.Current.fullLogging is true
        public void LogFull(string message)
        {
            if (Settings.Current.fullLogging)
                Log(message);
        }

        // This either queues a message to be logged with the BackgroundWorker, or otherwise just does a direct log
        void LogInternal(string message, LogType logType, bool inline = false, bool showMsgBox = false)
        {
            logItems.Add(new LogItem() { message = message, logType = logType, inline = inline } );
        }
    }
}
