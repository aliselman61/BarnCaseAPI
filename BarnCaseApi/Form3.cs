using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace BarnCaseApi
{
    public partial class FarmManagement : Form
    {
        public string LoggedInUsername { get; set; }

        private List<SimpleAnimal> animals = new List<SimpleAnimal>();
        private decimal balance = 500m;
        private readonly CultureInfo dollarCulture = CultureInfo.GetCultureInfo("en-US");
        private Timer agingTimer;
        private Random rnd = new Random();

        private string[] deathCauses = new string[]
        {
            "hit by a car while grazing",
            "attacked by a wild animal",
            "fell into a river",
            "caught in a storm",
            "sudden illness",
            "choked on food(ali ate)",
            "tripped and injured",
            "attacked by a predator",
            
            
        };

        public FarmManagement()
        {
            InitializeComponent();
            this.Load += FarmManagement_Load;

            txtAge.MaxLength = 2;
            cmbType.MaxLength = 8;
            cmbGender.MaxLength = 6;
            textBox1.MaxLength = 10;

            btnAddAnimal.Click += btnAddAnimal_Click;
            btnSellClick.Click += btnSellClick_Click;
        }

        private void FarmManagement_Load(object sender, EventArgs e)
        { 
            if (!string.IsNullOrEmpty(LoggedInUsername))
                label1.Text = LoggedInUsername;

            if (cmbType.Items.Count == 0)
                cmbType.Items.AddRange(new string[] { "Cow", "Chicken", "Sheep" });

            if (cmbGender.Items.Count == 0)
                cmbGender.Items.AddRange(new string[] { "Female" });

            if (lstAnimals.View != View.Details)
            {
                lstAnimals.View = View.Details;
                lstAnimals.FullRowSelect = true;
                lstAnimals.GridLines = true;
            }
            if (lstAnimals.Columns.Count == 0)
            {
                lstAnimals.Columns.Clear();
                lstAnimals.Columns.Add(new ColumnHeader() { Text = "Name", Width = 100 });
                lstAnimals.Columns.Add(new ColumnHeader() { Text = "Type", Width = 100 });
                lstAnimals.Columns.Add(new ColumnHeader() { Text = "Age", Width = 60 });
                lstAnimals.Columns.Add(new ColumnHeader() { Text = "Gender", Width = 70 });
                lstAnimals.Columns.Add(new ColumnHeader() { Text = "Price ($)", Width = 80 });
            }

            UpdateBalanceLabel();

            agingTimer = new Timer();
            agingTimer.Interval = 10000;
            agingTimer.Tick += AgingTimer_Tick;
            agingTimer.Start();
        }

        private void btnAddAnimal_Click(object sender, EventArgs e)
        {
            if (cmbType.SelectedIndex == -1 || cmbGender.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtAge.Text))
            {
                MessageBox.Show("Please fill all fields before adding an animal.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int age;
            if (!int.TryParse(txtAge.Text, out age))
            {
                MessageBox.Show("Age must be a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string type = cmbType.SelectedItem.ToString().Trim();
            string gender = cmbGender.SelectedItem.ToString();
            decimal price = GetAnimalPrice(type);

            if (balance < price)
            {
                MessageBox.Show("Not enough money to buy " + type + ". Required: " + price.ToString("C", dollarCulture),
                                "Insufficient Funds", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            balance -= price;

            int count = animals.Count(a => a.Type == type) + 1;
            string name = type + "_" + count;

            var animal = new SimpleAnimal
            {
                Id = Guid.NewGuid(),
                Name = name,
                Type = type,
                Age = age,
                Gender = gender,
                Price = price,
                DeathCause = ""
            };

            animals.Add(animal);

            ListViewItem item = new ListViewItem(animal.Name);
            item.SubItems.Add(animal.Type);
            item.SubItems.Add(animal.Age.ToString());
            item.SubItems.Add(animal.Gender);
            item.SubItems.Add(animal.Price.ToString("C", dollarCulture));
            lstAnimals.Items.Add(item);

            cmbType.SelectedIndex = -1;
            cmbGender.SelectedIndex = -1;
            txtAge.Clear();

            UpdateBalanceLabel();
        }

        private void btnSellClick_Click(object sender, EventArgs e)
        {
            string sellName = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(sellName))
            {
               MessageBox.Show("Please enter the name of the animal to sell.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               return;
            }

            SimpleAnimal animal = animals.FirstOrDefault(a => a.Name.Equals(sellName, StringComparison.OrdinalIgnoreCase));

            if (animal == null)
            {
               MessageBox.Show("No animal found with that name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return;
            }

            balance += animal.Price;
            UpdateBalanceLabel();
            animals.Remove(animal);

            foreach (ListViewItem i in lstAnimals.Items)
            {
                if (i.Text.Equals(animal.Name, StringComparison.OrdinalIgnoreCase))
                {
                    lstAnimals.Items.Remove(i);
                    break;
                }
            }

            MessageBox.Show(animal.Name + " has been sold for " + animal.Price.ToString("C", dollarCulture),
                            "Animal Sold", MessageBoxButtons.OK, MessageBoxIcon.Information);

            textBox1.Clear();
        }

        private void AgingTimer_Tick(object sender, EventArgs e)
        {
            List<SimpleAnimal> deadAnimals = new List<SimpleAnimal>();

            foreach (var a in animals)
            {
                a.Age++;

                int maxAge = GetMaxAge(a.Type);
                decimal basePrice = GetAnimalPrice(a.Type);

                int gainLimit = 0;

                if (a.Type.ToLower() == "cow") gainLimit = 20;
                else if (a.Type.ToLower() == "sheep") gainLimit = 10;
                else if (a.Type.ToLower() == "chicken") gainLimit = 4;

                decimal factor = 1.0m;

                if (a.Age <= gainLimit)
                {
                    factor += 0.05m * a.Age;
                }
                else
                {
                    int overAge = a.Age - gainLimit;
                    factor += 0.05m * gainLimit;
                    factor -= 0.05m * overAge;

                    if (factor < 0.5m)
                        factor = 0.5m;
                }

                a.Price = basePrice * factor;

                if (a.Age > maxAge)
                {
                    a.DeathCause = "old age";
                    deadAnimals.Add(a);
                }
                else if (rnd.Next(0, 100) < 5)
                {
                    a.DeathCause = deathCauses[rnd.Next(deathCauses.Length)];
                    deadAnimals.Add(a);
                }
            }

            foreach (var dead in deadAnimals)
            {
                animals.Remove(dead);

                ListViewItem itemToRemove = null;
                foreach (ListViewItem i in lstAnimals.Items)
                {
                    if (i.Text == dead.Name)
                    {
                        itemToRemove = i;
                        break;
                    }
                }
                if (itemToRemove != null)
                    lstAnimals.Items.Remove(itemToRemove);

                MessageBox.Show(dead.Name + " has died (" + dead.DeathCause + ").",
                "Animal Died", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            foreach (ListViewItem item in lstAnimals.Items)
            {
                var a = animals.FirstOrDefault(an => an.Name == item.Text);

                if (a != null)
                {
                    item.SubItems[2].Text = a.Age.ToString();
                    item.SubItems[4].Text = a.Price.ToString("C", dollarCulture);
                }
            }
        }

        private decimal GetAnimalPrice(string type)
        {
            type = type.ToLower();

            if (type == "cow") return 150m;
            if (type == "chicken") return 30m;
            if (type == "sheep") return 100m;

            return 0m;
        }

        private int GetMaxAge(string type)
        {
            type = type.ToLower();

            if (type == "cow") return 25;
            if (type == "chicken") return 6;
            if (type == "sheep") return 15;

            return 10;
        }

        private void UpdateBalanceLabel()
        {
            string balanceText = balance.ToString("C", dollarCulture);

            foreach (Control c in this.Controls)
            {
                if (c is Label lbl && lbl.Name == "lblCash")
                {
                    lbl.Text = "Cash: " + balanceText;
                    return;
                }

                if (c is GroupBox || c is Panel)
                {
                    foreach (Control x in c.Controls)
                    {
                        if (x is Label && x.Name == "lblCash")
                        {
                            x.Text = "Cash: " + balanceText;
                            return;
                        }
                    }
                }
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
        
        private void txtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else e.Handled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}
