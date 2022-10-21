using otodik.Entities;
using otodik.MnbServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace otodik
{
    public partial class Form1 : Form
    {

        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();

        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = "EUR";
            comboBox1.DataSource = Currencies;
            RequestCurrencies();
            RefreshData();
        }

        private void RefreshData()
        {
            Rates.Clear();
            dataGridView1.DataSource = Rates;
            chartRateData.DataSource = Rates;
           
            RequestService();
            DisplayData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void RequestCurrencies()
        {
            /*var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetCurrenciesRequestBody();
            
            var response = mnbService.GetCurrencies(request);

            var result = response.GetCurrenciesResult;

            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach (XmlElement element in xml.DocumentElement)
            {
                

            }*/

            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetCurrenciesRequestBody();
            var MnbGetExResp = mnbService.GetCurrencies(request);
            var result = MnbGetExResp.GetCurrenciesResult;

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);
            foreach (XmlElement x in xml.DocumentElement.ChildNodes[0])
            {
                Currencies.Add(x.InnerText);

            }




        }

        private void RequestService()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody()
            {
                //currencyNames = comboBox1.SelectedItem.ToString(),
                currencyNames = "EUR",
                startDate = dtmStart.Value.ToString(),
                endDate = dtmEnd.Value.ToString()
            };
            var response = mnbService.GetExchangeRates(request);

            var result = response.GetExchangeRatesResult;

            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach (XmlElement element in xml.DocumentElement)
            {

                var rate = new RateData();
                
                Rates.Add(rate);

                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                var childElement = (XmlElement)element.ChildNodes[0];
                if (childElement == null) continue;
                rate.Currency = childElement.GetAttribute("request");
               
                
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0) rate.Value = value/unit;
            }

            

        }
        private void DisplayData()
        {
            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;


            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var ChartArea = chartRateData.ChartAreas[0];
            ChartArea.AxisX.MajorGrid.Enabled = false;
            ChartArea.AxisY.MajorGrid.Enabled = false;
            ChartArea.AxisY.IsStartedFromZero = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dtmEnd_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void chartRateData_Click(object sender, EventArgs e)
        {

        }
    }
}
