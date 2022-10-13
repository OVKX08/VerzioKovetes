using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gyakorlo
{
    public partial class Form1 : Form
    {
        List<OlympicResult> results;


        public Form1()
        {
            InitializeComponent();
            LoadData("Summer_olympic_Medals.csv");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LoadData(string filename)
        {
            StreamReader sr = new StreamReader(filename, Encoding.Default);

            var header = sr.ReadLine().Split(',');

            

            while (!sr.EndOfStream)
            {
                OlympicResult temp = new OlympicResult();
                
                string[] line = sr.ReadLine().Split(',');
                
                temp.Year = int.Parse(line[0]);
                temp.Country = line[3];
                temp.Medals[0] = int.Parse(line[5]);
                temp.Medals[1] = int.Parse(line[6]);
                temp.Medals[2] = int.Parse(line[7]);
                
                results.Add(temp);
                


                

            }


        }
    }
}
