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
using WindowsFormsApp1.Entities;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();


        List<int> m = new List<int>();
        List<int> f = new List<int>();
        List<int> y = new List<int>();

        Random rng = new Random(1234);
        public Form1()
        {
            InitializeComponent();

            

            Population = GetPopulation(@"C:\Temp\nép.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");

            

           

           
            
        }

        private void Simulation()
        {
            int end = (int)numericUpDown1.Value;


            for (int year = 2005; year < end+1; year++)
            {

                for (int i = 0; i < Population.Count; i++)
                {

                    Person p = new Person();
                    p = Population[i];

                    SimStep(year, p);


                }


                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                m.Add(nbrOfMales);

                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                f.Add(nbrOfFemales);
                y.Add(year);

                Console.WriteLine(
        string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
            }
        }


        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            };

            return population;
        }


        private void DisplayResults()
        {
            string result="";

            for (int i = 0; i < y.Count(); i++)
            {

                string r;
                string r1 = result;
                int males = m[i];
                int females = f[i];
                int year = y[i];

                r = "Szimulációs év: " + year + "\n \t Fiúk: " + males + "\n \t Lányok: " + females + "\n";

                result = result + r;
            }

            richTextBox1.Text = result;
        }

        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> bprob = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    bprob.Add(new BirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        P = double.Parse(line[2])
                    });
                }
            };

            return bprob;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> dprob = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    dprob.Add(new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        P = double.Parse(line[2])
                    });
                }
            };

            return dprob;
        }


        private void SimStep(int year, Person person)
        {
            if (!person.IsAlive) return;

            byte age = (byte)(year - person.BirthYear);
            //halval
            double dprob = (from x in DeathProbabilities
                            where x.Gender == person.Gender && x.Age == age
                            select x.P).FirstOrDefault();
            //pallos
            if (rng.NextDouble() <= dprob) person.IsAlive = false;



            if (person.IsAlive && person.Gender == Gender.Female) 
            {
                //szulval
                double bprob = (from x in BirthProbabilities
                                where x.Age == age
                                select x.P).FirstOrDefault();

                //gyerekgyar
                if (rng.NextDouble()<=bprob)
                {

                    Person newborn = new Person();
                    newborn.BirthYear = year;
                    newborn.NbrOfChildren = 0;
                    newborn.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(newborn);

                }
            }
            

            
        }




        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Simulation();
            DisplayResults();
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            
            if (of.ShowDialog() == DialogResult.OK)
            {

            }
            
        }

    }
}
