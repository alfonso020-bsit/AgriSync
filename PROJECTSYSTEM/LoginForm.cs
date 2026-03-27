using MySql.Data.MySqlClient;
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

namespace PROJECTSYSTEM
//{
//    public partial class LoginForm : Form
//    {
//        private string connectionString = "Server=localhost;Database=db_ricemillmanagement;User ID=root;";
//        public LoginForm()
//        {
//            InitializeComponent();
//        }

//        private string GetPasswordFromDatabase(string Username)
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // Retrieve password from the database
//                    string retrievePasswordQuery = "SELECT Password FROM tbl_users WHERE Username = @Username";

//                    using (MySqlCommand command = new MySqlCommand(retrievePasswordQuery, connection))
//                    {
//                        command.Parameters.AddWithValue("@Username", Username);

//                        object result = command.ExecuteScalar();

//                        // Check if a record was found
//                        if (result != null)
//                        {
//                            return result.ToString();
//                        }
//                        else
//                        {
//                            // User not found
//                            return null;
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return null;
//                }
//            }
//        }

//        private bool VerifyPassword(string enteredPassword, string db_Password)
//        {
//            // Compare the entered password with the database password
//            return string.Equals(enteredPassword, db_Password, StringComparison.OrdinalIgnoreCase);
//        }

//        private void ShowPasswordcb_CheckedChanged_1(object sender, EventArgs e)
//        {
//            if (ShowPasswordcb.Checked)
//            {
//                passwordtb.UseSystemPasswordChar = false;
//            }
//            else
//            {
//                passwordtb.UseSystemPasswordChar = true;
//            }
//        }
//        private void btnlogin_Click(object sender, EventArgs e)
//        {
//            string Username = usernametb.Text;
//            string Password = passwordtb.Text;

//            string storedPassword = GetPasswordFromDatabase(Username);

//            if (storedPassword != null)
//            {

//                // Verify the entered password against the database password
//                bool passwordMatch = VerifyPassword(Password, storedPassword);

//                if (passwordMatch)
//                {
//                    MessageBox.Show("Login Successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    this.Hide();
//                    HomePage FrmHome = new HomePage();
//                    FrmHome.Show();
//                }
//                else
//                {
//                    MessageBox.Show("Login Failed. Invalid username or password", "FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//            else
//            {
//                MessageBox.Show("Login Failed. User not found!", "FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void clearlbl_Click_1(object sender, EventArgs e)
//        {
//            usernametb.Text = "";
//            passwordtb.Text = "";
//        }

//        private void btnBack_Click_1(object sender, EventArgs e)
//        {
//            this.Hide();
//            SignUpForm signUpForm = new SignUpForm();
//            signUpForm.ShowDialog();
//        }

//        // X BUTTON
//        private void button1_Click(object sender, EventArgs e)
//        {
//            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//            {
//                Application.Exit();
//            }
//        }
//    }
//}

{
    public partial class LoginForm : Form
    {
        private string connectionString = "Server=localhost;Database=db_ricemillmanagement;User ID=root;Password=;";

        public LoginForm()
        {
            InitializeComponent();
        }

        // Method to fetch the password from the database based on the username
        private string GetPasswordFromDatabase(string Username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to fetch the password from the database for the given username
                    string retrievePasswordQuery = "SELECT Password FROM tbl_users WHERE Username = @Username";

                    using (MySqlCommand command = new MySqlCommand(retrievePasswordQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", Username);

                        object result = command.ExecuteScalar();

                        // Return password if found, else return null
                        return result != null ? result.ToString() : null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        // Method to verify the entered password matches the stored password (case-sensitive)
        private bool VerifyPassword(string enteredPassword, string dbPassword)
        {
            // Return true if both passwords match exactly (case-sensitive comparison)
            return string.Equals(enteredPassword, dbPassword, StringComparison.Ordinal);
        }

        private void ShowPasswordcb_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ShowPasswordcb.Checked)
            {
                passwordtb.UseSystemPasswordChar = false; // Show the password
            }
            else
            {
                passwordtb.UseSystemPasswordChar = true; // Hide the password
            }
        }

        // Method triggered when the user clicks the login button
        private void btnlogin_Click(object sender, EventArgs e)
        {
            string Username = usernametb.Text;
            string Password = passwordtb.Text;

            // Fetch the password stored in the database for the provided username
            string storedPassword = GetPasswordFromDatabase(Username);

            if (storedPassword != null)
            {
                // Verify the entered password against the stored password (case-sensitive)
                bool passwordMatch = VerifyPassword(Password, storedPassword);

                if (passwordMatch)
                {
                    MessageBox.Show("Login Successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    HomePage FrmHome = new HomePage();
                    FrmHome.Show();
                }
                else
                {
                    MessageBox.Show("Login Failed. Invalid username or password", "FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Login Failed. User not found!", "FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method triggered to clear the input fields
        private void clearlbl_Click_1(object sender, EventArgs e)
        {
            usernametb.Text = "";
            passwordtb.Text = "";
        }

        // Method triggered to navigate back to the SignUpForm
        private void btnBack_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            SignUpForm signUpForm = new SignUpForm();
            signUpForm.ShowDialog();
        }

        // Close button to exit the application
        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void usernametb_TextChanged(object sender, EventArgs e)
        {

        }

        private void passwordtb_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
 