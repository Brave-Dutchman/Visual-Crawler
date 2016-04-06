using System.ComponentModel;
using System.Runtime.CompilerServices;
using Core;
using Core.Data;
using Visual_Crawl.Annotations;

namespace Visual_Crawl
{
    /// <summary>
    /// Interaction logic for DispalyValue.xaml
    /// </summary>
    public partial class DispalyValue : INotifyPropertyChanged
    {
        private Link _link;

        public Link Link
        {
            get { return _link; }
            set
            {
                _link = value;
                OnPropertyChanged();
            }
        }

        public DispalyValue()
        {
            InitializeComponent();
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
