using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Core.Database;


namespace DebugForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Storage.Enable();
            button1.Text = "GetCrawledLink";
        }

        private void button1_Click(object sender, EventArgs e)
        {           
            var crawledlist = Storage.GetCrawledLinks();
            var notcrawledlist = Storage.ReadNotCrawledLinks();
            var finallist = new List<string>();

            foreach (var link in crawledlist)
            {
                foreach (var falselink in notcrawledlist)
                {
                    if (link.Link == falselink.Link && link.IsCrawled)
                    {
                        finallist.Add(link.Link);
                    }
                }
            }

            foreach (var link in finallist)
            {
                listBox1.Items.Add(link);
            }
        }
    }
}
