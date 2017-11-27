using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace explorer_explorer.mainwindow
{
    public class MainWindow
    {
        public static MainWindow Instance
        {
            get;
            private set;
        }

        public static Process ExplorerWindow
        {
            get;
            private set;
        }
        
        public static string LevelRoot
        {
            get;
            private set;
        }

        Item item1;
        Item item2;

        public MainWindow()
        {
            if (Instance != null)
                throw new InvalidOperationException("Don't double initialize singleton");
            Instance = this;

            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/level");

            ExplorerWindow = Process.Start($"{Directory.GetCurrentDirectory()}/level");

            LevelRoot = Directory.GetCurrentDirectory() + @"\level";

            item1 = new Item("0_ Close.txt");
            item2 = new Item("1_ BlueScreen.txt");

            item1.Triggered += Item1_Triggered;
            item2.Triggered += Item2_Triggered;
        }

        private void Item2_Triggered(object sender, EventArgs e)
        {
            NativeMethods.BlueScreenMeDaddy();
        }

        private void Item1_Triggered(object sender, EventArgs e)
        {
            ExplorerWindow_Exited(sender, e);
        }

        private void ExplorerWindow_Exited(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => { explorer_explorer.MainWindow.Instance.Close(); });
        }
    }
}
