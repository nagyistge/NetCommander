using System;
using System.IO;
using System.Windows.Media;
using NetCommander.Util;

namespace NetCommander.Model
{
    public class DirectoryItem : ExplorerItem
    {
        private readonly string _path;
        private readonly string _name;
        private readonly DirectoryInfo _directoryInfo;
        private readonly ImageSource _image;
        
        public override string Path
        {
            get { return _path; }
        }
        
        public override string Name
        {
            get { return _name ?? _directoryInfo.Name; }
        }
        
        public override long? Size
        {
            get { return null; }
        }
        
        public override DateTime Modified
        {
            get { return _directoryInfo.LastWriteTime; }
        }
        
        public override FileAttributes Attributes
        {
            get { return _directoryInfo.Attributes; }
        }
        
        public override ImageSource Image
        {
            get { return _image; }
        }
        
        public string Root
        {
            get { return _directoryInfo.Root == null ? null : _directoryInfo.Root.FullName; }
        }
        
        public string Parent
        {
            get { return _directoryInfo.Parent == null ? null : _directoryInfo.Parent.FullName; }
        }

        public DirectoryItem(string path, bool smallIcon = true) 
            : this(path, null, smallIcon)
        {
        }

        public DirectoryItem(string path, string name, bool smallIcon = true)
        {
            _path = path;
            _name = name;
            _directoryInfo = new DirectoryInfo(path);
            _image = smallIcon ? Shell.GetSmallIcon(path): Shell.GetLargeIcon(path);
        }
    }
}
