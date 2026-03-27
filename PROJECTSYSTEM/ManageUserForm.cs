using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace PROJECTSYSTEM
{
    public partial class ManageUserForm : Form
    {
        private string connectionString = "Server=localhost;Database=db_ricemillmanagement;User ID=root;";
        public ManageUserForm()
        {
            InitializeComponent();
            panel3.Hide();
        }
        private string GetPasswordFromDatabase(string username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string retrievePasswordQuery = "SELECT Password FROM tbl_users WHERE Username = @Username";

                    using (MySqlCommand command = new MySqlCommand(retrievePasswordQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            return result.ToString();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }
        private void btnCheck_Click(object sender, EventArgs e)
        {
            string currentUsername = txtCurrentUsername.Text;
            string enteredPassword = txtCPassword.Text;
            string dbPassword = GetPasswordFromDatabase(currentUsername);

            if (VerifyPassword(enteredPassword, dbPassword))
            {
                // Old password is correct, show controls for password change
                panel3.Show();
                txtCPassword.Clear();
                txtCurrentUsername.Clear();
            }
            else
            {
                Console.WriteLine($"Entered Password: '{enteredPassword}'");
                Console.WriteLine($"DB Password: '{dbPassword}'");
                // Old password is incorrect, show an error message or take appropriate action
                MessageBox.Show("Incorrect old password. Please try again.");
            }
        }

        private bool VerifyPassword(string enteredPassword, string dbPassword)
        {
            return string.Equals(enteredPassword, dbPassword, StringComparison.OrdinalIgnoreCase);
        }
        private bool UpdateUserData(string username, string newUsername, string newFirstName, string newLastName, string newPassword)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE tbl_users SET Username = @NewUsername, FirstName = @NewFirstName, LastName = @NewLastName, Password = @NewPassword WHERE Username = @Username";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NewUsername", newUsername);
                        command.Parameters.AddWithValue("@NewFirstName", newFirstName);
                        command.Parameters.AddWithValue("@NewLastName", newLastName);
                        command.Parameters.AddWithValue("@NewPassword", newPassword);
                        command.Parameters.AddWithValue("@Username", username);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
       

        private void btnUpdatePass_Click(object sender, EventArgs e)
        {

            // Check if all required fields are filled out
            if (string.IsNullOrEmpty(txtUsernameMU.Text) || string.IsNullOrEmpty(txtFirstName.Text) ||
                string.IsNullOrEmpty(txtLastName.Text) || string.IsNullOrEmpty(txtNPassword.Text) ||
                string.IsNullOrEmpty(txtConfirmPass.Text))
            {
                MessageBox.Show("Please Fill Out the needed Information.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the method if validation fails
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Update data in a table
                    string updateQuery = "UPDATE tbl_users SET Username = @newUsername, FirstName = @newFirstName, LastName = @newLastName, Password = @newPassword";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        // Replace parameter values with actual data
                        command.Parameters.AddWithValue("@newUsername", txtUsernameMU.Text);
                        command.Parameters.AddWithValue("@newFirstName", txtFirstName.Text);
                        command.Parameters.AddWithValue("@newLastName", txtLastName.Text);
                        command.Parameters.AddWithValue("@newPassword", txtNPassword.Text);

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Profile updated successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtUsernameMU.Clear();
                            txtFirstName.Clear();
                            txtLastName.Clear();
                            txtNPassword.Clear();
                            txtConfirmPass.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update", "FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            HomePage homePage = new HomePage();
            homePage.Show();
        }

        //X BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void ShowPassword_Click(object sender, EventArgs e)
        {
            txtCPassword.UseSystemPasswordChar = !txtCPassword.UseSystemPasswordChar;
        }

        private void ShowNPassword_Click(object sender, EventArgs e)
        {
            txtNPassword.UseSystemPasswordChar = !txtNPassword.UseSystemPasswordChar;
        }

        private void ShowCPassword_Click(object sender, EventArgs e)
        {
            txtConfirmPass.UseSystemPasswordChar = !txtConfirmPass.UseSystemPasswordChar;
        }
    }
}


  