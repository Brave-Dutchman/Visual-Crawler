using System;
using System.Collections.Generic;
using System.Linq;
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

        private const double LeftStart = 0;
        private const double DefaultLeftMargin = 250;


        public List<Link> Links { get; set; }

        public List<VisualLink> VisualLinks { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            VisualLinks = new List<VisualLink>();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            Links = TestData.GetTestData();

            LinkNode node = new LinkNode(Links[0]);
            node.Nodes = GetByFrom(node.Link.To);

            foreach (LinkNode linkNode in node.Nodes)
            {
                linkNode.Nodes = GetByFrom(linkNode.Link.To);
            }

            //PlaceOnField();
        }

        private void PlaceOnField()
        {
            ////Place the root
            //AddLinks(VisualLinks[0], TopStart);


            //foreach (VisualLink visualLink in VisualLinks)
            //{
            //    //Find all nodes connected to the root
            //    VisualLink[] arr = GetByFrom(visualLink.Link.To);
            //    double top = visualLink.Top + DefaultTopMargin;
            //    double left = LeftStart;

            //    //Place all connected on the field
            //    foreach (VisualLink foundLinks in arr)
            //    {
            //        AddLinks(foundLinks, top, left);
            //        left += DefaultLeftMargin;
            //    }
            //}
        }

        private void AddLinks(VisualLink visual, double top, double left = 0)
        {
            visual.Left = Field.Width / 2 + left;
            visual.Top = top;

            visual.MouseEnter += VisualOnMouseEnter;
            visual.MouseLeave += VisualOnMouseLeave;

            Line line = new Line();
            line.Stroke = new SolidColorBrush(Colors.Gray);
            line.StrokeThickness = 1;
            Panel.SetZIndex(line,  -100);

            VisualLink from = GetLinkByLink(visual.Link.From);

            line.X1 = from.CenterLeft;
            line.Y1 = from.CenterTop;
            line.X2 = visual.CenterLeft;
            line.Y2 = visual.CenterTop;

            Field.Children.Add(line);
            Field.Children.Add(visual);
        }

        private LinkNode[] GetByFrom(string link)
        {
            return (from link1 in Links where link1.From == link && link1.To != link select new LinkNode(link1)).ToArray();
        }

        private VisualLink GetLinkByLink(string link)
        {
            foreach (VisualLink visualLink in VisualLinks)
            {
                if (visualLink.Link.To == link)
                {
                    return visualLink;
                }
            }

            throw new Exception();
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