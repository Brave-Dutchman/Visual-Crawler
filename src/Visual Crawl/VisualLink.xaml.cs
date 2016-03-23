using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Core;
using Visual_Crawl.Annotations;

namespace Visual_Crawl
{
    /// <summary>
    /// Interaction logic for VisualLink.xaml
    /// </summary>
    public partial class VisualLink : INotifyPropertyChanged
    {
        private Link _link;
        private double _left;
        private double _top;

        public double Left
        {
            get { return _left; }
            set
            {
                _left = Width / 2 + value;
                Canvas.SetLeft(this, _left);
            }
        }

        public double Top
        {
            get { return _top; }
            set
            {
                _top = Height / 2 + value;
                Canvas.SetTop(this, _top);
            }
        }

        public double CenterLeft
        {
            get { return _left + 50; }
        }

        public double CenterTop
        {
            get { return _top + 50; }
        }

        public Link Link
        {
            get { return _link; }
            set
            {
                _link = value; 
                OnPropertyChanged("Link");
            }
        }

        public VisualLink()
        {
            InitializeComponent();
        }

        public VisualLink(Link link) : this()
        {
            Link = link;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
