using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJECTSYSTEM
{
    public partial class Inventory : Form
    {
        private string connectionString = "Server=localhost;Database=db_ricemillmanagement;User ID=root;";
        private bool isPrinting = false;

        public Inventory()
        {
            InitializeComponent();
            lvProductsRice.SelectedIndexChanged += lvProducts_SelectedIndexChanged;
            lvDarak.SelectedIndexChanged += lvDarak_SelectedIndexChanged;
            lvPinlid.SelectedIndexChanged += lvPinlid_SelectedIndexChanged;
            lvMais.SelectedIndexChanged += lvMais_SelectedIndexChanged;


            panelRice.Visible = true; 
            panelDarak.Visible = false;
            panelPinlid.Visible = false;
            panelMais.Visible = false;
        }

        private void ClearTextBoxesRice()
        {
            txtID.Text = string.Empty;          
            txtCName.Text = string.Empty;      
            cmbVariety.SelectedIndex = -1;      
            txtQuantity.Text = string.Empty;    
            txtPrice.Text = string.Empty;       
            txtTotalPrice.Text = string.Empty;  
        }

        private void LoadRecords()
        {
            lvProductsRice.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string selectQuery = "SELECT * FROM tbl_inventoryrice";
                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                                item.SubItems.Add(reader["CUSTOMERNAME"].ToString());
                                item.SubItems.Add(reader["VARIETY"].ToString());
                                item.SubItems.Add(reader["QUANTITY"].ToString());
                                item.SubItems.Add(reader["PRICE"].ToString());
                                item.SubItems.Add(reader["TOTALPRICE"].ToString());

                                string formattedDate = reader["DATE"].ToString();

                                // Use the obtained formatted date
                                item.SubItems.Add(formattedDate);
                                lvProductsRice.Items.Add(item);
                                ClearTextBoxesRice();
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

        // Variables to track last action and data for undo
        private ActionType lastAction = ActionType.None;
        private Dictionary<string, object> lastProductData;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string CUSTOMERNAME = txtCName.Text;
            string VARIETY = cmbVariety.Text;
            int QUANTITY = Convert.ToInt32(txtQuantity.Text);
            int PRICE = Convert.ToInt32(txtPrice.Text);
            int TOTALPRICE = QUANTITY * PRICE;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO tbl_inventoryrice (CUSTOMERNAME, VARIETY, QUANTITY, PRICE, TOTALPRICE) " +
                                         "VALUES (@CUSTOMERNAME, @VARIETY, @QUANTITY, @PRICE, @TOTALPRICE)";

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CUSTOMERNAME", CUSTOMERNAME);
                        command.Parameters.AddWithValue("@VARIETY", VARIETY);
                        command.Parameters.AddWithValue("@QUANTITY", QUANTITY);
                        command.Parameters.AddWithValue("@PRICE", PRICE);
                        command.Parameters.AddWithValue("@TOTALPRICE", TOTALPRICE);

                        command.ExecuteNonQuery();

                        // Save data for undo
                        lastAction = ActionType.Add;
                        lastProductData = new Dictionary<string, object>
                {
                    { "ID", command.LastInsertedId },
                    { "CUSTOMERNAME", CUSTOMERNAME },
                    { "VARIETY", VARIETY },
                    { "QUANTITY", QUANTITY },
                    { "PRICE", PRICE },
                    { "TOTALPRICE", TOTALPRICE }
                };
                    }
                    MessageBox.Show("Record added successfully!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecords();
                    ClearTextBoxesRice();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Please select a record to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string CUSTOMERNAME = txtCName.Text;
            string VARIETY = cmbVariety.Text;
            int QUANTITY = Convert.ToInt32(txtQuantity.Text);
            int PRICE = Convert.ToInt32(txtPrice.Text);
            int TOTALPRICE = QUANTITY * PRICE;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Fetch existing record before update (for undo purposes)
                    string selectQuery = "SELECT * FROM tbl_inventoryrice WHERE ID = @ID";
                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@ID", txtID.Text);
                        using (MySqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lastAction = ActionType.Update;
                                lastProductData = new Dictionary<string, object>
                        {
                            { "ID", reader["ID"] },
                            { "CUSTOMERNAME", reader["CUSTOMERNAME"] },
                            { "VARIETY", reader["VARIETY"] },
                            { "QUANTITY", reader["QUANTITY"] },
                            { "PRICE", reader["PRICE"] },
                            { "TOTALPRICE", reader["TOTALPRICE"] }
                        };
                            }
                        }
                    }

                    string updateQuery = "UPDATE tbl_inventoryrice SET CUSTOMERNAME = @CUSTOMERNAME, VARIETY = @VARIETY, QUANTITY = @QUANTITY, PRICE = @PRICE, TOTALPRICE = @TOTALPRICE WHERE ID = @ID";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ID", txtID.Text);
                        command.Parameters.AddWithValue("@CUSTOMERNAME", CUSTOMERNAME);
                        command.Parameters.AddWithValue("@VARIETY", VARIETY);
                        command.Parameters.AddWithValue("@QUANTITY", QUANTITY);
                        command.Parameters.AddWithValue("@PRICE", PRICE);
                        command.Parameters.AddWithValue("@TOTALPRICE", TOTALPRICE);

                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Record updated successfully!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecords();
                    ClearTextBoxesRice();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Please select a record to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Fetch record before delete (for undo purposes)
                    string selectQuery = "SELECT * FROM tbl_inventoryrice WHERE ID = @ID";
                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@ID", txtID.Text);
                        using (MySqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lastAction = ActionType.Delete;
                                lastProductData = new Dictionary<string, object>
                        {
                            { "ID", reader["ID"] },
                            { "CUSTOMERNAME", reader["CUSTOMERNAME"] },
                            { "VARIETY", reader["VARIETY"] },
                            { "QUANTITY", reader["QUANTITY"] },
                            { "PRICE", reader["PRICE"] },
                            { "TOTALPRICE", reader["TOTALPRICE"] }
                        };
                            }
                        }
                    }

                    string deleteQuery = "DELETE FROM tbl_inventoryrice WHERE ID = @ID";
                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ID", txtID.Text);
                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Record deleted successfully!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecords();
                    ClearTextBoxesRice();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void lvProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvProductsRice.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvProductsRice.SelectedItems[0];
                txtID.Text = selectedItem.SubItems[0].Text;
                txtCName.Text = selectedItem.SubItems[1].Text;
                cmbVariety.Text = selectedItem.SubItems[2].Text;
                txtQuantity.Text = selectedItem.SubItems[3].Text;
                txtPrice.Text = selectedItem.SubItems[4].Text;
                txtTotalPrice.Text = selectedItem.SubItems[5].Text;
            }
        }

        //Rice//
        // Automatically calculate total price when quantity or price changes
        private void txtPrice_TextChanged_1(object sender, EventArgs e)
        {
            CalculateTotalPrice();

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

        private void txtTotalPrice_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }
        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice(); 
            
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


        private void CalculateTotalPrice()
        {
            if (int.TryParse(txtQuantity.Text, out int quantity) && int.TryParse(txtPrice.Text, out int price))
            {
                int totalPrice = quantity * price;
                txtTotalPrice.Text = totalPrice.ToString();
            }
        }

        private void btnLoadRecords_Click(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void btnRice_Click(object sender, EventArgs e)
        {
            panelRice.Show();
            panelRice.BringToFront();
            ClearTextBoxesRice();
        }

        private void undo_Click(object sender, EventArgs e)
        {
            if (lastAction == ActionType.None)
            {
                MessageBox.Show("No action to undo.", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (lastAction == ActionType.Add)
                    {
                        // Undo Add (Delete the last added record)
                        string deleteQuery = "DELETE FROM tbl_inventoryrice WHERE ID = @ID";
                        using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (lastAction == ActionType.Update)
                    {
                        // Undo Update (Revert to the previous values)
                        string updateQuery = "UPDATE tbl_inventoryrice SET CUSTOMERNAME = @CUSTOMERNAME, VARIETY = @VARIETY, QUANTITY = @QUANTITY, PRICE = @PRICE, TOTALPRICE = @TOTALPRICE WHERE ID = @ID";
                        using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                            command.Parameters.AddWithValue("@CUSTOMERNAME", lastProductData["CUSTOMERNAME"]);
                            command.Parameters.AddWithValue("@VARIETY", lastProductData["VARIETY"]);
                            command.Parameters.AddWithValue("@QUANTITY", lastProductData["QUANTITY"]);
                            command.Parameters.AddWithValue("@PRICE", lastProductData["PRICE"]);
                            command.Parameters.AddWithValue("@TOTALPRICE", lastProductData["TOTALPRICE"]);
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (lastAction == ActionType.Delete)
                    {
                        // Undo Delete (Re-insert the deleted record)
                        string insertQuery = "INSERT INTO tbl_inventoryrice (ID, CUSTOMERNAME, VARIETY, QUANTITY, PRICE, TOTALPRICE) " +
                                             "VALUES (@ID, @CUSTOMERNAME, @VARIETY, @QUANTITY, @PRICE, @TOTALPRICE)";
                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                            command.Parameters.AddWithValue("@CUSTOMERNAME", lastProductData["CUSTOMERNAME"]);
                            command.Parameters.AddWithValue("@VARIETY", lastProductData["VARIETY"]);
                            command.Parameters.AddWithValue("@QUANTITY", lastProductData["QUANTITY"]);
                            command.Parameters.AddWithValue("@PRICE", lastProductData["PRICE"]);
                            command.Parameters.AddWithValue("@TOTALPRICE", lastProductData["TOTALPRICE"]);
                            command.ExecuteNonQuery();
                        }
                    }

                    // Clear last action after undo
                    lastAction = ActionType.None;
                    lastProductData.Clear();
                    MessageBox.Show("Undo action performed successfully!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecords();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            HomePage frm = new HomePage();
            frm.ShowDialog();
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

                    string selectQuery = $"SELECT * FROM tbl_inventoryrice WHERE VARIETY LIKE '%{searchKeyword}%'";

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        lvProductsRice.Items.Clear();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                                item.SubItems.Add(reader["CUSTOMERNAME"].ToString());
                                item.SubItems.Add(reader["VARIETY"].ToString());
                                item.SubItems.Add(reader["QUANTITY"].ToString());
                                item.SubItems.Add(reader["PRICE"].ToString());
                                item.SubItems.Add(reader["TOTALPRICE"].ToString());

                                lvProductsRice.Items.Add(item);
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadRecords();
            LoadRecordsDarak();
            LoadRecordsPinlid();
            LoadRecordsMais();
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
            xPos += 120;
            e.Graphics.DrawString("Customer Name", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

            xPos += 120;
            e.Graphics.DrawString("Variety", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

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
            for (int i = 0; i <= lvProductsRice.Items.Count; i++)
            {
                e.Graphics.DrawLine(gridPen, 30, yPos + i * 15, xPos + 150, yPos + i * 15);
            }

            for (int i = 0; i < 8; i++) // 8 columns in total
            {
                e.Graphics.DrawLine(gridPen, 30 + i * columnWidth, yPos, 30 + i * columnWidth, yPos + lvProductsRice.Items.Count * 15);
            }

            // Code to print ListView items
            foreach (ListViewItem item in lvProductsRice.Items)
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
            e.Graphics.DrawLine(gridPen, dateColumnRightEdge, 90, dateColumnRightEdge, yPos - 20 + lvProductsRice.Items.Count * 8);
        }




        //-------------------------DARAK SECTION----------------------------//


        private void ClearTextBoxesDarak()
        {
            txtIDDarak.Text = string.Empty;
            txtCNameDarak.Text = string.Empty;
            txtQuantityDarak.Text = string.Empty;
            txtPriceDarak.Text = string.Empty;
            txtTotalPriceDarak.Text = string.Empty;
        }
        private void btnDarak_Click_1(object sender, EventArgs e)
        {
            panelDarak.Show();
            panelDarak.BringToFront();
            ClearTextBoxesDarak();
        }

        private void LoadRecordsDarak()
        {
            lvDarak.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string selectQuery = "SELECT * FROM tbl_inventorydarak";
                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                                item.SubItems.Add(reader["CUSTOMERNAME"].ToString());
                                item.SubItems.Add(reader["QUANTITY"].ToString());
                                item.SubItems.Add(reader["PRICE"].ToString());
                                item.SubItems.Add(reader["TOTALPRICE"].ToString());

                                string formattedDate = reader["DATE"].ToString();

                                // Use the obtained formatted date
                                item.SubItems.Add(formattedDate);
                                lvDarak.Items.Add(item);
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
        private void lvDarak_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvDarak.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvDarak.SelectedItems[0];
                txtIDDarak.Text = selectedItem.SubItems[0].Text;
                txtCNameDarak.Text = selectedItem.SubItems[1].Text;
                txtQuantityDarak.Text = selectedItem.SubItems[2].Text;
                txtPriceDarak.Text = selectedItem.SubItems[3].Text;
                txtTotalPriceDarak.Text = selectedItem.SubItems[4].Text;
            }
        }


        private void btnAddDarak_Click(object sender, EventArgs e)
        {
            string CustomerName = txtCNameDarak.Text;
            string Quantity = txtQuantityDarak.Text;
            string Price = txtPriceDarak.Text;
            string tablename = "tbl_inventorydarak";

            AddData(tablename, CustomerName, Quantity, Price);
        }

        private void btnUpdateDarak_Click(object sender, EventArgs e)
        {
            string ID = txtIDDarak.Text;
            string CustomerName = txtCNameDarak.Text;
            string Quantity = txtQuantityDarak.Text;
            string Price = txtPriceDarak.Text;
            string tablename = "tbl_inventorydarak";

            UpdateData(tablename, ID, CustomerName, Quantity, Price);
        }

        private void btnDeleteDarak_Click_1(object sender, EventArgs e)
        {
           string ID = txtIDDarak.Text;
           string tablename = "tbl_inventorydarak";

            DeleteData(ID, tablename);
        }

        private void DeleteData(string ID, string tablename)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                MessageBox.Show("Please select a record to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Fetch record before delete (for undo purposes)
                    string selectQuery = "SELECT * FROM "+tablename+" WHERE ID = @ID";
                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@ID", ID);
                        using (MySqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lastAction = ActionType.Delete;
                                lastProductData = new Dictionary<string, object>
                        {
                            { "ID", reader["ID"] },
                            { "CUSTOMERNAME", reader["CUSTOMERNAME"] },
                            { "QUANTITY", reader["QUANTITY"] },
                            { "PRICE", reader["PRICE"] },
                            { "TOTALPRICE", reader["TOTALPRICE"] }
                        };
                            }
                        }
                    }

                    string deleteQuery = "DELETE FROM "+tablename+" WHERE ID = @ID";
                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ID", ID);
                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Record deleted successfully!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecords();
                    ClearTextBoxesDarak();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnSearchDarak_Click(object sender, EventArgs e)
        {
            string searchKeyword = txtSearchDarak.Text.Trim().ToLower();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = $"SELECT * FROM tbl_inventorydarak WHERE CUSTOMERNAME LIKE '%{searchKeyword}%'";

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        lvDarak.Items.Clear();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                                item.SubItems.Add(reader["CUSTOMERNAME"].ToString());
                                item.SubItems.Add(reader["QUANTITY"].ToString());
                                item.SubItems.Add(reader["PRICE"].ToString());
                                item.SubItems.Add(reader["TOTALPRICE"].ToString());

                                lvDarak.Items.Add(item);
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

        //Darak//
        private void txtQuantityDarak_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtQuantityDarak.Text, out int quantity))
            {
                if (quantity < 0)
                {
                    MessageBox.Show("Quantity cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuantityDarak.Clear();
                }
            }
            else if (!string.IsNullOrEmpty(txtQuantityDarak.Text))
            {
                MessageBox.Show("Please enter a valid numeric value for Quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantityDarak.Clear();
            }

        } 
        
        private void txtTotalPriceDarak_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPriceDarak();

            if (int.TryParse(txtTotalPriceDarak.Text, out int price))
            {
                if (price < 0)
                {
                    MessageBox.Show("Price cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTotalPriceDarak.Clear();
                }
            }
            else if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                MessageBox.Show("Please enter a valid numeric value for Price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTotalPriceDarak.Clear();
            }
        }


        //Darak//
        private void txtPriceDarak_TextChanged_1(object sender, EventArgs e)
        {
            CalculateTotalPriceDarak();
            if (int.TryParse(txtPriceDarak.Text, out int price))
            {
                if (price < 0)
                {
                    MessageBox.Show("Price cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPriceDarak.Clear();
                }
            }
            else if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                MessageBox.Show("Please enter a valid numeric value for Price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPriceDarak.Clear();
            }
        }

        private void txtQuantityDarak_TextAlignChanged(object sender, EventArgs e)
        {
            CalculateTotalPriceDarak();
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
        

       



        private void CalculateTotalPriceDarak()
        {
            if (int.TryParse(txtQuantityDarak.Text, out int quantity) && int.TryParse(txtPriceDarak.Text, out int price))
            {
                int totalPrice = quantity * price;
                txtTotalPriceDarak.Text = totalPrice.ToString();
            }
        }

        private void btnPrintDarak_Click_1(object sender, EventArgs e)
        {
            isPrinting = true;

            // Create a new PrintDocument
            PrintDocument printDocument = new PrintDocument();

            // Attach event handler for the PrintPage event
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocumentDarak_PrintPage);

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

        private void PrintDocumentDarak_PrintPage(object sender, PrintPageEventArgs e)
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
            xPos += 120;
            e.Graphics.DrawString("Customer Name", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

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
            for (int i = 0; i <= lvDarak.Items.Count; i++)
            {
                e.Graphics.DrawLine(gridPen, 30, yPos + i * 15, xPos + 150, yPos + i * 15);
            }

            for (int i = 0; i < 8; i++) // 8 columns in total
            {
                e.Graphics.DrawLine(gridPen, 30 + i * columnWidth, yPos, 30 + i * columnWidth, yPos + lvDarak.Items.Count * 15);
            }

            // Code to print ListView items
            foreach (ListViewItem item in lvDarak.Items)
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
            e.Graphics.DrawLine(gridPen, dateColumnRightEdge, 90, dateColumnRightEdge, yPos - 20 + lvDarak.Items.Count * 8);
        }

        private void btnLoadDarak_Click_1(object sender, EventArgs e)
        {
            LoadRecordsDarak();
        }

        private void undoDarak_Click_1(object sender, EventArgs e)
        {
            if (lastAction == ActionType.None)
            {
                MessageBox.Show("No action to undo.", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (lastAction == ActionType.Add)
                    {
                        // Undo Add (Delete the last added record)
                        string deleteQuery = "DELETE FROM tbl_inventorydarak WHERE ID = @ID";
                        using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (lastAction == ActionType.Update)
                    {
                        // Undo Update (Revert to the previous values)
                        string updateQuery = "UPDATE tbl_inventorydarak SET CUSTOMERNAME = @CUSTOMERNAME, QUANTITY = @QUANTITY, PRICE = @PRICE, TOTALPRICE = @TOTALPRICE WHERE ID = @ID";
                        using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                            command.Parameters.AddWithValue("@CUSTOMERNAME", lastProductData["CUSTOMERNAME"]);
                            command.Parameters.AddWithValue("@QUANTITY", lastProductData["QUANTITY"]);
                            command.Parameters.AddWithValue("@PRICE", lastProductData["PRICE"]);
                            command.Parameters.AddWithValue("@TOTALPRICE", lastProductData["TOTALPRICE"]);
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (lastAction == ActionType.Delete)
                    {
                        // Undo Delete (Re-insert the deleted record)
                        string insertQuery = "INSERT INTO tbl_inventorydarak (ID, CUSTOMERNAME, QUANTITY, PRICE, TOTALPRICE) " +
                                             "VALUES (@ID, @CUSTOMERNAME, @QUANTITY, @PRICE, @TOTALPRICE)";
                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                            command.Parameters.AddWithValue("@CUSTOMERNAME", lastProductData["CUSTOMERNAME"]);
                            command.Parameters.AddWithValue("@QUANTITY", lastProductData["QUANTITY"]);
                            command.Parameters.AddWithValue("@PRICE", lastProductData["PRICE"]);
                            command.Parameters.AddWithValue("@TOTALPRICE", lastProductData["TOTALPRICE"]);
                            command.ExecuteNonQuery();
                        }
                    }

                    // Clear last action after undo
                    lastAction = ActionType.None;
                    lastProductData.Clear();
                    MessageBox.Show("Undo action performed successfully!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecordsDarak();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }




        //-----------------PINLID SECTION-------------------------//


        private void ClearTextBoxesPinlid()
        {
            txtIDPinlid.Text = string.Empty;
            txtCNamePinlid.Text = string.Empty;
            txtQuantityPinlid.Text = string.Empty;
            txtPricePinlid.Text = string.Empty;
            txtTotalPricePinlid.Text = string.Empty;
        }
        private void btnPinlid_Click_1(object sender, EventArgs e)
        {
            panelPinlid.Show();
            panelPinlid.BringToFront();
            ClearTextBoxesPinlid();
        }
        private void LoadRecordsPinlid()
        {
            lvPinlid.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string selectQuery = "SELECT * FROM tbl_inventorypinlid";
                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                                item.SubItems.Add(reader["CUSTOMERNAME"].ToString());
                                item.SubItems.Add(reader["QUANTITY"].ToString());
                                item.SubItems.Add(reader["PRICE"].ToString());
                                item.SubItems.Add(reader["TOTALPRICE"].ToString());

                                string formattedDate = reader["DATE"].ToString();

                                // Use the obtained formatted date
                                item.SubItems.Add(formattedDate);
                                lvPinlid.Items.Add(item);
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

        private void lvPinlid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPinlid.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvPinlid.SelectedItems[0];
                txtIDPinlid.Text = selectedItem.SubItems[0].Text;
                txtCNamePinlid.Text = selectedItem.SubItems[1].Text;
                txtQuantityPinlid.Text = selectedItem.SubItems[2].Text;
                txtPricePinlid.Text = selectedItem.SubItems[3].Text;
                txtTotalPricePinlid.Text = selectedItem.SubItems[4].Text;
            }
        }

        private void btnLoadPinlid_Click_1(object sender, EventArgs e)
        {
            LoadRecordsPinlid();
        }

        private void btnAddPinlid_Click_1(object sender, EventArgs e)
        {
            string CustomerName = txtCNamePinlid.Text;
            string Quantity = txtQuantityPinlid.Text;
            string Price = txtPricePinlid.Text;
            string tablename = "tbl_inventorypinlid";
          
            AddData(tablename,CustomerName,Quantity,Price);
        }

        private void AddData(string tablename, string CustomerName, string Quantity, string Price)
        {
            string CUSTOMERNAME = CustomerName;
            int QUANTITY = Convert.ToInt32(Quantity);
            int PRICE = Convert.ToInt32(Price);
            int TOTALPRICE = QUANTITY * PRICE;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO "+tablename+" (CUSTOMERNAME, QUANTITY, PRICE, TOTALPRICE) " +
                                         "VALUES (@CUSTOMERNAME, @QUANTITY, @PRICE, @TOTALPRICE)";

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CUSTOMERNAME", CUSTOMERNAME);
                        command.Parameters.AddWithValue("@QUANTITY", QUANTITY);
                        command.Parameters.AddWithValue("@PRICE", PRICE);
                        command.Parameters.AddWithValue("@TOTALPRICE", TOTALPRICE);

                        command.ExecuteNonQuery();

                        // Save data for undo
                        lastAction = ActionType.Add;
                        lastProductData = new Dictionary<string, object>
                {
                    { "ID", command.LastInsertedId },
                    { "CUSTOMERNAME", CUSTOMERNAME },
                    { "QUANTITY", QUANTITY },
                    { "PRICE", PRICE },
                    { "TOTALPRICE", TOTALPRICE }
                };
                    }
                    MessageBox.Show("Record added successfully!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecordsPinlid();
                    ClearTextBoxesPinlid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void UpdateData(string tablename, string ID, string CustomerName, string Quantity, string Price)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                MessageBox.Show("Please select a record to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            string CUSTOMERNAME = CustomerName;
            int QUANTITY = Convert.ToInt32(Quantity);
            int PRICE = Convert.ToInt32(Price);
            int TOTALPRICE = QUANTITY * PRICE;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Fetch existing record before update (for undo purposes)
                    string selectQuery = "SELECT * FROM tbl_inventorypinlid WHERE ID = @ID";
                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@ID", ID);
                        using (MySqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lastAction = ActionType.Update;
                                lastProductData = new Dictionary<string, object>
                        {
                            { "ID", reader["ID"] },
                            { "CUSTOMERNAME", reader["CUSTOMERNAME"] },
                            { "QUANTITY", reader["QUANTITY"] },
                            { "PRICE", reader["PRICE"] },
                            { "TOTALPRICE", reader["TOTALPRICE"] }
                        };
                            }
                        }
                    }

                    string updateQuery = "UPDATE "+tablename+" SET CUSTOMERNAME = @CUSTOMERNAME, QUANTITY = @QUANTITY, PRICE = @PRICE, TOTALPRICE = @TOTALPRICE WHERE ID = @ID";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ID", ID);
                        command.Parameters.AddWithValue("@CUSTOMERNAME", CUSTOMERNAME);
                        command.Parameters.AddWithValue("@QUANTITY", QUANTITY);
                        command.Parameters.AddWithValue("@PRICE", PRICE);
                        command.Parameters.AddWithValue("@TOTALPRICE", TOTALPRICE);

                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Record updated successfully!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecordsPinlid();
                    ClearTextBoxesPinlid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnUpdatePinlid_Click_1(object sender, EventArgs e)
        {
            string ID = txtIDPinlid.Text;
            string CustomerName = txtCNamePinlid.Text;
            string Quantity = txtQuantityPinlid.Text;
            string Price = txtPricePinlid.Text;
            string tablename = "tbl_inventorypinlid";

            UpdateData(tablename, ID, CustomerName, Quantity, Price);
            
        }

        private void btnDeletePinlid_Click_1(object sender, EventArgs e)
        {
            string ID = txtIDPinlid.Text;
            string tablename = "tbl_inventorypinlid";

            DeleteData(ID, tablename);
        }

        private void btnSearchPinlid_Click_1(object sender, EventArgs e)
        {
            string searchKeyword = txtSearchPinlid.Text.Trim().ToLower();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = $"SELECT * FROM tbl_inventorypinlid WHERE CUSTOMERNAME LIKE '%{searchKeyword}%'";

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        lvPinlid.Items.Clear();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                                item.SubItems.Add(reader["CUSTOMERNAME"].ToString());
                                item.SubItems.Add(reader["QUANTITY"].ToString());
                                item.SubItems.Add(reader["PRICE"].ToString());
                                item.SubItems.Add(reader["TOTALPRICE"].ToString());

                                lvPinlid.Items.Add(item);
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

        private void CalculateTotalPricePinlid()
        {
            if (int.TryParse(txtQuantityPinlid.Text, out int quantity) && int.TryParse(txtPricePinlid.Text, out int price))
            {
                int totalPrice = quantity * price;
                txtTotalPricePinlid.Text = totalPrice.ToString();
            }
        }

        private void btnPrintPinlid_Click_1(object sender, EventArgs e)
        {
            isPrinting = true;

            // Create a new PrintDocument
            PrintDocument printDocument = new PrintDocument();

            // Attach event handler for the PrintPage event
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocumentPinlid_PrintPage);

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

        private void PrintDocumentPinlid_PrintPage(object sender, PrintPageEventArgs e)
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
            xPos += 120;
            e.Graphics.DrawString("Customer Name", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

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
            for (int i = 0; i <= lvPinlid.Items.Count; i++)
            {
                e.Graphics.DrawLine(gridPen, 30, yPos + i * 15, xPos + 150, yPos + i * 15);
            }

            for (int i = 0; i < 8; i++) // 8 columns in total
            {
                e.Graphics.DrawLine(gridPen, 30 + i * columnWidth, yPos, 30 + i * columnWidth, yPos + lvPinlid.Items.Count * 15);
            }

            // Code to print ListView items
            foreach (ListViewItem item in lvPinlid.Items)
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
            e.Graphics.DrawLine(gridPen, dateColumnRightEdge, 90, dateColumnRightEdge, yPos - 20 + lvPinlid.Items.Count * 8);
        }

        private void undoPinlid_Click(object sender, EventArgs e)
        {
            if (lastAction == ActionType.None)
            {
                MessageBox.Show("No action to undo.", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (lastAction == ActionType.Add)
                    {
                        // Undo Add (Delete the last added record)
                        string deleteQuery = "DELETE FROM tbl_inventorypinlid WHERE ID = @ID";
                        using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (lastAction == ActionType.Update)
                    {
                        // Undo Update (Revert to the previous values)
                        string updateQuery = "UPDATE tbl_inventorypinlid SET CUSTOMERNAME = @CUSTOMERNAME, QUANTITY = @QUANTITY, PRICE = @PRICE, TOTALPRICE = @TOTALPRICE WHERE ID = @ID";
                        using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                            command.Parameters.AddWithValue("@CUSTOMERNAME", lastProductData["CUSTOMERNAME"]);
                            command.Parameters.AddWithValue("@QUANTITY", lastProductData["QUANTITY"]);
                            command.Parameters.AddWithValue("@PRICE", lastProductData["PRICE"]);
                            command.Parameters.AddWithValue("@TOTALPRICE", lastProductData["TOTALPRICE"]);
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (lastAction == ActionType.Delete)
                    {
                        // Undo Delete (Re-insert the deleted record)
                        string insertQuery = "INSERT INTO tbl_inventorypinlid (ID, CUSTOMERNAME, QUANTITY, PRICE, TOTALPRICE) " +
                                             "VALUES (@ID, @CUSTOMERNAME, @QUANTITY, @PRICE, @TOTALPRICE)";
                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                            command.Parameters.AddWithValue("@CUSTOMERNAME", lastProductData["CUSTOMERNAME"]);
                            command.Parameters.AddWithValue("@QUANTITY", lastProductData["QUANTITY"]);
                            command.Parameters.AddWithValue("@PRICE", lastProductData["PRICE"]);
                            command.Parameters.AddWithValue("@TOTALPRICE", lastProductData["TOTALPRICE"]);
                            command.ExecuteNonQuery();
                        }
                    }

                    // Clear last action after undo
                    lastAction = ActionType.None;
                    lastProductData.Clear();
                    MessageBox.Show("Undo action performed successfully!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecordsDarak();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        //pinlid//
        private void txtTotalPricePinlid_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPricePinlid();
        }

        private void txtPricePinlid_TextChanged_1(object sender, EventArgs e)
        {
            CalculateTotalPricePinlid();
            if (int.TryParse(txtPricePinlid.Text, out int price))
            {
                if (price < 0)
                {
                    MessageBox.Show("Price cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPricePinlid.Clear();
                }
            }
            else if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                MessageBox.Show("Please enter a valid numeric value for Price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPricePinlid.Clear();
            }
        }
        private void txtQuantityPinlid_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPricePinlid();
            if (int.TryParse(txtQuantityPinlid.Text, out int quantity))
            {
                if (quantity < 0)
                {
                    MessageBox.Show("Quantity cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuantityPinlid.Clear();
                }
            }
            else if (!string.IsNullOrEmpty(txtQuantityPinlid.Text))
            {
                MessageBox.Show("Please enter a valid numeric value for Quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantityPinlid.Clear();
            }
        }
       

        //-----------------------------------MAIS SECTION----------------------------------------------//

        private void btnMais_Click(object sender, EventArgs e)
        {
            panelMais.Show();
            panelMais.BringToFront();
            ClearTextBoxesMais();
        }
        private void ClearTextBoxesMais()
        {
            txtIDMais.Text = string.Empty;
            txtCNameMais.Text = string.Empty;
            txtQuantityMais.Text = string.Empty;
            txtPriceMais.Text = string.Empty;
            txtTotalPriceMais.Text = string.Empty;
        }
        private void LoadRecordsMais()
        {
            lvMais.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string selectQuery = "SELECT * FROM tbl_inventorymais";
                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                                item.SubItems.Add(reader["CUSTOMERNAME"].ToString());
                                item.SubItems.Add(reader["QUANTITY"].ToString());
                                item.SubItems.Add(reader["PRICE"].ToString());
                                item.SubItems.Add(reader["TOTALPRICE"].ToString());

                                string formattedDate = reader["DATE"].ToString();

                                // Use the obtained formatted date
                                item.SubItems.Add(formattedDate);
                                lvMais.Items.Add(item);
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

        private void lvMais_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMais.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvMais.SelectedItems[0];
                txtIDMais.Text = selectedItem.SubItems[0].Text;
                txtCNameMais.Text = selectedItem.SubItems[1].Text;
                txtQuantityMais.Text = selectedItem.SubItems[2].Text;
                txtPriceMais.Text = selectedItem.SubItems[3].Text;
                txtTotalPriceMais.Text = selectedItem.SubItems[4].Text;
            }
        }

        private void btnAddMais_Click(object sender, EventArgs e)
        {
            string CustomerName = txtCNameMais.Text;
            string Quantity = txtQuantityMais.Text;
            string Price = txtPriceMais.Text;
            string tablename = "tbl_inventorymais";

            AddData(tablename, CustomerName, Quantity, Price);
            ClearTextBoxesMais();
        }

        private void btnUpdateMais_Click(object sender, EventArgs e)
        {
            string ID = txtIDMais.Text;
            string CustomerName = txtCNameMais.Text;
            string Quantity = txtQuantityMais.Text;
            string Price = txtPriceMais.Text;
            string tablename = "tbl_inventorymais";

            UpdateData(tablename, ID, CustomerName, Quantity, Price);
            ClearTextBoxesMais();
        }

        private void btnDeleteMais_Click(object sender, EventArgs e)
        {
            string ID = txtIDMais.Text;
            string tablename = "tbl_inventorymais";

            DeleteData(ID, tablename);
            ClearTextBoxesMais();
        }

        private void btnLoadMais_Click(object sender, EventArgs e)
        {
            LoadRecordsMais();
        }

        private void undoMais_Click(object sender, EventArgs e)
        {
            if (lastAction == ActionType.None)
            {
                MessageBox.Show("No action to undo.", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (lastAction == ActionType.Add)
                    {
                        // Undo Add (Delete the last added record)
                        string deleteQuery = "DELETE FROM tbl_inventorymais WHERE ID = @ID";
                        using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (lastAction == ActionType.Update)
                    {
                        // Undo Update (Revert to the previous values)
                        string updateQuery = "UPDATE tbl_inventorymais SET CUSTOMERNAME = @CUSTOMERNAME, QUANTITY = @QUANTITY, PRICE = @PRICE, TOTALPRICE = @TOTALPRICE WHERE ID = @ID";
                        using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                            command.Parameters.AddWithValue("@CUSTOMERNAME", lastProductData["CUSTOMERNAME"]);
                            command.Parameters.AddWithValue("@QUANTITY", lastProductData["QUANTITY"]);
                            command.Parameters.AddWithValue("@PRICE", lastProductData["PRICE"]);
                            command.Parameters.AddWithValue("@TOTALPRICE", lastProductData["TOTALPRICE"]);
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (lastAction == ActionType.Delete)
                    {
                        // Undo Delete (Re-insert the deleted record)
                        string insertQuery = "INSERT INTO tbl_inventorymais (ID, CUSTOMERNAME, QUANTITY, PRICE, TOTALPRICE) " +
                                             "VALUES (@ID, @CUSTOMERNAME, @QUANTITY, @PRICE, @TOTALPRICE)";
                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", lastProductData["ID"]);
                            command.Parameters.AddWithValue("@CUSTOMERNAME", lastProductData["CUSTOMERNAME"]);
                            command.Parameters.AddWithValue("@QUANTITY", lastProductData["QUANTITY"]);
                            command.Parameters.AddWithValue("@PRICE", lastProductData["PRICE"]);
                            command.Parameters.AddWithValue("@TOTALPRICE", lastProductData["TOTALPRICE"]);
                            command.ExecuteNonQuery();
                        }
                    }

                    // Clear last action after undo
                    lastAction = ActionType.None;
                    lastProductData.Clear();
                    MessageBox.Show("Undo action performed successfully!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecordsMais();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void CalculateTotalPriceMais()
        {
            if (int.TryParse(txtQuantityMais.Text, out int quantity) && int.TryParse(txtPriceMais.Text, out int price))
            {
                int totalPrice = quantity * price;
                txtTotalPriceMais.Text = totalPrice.ToString();
            }
        }
        // mais//
        private void txtPriceMais_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPriceMais(); 
            if (int.TryParse(txtPriceMais.Text, out int price))
            {
                if (price < 0)
                {
                    MessageBox.Show("Price cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPriceMais.Clear();
                }
            }
            else if (!string.IsNullOrEmpty(txtPriceMais.Text))
            {
                MessageBox.Show("Please enter a valid numeric value for Price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPriceMais.Clear();
            }
        }

        private void txtTotalPriceMais_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPriceMais();
        }


     
        private void btnSearchMais_Click(object sender, EventArgs e)
        {
            string searchKeyword = txtSearchMais.Text.Trim().ToLower();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = $"SELECT * FROM tbl_inventorymais WHERE CUSTOMERNAME LIKE '%{searchKeyword}%'";

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        lvMais.Items.Clear();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                                item.SubItems.Add(reader["CUSTOMERNAME"].ToString());
                                item.SubItems.Add(reader["QUANTITY"].ToString());
                                item.SubItems.Add(reader["PRICE"].ToString());
                                item.SubItems.Add(reader["TOTALPRICE"].ToString());

                                lvMais.Items.Add(item);
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

        private void btnPrintMais_Click(object sender, EventArgs e)
        {
            isPrinting = true;

            // Create a new PrintDocument
            PrintDocument printDocument = new PrintDocument();

            // Attach event handler for the PrintPage event
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocumentMais_PrintPage);

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
        private void PrintDocumentMais_PrintPage(object sender, PrintPageEventArgs e)
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
            xPos += 120;
            e.Graphics.DrawString("Customer Name", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(xPos, yPos));

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
            for (int i = 0; i <= lvPinlid.Items.Count; i++)
            {
                e.Graphics.DrawLine(gridPen, 30, yPos + i * 15, xPos + 150, yPos + i * 15);
            }

            for (int i = 0; i < 8; i++) // 8 columns in total
            {
                e.Graphics.DrawLine(gridPen, 30 + i * columnWidth, yPos, 30 + i * columnWidth, yPos + lvMais.Items.Count * 15);
            }

            // Code to print ListView items
            foreach (ListViewItem item in lvMais.Items)
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
            e.Graphics.DrawLine(gridPen, dateColumnRightEdge, 90, dateColumnRightEdge, yPos - 20 + lvMais.Items.Count * 8);
        }

        private void txtQuantityMais_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPriceMais();

            if (int.TryParse(txtQuantityMais.Text, out int quantity))
            {
                if (quantity < 0)
                {
                    MessageBox.Show("Quantity cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuantityMais.Clear();
                }
            }
            else if (!string.IsNullOrEmpty(txtQuantity.Text))
            {
                MessageBox.Show("Please enter a valid numeric value for Quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantityMais.Clear();
            }

        }


        private void panelDarak_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Inventory_Load(object sender, EventArgs e)
        {

        }

        private void lvDarak_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
