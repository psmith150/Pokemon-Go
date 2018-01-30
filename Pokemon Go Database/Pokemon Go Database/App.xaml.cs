using System.Windows;
using GalaSoft.MvvmLight.Threading;
using Pokemon_Go_Database.IOC;
using Pokemon_Go_Database.Windows;
using System;
using System.IO;
using System.Diagnostics;

namespace Pokemon_Go_Database
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string COMPANY_NAME = "Patrick Smith";
        private const string APPLICATION_NAME = "Pokemon Go Database";
        private readonly string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), COMPANY_NAME, APPLICATION_NAME);

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //EventLog.Initialize(this.logPath);

            IoCContainer.Build();

            // Set the starting window
            this.MainWindow = IoCContainer.ResolveWindow(typeof(MainWindow));
            this.MainWindow.Show();
        }

        // Log any unhandled exceptions to the log directory in a CrashDump file
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string message;

            if (e.ExceptionObject is Exception)
            {
                try
                {
                    string directory;

                    try
                    {
                        directory = this.logPath ?? ".";
                    }
                    catch
                    {
                        directory = ".";
                    }

                    File.WriteAllText
                    (
                        Path.Combine(directory, "CrashDump - " + DateTime.Now.ToString("yyyyMMdd - HHmmss") + ".txt"),
                        ((Exception)e.ExceptionObject).ToString()
                    );
                }
                finally
                {
                    message = ((Exception)e.ExceptionObject).ToString();
                }
            }
            else
            {
                message = "An unknown error occurred.";
            }

            Debug.Assert(false, message);
            Environment.Exit(-1);
        }
    }
}
