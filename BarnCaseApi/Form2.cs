using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
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
            string password = textBox2.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("SELECT PasswordHash, PasswordSalt, Iterations FROM dbo.Hash WHERE Username = @Username", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 40) { Value = username });
                    conn.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (!rdr.Read())
                        {
                            MessageBox.Show("Username or password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        byte[] storedHash = (byte[])rdr["PasswordHash"];
                        byte[] storedSalt = (byte[])rdr["PasswordSalt"];
                        int iterations = (int)rdr["Iterations"];

                        bool verified = VerifyPBKDF2(password, storedSalt, iterations, storedHash);

                        if (verified)
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

        #region PBKDF2 Verification
        private bool VerifyPBKDF2(string password, byte[] salt, int iterations, byte[] expectedHash)
        {
            using (var pbk = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] computedHash = pbk.GetBytes(expectedHash.Length);
                return ByteArrayEquals(computedHash, expectedHash);
            }
        }

        private bool ByteArrayEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }
        #endregion
    }
}
