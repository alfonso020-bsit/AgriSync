using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PROJECTSYSTEM
{
    public partial class SignUpForm : Form
    {
        private string connectionString = "Server=localhost;Database=db_ricemillmanagement;User ID=root;";
        public SignUpForm()
        {
            InitializeComponent();
        }
        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string Username = txtUsername.Text;
            string FirstName = txtFName.Text;
            string LastName = txtLName.Text;
            string Password = txtPassword.Text;

            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
            {

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Create a new user account
                        string SignUpQuery = "INSERT INTO tbl_users (Username, FirstName, LastName, Password) VALUES (@Username,@FirstName, @LastName, @Password)";

                        using (MySqlCommand command = new MySqlCommand(SignUpQuery, connection))
                        {
                            // Replace parameter values with actual data
                            command.Parameters.AddWithValue("@Username", Username);
                            command.Parameters.AddWithValue("@FirstName", FirstName);
                            command.Parameters.AddWithValue("@LastName", LastName);
                            command.Parameters.AddWithValue("@Password", Password); 

                            // Execute the command
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Account created successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtFName.Clear();
                                txtLName.Clear();
                                txtPassword.Clear();
                                txtUsername.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Failed to create account", "FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please fill out the needed informations", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPassword.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void lblClear_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtFName.Text = "";
            txtLName.Text = "";
            txtPassword.Text = "";
        }

        private void lblLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginform = new LoginForm();
            loginform.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void SignUpForm_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
