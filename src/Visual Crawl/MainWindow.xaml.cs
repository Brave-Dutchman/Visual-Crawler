﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Core.Data;
using Core.Database;

namespace Visual_Crawl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public string Text { get; set; }

        public const double TOP_START = 120;
        public const double DEFAULT_TOP_MARGIN = 70;

        public const double LEFT_START = 70;
        public const double DEFAULT_LEFT_MARGIN = 70;

        public List<Link> Links { get; set; }

        private readonly MultiDimentionalList _multi;

        public MainWindow()
        {
            InitializeComponent();
            Links = new List<Link>();
            _multi = new MultiDimentionalList();

            Storage.Enable();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            Task.Run(new Action(CreateLinks));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Link link = FindStarter(FormatText());

            if (link == null)
            {
                MessageBox.Show("Link is niet gevonden");
                return;
            }

            GrdSelect.Visibility = Visibility.Hidden;
            GrdContent.Visibility = Visibility.Visible;

            VisualLink visual = new VisualLink(link, null, GetNumberOfChilderen(link.To));
            AddLinks(visual, TOP_START, LEFT_START);
        }

        private string FormatText()
        {
            string text = Text;

            if (text.EndsWith("/"))
            {
                text = text.Substring(0, Text.Length - 1);
            }

            return text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
            GrdSelect.Visibility = Visibility.Visible;
            GrdContent.Visibility = Visibility.Hidden;
        }

        private void ClearCanvas()
        {
            Field.Children.Clear();
        }

        private void CreateLinks()
        {
            Links.AddRange(Storage.GetLinks());
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
                int index = _multi.FindParentList(visual.Parent);
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
                AddLinks(temp, DEFAULT_TOP_MARGIN + visual.Top, visual.Left + count * DEFAULT_LEFT_MARGIN);
                count++;
            }

            Field.Width = count * DEFAULT_LEFT_MARGIN + LEFT_START + DEFAULT_LEFT_MARGIN;
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

        private Link FindStarter(string link)
        {
            return Links.Find(x => x.To == link);
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