using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using NetCommander.Model;

namespace NetCommander.Controls
{
    public partial class PanelControl : UserControl
    {
        public PanelControl()
        {
            InitializeComponent();
            
            tabControl.SelectionChanged += tabControl_SelectionChanged;
            tabControl.KeyDown += tabControl_KeyDown;
        }

        private void AddPane()
        {
            var panes = this.DataContext as IList<Pane>;
            if (panes != null)
            {
                try
                {
                    var pane = new ExplorerPane(Directory.GetCurrentDirectory());
                    panes.Insert(panes.Count - 1, pane);
                    tabControl.SelectedItem = pane;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }
            }
        }

        private void RemovePane(Pane pane)
        {
            var panes = this.DataContext as IList<Pane>;
            if (panes != null && pane != null)
            {
                panes.Remove(pane);
                tabControl.SelectedItem = panes.FirstOrDefault();
            }
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem != null && tabControl.SelectedItem is AddPane)
            {
                AddPane();
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            RemovePane((sender as CrossButton).DataContext as Pane);
        }

        private void tabControl_KeyDown(object sender, KeyEventArgs e)
        {
            var control = Keyboard.Modifiers == ModifierKeys.Control;
            var panes = this.DataContext as IList<Pane>;
            if (panes != null && control == true)
            {
                switch (e.Key) 
                {
                    // Add pane.
                    case Key.T:
                        {
                            AddPane();
                        }
                        break;
                    // Remove pane.
                    case Key.W:
                        {
                            if (tabControl.SelectedItem != null && !(tabControl.SelectedItem is AddPane))
                            {
                                RemovePane(tabControl.SelectedItem as Pane);
                            }
                        }
                        break;
                }
            }
        }
    }

    public class CrossButton : Button
    {
        static CrossButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CrossButton), new FrameworkPropertyMetadata(typeof(CrossButton)));
        }
    }

    public class PaneDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;
            if (element != null && item != null)
            {
                var key = item.GetType().Name + "DataTemplateKey";
                var template = element.FindResource(key) as DataTemplate;
                if (template != null)
                {
                    return template;
                }
            }
            return base.SelectTemplate(item, container); 
        }
    }
}
