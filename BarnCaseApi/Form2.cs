using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BarnCaseApi
{
    public partial class Form2 : Form
    {
        private string connectionString = @"Data Source=ALI;Initial Catalog=BarnCaseDB;Integrated Security=True";

        public Form2()
        {
            InitializeComponent();

            textBox1.MaxLength = 50;
            textBox2.MaxLength = 255;
            textBox2.UseSystemPasswordChar = true;

            lblRegister.ForeColor = Color.Blue;
            lblRegister.Cursor = Cursors.Hand;
            lblRegister.Font = new Font(lblRegister.Font, FontStyle.Underline);

          
            lblRegister.Click += lblRegister_Click;
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                  
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        int userCount = (int)cmd.ExecuteScalar();

                        if (userCount > 0)
                        {
                            MessageBox.Show($"Login successful! Welcome {username}.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            
                            Form3 form3 = new Form3();
                            form3.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Username or password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("SQL connection failed: " + sqlEx.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblRegister_Click(object sender, EventArgs e)
        {
            Form1 registerForm = new Form1();
            registerForm.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !checkBox1.Checked;
        }
    }
}
