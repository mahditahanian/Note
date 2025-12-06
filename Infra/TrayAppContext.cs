using System;
using System.Drawing;
using System.Windows.Forms;

namespace NoteApp.Infra
{
    public class TrayAppContext : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;

        public TrayAppContext()
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("پنجره جدید", null, OnNewWindowClick);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("خروج", null, OnExitClick);

            _trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "NoteApp",
                ContextMenuStrip = menu
            };

            _trayIcon.DoubleClick += (s, e) => OpenNewEditor();
        }

        private void OnNewWindowClick(object? sender, EventArgs e)
        {
            OpenNewEditor();
        }

        private void OpenNewEditor()
        {
            var form = new EditorForm();
            form.Show();
        }

        private void OnExitClick(object? sender, EventArgs e)
        {
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            Application.Exit();
        }
    }
}
