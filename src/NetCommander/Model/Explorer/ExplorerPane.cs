using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace NetCommander.Model
{
    public class ExplorerPane : Pane
    {
        public const string ParentName = "..";

        private string _name;
        private string _path;
        private IList<ExplorerItem> _items;
        private string _status;
        private ExplorerItem _currentItem;

        public override string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    Notify("Name");
                }
            }
        }
 
        public string Path
        {
            get { return _path; }
            set
            {
                if (value != _path)
                {
                    try
                    {
                        if (Directory.Exists(value))
                        {
                            SetPath(value);
                        }
                        else
                        {
                            _path = value;
                            Notify("Path");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Print(ex.Message);
                    }
                }
            }
        }

        public IList<ExplorerItem> Items
        {
            get { return _items; }
        }
        
        public string Status
        {
            get { return _status; }
        }

        public ExplorerItem CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (value != _currentItem)
                {
                    _currentItem = value;
                    Notify("CurrentItem");
                }
            }
        }

        public ExplorerPane(string path)
        {
            SetPath(path);
        }

        private void AddParentDirectory(DirectoryItem currentItem)
        {
            if (!string.IsNullOrWhiteSpace(currentItem.Parent))
            {
                _items.Add(new DirectoryItem(currentItem.Path, ParentName));
            }
        }
        
        private void AddDirectories(string path)
        {
            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                var item = new DirectoryItem(dir);
                _items.Add(item);
            }
        }

        private void AddFiles(string path)
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var item = new FileItem(file);
                _items.Add(item);
            }
        }

        public void SetPath(string path)
        {
            var dir = new DirectoryItem(path);

            _path = path;
            _name = dir.Name;
            _items = new ObservableCollection<ExplorerItem>();

            AddParentDirectory(dir);
            AddDirectories(path);
            AddFiles(path);

            _currentItem = _items.FirstOrDefault();
            _status = "Objects: " + _items.Count;

            Notify("Path");
            Notify("Name");
            Notify("Items");
            Notify("CurrentItem");
            Notify("Status");
        }
    }
}
