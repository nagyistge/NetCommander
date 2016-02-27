using System;
using System.IO;
using System.Windows.Media;
using NetCommander.Util;

namespace NetCommander.Model
{
    public class FileItem : ExplorerItem
    {
        private readonly string _fileName;
        private readonly FileInfo _fileInfo;
        private readonly ImageSource _image;

        public override string Path
        {
            get { return _fileName; }
        }
        
        public override string Name
        {
            get { return _fileInfo.Name; }
        }
        
        public override long? Size
        {
            get { return _fileInfo.Length; }
        }
        
        public override DateTime Modified
        {
            get { return _fileInfo.LastWriteTime; }
        }
        
        public override FileAttributes Attributes
        {
            get { return _fileInfo.Attributes; }
        }
        
        public override ImageSource Image
        {
            get { return _image; }
        }
        
        public FileItem(string fileName, bool smallIcon = true)
        {
            _fileName = fileName;
            _fileInfo = new FileInfo(_fileName);
            _image = smallIcon ? Shell.GetSmallIcon(_fileName): Shell.GetLargeIcon(_fileName);
        }
    }
}
