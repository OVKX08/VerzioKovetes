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
            LoadData();
            CreateExcel();
            CreateTable();
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

            for (int i = 0; i < headers.Length; i++)
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
                values[counter, 8] = $"=(H{counter + 2} /G{counter + 2}) * 1000000";

                /*  decimal sqm = f.FloorArea / f.Price;

                  values[counter, 8] = sqm;*/
                counter++;
            }
            xlSheet.get_Range(
            GetCell(2, 1),
            GetCell(1 + values.GetLength(0), values.GetLength(1))).Value2 = values;

            

            //Törzs

            int lastRowID = xlSheet.UsedRange.Rows.Count;
            Excel.Range tableRange = xlSheet.get_Range(GetCell(2, 1), GetCell(lastRowID, headers.Length));
            tableRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

            //1.oszlop
            Excel.Range firstcolumnRange = xlSheet.get_Range(GetCell(2, 1), GetCell(lastRowID-1, 1));
            firstcolumnRange.Interior.Color = Color.LightYellow;
            firstcolumnRange.Font.Bold=true;

            //utolso oszlop
            int lastColID = xlSheet.UsedRange.Columns.Count;

            Excel.Range lastcolumnRange = xlSheet.get_Range(GetCell(2, lastColID), GetCell(lastRowID-1, lastColID));
            lastcolumnRange.Interior.Color = Color.LightGreen;
            lastcolumnRange.NumberFormat = "0.00";

            //header
            Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
            headerRange.Font.Bold = true;
            headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 40;
            headerRange.Interior.Color = Color.LightBlue;
            headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
        }

        private string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }


    }    
}   
