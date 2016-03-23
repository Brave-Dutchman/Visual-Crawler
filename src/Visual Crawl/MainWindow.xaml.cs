﻿using System;
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

        private const double LeftStart = 0;
        private const double DefaultLeftMargin = 250;

        public List<VisualLink> Links { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Links = new List<VisualLink>();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            List<Link> links = TestData.GetTestData();

            Links.Add(new VisualLink(new Link("insidegamer.nl", "http://insidegamer.nl/", "http://insidegamer.nl/")));
            foreach (Link link in links)
            {
                Links.Add(new VisualLink(link));
            }

            PlaceOnField();
        }

        private void PlaceOnField()
        {
            //Place the root
            AddLinks(Links[0], TopStart);

            foreach (VisualLink visualLink in Links)
            {
                //find al nodes connected to the root
                VisualLink[] arr = GetByFrom(visualLink.Link.To);
                double top = visualLink.Top + DefaultTopMargin;
                double left = LeftStart;

                //place all connected on the field
                foreach (VisualLink foundLinks in arr)
                {
                    AddLinks(foundLinks, top, left);
                    left += DefaultLeftMargin;
                }
            }

            //AddLinks(Links[1], 200, -375);
            //AddLinks(Links[2], 200, -125);
            //AddLinks(Links[3], 200, 125);
            //AddLinks(Links[4], 200, 375);

            //AddLinks(Links[5], 350, -125);
            //AddLinks(Links[6], 350, 125);
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

        private VisualLink[] GetByFrom(string link)
        {
            return Links.FindAll(x => x.Link.From == link && x.Link.To != link).ToArray();
        }

        private VisualLink GetLinkByLink(string link)
        {
            foreach (VisualLink visualLink in Links)
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