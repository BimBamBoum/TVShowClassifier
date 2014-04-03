using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;

namespace TVShowClassifier
{
    class TrayIcon : Form
    {
        private static TrayIcon instance;

        private NotifyIcon trayIcon;
        private const int timeOut = 3000;

        private TrayIcon()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "TVShow Classifier";
            trayIcon.Icon = Properties.Resources.Folders_Videos;
            trayIcon.Visible = true;
        }

        public static TrayIcon Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TrayIcon();
                }
                return instance;
            }
        }

        public void ShowTooltip(string msg)
        {
            trayIcon.ShowBalloonTip(timeOut, "TVShow Classifier", msg, System.Windows.Forms.ToolTipIcon.Info);
        }
    }
}
