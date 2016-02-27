using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using NetCommander.Model;

namespace NetCommander
{
    public partial class MainWindow : Window
    {
        public IList<Pane> LeftPanes { get; set; }
        public IList<Pane> RightPanes { get; set; }
        
        public string Command { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            
            LeftPanes = new ObservableCollection<Pane>();
            RightPanes = new ObservableCollection<Pane>();

            try
            {
                LeftPanes.Add(new ExplorerPane(Directory.GetCurrentDirectory()));
                LeftPanes.Add(new AddPane { Name = "+" });

                RightPanes.Add(new ExplorerPane(Directory.GetCurrentDirectory()));
                RightPanes.Add(new AddPane { Name = "+" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }

            DataContext = this;
            
            Loaded += (sender, e) => leftPane.Focus();
        }
    }
}
