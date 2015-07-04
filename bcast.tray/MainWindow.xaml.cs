using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace bcast.tray
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebServer ws; 

        public MainWindow()
        {
            InitializeComponent();

            //  requires    netsh http add urlacl http://+:1508 user=Everyone listen=yes
            //ws = new WebServer(HandleRequest, "http://+:1508/"); 
            //ws.Run();
            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("bcast.ico");
            ni.Visible = true;
            ni.ContextMenu = new System.Windows.Forms.ContextMenu();
            ni.ContextMenu.MenuItems.Add("Exit", cmExit);
            ni.DoubleClick +=
                delegate(object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };

        }

        private void cmExit(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        //private string HandleRequest(System.Net.HttpListenerRequest request)
        //{
        //    Dispatcher.Invoke(new Action<ListBox, string>(AddToHistory), lbRequests, request.RawUrl);
        //    return "";
        //}

        //private static void AddToHistory(ListBox lb, string url)
        //{
        //    lb.Items.Add(url);
        //}
    }
}
