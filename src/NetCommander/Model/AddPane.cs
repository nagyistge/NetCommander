using System;

namespace NetCommander.Model
{
    public class AddPane : Pane
    {
        private string _name;

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

    }
}
