using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.SqlClient;
using BarnCaseApi;

namespace BarnCaseApi
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Data Source=ALI;Initial Catalog=BarnCaseDB;Integrated Security=True";


        public Form1()
        {
            InitializeComponent();

            textBox1.MaxLength = 50; 
            textBox2.MaxLength = 100;
            textBox3.MaxLength = 50; 
            textBox4.MaxLength = 50; 

         
            textBox3.UseSystemPasswordChar = true;
            textBox4.UseSystemPasswordChar = true;

            
            lblError.Visible = false;
            lblErrorPassword.Visible = false;

            
            textBox2.TextChanged += TextBoxes_TextChanged;
            textBox3.TextChanged += TextBoxes_TextChanged;
            textBox4.TextChanged += TextBoxes_TextChanged;


            textBox1.KeyPress += OnlyLetters_KeyPress;
            textBox2.KeyPress += NoWhitespace_KeyPress;
            textBox3.KeyPress += NoWhitespace_KeyPress;
            textBox4.KeyPress += NoWhitespace_KeyPress;

            
            ShowPassword.CheckedChanged += ShowPassword_CheckedChanged;
        }

        private void ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool show = ShowPassword.Checked;
            textBox3.UseSystemPasswordChar = !show;
            textBox4.UseSystemPasswordChar = !show;
        }

        private void TextBoxes_TextChanged(object sender, EventArgs e)
        {
            ValidateAll();
        }

        private void OnlyLetters_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }

        private void NoWhitespace_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }

        private void ValidateAll()
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            bool emailEmpty = string.IsNullOrWhiteSpace(textBox2.Text);
            bool emailValid = Regex.IsMatch(textBox2.Text, emailPattern);
            bool passwordsMatch = textBox3.Text == textBox4.Text;
            string password = textBox3.Text;
            bool passwordRule = password.Length >= 8 &&
                                Regex.IsMatch(password, "[A-Z]") &&
                                Regex.IsMatch(password, "[a-z]") &&
                                Regex.IsMatch(password, "[0-9]");

            string errors = "";

            if (!emailEmpty && !emailValid)
                errors += "Please enter a valid email address\n";

            if (!string.IsNullOrEmpty(textBox3.Text) &&
                !string.IsNullOrEmpty(textBox4.Text) &&
                !passwordsMatch)
                errors += "Passwords do not match\n";

            lblError.Text = errors.Trim();
            lblError.ForeColor = Color.DarkRed;
            lblError.Visible = !string.IsNullOrEmpty(lblError.Text);

            if (!string.IsNullOrEmpty(password) && !passwordRule)
            {
                lblErrorPassword.Text = "Password must be at least 8 characters, include uppercase, lowercase, and a number";
                lblErrorPassword.ForeColor = Color.DarkRed;
                lblErrorPassword.Visible = true;
            }
            else
            {
                lblErrorPassword.Text = "";
                lblErrorPassword.Visible = false;
            }
        }
        
        private void Button1_Click(object sender, EventArgs e)
        {

            ValidateAll();

            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(lblError.Text) || !string.IsNullOrEmpty(lblErrorPassword.Text))
            {
                MessageBox.Show("Please correct the errors.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
           
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Users (Username, Email, [Password]) VALUES (@Username, @Email, @Password)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", textBox1.Text);
                        cmd.Parameters.AddWithValue("@Email", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Password", textBox3.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Registration successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Form2 form2 = new Form2();
                form2.Show();
                this.Hide();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                    MessageBox.Show("This email is already registered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         
            }
        }

        private void picToLogin_Click(object sender, EventArgs e)
        {
            
            Form2 loginForm = new Form2();
            loginForm.Show();
            this.Hide();
        }

    }
}