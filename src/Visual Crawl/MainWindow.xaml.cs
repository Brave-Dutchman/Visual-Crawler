using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Core;
using Core.Objects;

namespace Visual_Crawl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public const double TopStart = 120;
        public const double DefaultTopMargin = 70;

        public const double LeftStart = 70;
        public const double DefaultLeftMargin = 70;

        public List<Link> Links { get; set; }

        private readonly MultiDimentionalList _multi;


        public MainWindow()
        {
            InitializeComponent();
            Links = new List<Link>();
            _multi = new MultiDimentionalList();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            Links = Storage.GetLinks();
            Links[0].To = "http://www.insidegamer.nl/";

            VisualLink visual = new VisualLink(Links[0], null, GetNumberOfChilderen(Links[0].To));
            AddLinks(visual, TopStart, LeftStart);
        }

        private void VisualOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            VisualLink visual = (VisualLink) sender;

            switch (visual.DisplayedLink)
            {
                case DisplayedLink.Collapsed:
                    Show(visual);
                    break;
                case DisplayedLink.PartVisable:
                    CheckIfRoot(visual);
                    Show(visual);
                    break;
                case DisplayedLink.FullVisable:
                    Clear(visual);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Clear(VisualLink visual)
        {
            visual.DisplayedLink = DisplayedLink.Collapsed;
        }

        private void CheckIfRoot(VisualLink visual)
        {
            if (visual.Parent != null) return;

            const int index = 0;
            int total = _multi.Count - 1;

            for (int i = total; i > index; i--)
            {
                foreach (VisualLink visualLink in _multi.GetByIndex(i).ToArray())
                {
                    Field.Children.Remove(visualLink);
                    _multi.RemoveAt(visualLink, i);
                }
            }
        }

        private void Show(VisualLink visual)
        {
            if (visual.Parent != null)
            {
                int index = _multi.FindParent(visual.Parent);
                int total = _multi.Count -1;

                for (int i = total; i > index; i--)
                {
                    foreach (VisualLink visualLink in _multi.GetByIndex(i).ToArray())
                    {
                        Field.Children.Remove(visualLink);
                        _multi.RemoveAt(visualLink, i);
                    }
                }

                AddLinks(visual, visual.Top, visual.Parent.Left);
                visual.Parent.DisplayedLink = DisplayedLink.PartVisable;
            }

            StaticDisplay.Link = visual.Link;

            int count = 0;
            foreach (Link connectedLink in GetLinksFrom(visual.Link.To))
            {
                VisualLink temp = new VisualLink(connectedLink, visual, GetNumberOfChilderen(connectedLink.To));
                AddLinks(temp, DefaultTopMargin + visual.Top, visual.Left + count * DefaultLeftMargin);
                count++;
            }

            Field.Width = count * DefaultLeftMargin + LeftStart + DefaultLeftMargin;
            visual.DisplayedLink = DisplayedLink.FullVisable;
        }

        private void AddLinks(VisualLink visual, double top, double left)
        {
            visual.Left = left;
            visual.Top = top;

            visual.MouseEnter += VisualOnMouseEnter;
            visual.MouseLeave += VisualOnMouseLeave;
            visual.MouseLeftButtonDown += VisualOnMouseLeftButtonDown;

            Field.Children.Add(visual);
            _multi.Add(visual);
        }

        private Link[] GetLinksFrom(string link)
        {
            return Links.FindAll(x => x.From == link && x.To != link).ToArray();
        }

        private int GetNumberOfChilderen(string link)
        {
            return Links.FindAll(x => x.From == link && x.To != link).Count;
        }

        private void VisualOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            Display.Link = null;
        }

        private void VisualOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            Display.Link = ((VisualLink) sender).Link;
        }
    }
}