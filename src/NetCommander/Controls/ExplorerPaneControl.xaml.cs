using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using NetCommander.Model;

namespace NetCommander.Controls
{
    public partial class ExplorerPaneControl : UserControl
    {
        public ExplorerPaneControl()
        {
            InitializeComponent();

            listView.MouseDoubleClick += listView_MouseDoubleClick;
            listView.SelectionChanged += listView_SelectionChanged;
            listView.KeyDown += listView_KeyDown;
            listView.PreviewMouseMove += listView_PreviewMouseMove;
            
            Loaded += (sender, e) => this.Focus();
        }

        private void HandleDoubleClick(ExplorerPane pane, object item)
        {
            if (item is FileItem)
            {
                var fileItem = item as FileItem;
                System.Diagnostics.Process.Start(fileItem.Path);
            }
            else if (item is DirectoryItem)
            {
                var directoryItem = item as DirectoryItem;
                if (directoryItem.Name == ExplorerPane.ParentName && directoryItem.Parent != null)
                {
                    pane.SetPath(directoryItem.Parent);
                }
                else
                {
                    pane.SetPath(directoryItem.Path);
                }
            }
        }
        
        private object GetListViewItemData(object source)
        {
            var obj = (DependencyObject)source;
            while (obj != null && obj != listView)
            {
                if (obj.GetType() == typeof(ListViewItem))
                {
                    var listViewItem = obj as ListViewItem;
                    return listViewItem.DataContext;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }
        
        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var pane = this.DataContext as ExplorerPane;
            if (pane != null)
            {
                var item = GetListViewItemData(e.OriginalSource);
                if (item != null)
                {
                    try
                    {
                        HandleDoubleClick(pane, item);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Print(ex.Message);
                    }
                }
            }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO: Update pane status.
        }

        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            var pane = this.DataContext as ExplorerPane;
            if (pane != null)
            {
                switch (e.Key) 
                {
                    // Browse or start.
                    case Key.Return:
                        {
                            var item = GetListViewItemData(e.OriginalSource);
                            if (item != null)
                            {
                                try
                                {
                                    HandleDoubleClick(pane, item);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.Print(ex.Message);
                                }
                            }
                        }
                        break;
                    // Navigate back.
                    case Key.Back:
                        {
                            if (pane.Items.Count >= 1 && pane.Items[0].Name == ExplorerPane.ParentName)
                            {
                                try
                                {
                                    HandleDoubleClick(pane, pane.Items[0]);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.Print(ex.Message);
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void listView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //listView.Focus();
        }
    }
    
    public class GridViewRowPresenterWithGridLines : GridViewRowPresenter
    {
        private static readonly Style DefaultSeparatorStyle;
        public static readonly DependencyProperty SeparatorStyleProperty;
        private readonly List<FrameworkElement> _lines = new List<FrameworkElement>();

        static GridViewRowPresenterWithGridLines()
        {
            DefaultSeparatorStyle = new Style(typeof(Rectangle));
            DefaultSeparatorStyle.Setters.Add(new Setter(Shape.FillProperty, SystemColors.ControlLightBrush));
            SeparatorStyleProperty = DependencyProperty.Register("SeparatorStyle", typeof(Style), typeof (GridViewRowPresenterWithGridLines), new UIPropertyMetadata(DefaultSeparatorStyle, SeparatorStyleChanged));
        }

        public Style SeparatorStyle
        {
            get { return (Style) GetValue(SeparatorStyleProperty); }
            set { SetValue(SeparatorStyleProperty, value); }
        }

        private IEnumerable<FrameworkElement> Children
        {
            get { return LogicalTreeHelper.GetChildren(this).OfType<FrameworkElement>(); }
        }

        private static void SeparatorStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var presenter = (GridViewRowPresenterWithGridLines) d;
            var style = (Style) e.NewValue;
            foreach (FrameworkElement line in presenter._lines)
            {
                line.Style = style;
            }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var size = base.ArrangeOverride(arrangeSize);
            var children = Children.ToList();
            EnsureLines(children.Count);
            for (var i = 0; i < _lines.Count; i++)
            {
                var child = children[i];
                var x = child.TransformToAncestor(this).Transform(new Point(child.ActualWidth, 0)).X + child.Margin.Right;
                var rect = new Rect(x - 1, -Margin.Top, 1, size.Height + Margin.Top + Margin.Bottom);
                var line = _lines[i];
                line.Measure(rect.Size);
                line.Arrange(rect);
            }
            return size;
        }

        private void EnsureLines(int count)
        {
            count = count - _lines.Count;
            for (var i = 0; i < count; i++)
            {
                var line = (FrameworkElement) Activator.CreateInstance(SeparatorStyle.TargetType);
                line = new Rectangle{Fill=Brushes.LightGray};
                line.Style = SeparatorStyle;
                AddVisualChild(line);
                _lines.Add(line);
            }
        }

        protected override int VisualChildrenCount
        {
            get { return base.VisualChildrenCount + _lines.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            var count = base.VisualChildrenCount;
            return index < count ? base.GetVisualChild(index) : _lines[index - count];
        }
    }
}
