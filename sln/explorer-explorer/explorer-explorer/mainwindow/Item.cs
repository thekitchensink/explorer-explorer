using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace explorer_explorer.mainwindow
{
    public class Item
    {
        static FileSystemWatcher Watcher;

        static readonly Dictionary<string, Item> m_items = new Dictionary<string, Item>();

        private string fullpath;
        private FileStream fs;

        public Item(string s)
        {
            fullpath = MainWindow.LevelRoot + @"\" + s;
            m_items[fullpath] = this;
            fs = new FileStream(fullpath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            fs.Close();
        }

        public void Delete()
        {
            m_items.Remove(fullpath);
        }

        ~Item()
        {
            m_items.Remove(fullpath);
            File.Delete(fullpath);
        }

        static Item()
        {
            Watcher = new FileSystemWatcher(MainWindow.LevelRoot);

            Watcher.IncludeSubdirectories = true;
            Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName;
            Watcher.Filter = "";
            Watcher.Changed += Watcher_Changed;
            Watcher.Deleted += Watcher_Deleted;
            Watcher.Error += Watcher_Error;
            Watcher.EnableRaisingEvents = true;

        }

        private static void Watcher_Error(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (false == m_items.ContainsKey(e.FullPath))
                return;

            m_items[e.FullPath].Triggered?.Invoke(m_items[e.FullPath], null);
        }

        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (false == m_items.ContainsKey(e.FullPath))
                return;

            m_items[e.FullPath].Triggered?.Invoke(m_items[e.FullPath], null);
        }

        public event EventHandler Triggered;
    }
}
