using System;
using System.IO;
using System.Windows.Media;

namespace NetCommander.Model
{
    public abstract class ExplorerItem : NotifyObject
    {
        public abstract string Path { get; }
        
        public abstract string Name { get; }
        
        public abstract long? Size { get; }
        
        public abstract DateTime Modified { get; }
        
        public abstract FileAttributes Attributes { get; }
        
        public abstract ImageSource Image { get; }
    }
}
