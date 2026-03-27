//using MySql.Data.MySqlClient;

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace PROJECTSYSTEM
//{
//    public partial class DarakPinlidForm : Form
//    {
//        private string connectionString = "Server=localhost;Database=db_ricemillmanagement;User ID=root;";
//        public DarakPinlidForm()
//        {
//            InitializeComponent();
//            lvDarakPinlid.SelectedIndexChanged += lvDarakPinlid_SelectedIndexChanged;
//        }
//        static void InsertFactoryData(string connectionString, string productName, int quantityPerSack, int pricePerSack)
//        {
//            try
//            {
//                using (MySqlConnection connection = new MySqlConnection(connectionString))
//                {
//                    connection.Open();

//                    string insertQuery = "INSERT INTO tbl_darakpinlid (PRODUCTNAME, QUANTITYPERSACK, PRICEPERSACK) VALUES (@PRODUCTNAME, @QUANTITYPERSACK, @PRICEPERSACK)";

//                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
//                    {
//                        command.Parameters.AddWithValue("@PRODUCTNAME", productName);
//                        command.Parameters.AddWithValue("@QUANTITYPERSACK", quantityPerSack);
//                        command.Parameters.AddWithValue("@PRICEPERSACK", pricePerSack);

//                        command.ExecuteNonQuery();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }
//        private void LoadDarakPinlid()
//        {
//            lvDarakPinlid.Items.Clear();

//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    string selectQuery = "SELECT * FROM tbl_darakpinlid";

//                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
//                    {
//                        using (MySqlDataReader reader = command.ExecuteReader())
//                        {
//                            while (reader.Read())
//                            {
//                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
//                                item.SubItems.Add(reader["PRODUCTNAME"].ToString());
//                                item.SubItems.Add(reader["QUANTITYPERSACK"].ToString());
//                                item.SubItems.Add(reader["PRICEPERSACK"].ToString());

//                                lvDarakPinlid.Items.Add(item);
//                            }
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Error: " + ex.Message);
//                }
//            }
//        }

//        private void btnBack_Click(object sender, EventArgs e)
//        {
//            this.Hide();
//            StocksForm stckfrm = new StocksForm();
//            stckfrm.ShowDialog();
//        }

//        private void button2_Click(object sender, EventArgs e)
//        {
//            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//            {
//                Application.Exit();
//            }
//        }

//        private void btnSearch_Click(object sender, EventArgs e)
//        {
//            string searchKeyword = txtSearch.Text.Trim().ToLower();

//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    string selectQuery = $"SELECT * FROM tbl_darakpinlid WHERE PRODUCTNAME LIKE '%{searchKeyword}%'";

//                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
//                    {
//                        lvDarakPinlid.Items.Clear();

//                        using (MySqlDataReader reader = command.ExecuteReader())
//                        {
//                            while (reader.Read())
//                            {
//                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
//                                item.SubItems.Add(reader["PRODUCTNAME"].ToString());
//                                item.SubItems.Add(reader["QUANTITYPERSACK"].ToString());
//                                item.SubItems.Add(reader["PRICEPERSACK"].ToString());

//                                lvDarakPinlid.Items.Add(item);
//                            }
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Error: " + ex.Message);
//                }

//            }
//        }

//        private void btnAdd_Click(object sender, EventArgs e)
//        {
//            string PRODUCTNAME = cmbPName.Text;
//            string QUANTITYPERSACK = txtQuantity.Text;
//            string PRICEPERSACK = txtPrice.Text;

//            if (!string.IsNullOrWhiteSpace(PRODUCTNAME) && !string.IsNullOrWhiteSpace(QUANTITYPERSACK) && !string.IsNullOrWhiteSpace(PRICEPERSACK))
//            {
//                try
//                {
//                    InsertFactoryData(connectionString, PRODUCTNAME, Convert.ToInt32(QUANTITYPERSACK), Convert.ToInt32(PRICEPERSACK));

