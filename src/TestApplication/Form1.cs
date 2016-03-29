using System;
using Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Objects;

namespace TestApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            Link link = new Link("http://yolo.com","me","you");
            List<Link> links = new List<Link>();
            links.Add(link);
            CrawledLink link2 = new CrawledLink("Meow Meow");
            List<CrawledLink> links2 = new List<CrawledLink>();
            links2.Add(link2);
            Storage.WriteLinks(links);
            Storage.WriteLinks(links2);

            var list  = Storage.GetLinks();

            foreach (var item in list)
            {
                listBox1.Items.Add(item.Host);
   
            }
            var clist = Storage.GetCrawledLinks();
            foreach (var item in clist)
            {
                listBox1.Items.Add(item.Link);

            }



        }
    }
}
