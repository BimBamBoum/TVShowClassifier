using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace TVShowClassifier
{
    class Program
    {
        static void Main(string[] args)
        {
            LogMessageToFile("Program started");

            // Config file
            Configuration config = new Configuration();
            config.LoadConfiguration("Configuration.xml");

            // Classification
            Classifier classifier = new Classifier();
            classifier.ClassifyTvShows(config);
        }

        public static void LogMessageToFile(string msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText("Log.txt");
            try
            {
                string logLine = System.String.Format("{0:G}: {1}.", System.DateTime.Now, msg);
                sw.WriteLine(logLine);

                Debug.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }

        public static void ShowNotificationOnTrayIcon(string msg)
        {
            TrayIcon.Instance.ShowTooltip(msg);
        }
    }
}
