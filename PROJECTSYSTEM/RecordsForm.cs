using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJECTSYSTEM
{
    public partial class RecordsForm : Form
    {
        private string connectionString = "Server=localhost;Database=db_ricemillmanagement;User ID=root;";
        private bool isPrinting = false;
        public RecordsForm()
        {
            InitializeComponent();
            LoadProducts();

            txtQuantity.TextChanged += TextBox_TextChanged;
            txtPrice.TextChanged += TextBox_TextChanged;
            lvProducts.SelectedIndexChanged += lvProducts_SelectedIndexChanged;
            btnPrint.Click += btnPrint_Click;
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
                        string CUSTOMERNAME = $"value1_{i}";
                        string PRODUCTNAME = $"value2_{i}";
                        string VARIETY = $"value3_{i}";
                        string TRANSACTIONTYPE = $"value4_{i}";
                        int QUANTITY = Convert.ToInt32($"value5_{i}");
                        int PRICE = Convert.ToInt32($"value6_{i}");
                       

                        double TOTALPRICE = Convert.ToDouble(QUANTITY * PRICE);

                        // Insert data into the table using parameterized query
                        string insertQuery = "INSERT INTO tbl_products (CUSTOMERNAME, PRODUCTNAME, VARIETY, TRANSACTIONTYPE, QUNATITY, PRICE, TOTALPRICE) VALUES (,@CUSTOMERNAME, @PRODUCTNAME, @VARIETY, @TRANSACTIONTYPE, @QUANTITY, @PRICE, @TOTALPRICE)";

                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                        {
                            // Add parameters and their values
                            command.Parameters.AddWithValue("@CUSTOMERNAME", CUSTOMERNAME);
                            command.Parameters.AddWithValue("@PRODUCTNAME", PRODUCTNAME);
                            command.Parameters.AddWithValue("@VARIETY", VARIETY);
                            command.Parameters.AddWithValue("@TRANSACTIONTYPE", TRANSACTIONTYPE);
                            command.Parameters.AddWithValue("@QUANTITY", QUANTITY);
                            command.Parameters.AddWithValue("@PRICE", PRICE);
                            command.Parameters.AddWithValue("@TOTALPRICE", TOTALPRICE);


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
        private void LoadProducts()
        {
            lvProducts.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = "SELECT ID, CUSTOMERNAME, PRODUCTNAME, VARIETY,TRANSACTIONTYPE, QUANTITY, PRICE, TOTALPRICE, DATE_FORMAT(DATE, '%Y-%m-%d %H:%i:%s') AS DATE FROM tbl_products";

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                                item.SubItems.Add(reader["CUSTOMERNAME"].ToString());
                                item.SubItems.Add(reader["PRODUCTNAME"].ToString());
                                item.SubItems.Add(reader["VARIETY"].ToString());
                                item.SubItems.Add(reader["TRANSACTIONTYPE"].ToString());
                                item.SubItems.Add(reader["QUANTITY"].ToString());
                                item.SubItems.Add(reader["PRICE"].ToString());
                                item.SubItems.Add(reader["TOTALPRICE"].ToString());

                                // Retrieve the formatted date directly as a string
                                string formattedDate = reader["DATE"].ToString();

                                // Use the obtained formatted date
                                item.SubItems.Add(formattedDate);

                                lvProducts.Items.Add(item);
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


        public enum ActionType
        {
            None,
            Add,
            Update,
            Delete
        }

        // Class-level variables for tracking the last action and product details for undo
        private ActionType lastAction;
        private Dictionary<string, object> lastProductData;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string CUSTOMERNAME = txtCName.Text;
            string PRODUCTNAME = txtPName.Text.Trim(); // Trim any extra spaces
            string VARIETY = cmbVariety.Text.Trim();
            string TRANSACTIONTYPE = cmbTransactionType.Text.Trim(); // Trim any extra spaces
            string QUANTITY = txtQuantity.Text;
            string PRICE = txtPrice.Text;
            string TOTALPRICE = txtTotalPrice.Text;

            // Convert the PRODUCTNAME to Title Case for consistent handling
            PRODUCTNAME = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(PRODUCTNAME.ToLower());

            if (!string.IsNullOrWhiteSpace(CUSTOMERNAME) && !string.IsNullOrWhiteSpace(PRODUCTNAME) &&
                !string.IsNullOrWhiteSpace(TRANSACTIONTYPE) && !string.IsNullOrWhiteSpace(QUANTITY) &&
                !string.IsNullOrWhiteSpace(PRICE) && !string.IsNullOrWhiteSpace(TOTALPRICE))
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Add the transaction to tbl_products
                        string AddProductQuery = "INSERT INTO tbl_products (CUSTOMERNAME, PRODUCTNAME, VARIETY, TRANSACTIONTYPE, QUANTITY, PRICE, TOTALPRICE) VALUES (@CUSTOMERNAME, @PRODUCTNAME, @VARIETY, @TRANSACTIONTYPE, @QUANTITY, @PRICE, @TOTALPRICE)";
                        using (MySqlCommand command = new MySqlCommand(AddProductQuery, connection))
                        {
                            command.Parameters.AddWithValue("@CUSTOMERNAME", CUSTOMERNAME);
                            command.Parameters.AddWithValue("@PRODUCTNAME", PRODUCTNAME);
                            command.Parameters.AddWithValue("@VARIETY", VARIETY);
                            command.Parameters.AddWithValue("@TRANSACTIONTYPE", TRANSACTIONTYPE);
                            command.Parameters.AddWithValue("@QUANTITY", Convert.ToInt32(QUANTITY));
                            command.Parameters.AddWithValue("@PRICE", Convert.ToInt32(PRICE));
                            command.Parameters.AddWithValue("@TOTALPRICE", Convert.ToDouble(TOTALPRICE));

                            int rowsAffected = command.ExecuteNonQuery();

                            StringBuilder messageBuilder = new StringBuilder();

                            if (rowsAffected > 0)
                            {
                                messageBuilder.AppendLine("Product added successfully.");

                                // Save the last action and product details for undo
                                lastAction = ActionType.Add; // Track that the last action was an "Add"
                                lastProductData = new Dictionary<string, object>
                        {
                            { "ID", command.LastInsertedId }, // Save the newly added product ID
                            { "CUSTOMERNAME", CUSTOMERNAME },
                            { "PRODUCTNAME", PRODUCTNAME },
                            { "VARIETY", VARIETY },
                            { "TRANSACTIONTYPE", TRANSACTIONTYPE },
                            { "QUANTITY", QUANTITY },
                            { "PRICE", PRICE },
                            { "TOTALPRICE", TOTALPRICE }
                        };

                                // Now deduct the quantity if the TRANSACTIONTYPE is "Sell"
                                if (TRANSACTIONTYPE == "SELL")
                                {
                                    int quantityToDeduct = Convert.ToInt32(QUANTITY);

                                    // Deduct for specific product types (e.g., Rice, Darak, Pinlid)
                                    if (PRODUCTNAME == "Rice")
                                    {
                                        if (!string.IsNullOrWhiteSpace(VARIETY))
                                        {
                                            string DeductRiceQuery = "UPDATE tbl_rice SET QUANTITYPERSACK = QUANTITYPERSACK - @QUANTITY WHERE VARIETYNAME = @VARIETYNAME";
                                            using (MySqlCommand deductCommand = new MySqlCommand(DeductRiceQuery, connection))
                                            {
                                                deductCommand.Parameters.AddWithValue("@QUANTITY", quantityToDeduct);
                                                deductCommand.Parameters.AddWithValue("@VARIETYNAME", VARIETY);
                                                int riceRowsAffected = deductCommand.ExecuteNonQuery();
                                                if (riceRowsAffected > 0)
                                                {
                                                    messageBuilder.AppendLine("Rice quantity deducted successfully.");
                                                }
                                                else
                                                {
                                                    messageBuilder.AppendLine("No matching rice variety found to deduct quantity.");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            messageBuilder.AppendLine("Please select a valid variety for rice.");
                                        }
                                    }
                                    else if (PRODUCTNAME == "Darak" || PRODUCTNAME == "Pinlid")
                                    {
                                        string DeductDarakPinlidQuery = "UPDATE tbl_darakpinlid SET QUANTITYPERSACK = QUANTITYPERSACK - @QUANTITY WHERE PRODUCTNAME = @PRODUCTNAME";
                                        using (MySqlCommand deductCommand = new MySqlCommand(DeductDarakPinlidQuery, connection))
                                        {
                                            deductCommand.Parameters.AddWithValue("@QUANTITY", quantityToDeduct);
                                            deductCommand.Parameters.AddWithValue("@PRODUCTNAME", PRODUCTNAME);
                                            int darakPinlidRowsAffected = deductCommand.ExecuteNonQuery();
                                            if (darakPinlidRowsAffected > 0)
                                            {
                                                messageBuilder.AppendLine($"{PRODUCTNAME} quantity deducted successfully.");
                                            }
                                            else
                                            {
                                                messageBuilder.AppendLine($"No matching {PRODUCTNAME} found to deduct quantity.");
                                            }
                                        }
                                    }

                                    // Refresh product list and clear fields
                                    LoadProducts();
                                    txtCName.Clear();
                                    txtPName.Clear();
                                    txtQuantity.Clear();
                                    txtPrice.Clear();
                                    txtTotalPrice.Clear();
                                }
                            }
                            else
                            {
                                messageBuilder.AppendLine("Failed to add the product.");
                            }

                            // Show a single message box with all messages
                            MessageBox.Show(messageBuilder.ToString(), "Transaction Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Please fill out the needed information", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }








        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    string CUSTOMERNAME = txtCName.Text;
        //    string PRODUCTNAME = txtPName.Text;
        //    string TRANSACTIONTYPE = cmbTransactionType.Text;
        //    string QUANTITY = txtQuantity.Text;
        //    string PRICE = txtPrice.Text;
        //    string TOTALPRICE = txtTotalPrice.Text;


        //    if (!string.IsNullOrWhiteSpace(CUSTOMERNAME) && !string.IsNullOrWhiteSpace(PRODUCTNAME) && !string.IsNullOrWhiteSpace(TRANSACTIONTYPE) && !string.IsNullOrWhiteSpace(QUANTITY) && !string.IsNullOrWhiteSpace(PRICE) && !string.IsNullOrWhiteSpace(TOTALPRICE))
        //    {
        //        using (MySqlConnection connection = new MySqlConnection(connectionString))
        //        {
        //            try
        //            {
        //                connection.Open();

        //                string AddProductQuery = "INSERT INTO tbl_products (CUSTOMERNAME,PRODUCTNAME,TRANSACTIONTYPE,QUANTITY, PRICE, TOTALPRICE) VALUES (@CUSTOMERNAME, @PRODUCTNAME, @TRANSACTIONTYPE, @QUANTITY, @PRICE, @TOTALPRICE)";

        //                using (MySqlCommand command = new MySqlCommand(AddProductQuery, connection))
        //                {
        //                    command.Parameters.AddWithValue("@CUSTOMERNAME", CUSTOMERNAME);
        //                    command.Parameters.AddWithValue("@PRODUCTNAME", PRODUCTNAME);
        //                    command.Parameters.AddWithValue("@TRANSACTIONTYPE", TRANSACTIONTYPE);
        //                    command.Parameters.AddWithValue("@QUANTITY", Convert.ToInt32(QUANTITY));
        //                    command.Parameters.AddWithValue("@PRICE", Convert.ToInt32(PRICE));
        //                    command.Parameters.AddWithValue("@TOTALPRICE", Convert.ToDouble(TOTALPRICE));


        //                    int rowsAffected = command.ExecuteNonQuery();

        //                    if (rowsAffected > 0)
        //                    {
        //                        MessageBox.Show("Product added successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                        LoadProducts();
        //                        txtCName.Clear();
        //                        txtPName.Clear();
        //                        txtQuantity.Clear();
        //                        txtPrice.Clear();
        //                        txtTotalPrice.Clear();
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("Failed to add the product", "FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please fill out the needed informations", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            // Handle the text change and perform mathematical operations
            if (int.TryParse(txtQuantity.Text, out int num1) && int.TryParse(txtPrice.Text, out int num2))
            {
                // Perform the mathematical operation (e.g., addition)
                int result = num1 * num2;

                // Update the third textbox with the result
                txtTotalPrice.Text = result.ToString();
            }
            else
            {
                // Handle invalid input (non-numeric values)
                txtTotalPrice.Text = "";
            }
        }

        private void lvProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvProducts.SelectedItems.Count > 0)
            {

                if (lvProducts.SelectedItems.Count > 0)
                {
                    // Get the selected item.
                    ListViewItem selectedItem = lvProducts.SelectedItems[0];

                    // Display the text from subitems in the TextBoxes.
                    txtID.Text = selectedItem.SubItems[0].Text;
                    txtCName.Text = selectedItem.SubItems[1].Text;
                    txtPName.Text = selectedItem.SubItems[2].Text;
                    cmbVariety.Text = selectedItem.SubItems[3].Text;
                    cmbTransactionType.Text = selectedItem.SubItems[4].Text;
                    txtQuantity.Text = selectedItem.SubItems[5].Text;
                    txtPrice.Text = selectedItem.SubItems[6].Text;
                    txtTotalPrice.Text = selectedItem.SubItems[7].Text;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string CUSTOMERNAME = txtCName.Text;
            string PRODUCTNAME = txtPName.Text.Trim();
            string VARIETY = cmbVariety.Text.Trim();
            string TRANSACTIONTYPE = cmbTransactionType.Text.Trim();
            string QUANTITY = txtQuantity.Text;
            string PRICE = txtPrice.Text;
            string TOTALPRICE = txtTotalPrice.Text;

            // Convert the PRODUCTNAME to Title Case for consistency
            PRODUCTNAME = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(PRODUCTNAME.ToLower());

            if (!string.IsNullOrWhiteSpace(CUSTOMERNAME) && !string.IsNullOrWhiteSpace(PRODUCTNAME) &&
                !string.IsNullOrWhiteSpace(TRANSACTIONTYPE) && !string.IsNullOrWhiteSpace(QUANTITY) &&
                !string.IsNullOrWhiteSpace(PRICE) && !string.IsNullOrWhiteSpace(TOTALPRICE))
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Before updating, retrieve the current product data for undo purposes
                        string selectQuery = "SELECT * FROM tbl_products WHERE ID = @ID";
                        using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                        {
                            selectCommand.Parameters.AddWithValue("@ID", txtID.Text);
                            using (MySqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Save the current product details before the update for undo functionality
                                    lastAction = ActionType.Update; // Track that the last action was an "Update"
                                    lastProductData = new Dictionary<string, object>
                            {
                                { "ID", reader["ID"] },
                                { "CUSTOMERNAME", reader["CUSTOMERNAME"] },
                                { "PRODUCTNAME", reader["PRODUCTNAME"] },
                                { "VARIETY", reader["VARIETY"] },
                                { "TRANSACTIONTYPE", reader["TRANSACTIONTYPE"] },
                                { "QUANTITY", reader["QUANTITY"] },
                                { "PRICE", reader["PRICE"] },
                                { "TOTALPRICE", reader["TOTALPRICE"] }
                            };
                                }
                            }
                        }

                        // Update data in tbl_products
                        string updateQuery = "UPDATE tbl_products SET CUSTOMERNAME = @CUSTOMERNAME, PRODUCTNAME = @PRODUCTNAME, VARIETY = @VARIETY, TRANSACTIONTYPE = @TRANSACTIONTYPE, QUANTITY = @QUANTITY, PRICE = @PRICE, TOTALPRICE = @TOTALPRICE WHERE ID = @ID";

                        using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                        {
                            // Replace parameter values with actual data
                            command.Parameters.AddWithValue("@CUSTOMERNAME", CUSTOMERNAME);
                            command.Parameters.AddWithValue("@PRODUCTNAME", PRODUCTNAME);
                            command.Parameters.AddWithValue("@VARIETY", VARIETY);
                            command.Parameters.AddWithValue("@TRANSACTIONTYPE", TRANSACTIONTYPE);
                            command.Parameters.AddWithValue("@QUANTITY", Convert.ToInt32(QUANTITY));
                            command.Parameters.AddWithValue("@PRICE", Convert.ToInt32(PRICE));
                            command.Parameters.AddWithValue("@TOTALPRICE", Convert.ToDouble(TOTALPRICE));
                            command.Parameters.AddWithValue("@ID", txtID.Text); // Specify the ID of the row to update

                            // Execute the command
                            int rowsAffected = command.ExecuteNonQuery();

                            StringBuilder messageBuilder = new StringBuilder();

                            if (rowsAffected > 0)
                            {
                                messageBuilder.AppendLine("Product updated successfully.");

                                // Deduct or modify inventory based on the updated transaction type
                                if (TRANSACTIONTYPE == "SELL")
                                {
                                    int quantityToDeduct = Convert.ToInt32(QUANTITY);

                                    if (PRODUCTNAME == "Rice" && !string.IsNullOrWhiteSpace(VARIETY))
                                    {
                                        string DeductRiceQuery = "UPDATE tbl_rice SET QUANTITYPERSACK = QUANTITYPERSACK - @QUANTITY WHERE VARIETYNAME = @VARIETYNAME";
                                        using (MySqlCommand deductCommand = new MySqlCommand(DeductRiceQuery, connection))
                                        {
                                            deductCommand.Parameters.AddWithValue("@QUANTITY", quantityToDeduct);
                                            deductCommand.Parameters.AddWithValue("@VARIETYNAME", VARIETY);
                                            int riceRowsAffected = deductCommand.ExecuteNonQuery();
                                            if (riceRowsAffected > 0)
                                            {
                                                messageBuilder.AppendLine("Rice quantity deducted successfully.");
                                            }
                                            else
                                            {
                                                messageBuilder.AppendLine("No matching rice variety found to deduct quantity.");
                                            }
                                        }
                                    }
                                    else if (PRODUCTNAME == "Darak" || PRODUCTNAME == "Pinlid")
                                    {
                                        string DeductDarakPinlidQuery = "UPDATE tbl_darakpinlid SET QUANTITYPERSACK = QUANTITYPERSACK - @QUANTITY WHERE PRODUCTNAME = @PRODUCTNAME";
                                        using (MySqlCommand deductCommand = new MySqlCommand(DeductDarakPinlidQuery, connection))
                                        {
                                            deductCommand.Parameters.AddWithValue("@QUANTITY", quantityToDeduct);
                                            deductCommand.Parameters.AddWithValue("@PRODUCTNAME", PRODUCTNAME);
                                            int darakPinlidRowsAffected = deductCommand.ExecuteNonQuery();
                                            if (darakPinlidRowsAffected > 0)
                                            {
                                                messageBuilder.AppendLine($"{PRODUCTNAME} quantity deducted successfully.");
                                            }
                                            else
                                            {
                                                messageBuilder.AppendLine($"No matching {PRODUCTNAME} found to deduct quantity.");
                                            }
                                        }
                                    }
                                }

                                // Refresh product list and clear input fields
                                LoadProducts();
                                txtCName.Clear();
                                txtPName.Clear();
                                txtQuantity.Clear();
                                txtPrice.Clear();
                                txtTotalPrice.Clear();
                            }
                            else
                            {
                                messageBuilder.AppendLine("Failed to update the product.");
                            }

                            // Show a single message box with all messages
                            MessageBox.Show(messageBuilder.ToString(), "Transaction Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Please fill out the needed information", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //private void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();

        //            // Update data in a table
        //            string updateQuery = "UPDATE tbl_products SET CUSTOMERNAME = @CUSTOMERNAME, PRODUCTNAME = @PRODUCTNAME, VARIETY = @VARIETY, TRANSACTIONTYPE = @TRANSACTIONTYPE, QUANTITY = @QUANTITY, PRICE = @PRICE, TOTALPRICE = @TOTALPRICE WHERE ID = @ID";

        //            using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
        //            {
        //                // Replace parameter values with actual data
        //                command.Parameters.AddWithValue("@CUSTOMERNAME", txtCName.Text);
        //                command.Parameters.AddWithValue("@PRODUCTNAME", txtPName.Text);
        //                command.Parameters.AddWithValue("@VARIETY", cmbVariety.Text);
        //                command.Parameters.AddWithValue("@TRANSACTIONTYPE", cmbTransactionType.Text);
        //                command.Parameters.AddWithValue("@QUANTITY", Convert.ToInt32(txtQuantity.Text));
        //                command.Parameters.AddWithValue("@PRICE", Convert.ToInt32(txtPrice.Text));
        //                command.Parameters.AddWithValue("@TOTALPRICE", Convert.ToDouble(txtTotalPrice.Text));
        //                command.Parameters.AddWithValue("@ID", txtID.Text); // Specify the ID of the row to update

        //                // Execute the command
        //                int rowsAffected = command.ExecuteNonQuery();

        //                if (rowsAffected > 0)
        //                {
        //                    MessageBox.Show("Product updated successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                    LoadProducts();
        //                    txtCName.Clear();
        //                    txtPName.Clear();
        //                    txtQuantity.Clear();
        //                    txtPrice.Clear();
        //                    txtTotalPrice.Clear();
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Failed to update", "FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // First, retrieve the product details before deleting it for tracking purposes
                    string fetchQuery = "SELECT CUSTOMERNAME, PRODUCTNAME, VARIETY, TRANSACTIONTYPE, QUANTITY, PRICE, TOTALPRICE FROM tbl_products WHERE ID = @ID";
                    using (MySqlCommand fetchCommand = new MySqlCommand(fetchQuery, connection))
                    {
                        fetchCommand.Parameters.AddWithValue("@ID", txtID.Text);

                        using (MySqlDataReader reader = fetchCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Store product data for undo or tracking
                                lastProductData = new Dictionary<string, object>
                        {
                            { "ID", txtID.Text },
                            { "CUSTOMERNAME", reader["CUSTOMERNAME"].ToString() },
                            { "PRODUCTNAME", reader["PRODUCTNAME"].ToString() },
                            { "VARIETY", reader["VARIETY"].ToString() },
                            { "TRANSACTIONTYPE", reader["TRANSACTIONTYPE"].ToString() },
                            { "QUANTITY", reader["QUANTITY"].ToString() },
                            { "PRICE", reader["PRICE"].ToString() },
                            { "TOTALPRICE", reader["TOTALPRICE"].ToString() }
                        };
                            }
                            else
                            {
                                MessageBox.Show("Product not found", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return; // Exit if the product wasn't found
                            }
                        }
                    }

                    // Now proceed with the deletion
                    string deleteQuery = "DELETE FROM tbl_products WHERE ID = @ID";
                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ID", txtID.Text); // Specify the ID of the row to delete

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product deleted successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Track the action as "Delete"
                            lastAction = ActionType.Delete;

                            // Refresh the product list and clear the fields
                            LoadProducts();
                            txtCName.Clear();
                            txtPName.Clear();
                            txtQuantity.Clear();
                            txtPrice.Clear();
                            txtTotalPrice.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete", "FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadProducts();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            HomePage HomeFrm = new HomePage();
            HomeFrm.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchKeyword = txtSearch.Text.Trim().ToLower();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = $"SELECT * FROM tbl_products WHERE PRODUCTNAME LIKE '%{searchKeyword}%'";

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        lvProducts.Items.Clear();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                                item.SubItems.Add(reader["CUSTOMERNAME"].ToString());
                                item.SubItems.Add(reader["PRODUCTNAME"].ToString());
                                item.SubItems.Add(reader["VARIETY"].ToString());
                                item.SubItems.Add(reader["TRANSACTIONTYPE"].ToString());
                                item.SubItems.Add(reader["QUANTITY"].ToString());
                                item.SubItems.Add(reader["PRICE"].ToString());
                                item.SubItems.Add(reader["TOTALPRICE"].ToString());
                                item.SubItems.Add(reader["DATE"].ToString());

                                lvProducts.Items.Add(item);
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            isPrinting = true;

            // Create a new PrintDocument
            PrintDocument printDocument = new PrintDocument();

            // Attach event handler for the PrintPage event
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);

            // Create a PrintDialog
            PrintDialog printDialog = new PrintDialog();

            // Associate the PrintDocument with the PrintDialog
            printDialog.Document = printDocument;

            // Allow the user to choose the printer and configure settings
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                // Show the Print Preview Dialog
                PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                printPreviewDialog.Document = printDocument;

                printPreviewDialog.ShowDialog();
            }
        }
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            string headerText = "MORTEL RICE MILL";
            Font headerFont = new Font("Tahoma", 12, FontStyle.Bold);
            SizeF headerSize = e.Graphics.MeasureString(headerText, headerFont);
            float headerX = (e.PageBounds.Width - headerSize.Width) / 2;
            e.Graphics.DrawString(headerText, headerFont, Brushes.Black, new PointF(headerX, 10));

            // Custom formatting for footer
            string footerText = "AgriSync: Unifying Excellence in Rice Mill Operations";
            Font footerFont = new Font("Verdana", 10, FontStyle.Italic);
            SizeF footerSize = e.Graphics.MeasureString(footerText, footerFont);
            float footerX = (e.PageBounds.Width - footerSize.Width) / 2;
            e.Graphics.DrawString(footerText, footerFont, Brushes.Black, new PointF(footerX, e.PageBounds.Height - 30));


            // Code to print ListView items
            int yPos = 70; // Adjust the starting position as needed

            // Draw column headers
            int xPos = 30; // Adjust the starting position for columns
            e.Graphics.DrawString("ID", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

            // Adjust the spacing between columns as needed
            xPos += 110;
            e.Graphics.DrawString("Customer Name", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

            xPos += 130;
            e.Graphics.DrawString("Product Name", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

            xPos += 120;
            e.Graphics.DrawString("Variety", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

            xPos += 115;
            e.Graphics.DrawString("Transaction Type", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

            xPos += 130;
            e.Graphics.DrawString("Quantity", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

            xPos += 110;
            e.Graphics.DrawString("Price", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

            xPos += 115;
            e.Graphics.DrawString("Total Price", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

            xPos += 130;
            e.Graphics.DrawString("Date", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

            yPos += 20; // Adjust the spacing after headers

            Pen gridPen = new Pen(Color.Black, 1);
            int columnWidth = 120; // Adjust the column width based on your needs

            // Draw horizontal grid lines
            for (int i = 0; i <= lvProducts.Items.Count; i++)
            {
                e.Graphics.DrawLine(gridPen, 30, yPos + i * 15, xPos + 150, yPos + i * 15);
            }

            for (int i = 0; i < 8; i++) // 8 columns in total
            {
                e.Graphics.DrawLine(gridPen, 30 + i * columnWidth, yPos, 30 + i * columnWidth, yPos + lvProducts.Items.Count * 15);
            }

            // Code to print ListView items
            foreach (ListViewItem item in lvProducts.Items)
            {
                xPos = 30; // Reset starting position for each item
                e.Graphics.DrawString(item.Text, new Font("Arial", 10), Brushes.Black, new Point(xPos, yPos));

                xPos += columnWidth; // Adjust the spacing between columns as needed
                for (int i = 1; i < item.SubItems.Count; i++)
                {
                    e.Graphics.DrawString(item.SubItems[i].Text, new Font("Arial", 10), Brushes.Black, new Point(xPos, yPos));
                    xPos += columnWidth; // Adjust the spacing between columns as needed
                }

                yPos += 15; // Adjust the spacing between items as needed
            }

            // Calculate the right edge of the printed text in the "Date" column
            int dateColumnRightEdge = xPos + e.Graphics.MeasureString("Date", new Font("Arial", 10)).ToSize().Width;

            // Draw the last vertical grid line (after printing items)
            e.Graphics.DrawLine(gridPen, dateColumnRightEdge, 90, dateColumnRightEdge, yPos - 20 + lvProducts.Items.Count * 8);
        }

        private void btnLoadRecords_Click(object sender, EventArgs e)
        {
            LoadProducts();
            txtSearch.Clear();
        }

        private void undo_Click(object sender, EventArgs e)
        {
            if (lastAction == ActionType.None)
            {
                MessageBox.Show("No action to undo.", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Ask the user for confirmation before undoing
            DialogResult result = MessageBox.Show("Do you want to undo the last action?", "Undo Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        switch (lastAction)
                        {
                            case ActionType.Add:
                                // Undo Add: Delete the last added product
                                string deleteQuery = "DELETE FROM tbl_products WHERE ID = @ID";
                                using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                                    command.ExecuteNonQuery();
                                    MessageBox.Show("Last added product has been undone.", "Undo Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                break;

                            case ActionType.Update:
                                // Undo Update: Revert to previous product details
                                string updateQuery = "UPDATE tbl_products SET CUSTOMERNAME = @CUSTOMERNAME, PRODUCTNAME = @PRODUCTNAME, VARIETY = @VARIETY, TRANSACTIONTYPE = @TRANSACTIONTYPE, QUANTITY = @QUANTITY, PRICE = @PRICE, TOTALPRICE = @TOTALPRICE WHERE ID = @ID";
                                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@CUSTOMERNAME", lastProductData["CUSTOMERNAME"]);
                                    command.Parameters.AddWithValue("@PRODUCTNAME", lastProductData["PRODUCTNAME"]);
                                    command.Parameters.AddWithValue("@VARIETY", lastProductData["VARIETY"]);
                                    command.Parameters.AddWithValue("@TRANSACTIONTYPE", lastProductData["TRANSACTIONTYPE"]);
                                    command.Parameters.AddWithValue("@QUANTITY", lastProductData["QUANTITY"]);
                                    command.Parameters.AddWithValue("@PRICE", lastProductData["PRICE"]);
                                    command.Parameters.AddWithValue("@TOTALPRICE", lastProductData["TOTALPRICE"]);
                                    command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                                    command.ExecuteNonQuery();
                                    MessageBox.Show("Last update has been undone.", "Undo Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                break;

                            case ActionType.Delete:
                                // Undo Delete: Re-insert the last deleted product
                                string insertQuery = "INSERT INTO tbl_products (ID, CUSTOMERNAME, PRODUCTNAME, VARIETY, TRANSACTIONTYPE, QUANTITY, PRICE, TOTALPRICE) VALUES (@ID, @CUSTOMERNAME, @PRODUCTNAME, @VARIETY, @TRANSACTIONTYPE, @QUANTITY, @PRICE, @TOTALPRICE)";
                                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                                    command.Parameters.AddWithValue("@CUSTOMERNAME", lastProductData["CUSTOMERNAME"]);
                                    command.Parameters.AddWithValue("@PRODUCTNAME", lastProductData["PRODUCTNAME"]);
                                    command.Parameters.AddWithValue("@VARIETY", lastProductData["VARIETY"]);
                                    command.Parameters.AddWithValue("@TRANSACTIONTYPE", lastProductData["TRANSACTIONTYPE"]);
                                    command.Parameters.AddWithValue("@QUANTITY", lastProductData["QUANTITY"]);
                                    command.Parameters.AddWithValue("@PRICE", lastProductData["PRICE"]);
                                    command.Parameters.AddWithValue("@TOTALPRICE", lastProductData["TOTALPRICE"]);
                                    command.ExecuteNonQuery();
                                    MessageBox.Show("Last deletion has been undone.", "Undo Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                break;
                        }

                        // Clear the last action and product data after undoing
                        lastAction = ActionType.None;
                        lastProductData.Clear();
                        LoadProducts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void label9_Click(object sender, EventArgs e)
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void RecordsForm_Load(object sender, EventArgs e)
        {

        }
    }
}

