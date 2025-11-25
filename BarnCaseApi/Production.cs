using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarnCaseApi
{
    public partial class Production : Form
    {
        private Timer productiontimer;
        private int progressValue = 0;
        private int progressMax = 0;
        private SimpleAnimal animal;

        private int Milk = 0;
        private int Wool = 0;
        private int Eggs = 0;

        public List<FarmManagement.SimpleAnimal> Animals { get; set; }
        public decimal Balance { get; set; }
        private CultureInfo dollarCulture = CultureInfo.GetCultureInfo("en-US");

        public Production(List<FarmManagement.SimpleAnimal> animals, decimal balance)
        {
            InitializeComponent();
            Balance = balance;
            Animals = animals;

            productiontimer = new Timer();
            productiontimer.Interval = 100;
            productiontimer.Tick += ProductionTimer_Tick;
            productiontimer.Start();
        }
        private void ProductionTimer_Tick(object sender, EventArgs e)
        {
            progressValue++;

            if (progressValue > progressMax) progressValue = 0;
            {
                progressBar1.Value = progressValue++;
            }
            if (progressValue == progressMax)
            {
                ProduceProducts();
            }
        }
        private void ProduceProducts()
        {
            foreach (var animal in Animals);
            {
                string type = animal.Type.ToLower();
                if (type == "cow") Milk++;
                else if (type == "Sheep") Wool++;
                else if (type == "Chicken") Eggs++;
            }
            lblMilk.Text = "Milk" + Milk;
            lblWool.Text = "Wool" + Wool;
            lblEggs.Text = "Eggs" + Eggs;
        }
        private void btnSellProducts_Click(object sender, EventArgs e)
        {
            int total = (Milk * 5)+(Wool * 8 )+(Eggs * 2);

            Milk = 0;
            Wool = 0;
            Eggs = 0;

            lblMilk.Text = "Milk: 0";
            lblWool.Text = "Wool: 0";
            lblEggs.Text = "Eggs: 0";

            MessageBox.Show("All products sold for $"+ total);
        }
    }

    public class SimpleAnimal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public decimal Price { get; set; }
        public string DeathCause { get; set; }
    }
}