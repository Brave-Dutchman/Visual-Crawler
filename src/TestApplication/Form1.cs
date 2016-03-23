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
            Storage.Connect();
            Link link = new Link("http://yolo.com","me","you");
            List<Link> links = new List<Link>();
            links.Add(link);
            Storage.WriteLinks(links);
        }
    }
}
