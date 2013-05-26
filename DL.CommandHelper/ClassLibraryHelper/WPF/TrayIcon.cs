using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Resources;
using Application = System.Windows.Application;
using ContextMenu = System.Windows.Controls.ContextMenu;
using MouseEventHandler = System.Windows.Input.MouseEventHandler;
using ToolTip = System.Windows.Controls.ToolTip;


namespace ClassLibraryHelper.WPF
{
    public interface ITrayIcon : IDisposable
    {
        /// <summary>
        /// Gets or sets the wpf content to display when the mouse pointer rests on notification area icon.
        /// </summary>
        object TrayToolTip { get; set; }

        /// <summary>
        /// Gets or sets the ToolTip text displayed when the mouse pointer rests on notification area icon.
        /// </summary>
        string ToolTipText { get; set; }

        /// <summary>
        /// Gets or sets the title of the balloon tip displayed on the System.Windows.Forms.NotifyIcon.
        /// </summary>
        string BalloonTipTitle { get; set; }

        void ShowBalloonTip(string BalloonTipText);

        bool IconVisible { get; set; }

        ImageSource Icon { get; set; }

        ContextMenu ContextMenu { get; set; }
    }

    public class TrayIcon : FrameworkElement, ITrayIcon, IDisposable
    {

        private NotifyIcon notifyIcon;

        private TrayIcon()
        {

            notifyIcon = new NotifyIcon();

            notifyIcon.BalloonTipShown += new EventHandler(notifyIcon_BalloonTipShown);
            notifyIcon.BalloonTipClosed += new EventHandler(notifyIcon_BalloonTipClosed);
            notifyIcon.MouseClick += new global::System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);
            Application.Current.Exit += new ExitEventHandler(Current_Exit);
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            if (notifyIcon.Icon != null)
                this.Dispose();
        }

        //private static TrayIcon trayIcon;
        //public static TrayIcon Instance
        //{
        //    get
        //    {
        //        if (trayIcon == null) { trayIcon = new TrayIcon(); }
        //        return trayIcon;
        //    }
        //}

        public static TrayIcon CreateTrayIcon()
        {
            return new TrayIcon();
        }

        void notifyIcon_BalloonTipClosed(object sender, EventArgs e)
        {
            toolTip.IsOpen = false;
        }

        void notifyIcon_BalloonTipShown(object sender, EventArgs e)
        {
            if (TrayToolTip != null)
            {
                notifyIcon.Text = string.Empty;
                toolTip.StaysOpen = false;
                toolTip.IsOpen = true;
            }
        }


        private ToolTip toolTip = new ToolTip();

        void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (this.ContextMenu != null)
                {
                    this.ContextMenu.Placement = PlacementMode.Mouse;
                    this.ContextMenu.IsOpen = true;
                    var nativeWindow = this.notifyIcon.GetType().
                        GetField("window", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this.notifyIcon)
                        as NativeWindow;


                    SetForegroundWindow(nativeWindow.Handle);
                }
            }
            if (e.Button == MouseButtons.Left)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public string BalloonTipTitle
        {
            get { return (string)GetValue(BalloonTipTitleProperty); }
            set { SetValue(BalloonTipTitleProperty, value); }
        }

        public static readonly DependencyProperty BalloonTipTitleProperty =
            DependencyProperty.Register("BalloonTipTitle", typeof(string), typeof(TrayIcon),
            new UIPropertyMetadata(string.Empty, new PropertyChangedCallback((o, e) =>
            {
                var trayIcon = o as TrayIcon;
                trayIcon.notifyIcon.BalloonTipTitle = trayIcon.BalloonTipTitle;
            })));

        #region For WPF Property

        public object TrayToolTip
        {
            get { return (object)GetValue(TrayToolTipProperty); }
            set { SetValue(TrayToolTipProperty, value); }
        }

        public static readonly DependencyProperty TrayToolTipProperty =
            DependencyProperty.Register("TrayToolTip", typeof(object), typeof(TrayIcon),
            new UIPropertyMetadata(new PropertyChangedCallback((o, e) =>
            {
                var trayIcon = o as TrayIcon;
                trayIcon.toolTip.Content = trayIcon.TrayToolTip;

            })));


        #endregion

        public string ToolTipText
        {
            get { return (string)GetValue(ToolTipTextProperty); }
            set { SetValue(ToolTipTextProperty, value); }
        }

        public static readonly DependencyProperty ToolTipTextProperty =
            DependencyProperty.Register("ToolTipText", typeof(string), typeof(TrayIcon),
            new UIPropertyMetadata(new PropertyChangedCallback((o, e) =>
            {
                var trayIcon = o as TrayIcon;
                trayIcon.notifyIcon.Text = trayIcon.ToolTipText;

            })));

        public bool IconVisible
        {
            get { return (bool)GetValue(IconVisibleProperty); }
            set { SetValue(IconVisibleProperty, value); }
        }

        public static readonly DependencyProperty IconVisibleProperty =
            DependencyProperty.Register("IconVisible", typeof(bool), typeof(TrayIcon),
            new UIPropertyMetadata(new PropertyChangedCallback((o, e) =>
            {
                var trayIcon = o as TrayIcon;
                trayIcon.notifyIcon.Visible = trayIcon.IconVisible;
            })));



        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(TrayIcon),
            new UIPropertyMetadata(new PropertyChangedCallback((o, e) =>
            {
                var trayIcon = o as TrayIcon;
                trayIcon.notifyIcon.Icon = trayIcon.Icon.ToIcon();
                trayIcon.notifyIcon.Visible = true;
            })));


        public void Dispose()
        {
            this.notifyIcon.Dispose();
        }

        public void ShowBalloonTip(string BalloonTipText)
        {
            this.notifyIcon.BalloonTipText = BalloonTipText;
            this.notifyIcon.ShowBalloonTip(10);
        }

    }

    public static class TrayIconUtil
    {
        public static Icon ToIcon(this ImageSource imageSource)
        {
            if (imageSource == null) return null;

            Uri uri = new Uri(imageSource.ToString(), UriKind.RelativeOrAbsolute);
            StreamResourceInfo streamInfo = Application.GetResourceStream(uri);

            if (streamInfo == null)
            {
                string msg = "The supplied image source '{0}' could not be resolved.";
                msg = String.Format(msg, imageSource);
                throw new ArgumentException(msg);
            }


            return new Icon(streamInfo.Stream);
        }
    }
}
