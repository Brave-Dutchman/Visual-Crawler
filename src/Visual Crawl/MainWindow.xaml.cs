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
            foreach (Link link in TestData.GetTestData())
            {
                Links.Add(new VisualLink(link));
            }

            //Add the root node
            _multi.Add(new ParentChild(null, Links[0]));

            foreach (VisualLink visualLink in Links)
            {
                VisualLink[] arr = GetByFrom(visualLink.Link.To);
                foreach (VisualLink foundLink in arr)
                {
                    _multi.Add(new ParentChild(visualLink, foundLink));
                }
            }

            PlaceOnField();
        }

        private void PlaceOnField()
        {
            List<List<ParentChildren>> parentChildren = _multi.GetParentChildrenList();

            int widestChild = _multi.GetWightestChild();

            double height = DefaultTopMargin;
            double width = DefaultLeftMargin*widestChild * parentChildren.Count;

            Field.Width = width;

            double left = 0;
            double top = 0;

            foreach (VisualLink visualLink in parentChildren[0][0].VisualLinks)
            {
                BuildRectangel(height, width, left, top);
            }

            top = height;
            width = width / parentChildren[1][0].VisualLinks.Count;

            foreach (VisualLink visualLink in parentChildren[1][0].VisualLinks)
            {
                BuildRectangel(height, width, left, top);
                left += width;
            }


            //foreach (List<ParentChildren> list in parentChildren)
            //{
            //    foreach (ParentChildren children in list)
            //    {
                    
            //    }
            //}
        }

        private void BuildRectangel(double height, double width, double left, double top)
        {
            Rectangle rect = new Rectangle
            {
                Width = width -2,
                Height = height -2,
                Fill = new SolidColorBrush(Colors.Red)
            };

            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.StrokeThickness = 1;

            Canvas.SetLeft(rect, left);
            Canvas.SetTop(rect, top);

            Field.Children.Add(rect);
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