//                    MessageBox.Show("Product added successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    LoadDarakPinlid();
//                    txtQuantity.Clear();
//                    txtPrice.Clear();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//            else
//            {
//                MessageBox.Show("Please fill in all the fields", "FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void lvDarakPinlid_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (lvDarakPinlid.SelectedItems.Count > 0)
//            {
//                if (lvDarakPinlid.SelectedItems.Count > 0)
//                {
//                    // Get the selected item.
//                    ListViewItem selectedItem = lvDarakPinlid.SelectedItems[0];

//                    // Display the text from subitems in the TextBoxes.
//                    txtID.Text = selectedItem.SubItems[0].Text;
//                    cmbPName.Text = selectedItem.SubItems[1].Text;
//                    txtQuantity.Text = selectedItem.SubItems[2].Text;
//                    txtPrice.Text = selectedItem.SubItems[3].Text;
//                }
//            }
//        }

//        private void btnUpdate_Click(object sender, EventArgs e)
//        {
//            // Check if all necessary fields are filled out
//            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(cmbPName.Text) ||
//                string.IsNullOrWhiteSpace(txtQuantity.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
//            {
//                MessageBox.Show("Please Fill Out Necessary Information.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return; // Exit the method if validation fails
//            }

//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // Update data in a table
//                    string updateQuery = "UPDATE tbl_darakpinlid SET PRODUCTNAME = @PRODUCTNAME, QUANTITYPERSACK = @QUANTITYPERSACK, PRICEPERSACK = @PRICEPERSACK WHERE ID = @ID";

//                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
//                    {
//                        // Replace parameter values with actual data
//                        command.Parameters.AddWithValue("@PRODUCTNAME", cmbPName.Text);
//                        command.Parameters.AddWithValue("@QUANTITYPERSACK", Convert.ToInt32(txtQuantity.Text));
//                        command.Parameters.AddWithValue("@PRICEPERSACK", Convert.ToInt32(txtPrice.Text));
//                        command.Parameters.AddWithValue("@ID", txtID.Text); // Specify the ID of the row to update

//                        // Execute the command
//                        int rowsAffected = command.ExecuteNonQuery();

//                        if (rowsAffected > 0)
//                        {
//                            MessageBox.Show("Product updated successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                            LoadDarakPinlid();
//                            txtQuantity.Clear();
//                            txtPrice.Clear();
//                        }
//                        else
//                        {
//                            MessageBox.Show("Failed to update", "FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//        }

//        private void btnDelete_Click(object sender, EventArgs e)
//        {
//            // Check if the necessary field is filled out
//            if (string.IsNullOrWhiteSpace(txtID.Text))
//            {
//                MessageBox.Show("Please Fill Out Necessary Information.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return; // Exit the method if validation fails
//            }

//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // Delete data from a table
//                    // Corrected DELETE query
//                    string deleteQuery = "DELETE FROM tbl_darakpinlid WHERE ID = @ID";

//                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
//                    {
//                        // Replace parameter values with actual data
//                        command.Parameters.AddWithValue("@ID", txtID.Text); // Specify the ID of the row to delete

//                        // Execute the command
//                        int rowsAffected = command.ExecuteNonQuery();

//                        if (rowsAffected > 0)
//                        {
//                            MessageBox.Show("Product deleted successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                            LoadDarakPinlid();
//                            txtQuantity.Clear();
//                            txtPrice.Clear();
//                        }
//                        else
//                        {
//                            MessageBox.Show("Failed to delete", "FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//        }

//        private void DarakTimer_Tick(object sender, EventArgs e)
//        {
//            LoadDarakPinlid();
//        }

//        private void undoDarakPinlid_Click(object sender, EventArgs e)
//        {

