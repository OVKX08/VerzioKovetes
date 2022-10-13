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
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace gyakorlo
{
    public partial class Form1 : Form
    {

        Excel.Application xlApp;
        Excel.Workbook xlWB;
        Excel.Worksheet xlWS;
        List<OlympicResult> results = new List<OlympicResult>();
        public Form1()
        {


            InitializeComponent();
            // LoadData("Summer_olympic_Medals.csv");
            LoadData2("Summer_olympic_Medals.csv");
            LoadCombo();
            Calculate();


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void CreateExcel()
        {
            var headers = new string[] 
            {
                "Helyezes",
                "Ország",
                "Arany",
                "Ezüst",
                "Bronz"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                xlWS.Cells[1, i + 1] = headers[i];
            }

                var filtered = from r in results
                               where r.Year == (int)comboBox1.SelectedItem
                               select r;
            

            var counter = 2;

            foreach (var r in filtered)
            {
                xlWS.Cells[counter, 1] = r.Position;
                xlWS.Cells[counter, 2] = r.Country;
                for (int i = 0; i < 3; i++) xlWS.Cells[counter, i + 3] = r.Medals[i];

                counter++;
                
            }

            


        }   

        private void Export()
        {
            try
            {
                xlApp = new Excel.Application();
                xlWB = xlApp.Workbooks.Add(Missing.Value);
                xlWS = xlWB.ActiveSheet;

                CreateExcel();

                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }
           
            
        }

        private void LoadData(string filename)
        {
            StreamReader sr = new StreamReader(filename, Encoding.Default);

            sr.ReadLine();



            while (!sr.EndOfStream)
            {


                string[] line = sr.ReadLine().Split(',');

                var temp = new OlympicResult() {

                    Year = int.Parse(line[0]),
                    Country = line[3]
            };

                var medals = new int[3];
                medals[0] = int.Parse(line[5]);
                medals[1] = int.Parse(line[6]);
                medals[2] = int.Parse(line[7]);
                temp.Medals = medals;

                results.Add(temp);

               /* temp.Medals = new int[]
                {
                    medals[0]=int.Parse(line[5]),
                    medals[1]=int.Parse(line[6]),
                    medals[2]=int.Parse(line[7])
                };*/
                
                /*temp.Medals[1] = int.Parse(line[6]);
                temp.Medals[2] = int.Parse(line[7]);*/
                
               
                


                

            }


        }

        private void LoadData2(string filename)
        {
            StreamReader sr = new StreamReader(filename, Encoding.Default);

            sr.ReadLine();

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine().Split(',');
                var temp = new OlympicResult()
                {
                    Year = int.Parse(line[0]),
                    Country = line[3],
                    Medals = new int[]
                    {
                        int.Parse(line[5]),
                        int.Parse(line[6]),
                        int.Parse(line[7])
                    }
                };

                results.Add(temp);
            }


        }

        private void LoadCombo()
        {
            var yrs = (from r in results
                       orderby r.Year
                       select r.Year).Distinct();
            comboBox1.DataSource = yrs.ToList();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private int Ranking(OlympicResult o)
        {
            var bettercounter=0;

            var filtered = from r in results
                           where r.Year == o.Year && r.Country != o.Country
                           select r;

            foreach(var r in filtered)
            {
                if ((r.Medals[0] > o.Medals[0])
                    || (r.Medals[0] == o.Medals[0] && r.Medals[1] > o.Medals[1])
                    || (r.Medals[0] == o.Medals[0] && r.Medals[1] == o.Medals[1] && r.Medals[2] > o.Medals[2]))
                    bettercounter++; 
            }



            return bettercounter+1;
        }

        private void Calculate()
        {
            foreach (var r in results) 
                r.Position = Ranking(r);
            
                
        }
     
        

        private void button1_Click(object sender, EventArgs e)
        {
            Export();
        }
    }
}
