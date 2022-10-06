using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
namespace negyedik
{
    public partial class Form1 : Form
    {
        RealEstateEntities context = new RealEstateEntities();
        List<Flat> flats;

        Excel.Application xlApp; 
        Excel.Workbook xlWB; 
        Excel.Worksheet xlSheet; 

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void LoadData()
        {
            flats = context.Flat.ToList();
        }

        private void CreateExcel()
        {
            try
            {
                xlApp = new Excel.Application();

                xlWB = xlApp.Workbooks.Add(Missing.Value);

                xlSheet = xlWB.ActiveSheet;


                CreateTable();

                xlApp.Visible = true;
                xlApp.UserControl = true;

            }
            catch (Exception ex)
            {
                string errMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(errMsg, "Error");

                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
                
            }
        }

        private void CreateTable()
        {
            string[] headers = new string[]
            {
                "Kod",
                "Elado",
                "Oldal",
                "Kerulet",
                "Lift",
                "Szobak szama",
                "Alapterulet (m2)",
                "Ar (mFt)",
                "Negyzetmeter ar (Ft/m2)"
            };

            for (int i = 0; i < 9; i++)
            {
                xlSheet.Cells[1, i + 1] = headers[i];
            }


            object[,] values = new object[flats.Count, headers.Length];

            int counter = 0;
            foreach (Flat f in flats)
            {
                values[counter, 0] = f.Code;
                values[counter, 1] = f.Vendor;
                values[counter, 2] = f.Side;
                values[counter, 3] = f.District;
                if (f.Elevator == true)
                {
                    values[counter, 4] = "Van";
                }
                    else values[counter, 4] = "Nincs";
                values[counter, 5] = f.NumberOfRooms;
                values[counter, 6] = f.FloorArea;
                values[counter, 7] = f.Price;

                decimal sqm = f.FloorArea / f.Price;
                
                values[counter, 8] = sqm;
                counter++;
            }



        }


    }    
}   
