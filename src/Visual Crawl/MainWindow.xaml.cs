using System;
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
        private const double TopStart = 70;
        private const double DefaultTopMargin = 150;

        private const double LeftStart = 70;
        private const double DefaultLeftMargin = 250;

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
            List<Link> links = TestData.GetTestData();

            foreach (Link link in links)
            {
                Links.Add(new VisualLink(link));
            }

            PlaceOnField();
        }

        private void PlaceOnField()
        {
            List<ParentChild> nodes = new List<ParentChild> { new ParentChild(null, Links[0]) };

            foreach (VisualLink visualLink in Links)
            {
                VisualLink[] arr = GetByFrom(visualLink.Link.To);
                foreach (VisualLink foundLink in arr)
                {
                    nodes.Add(new ParentChild(visualLink, foundLink));
                }
            }

            foreach (ParentChild parentChild in nodes)
            {
                _multi.Add(parentChild);
            }
            
            double top = TopStart;
            double left = LeftStart;

            foreach (List<ParentChild> parentChildren in _multi.Get())
            {
                foreach (ParentChild parentChild in parentChildren)
                {
                    AddLinks(parentChild.VisualLink, top, left);
                    DrawLine(parentChild);

                    left += DefaultLeftMargin;
                }

                left = LeftStart;
                top += DefaultTopMargin;
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