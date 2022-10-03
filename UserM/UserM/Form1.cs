using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = Resource.LastName; //label1
            label2.Text = Resource.FirstName; //labe2
            button1.Text = Resource.Add;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
