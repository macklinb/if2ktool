using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace if2ktool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Load settings
            Settings.Init();

            // Set up exception handling
            SetupExceptionHandling();

            // Open log file (use current setting to determine whether to open it)
            Debug.ToggleLogToFile(Settings.Current.logToFile);
            Debug.Log("if2ktool v" + Application.ProductVersion +", © 2019, Macklin Brosens");
            
            // Start main form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());

            // -> Program running <-

            // Save settings
            Properties.Settings.Default.savedSettings = Settings.Current;
            Properties.Settings.Default.Save();

            // Close log file
            Debug.ToggleLogToFile(false);
        }

        static void SetupExceptionHandling()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }
        
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Debug.LogError("An unhandled exception occurred:\n" + e.Exception.ToString());
            Debug.LogStackTrace(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
            {
                if (Settings.Current.logToFile == false)
                {
                    // Always open a new log file
                    Debug.ToggleLogToFile(true);

                    // And additionally, disable threaded logging so that logs are processed immediately)
                    Settings.Current.threadedLogging = false;
                }

                try
                {
                    Debug.LogError("--- CRASH ---");
                    Debug.LogError("An error was encountered that has caused the program to terminate.\n\n" + e.ExceptionObject.ToString(), true);

                    // Show the close button on the console window
                    ConsoleHelper.ToggleCloseButton(true);
                    
                    // Close the log file
                    Debug.ToggleLogToFile(false);

                    // Ask the user to press enter to close
                    Console.WriteLine("\nPress enter to close...");
                    Console.ReadLine();
                }
                finally
                {
                    Environment.Exit(-1);
                }
               
            }
            else
                Debug.LogError(e.ExceptionObject.ToString());
        }
    }
}
