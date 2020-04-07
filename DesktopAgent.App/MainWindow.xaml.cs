using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;

namespace DesktopAgent.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notifyIcon = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.MouseClick += NotifyIcon_MouseClick;
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.Icon = DesktopAgent.App.Resources.Resources.small_tree;
            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            notifyIcon.Visible = true;

            base.OnInitialized(e);
        }

        private void NotifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon, null);
            }
            else
            {
                if (WindowState == WindowState.Normal || WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Minimized;
                }
                else
                {
                    WindowState = WindowState.Normal;
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            WindowState = WindowState.Minimized;
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;

            if (notifyIcon.ContextMenuStrip.Items.Count == 0)
            {
                notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, new EventHandler((object send, EventArgs args) =>
                {
                    System.Windows.Application.Current.Shutdown(99);
                })));
            }
        }
    }
}
