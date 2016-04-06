using System.Windows.Controls;
using System.Windows.Media;
using Core.Data;

namespace Visual_Crawl
{
    /// <summary>
    /// Interaction logic for VisualLink.xaml
    /// </summary>
    public partial class VisualLink
    {
        private double _left;
        private double _top;

        public double Left
        {
            get { return _left; }
            set
            {
                _left = /* Width / 2 + */ value;
                Canvas.SetLeft(this, _left);
            }
        }

        public double Top
        {
            get { return _top; }
            set
            {
                _top = /*Height / 2 + */ value;
                Canvas.SetTop(this, _top);
            }
        }

        public new VisualLink Parent { get; set; }
        public Link Link { get; set; }

        public int Children { get; set; }

        public DisplayedLink DisplayedLink { get; set; }


        public VisualLink(Link link, VisualLink parent, int children)
        {
            Link = link;
            Parent = parent;
            Children = children;
            DisplayedLink = DisplayedLink.Collapsed;

            InitializeComponent();

            if (!Link.To.Contains(Link.Host))
            {
               Ellipse.Fill = new SolidColorBrush(Colors.Blue);
            }
        }

        public override string ToString()
        {
            return Link.ToString();
        }
    }
}

public enum DisplayedLink
{
    Collapsed,
    PartVisable,
    FullVisable
}
