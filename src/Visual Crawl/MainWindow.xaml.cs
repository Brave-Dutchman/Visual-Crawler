using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Core;

namespace Visual_Crawl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public const double TopStart = 70;
        public const double DefaultTopMargin = 200;

        public const double LeftStart = 70;
        public const double DefaultLeftMargin = 250;

        public List<VisualLink> Links { get; set; }

        private readonly MultiDimentionalList _multi;

        public MainWindow()
        {
            InitializeComponent();
            Links = new List<VisualLink>();
            _multi = new MultiDimentionalList();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (Link link in TestData.GetTestData())
            {
                Links.Add(new VisualLink(link));
            }

            //Add the root node
            _multi.Add(null, Links[0]);

            foreach (VisualLink visualLink in Links)
            {
                VisualLink[] arr = GetByFrom(visualLink.Link.To);
                foreach (VisualLink foundLink in arr)
                {
                    _multi.Add(visualLink, foundLink);
                }
            }

            PlaceOnField();
        }

        private void PlaceOnField()
        {
            double top = TopStart;
            double left = LeftStart;

            List<List<ParentChild>> list = _multi.Get();

            foreach (List<ParentChild> parentChildren in list)
            {
                foreach (ParentChild parentChild in parentChildren)
                {
                    if (parentChild.Parent != null && left > parentChild.Parent.Left && parentChild.ChildIndex == 0)
                    {
                        parentChild.Parent.Left = left;
                        _multi.ResetByParentIndex(parentChild.Parent);
                    }
                    else if (parentChild.Parent != null && left + 50 < parentChild.Parent.Left)
                    {
                        double tempLeft = (parentChild.Parent.Left -50) + parentChild.ChildIndex * DefaultLeftMargin;
                        AddLinks(parentChild.VisualLink, top, tempLeft);
                        continue;
                    }

                    AddLinks(parentChild.VisualLink, top, left);
                    
                    left += DefaultLeftMargin;
                }

                left = LeftStart;
                top += DefaultTopMargin;
            }

            int count = list[1].Count;

            foreach (List<ParentChild> parentChildren in list)
            {
                foreach (ParentChild parentChild in parentChildren)
                {
                    DrawLine(parentChild);
                }
            }
        }

        private void AddLinks(VisualLink visual, double top, double left)
        {
            visual.Left = left;
            visual.Top = top;

            visual.MouseEnter += VisualOnMouseEnter;
            visual.MouseLeave += VisualOnMouseLeave;

            Field.Children.Add(visual);
        }

        private void DrawLine(ParentChild child)
        {
            if (child.Parent == null) return;

            Line line = new Line
            {
                Stroke = new SolidColorBrush(Colors.Gray),
                StrokeThickness = 1
            };

            Panel.SetZIndex(line, -100);

            line.X1 = child.Parent.CenterLeft;
            line.Y1 = child.Parent.CenterTop;
            line.X2 = child.VisualLink.CenterLeft;
            line.Y2 = child.VisualLink.CenterTop;

            Field.Children.Add(line);
        }

        private VisualLink[] GetByFrom(string link)
        {
            return Links.FindAll(x => x.Link.From == link && x.Link.To != link).ToArray();
        }

        private void VisualOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            Display.Link = null;
        }

        private void VisualOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            Display.Link = ((VisualLink)sender).Link;
        }
    }
}