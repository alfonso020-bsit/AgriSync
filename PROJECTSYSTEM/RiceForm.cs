using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PROJECTSYSTEM
{
    public partial class RiceForm : Form
    {
        private string connectionString = "Server=localhost;Database=db_ricemillmanagement;User ID=root;";
        private Stack<ActionHistory> actionHistoryStack = new Stack<ActionHistory>();

        public RiceForm()
        {
            InitializeComponent();
            lvRice.SelectedIndexChanged += lvRice_SelectedIndexChanged;
        }
        static void InsertFactoryData(string connectionString, int numberOfRows)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    for (int i = 0; i < numberOfRows; i++)
                    {
                        // Generate sample data (adjust as needed)
                        string VARIETYNAME = $"value1_{i}";
                        int QUANTITYPERSACK = Convert.ToInt32($"value2_{i}");
                        int PRICEPERSACK = Convert.ToInt32($"value3_{i}");

                        // Insert data into the table using parameterized query
                        string insertQuery = "INSERT INTO tbl_rice (VARIETYNAME, QUANTITYPERSACK, PRICEPERSACK) VALUES (@VARIETYNAME, @QUANTITYPERSACK, @PRICEPERSACK)";


                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                        {
                            // Add parameters and their values
                            command.Parameters.AddWithValue("@VARIETYNAME", VARIETYNAME);
                            command.Parameters.AddWithValue("@QUANTITYPERSACK", QUANTITYPERSACK);
                            command.Parameters.AddWithValue("@PRICEPERSACK", PRICEPERSACK);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LoadRice()
        {
            lvRice.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM tbl_rice";

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                                item.SubItems.Add(reader["VARIETYNAME"].ToString());
                                item.SubItems.Add(reader["QUANTITYPERSACK"].ToString());
                                item.SubItems.Add(reader["PRICEPERSACK"].ToString());

                                lvRice.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string VARIETYNAME = cmbVName.Text;
            string QUANTITYPERSACK = txtQuantity.Text;
            string PRICEPERSACK = txtPrice.Text;

            // Check for empty fields
            if (string.IsNullOrWhiteSpace(VARIETYNAME) || string.IsNullOrWhiteSpace(QUANTITYPERSACK) || string.IsNullOrWhiteSpace(PRICEPERSACK))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Validate that QUANTITYPERSACK and PRICEPERSACK are numeric
            if (!int.TryParse(QUANTITYPERSACK, out int quantity) || !int.TryParse(PRICEPERSACK, out int price))
            {
                MessageBox.Show("Quantity and Price must be numeric values.");
                return;
            }

            // Ensure quantity and price are not negative
            if (quantity < 0 || price < 0)
            {
                MessageBox.Show("Quantity and Price cannot be negative.");
                return;
            }

            // Check if the variety name already exists
            if (CheckIfVarietyExists(VARIETYNAME))
            {
                MessageBox.Show("This variety name already exists. Please choose a different name.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm add action
            if (MessageBox.Show("Are you sure you want to add this record?", "Confirm Add", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string addRiceQuery = "INSERT INTO tbl_rice (VARIETYNAME, QUANTITYPERSACK, PRICEPERSACK) VALUES (@VARIETYNAME, @QUANTITYPERSACK, @PRICEPERSACK)";

                        using (MySqlCommand command = new MySqlCommand(addRiceQuery, connection))
                        {
                            // Add parameters
                            command.Parameters.AddWithValue("@VARIETYNAME", VARIETYNAME);
                            command.Parameters.AddWithValue("@QUANTITYPERSACK", quantity);
                            command.Parameters.AddWithValue("@PRICEPERSACK", price);

                            // Execute the command
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                long newId = command.LastInsertedId; // Get the ID of the newly added record
                                actionHistoryStack.Push(new ActionHistory("Add", VARIETYNAME, newId.ToString())); // Store the ID
                                MessageBox.Show("Variety added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadRice();
                            }
                            else
                            {
                                MessageBox.Show("Failed to add the Variety.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private bool CheckIfVarietyExists(string varietyName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM tbl_rice WHERE VARIETYNAME = @VARIETYNAME";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VARIETYNAME", varietyName);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private void lvRice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvRice.SelectedItems.Count > 0)
            {

                if (lvRice.SelectedItems.Count > 0)
                {
                    // Get the selected item.
                    ListViewItem selectedItem = lvRice.SelectedItems[0];

                    // Display the text from subitems in the TextBoxes.
                    txtID.Text = selectedItem.SubItems[0].Text;
                    cmbVName.Text = selectedItem.SubItems[1].Text;
                    txtQuantity.Text = selectedItem.SubItems[2].Text;
                    txtPrice.Text = selectedItem.SubItems[3].Text;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Ensure necessary fields are filled out
            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(cmbVName.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Please Fill Out Necessary Information.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate that QUANTITYPERSACK and PRICEPERSACK are numeric
            if (!int.TryParse(txtQuantity.Text, out int quantity) || !int.TryParse(txtPrice.Text, out int price))
            {
                MessageBox.Show("Quantity and Price must be numeric values.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ensure quantity and price are not negative
            if (quantity < 0 || price < 0)
            {
                MessageBox.Show("Quantity and Price cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to update this record?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string updateQuery = "UPDATE tbl_rice SET VARIETYNAME = @VARIETYNAME, QUANTITYPERSACK = @QUANTITYPERSACK, PRICEPERSACK = @PRICEPERSACK WHERE ID = @ID";

                        // Store previous values for undo
                        string previousData = $"{txtID.Text}|{cmbVName.Text}|{txtQuantity.Text}|{txtPrice.Text}";

                        using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@VARIETYNAME", cmbVName.Text);
                            command.Parameters.AddWithValue("@QUANTITYPERSACK", quantity);
                            command.Parameters.AddWithValue("@PRICEPERSACK", price);
                            command.Parameters.AddWithValue("@ID", txtID.Text);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                actionHistoryStack.Push(new ActionHistory("Update", previousData, txtID.Text)); // Store the ID
                                MessageBox.Show("Variety updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadRice();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update the variety.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvRice.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvRice.SelectedItems[0];
                string varietyName = selectedItem.SubItems[1].Text;
                string quantity = selectedItem.SubItems[2].Text;
                string price = selectedItem.SubItems[3].Text;

                if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            string deleteQuery = "DELETE FROM tbl_rice WHERE ID = @ID";
                            using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                            {
                                command.Parameters.AddWithValue("@ID", selectedItem.Text);

                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    // Push action to stack with all necessary data
                                    actionHistoryStack.Push(new ActionHistory("Delete", $"{varietyName}|{quantity}|{price}", selectedItem.Text)); // Store the ID
                                    MessageBox.Show("Variety deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadRice();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to delete the variety.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a variety to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            StocksForm stckfrm = new StocksForm();
            stckfrm.ShowDialog();
        }

        //X BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void Ricetimer_Tick(object sender, EventArgs e)
        {
            LoadRice();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchKeyword = txtSearch.Text.Trim().ToLower();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = $"SELECT * FROM tbl_rice WHERE VARIETYNAME LIKE '%{searchKeyword}%'";

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        lvRice.Items.Clear();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                                item.SubItems.Add(reader["VARIETYNAME"].ToString());
                                item.SubItems.Add(reader["QUANTITYPERSACK"].ToString());
                                item.SubItems.Add(reader["PRICEPERSACK"].ToString());

                                lvRice.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

            }
        }

        private void btnLoadRecords_Click(object sender, EventArgs e)
        {
            LoadRice();
        }

        private void undo_Click(object sender, EventArgs e)
        {
            if (actionHistoryStack.Count > 0)
            {
                var lastAction = actionHistoryStack.Pop();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        if (lastAction.ActionType == "Add")
                        {
                            // Undo the addition by deleting the last added record
                            string deleteQuery = "DELETE FROM tbl_rice WHERE ID = @ID";
                            using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                            {
                                command.Parameters.AddWithValue("@ID", lastAction.RecordId);

                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Undo Add: Variety deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadRice();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to undo theadd action.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else if (lastAction.ActionType == "Update")
                        {
                            // Undo the update by reverting to the old values
                            string[] previousData = lastAction.Description.Split('|');
                            if (previousData.Length == 4) // Ensure data includes ID and previous values
                            {
                                string recordId = previousData[0];
                                string prevVarietyName = previousData[1];
                                string prevQuantity = previousData[2];
                                string prevPrice = previousData[3];

                                string updateQuery = "UPDATE tbl_rice SET VARIETYNAME = @VARIETYNAME, QUANTITYPERSACK = @QUANTITYPERSACK, PRICEPERSACK = @PRICEPERSACK WHERE ID = @ID";

                                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@VARIETYNAME", prevVarietyName);
                                    command.Parameters.AddWithValue("@QUANTITYPERSACK", Convert.ToInt32(prevQuantity));
                                    command.Parameters.AddWithValue("@PRICEPERSACK", Convert.ToInt32(prevPrice));
                                    command.Parameters.AddWithValue("@ID", recordId);

                                    int rowsAffected = command.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        MessageBox.Show("Undo Update: Variety updated successfully to previous values.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        LoadRice();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to undo the update action.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid data for undoing the update action.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (lastAction.ActionType == "Delete")
                        {
                            // Undo the delete by adding the record back
                            string[] deletedData = lastAction.Description.Split('|');
                            if (deletedData.Length == 3) // Ensure data includes all required fields
                            {
                                string deletedVarietyName = deletedData[0];
                                string deletedQuantity = deletedData[1];
                                string deletedPrice = deletedData[2];

                                string insertQuery = "INSERT INTO tbl_rice (VARIETYNAME, QUANTITYPERSACK, PRICEPERSACK) VALUES (@VARIETYNAME, @QUANTITYPERSACK, @PRICEPERSACK)";
                                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@VARIETYNAME", deletedVarietyName);
                                    command.Parameters.AddWithValue("@QUANTITYPERSACK", Convert.ToInt32(deletedQuantity));
                                    command.Parameters.AddWithValue("@PRICEPERSACK", Convert.ToInt32(deletedPrice));

                                    int rowsAffected = command.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        MessageBox.Show("Undo Delete: Variety restored successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        LoadRice();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to undo the delete action.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid data for undoing the delete action.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No actions to undo.", "Undo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private class ActionHistory
        {
            public string ActionType { get; }
            public string Description { get; }
            public string RecordId { get; } // Store the ID for undo operations

            public ActionHistory(string actionType, string description, string recordId = null)
            {
                ActionType = actionType;
                Description = description;
                RecordId = recordId; // Store the ID if available
            }
        }

        private void RiceForm_Load(object sender, EventArgs e)
        {

        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtQuantity.Text, out int quantity))
            {
                if (quantity < 0)
                {
                    MessageBox.Show("Quantity cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuantity.Clear();
                }
            }
            else if (!string.IsNullOrEmpty(txtQuantity.Text))
            {
                MessageBox.Show("Please enter a valid numeric value for Quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Clear();
            }

        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtPrice.Text, out int price))
            {
                if (price < 0)
                {
                    MessageBox.Show("Price cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Clear();
                }
            }
            else if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                MessageBox.Show("Please enter a valid numeric value for Price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Clear();
            }

        }
    }
}