//        }
//    }
//}
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PROJECTSYSTEM
{
    public partial class DarakPinlidForm : Form
    {
        private string connectionString = "Server=localhost;Database=db_ricemillmanagement;User ID=root;";
        private Stack<ActionData> actionHistory = new Stack<ActionData>();

        public DarakPinlidForm()
        {
            InitializeComponent();
            lvDarakPinlid.SelectedIndexChanged += lvDarakPinlid_SelectedIndexChanged;
            btnLoadDarakPinlid.Click += btnLoadDarakPinlid_Click;
        }

        // Action data to keep track of the operation
        private class ActionData
        {
            public string Operation { get; set; }
            public int ID { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public int Price { get; set; }
        }

        // Method to insert data
        static void InsertFactoryData(string connectionString, string productName, int quantityPerSack, int pricePerSack)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO tbl_darakpinlid (PRODUCTNAME, QUANTITYPERSACK, PRICEPERSACK) VALUES (@PRODUCTNAME, @QUANTITYPERSACK, @PRICEPERSACK)";
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@PRODUCTNAME", productName);
                    command.Parameters.AddWithValue("@QUANTITYPERSACK", quantityPerSack);
                    command.Parameters.AddWithValue("@PRICEPERSACK", pricePerSack);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Load data from the database
        private void LoadDarakPinlid()
        {
            lvDarakPinlid.Items.Clear();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM tbl_darakpinlid";
                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListViewItem item = new ListViewItem(reader["ID"].ToString());
                            item.SubItems.Add(reader["PRODUCTNAME"].ToString());
                            item.SubItems.Add(reader["QUANTITYPERSACK"].ToString());
                            item.SubItems.Add(reader["PRICEPERSACK"].ToString());
                            lvDarakPinlid.Items.Add(item);
                        }
                    }
                }
            }
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            StocksForm stckfrm = new StocksForm();
            stckfrm.ShowDialog();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchKeyword = txtSearch.Text.Trim().ToLower();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = $"SELECT * FROM tbl_darakpinlid WHERE PRODUCTNAME LIKE '%{searchKeyword}%'";

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        lvDarakPinlid.Items.Clear();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                                item.SubItems.Add(reader["PRODUCTNAME"].ToString());
                                item.SubItems.Add(reader["QUANTITYPERSACK"].ToString());
                                item.SubItems.Add(reader["PRICEPERSACK"].ToString());

                                lvDarakPinlid.Items.Add(item);
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
        private void DarakTimer_Tick(object sender, EventArgs e)
        {
            LoadDarakPinlid();
        }

        // Add action
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string PRODUCTNAME = cmbPName.Text;
            string QUANTITYPERSACK = txtQuantity.Text;
            string PRICEPERSACK = txtPrice.Text;

            if (!string.IsNullOrWhiteSpace(PRODUCTNAME) && !string.IsNullOrWhiteSpace(QUANTITYPERSACK) && !string.IsNullOrWhiteSpace(PRICEPERSACK))
            {
                try
                {
                    // Add new product
                    InsertFactoryData(connectionString, PRODUCTNAME, Convert.ToInt32(QUANTITYPERSACK), Convert.ToInt32(PRICEPERSACK));

                    // Record the add action for undo
                    actionHistory.Push(new ActionData
                    {
                        Operation = "Add",
                        ProductName = PRODUCTNAME,
                        Quantity = Convert.ToInt32(QUANTITYPERSACK),
                        Price = Convert.ToInt32(PRICEPERSACK)
                    });

                    MessageBox.Show("Product added successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDarakPinlid();
                    txtQuantity.Clear();
                    txtPrice.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please fill in all the fields", "FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Update action
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(cmbPName.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Please Fill Out Necessary Information.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM tbl_darakpinlid WHERE ID = @ID";
                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ID", txtID.Text);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Store the current values for undo
                                actionHistory.Push(new ActionData
                                {
                                    Operation = "Update",
                                    ID = Convert.ToInt32(txtID.Text),
                                    ProductName = reader["PRODUCTNAME"].ToString(),
                                    Quantity = Convert.ToInt32(reader["QUANTITYPERSACK"]),
                                    Price = Convert.ToInt32(reader["PRICEPERSACK"])
                                });
                            }
                        }
                    }

                    string updateQuery = "UPDATE tbl_darakpinlid SET PRODUCTNAME = @PRODUCTNAME, QUANTITYPERSACK = @QUANTITYPERSACK, PRICEPERSACK = @PRICEPERSACK WHERE ID = @ID";
                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@PRODUCTNAME", cmbPName.Text);
                        command.Parameters.AddWithValue("@QUANTITYPERSACK", Convert.ToInt32(txtQuantity.Text));
                        command.Parameters.AddWithValue("@PRICEPERSACK", Convert.ToInt32(txtPrice.Text));
                        command.Parameters.AddWithValue("@ID", txtID.Text);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Product updated successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDarakPinlid();
                    txtQuantity.Clear();
                    txtPrice.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Delete action
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Please Fill Out Necessary Information.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM tbl_darakpinlid WHERE ID = @ID";
                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ID", txtID.Text);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Store the deleted data for undo
                                actionHistory.Push(new ActionData
                                {
                                    Operation = "Delete",
                                    ID = Convert.ToInt32(txtID.Text),
                                    ProductName = reader["PRODUCTNAME"].ToString(),
                                    Quantity = Convert.ToInt32(reader["QUANTITYPERSACK"]),
                                    Price = Convert.ToInt32(reader["PRICEPERSACK"])
                                });
                            }
                        }
                    }

                    string deleteQuery = "DELETE FROM tbl_darakpinlid WHERE ID = @ID";
                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ID", txtID.Text);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Product deleted successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDarakPinlid();
                    txtQuantity.Clear();
                    txtPrice.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Undo action
        private void undoDarakPinlid_Click(object sender, EventArgs e)
        {
            if (actionHistory.Count > 0)
            {
                ActionData lastAction = actionHistory.Pop();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    if (lastAction.Operation == "Add")
                    {
                        // Undo Add: Delete the last added record
                        string deleteQuery = "DELETE FROM tbl_darakpinlid WHERE PRODUCTNAME = @PRODUCTNAME AND QUANTITYPERSACK = @QUANTITYPERSACK AND PRICEPERSACK = @PRICEPERSACK";
                        using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@PRODUCTNAME", lastAction.ProductName);
                            command.Parameters.AddWithValue("@QUANTITYPERSACK", lastAction.Quantity);
                            command.Parameters.AddWithValue("@PRICEPERSACK", lastAction.Price);
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (lastAction.Operation == "Update")
                    {
                        // Undo Update: Restore previous values
                        string updateQuery = "UPDATE tbl_darakpinlid SET PRODUCTNAME = @PRODUCTNAME, QUANTITYPERSACK = @QUANTITYPERSACK, PRICEPERSACK = @PRICEPERSACK WHERE ID = @ID";
                        using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@PRODUCTNAME", lastAction.ProductName);
                            command.Parameters.AddWithValue("@QUANTITYPERSACK", lastAction.Quantity);
                            command.Parameters.AddWithValue("@PRICEPERSACK", lastAction.Price);
                            command.Parameters.AddWithValue("@ID", lastAction.ID);
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (lastAction.Operation == "Delete")
                    {
                        // Undo Delete: Re-insert the deleted product
                        string insertQuery = "INSERT INTO tbl_darakpinlid (PRODUCTNAME, QUANTITYPERSACK, PRICEPERSACK) VALUES (@PRODUCTNAME, @QUANTITYPERSACK, @PRICEPERSACK)";
                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@PRODUCTNAME", lastAction.ProductName);
                            command.Parameters.AddWithValue("@QUANTITYPERSACK", lastAction.Quantity);
                            command.Parameters.AddWithValue("@PRICEPERSACK", lastAction.Price);
                            command.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Undo operation completed.", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDarakPinlid();
            }
            else
            {
                MessageBox.Show("No action to undo.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Item selected from ListView
        private void lvDarakPinlid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvDarakPinlid.SelectedItems.Count > 0)
            {
                ListViewItem item = lvDarakPinlid.SelectedItems[0];
                txtID.Text = item.Text;
                cmbPName.Text = item.SubItems[1].Text;
                txtQuantity.Text = item.SubItems[2].Text;
                txtPrice.Text = item.SubItems[3].Text;
            }
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

        private void btnLoadDarakPinlid_Click(object sender, EventArgs e)
        {
            LoadDarakPinlid();
        }
    }
}
