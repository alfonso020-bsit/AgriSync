//using MySql.Data.MySqlClient;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Globalization;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace PROJECTSYSTEM
//{
//    public partial class HomePage : Form
//    {
//        private string connectionString = "Server=localhost;Database=db_ricemillmanagement;User ID=root;";
//        private CultureInfo philippineCulture = new CultureInfo("fil-PH");
//        public HomePage()
//        {
//            InitializeComponent();
//            GetTotalQuantityOfRice();
//            timer1.Start();

//        }
//        // X BUTTON
//        private void button2_Click(object sender, EventArgs e)
//        {
//            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//            {
//                Application.Exit();
//            }
//        }

//        private void btnRecords_Click(object sender, EventArgs e)
//        {
//            this.Hide();
//            RecordsForm recfrm = new RecordsForm();
//            recfrm.ShowDialog();
//        }

//        private void btnManageUser_Click(object sender, EventArgs e)
//        {
//            this.Hide();
//            ManageUserForm MUserform = new ManageUserForm();
//            MUserform.ShowDialog();
//        }

//        private void btnStacks_Click(object sender, EventArgs e)
//        {
//            this.Hide();
//            StocksForm frm = new StocksForm();
//            frm.ShowDialog();
//        }

//        private void btnSettings_Click(object sender, EventArgs e)
//        {
//            this.Hide();
//            SalesForm Salesfrm = new SalesForm();
//            Salesfrm.ShowDialog();
//        }

//        private void lbllogout_Click(object sender, EventArgs e)
//        {
//            this.Hide();
//            LoginForm LoginForm = new LoginForm();
//            LoginForm.ShowDialog();
//        }


//        private string GetTotalUsers()
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // Get total number of rows in tbl_users
//                    string countQuery = "SELECT COUNT(*) FROM tbl_users";

//                    using (MySqlCommand command = new MySqlCommand(countQuery, connection))
//                    {
//                        long rowCount = (long)command.ExecuteScalar();
//                        return rowCount.ToString();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return null;
//                }
//            }
//        }

//        // Get total sales from the database
//        private string GetTotalSalesSold()
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // Query to sum TOTALPRICE from tbl_products where TRANSACTIONTYPE is 'SELL'
//                    string sumQuery = "SELECT SUM(TOTALPRICE) FROM tbl_products WHERE TRANSACTIONTYPE = @TransactionType";

//                    using (MySqlCommand command = new MySqlCommand(sumQuery, connection))
//                    {
//                        // Add parameter to prevent SQL injection
//                        command.Parameters.AddWithValue("@TransactionType", "SELL");

//                        object result = command.ExecuteScalar();

//                        // Handle if the result is NULL (e.g., no records in tbl_products)
//                        if (result != DBNull.Value && result != null)
//                        {
//                            decimal totalSales = Convert.ToDecimal(result);
//                            return totalSales.ToString("C", philippineCulture); // Format as currency
//                        }
//                        else
//                        {
//                            return "₱0.00"; // Return zero if no sales found
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return null; // Return null in case of an error
//                }
//            }
//        }

//        private string GetTotalSalesService()
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // Query to sum TOTALPRICE from tbl_products where TRANSACTIONTYPE is 'SELL'
//                    string sumQuery = "SELECT SUM(TOTALPRICE) FROM tbl_products WHERE TRANSACTIONTYPE = @TransactionType";

//                    using (MySqlCommand command = new MySqlCommand(sumQuery, connection))
//                    {
//                        // Add parameter to prevent SQL injection
//                        command.Parameters.AddWithValue("@TransactionType", "SERVICE");

//                        object result = command.ExecuteScalar();

//                        // Handle if the result is NULL (e.g., no records in tbl_products)
//                        if (result != DBNull.Value && result != null)
//                        {
//                            decimal totalSales = Convert.ToDecimal(result);
//                            return totalSales.ToString("C", philippineCulture); // Format as currency
//                        }
//                        else
//                        {
//                            return "₱0.00"; // Return zero if no sales found
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return null; // Return null in case of an error
//                }
//            }
//        }



//        private int GetTotalQuantityOfRice()
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // Query to sum QUANTITYPERSACK from tbl_rice
//                    string sumQuery = "SELECT SUM(QUANTITYPERSACK) FROM tbl_rice";

//                    using (MySqlCommand command = new MySqlCommand(sumQuery, connection))
//                    {
//                        object result = command.ExecuteScalar();

//                        // Handle if the result is NULL (e.g., no records in tbl_rice)
//                        if (result != DBNull.Value && result != null)
//                        {
//                            return Convert.ToInt32(result); // Return total quantity as an integer
//                        }
//                        else
//                        {
//                            return 0; // Return zero if no rice found
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return -1; // Return -1 in case of an error to indicate failure
//                }
//            }
//        }


//        private void timer1_Tick(object sender, EventArgs e)
//        {
//            lblTotalUsers.Text = GetTotalUsers();
//            lblTotalSales.Text = GetTotalSalesSold();
//            lblServiceSales.Text = GetTotalSalesService();
//            lblRice.Text = GetTotalQuantityOfRice().ToString();
//            lblDarak.Text = GetTotalDarak().ToString();
//            lblPinlid.Text = GetTotalPinlid().ToString();
//            lblMais.Text = GetTotalMais().ToString();
//            lblInRice.Text = GetTotalRiceInventory().ToString();
//            lblInPinlid.Text = GetTotalPinlidInventory().ToString();
//            lblInDarak.Text = GetTotalDarakInventory().ToString();
//            lblInMais.Text = GetTotalMaisInventory().ToString();
//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
//            this.Hide();
//            Inventory frm = new Inventory();
//            frm.ShowDialog();
//        }

//        private void filterRice_Click(object sender, EventArgs e)
//        {
//            // Check if a variety is selected in the ComboBox
//            if (cmbVName.SelectedItem != null)
//            {
//                timer1.Stop();
//                string selectedVariety = cmbVName.SelectedItem.ToString(); // Get the selected variety
//                int quantityByVariety = GetQuantityByVariety(selectedVariety); // Get the quantity of rice for the selected variety

//                // Display the quantity in lblRice
//                if (quantityByVariety >= 0)
//                {
//                    lblRice.Text = quantityByVariety.ToString(); // Update lblRice with filtered quantity
//                }
//                else
//                {
//                    lblRice.Text = "Error"; // Show error if something goes wrong
//                }
//            }
//            else
//            {
//                MessageBox.Show("Please select a rice variety.", "No Variety Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            }
//        }
//        private int GetQuantityByVariety(string varietyName)
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // SQL query to sum QUANTITYPERSACK for the selected variety
//                    string query = "SELECT SUM(QUANTITYPERSACK) FROM tbl_rice WHERE VARIETYNAME = @varietyName";

//                    using (MySqlCommand command = new MySqlCommand(query, connection))
//                    {
//                        command.Parameters.AddWithValue("@varietyName", varietyName); // Use parameter to avoid SQL injection

//                        object result = command.ExecuteScalar();

//                        // Check if the result is valid (not NULL)
//                        if (result != DBNull.Value && result != null)
//                        {
//                            return Convert.ToInt32(result); // Return the total quantity for the selected variety
//                        }
//                        else
//                        {
//                            return 0; // If no data for the selected variety, return 0
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return -1; // In case of an error, return -1 to indicate failure
//                }
//            }
//        }

//        private void refreshRice_Click(object sender, EventArgs e)
//        {
//            lblRice.Text = GetTotalQuantityOfRice().ToString();
//            timer1.Start();
//            cmbVName.SelectedIndex = -1;
//        }

//        private int GetTotalDarak()
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // SQL query to sum QUANTITYPERSACK where PRODUCTNAME is 'Darak'
//                    string query = "SELECT SUM(QUANTITYPERSACK) FROM tbl_darakpinlid WHERE PRODUCTNAME = 'Darak'";

//                    using (MySqlCommand command = new MySqlCommand(query, connection))
//                    {
//                        object result = command.ExecuteScalar();

//                        // Check if the result is valid (not NULL)
//                        if (result != DBNull.Value && result != null)
//                        {
//                            return Convert.ToInt32(result); // Return the total quantity of Darak
//                        }
//                        else
//                        {
//                            return 0; // Return 0 if no records found for Darak
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return -1; // Return -1 in case of error
//                }
//            }
//        }
//        private int GetTotalPinlid()
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // SQL query to sum QUANTITYPERSACK where PRODUCTNAME is 'Pinlid'
//                    string query = "SELECT SUM(QUANTITYPERSACK) FROM tbl_darakpinlid WHERE PRODUCTNAME = 'Pinlid'";

//                    using (MySqlCommand command = new MySqlCommand(query, connection))
//                    {
//                        object result = command.ExecuteScalar();

//                        // Check if the result is valid (not NULL)
//                        if (result != DBNull.Value && result != null)
//                        {
//                            return Convert.ToInt32(result); // Return the total quantity of Pinlid
//                        }
//                        else
//                        {
//                            return 0; // Return 0 if no records found for Pinlid
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return -1; // Return -1 in case of error
//                }
//            }
//        }

//        private int GetTotalMais()
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // SQL query to sum QUANTITYPERSACK where PRODUCTNAME is 'Pinlid'
//                    string query = "SELECT SUM(QUANTITYPERSACK) FROM tbl_darakpinlid WHERE PRODUCTNAME = 'Mais'";

//                    using (MySqlCommand command = new MySqlCommand(query, connection))
//                    {
//                        object result = command.ExecuteScalar();

//                        // Check if the result is valid (not NULL)
//                        if (result != DBNull.Value && result != null)
//                        {
//                            return Convert.ToInt32(result); // Return the total quantity of Pinlid
//                        }
//                        else
//                        {
//                            return 0; // Return 0 if no records found for Pinlid
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return -1; // Return -1 in case of error
//                }
//            }
//        }

//        private int GetTotalRiceInventory()
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // SQL query to sum QUANTITY from tbl_inventoryrice
//                    string query = "SELECT SUM(QUANTITY) FROM tbl_inventoryrice";

//                    using (MySqlCommand command = new MySqlCommand(query, connection))
//                    {
//                        object result = command.ExecuteScalar();

//                        // Check if the result is valid (not NULL)
//                        if (result != DBNull.Value && result != null)
//                        {
//                            return Convert.ToInt32(result); // Return the total rice quantity
//                        }
//                        else
//                        {
//                            return 0; // Return 0 if no records found in tbl_inventoryrice
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return -1; // Return -1 in case of error
//                }
//            }
//        }
//        private int GetTotalPinlidInventory()
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // SQL query to sum QUANTITY from tbl_inventoryrice
//                    string query = "SELECT SUM(QUANTITY) FROM tbl_inventorypinlid";

//                    using (MySqlCommand command = new MySqlCommand(query, connection))
//                    {
//                        object result = command.ExecuteScalar();

//                        // Check if the result is valid (not NULL)
//                        if (result != DBNull.Value && result != null)
//                        {
//                            return Convert.ToInt32(result); 
//                        }
//                        else
//                        {
//                            return 0; 
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return -1; // Return -1 in case of error
//                }
//            }
//        }

//        private int GetTotalDarakInventory()
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // SQL query to sum QUANTITY from tbl_inventoryrice
//                    string query = "SELECT SUM(QUANTITY) FROM tbl_inventorydarak";

//                    using (MySqlCommand command = new MySqlCommand(query, connection))
//                    {
//                        object result = command.ExecuteScalar();

//                        // Check if the result is valid (not NULL)
//                        if (result != DBNull.Value && result != null)
//                        {
//                            return Convert.ToInt32(result); 
//                        }
//                        else
//                        {
//                            return 0; // Return 0 if no records found in tbl_inventoryrice
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return -1; // Return -1 in case of error
//                }
//            }
//        }
//        private int GetTotalMaisInventory()
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // SQL query to sum QUANTITY from tbl_inventoryrice
//                    string query = "SELECT SUM(QUANTITY) FROM tbl_inventorymais";

//                    using (MySqlCommand command = new MySqlCommand(query, connection))
//                    {
//                        object result = command.ExecuteScalar();

//                        // Check if the result is valid (not NULL)
//                        if (result != DBNull.Value && result != null)
//                        {
//                            return Convert.ToInt32(result); 
//                        }
//                        else
//                        {
//                            return 0; 
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return -1; 
//                }
//            }
//        }

//    }
//}


using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJECTSYSTEM
{
    public partial class HomePage : Form
    {
        private string connectionString = "Server=localhost;Database=db_ricemillmanagement;User ID=root;";
        private CultureInfo philippineCulture = new CultureInfo("fil-PH");
        public HomePage()
        {
            InitializeComponent();
            GetTotalQuantityOfRice();
            timer1.Start();

        }
        // X BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnRecords_Click(object sender, EventArgs e)
        {
            this.Hide();
            RecordsForm recfrm = new RecordsForm();
            recfrm.ShowDialog();
        }

        private void btnManageUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageUserForm MUserform = new ManageUserForm();
            MUserform.ShowDialog();
        }

        private void btnStacks_Click(object sender, EventArgs e)
        {
            this.Hide();
            StocksForm frm = new StocksForm();
            frm.ShowDialog();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            this.Hide();
            SalesForm Salesfrm = new SalesForm();
            Salesfrm.ShowDialog();
        }

        private void lbllogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm LoginForm = new LoginForm();
            LoginForm.ShowDialog();
        }


        private string GetTotalUsers()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get total number of rows in tbl_users
                    string countQuery = "SELECT COUNT(*) FROM tbl_users";

                    using (MySqlCommand command = new MySqlCommand(countQuery, connection))
                    {
                        long rowCount = (long)command.ExecuteScalar();
                        return rowCount.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        // Get total sales from the database
        private string GetTotalSalesSold()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Query to sum TOTALPRICE from tbl_products where TRANSACTIONTYPE is 'SELL'
                    string sumQuery = "SELECT SUM(TOTALPRICE) FROM tbl_products WHERE TRANSACTIONTYPE = @TransactionType";

                    using (MySqlCommand command = new MySqlCommand(sumQuery, connection))
                    {
                        // Add parameter to prevent SQL injection
                        command.Parameters.AddWithValue("@TransactionType", "SELL");

                        object result = command.ExecuteScalar();

                        // Handle if the result is NULL (e.g., no records in tbl_products)
                        if (result != DBNull.Value && result != null)
                        {
                            decimal totalSales = Convert.ToDecimal(result);
                            return totalSales.ToString("C", philippineCulture); // Format as currency
                        }
                        else
                        {
                            return "₱0.00"; // Return zero if no sales found
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null; // Return null in case of an error
                }
            }
        }

        private string GetTotalSalesService()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Query to sum TOTALPRICE from tbl_products where TRANSACTIONTYPE is 'SELL'
                    string sumQuery = "SELECT SUM(TOTALPRICE) FROM tbl_products WHERE TRANSACTIONTYPE = @TransactionType";

                    using (MySqlCommand command = new MySqlCommand(sumQuery, connection))
                    {
                        // Add parameter to prevent SQL injection
                        command.Parameters.AddWithValue("@TransactionType", "SERVICE");

                        object result = command.ExecuteScalar();

                        // Handle if the result is NULL (e.g., no records in tbl_products)
                        if (result != DBNull.Value && result != null)
                        {
                            decimal totalSales = Convert.ToDecimal(result);
                            return totalSales.ToString("C", philippineCulture); // Format as currency
                        }
                        else
                        {
                            return "₱0.00"; // Return zero if no sales found
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null; // Return null in case of an error
                }
            }
        }



        private int GetTotalQuantityOfRice()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Query to sum QUANTITYPERSACK from tbl_rice
                    string sumQuery = "SELECT SUM(QUANTITYPERSACK) FROM tbl_rice";

                    using (MySqlCommand command = new MySqlCommand(sumQuery, connection))
                    {
                        object result = command.ExecuteScalar();

                        // Handle if the result is NULL (e.g., no records in tbl_rice)
                        if (result != DBNull.Value && result != null)
                        {
                            return Convert.ToInt32(result); // Return total quantity as an integer
                        }
                        else
                        {
                            return 0; // Return zero if no rice found
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1; // Return -1 in case of an error to indicate failure
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTotalUsers.Text = GetTotalUsers();
            lblTotalSales.Text = GetTotalSalesSold();
            lblServiceSales.Text = GetTotalSalesService();
            lblRice.Text = GetTotalQuantityOfRice().ToString();
            lblDarak.Text = GetTotalDarak().ToString();
            lblPinlid.Text = GetTotalPinlid().ToString();
            lblMais.Text = GetTotalMais().ToString();
            lblInRice.Text = GetTotalRiceInventory().ToString();
            lblInPinlid.Text = GetTotalPinlidInventory().ToString();
            lblInDarak.Text = GetTotalDarakInventory().ToString();
            lblInMais.Text = GetTotalMaisInventory().ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Inventory frm = new Inventory();
            frm.ShowDialog();
        }

        private void filterRice_Click(object sender, EventArgs e)
        {
            // Check if a variety is selected in the ComboBox
            if (cmbVName.SelectedItem != null)
            {
                timer1.Stop();
                string selectedVariety = cmbVName.SelectedItem.ToString(); // Get the selected variety
                int quantityByVariety = GetQuantityByVariety(selectedVariety); // Get the quantity of rice for the selected variety

                // Display the quantity in lblRice
                if (quantityByVariety >= 0)
                {
                    lblRice.Text = quantityByVariety.ToString(); // Update lblRice with filtered quantity
                }
                else
                {
                    lblRice.Text = "Error"; // Show error if something goes wrong
                }
            }
            else
            {
                MessageBox.Show("Please select a rice variety.", "No Variety Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private int GetQuantityByVariety(string varietyName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to sum QUANTITYPERSACK for the selected variety
                    string query = "SELECT SUM(QUANTITYPERSACK) FROM tbl_rice WHERE VARIETYNAME = @varietyName";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@varietyName", varietyName); // Use parameter to avoid SQL injection

                        object result = command.ExecuteScalar();

                        // Check if the result is valid (not NULL)
                        if (result != DBNull.Value && result != null)
                        {
                            return Convert.ToInt32(result); // Return the total quantity for the selected variety
                        }
                        else
                        {
                            return 0; // If no data for the selected variety, return 0
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1; // In case of an error, return -1 to indicate failure
                }
            }
        }

        private void refreshRice_Click(object sender, EventArgs e)
        {
            lblRice.Text = GetTotalQuantityOfRice().ToString();
            timer1.Start();
            cmbVName.SelectedIndex = -1;
        }

        private int GetTotalDarak()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to sum QUANTITYPERSACK where PRODUCTNAME is 'Darak'
                    string query = "SELECT SUM(QUANTITYPERSACK) FROM tbl_darakpinlid WHERE PRODUCTNAME = 'Darak'";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();

                        // Check if the result is valid (not NULL)
                        if (result != DBNull.Value && result != null)
                        {
                            return Convert.ToInt32(result); // Return the total quantity of Darak
                        }
                        else
                        {
                            return 0; // Return 0 if no records found for Darak
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1; // Return -1 in case of error
                }
            }
        }
        private int GetTotalPinlid()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to sum QUANTITYPERSACK where PRODUCTNAME is 'Pinlid'
                    string query = "SELECT SUM(QUANTITYPERSACK) FROM tbl_darakpinlid WHERE PRODUCTNAME = 'Pinlid'";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();

                        // Check if the result is valid (not NULL)
                        if (result != DBNull.Value && result != null)
                        {
                            return Convert.ToInt32(result); // Return the total quantity of Pinlid
                        }
                        else
                        {
                            return 0; // Return 0 if no records found for Pinlid
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1; // Return -1 in case of error
                }
            }
        }

        private int GetTotalMais()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to sum QUANTITYPERSACK where PRODUCTNAME is 'Pinlid'
                    string query = "SELECT SUM(QUANTITYPERSACK) FROM tbl_darakpinlid WHERE PRODUCTNAME = 'Mais'";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();

                        // Check if the result is valid (not NULL)
                        if (result != DBNull.Value && result != null)
                        {
                            return Convert.ToInt32(result); // Return the total quantity of Pinlid
                        }
                        else
                        {
                            return 0; // Return 0 if no records found for Pinlid
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1; // Return -1 in case of error
                }
            }
        }

        private int GetTotalRiceInventory()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to sum QUANTITY from tbl_inventoryrice
                    string query = "SELECT SUM(QUANTITY) FROM tbl_inventoryrice";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();

                        // Check if the result is valid (not NULL)
                        if (result != DBNull.Value && result != null)
                        {
                            return Convert.ToInt32(result); // Return the total rice quantity
                        }
                        else
                        {
                            return 0; // Return 0 if no records found in tbl_inventoryrice
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1; // Return -1 in case of error
                }
            }
        }
        private int GetTotalPinlidInventory()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to sum QUANTITY from tbl_inventoryrice
                    string query = "SELECT SUM(QUANTITY) FROM tbl_inventorypinlid";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();

                        // Check if the result is valid (not NULL)
                        if (result != DBNull.Value && result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1; // Return -1 in case of error
                }
            }
        }

        private int GetTotalDarakInventory()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to sum QUANTITY from tbl_inventoryrice
                    string query = "SELECT SUM(QUANTITY) FROM tbl_inventorydarak";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();

                        // Check if the result is valid (not NULL)
                        if (result != DBNull.Value && result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            return 0; // Return 0 if no records found in tbl_inventoryrice
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1; // Return -1 in case of error
                }
            }
        }
        private int GetTotalMaisInventory()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to sum QUANTITY from tbl_inventoryrice
                    string query = "SELECT SUM(QUANTITY) FROM tbl_inventorymais";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();

                        // Check if the result is valid (not NULL)
                        if (result != DBNull.Value && result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
        }

        private void filterSales_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFrom.Value;
            DateTime toDate = dtpTo.Value;
            timer1.Stop();

            if (fromDate <= toDate)
            {
                lblTotalSales.Text = GetFilteredSales("SELL", fromDate, toDate);
                lblServiceSales.Text = GetFilteredSales("SERVICE", fromDate, toDate);
            }
            else
            {
                MessageBox.Show("The 'From' date cannot be later than the 'To' date.", "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private string GetFilteredSales(string transactionType, DateTime fromDate, DateTime toDate)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT SUM(TOTALPRICE) FROM tbl_products WHERE TRANSACTIONTYPE = @TransactionType AND DATE >= @FromDate AND DATE <= @ToDate";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TransactionType", transactionType);
                        command.Parameters.AddWithValue("@FromDate", fromDate);
                        command.Parameters.AddWithValue("@ToDate", toDate);
                        object result = command.ExecuteScalar();

                        if (result != DBNull.Value && result != null)
                        {
                            decimal totalSales = Convert.ToDecimal(result);
                            return totalSales.ToString("C", philippineCulture);
                        }
                        else
                        {
                            return "₱0.00"; // Default if no sales found
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error retrieving filtered sales: {ex.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "₱0.00"; // Default value
                }
            }
        }

        private void refreshSales_Click(object sender, EventArgs e)
        {
            lblTotalSales.Text = GetTotalSalesSold();
            lblServiceSales.Text = GetTotalSalesService();
            dtpFrom.Value = DateTime.Now; // Reset date pickers to current date
            dtpTo.Value = DateTime.Now; // Reset date pickers to current date
            timer1.Start();
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void HomePage_Load(object sender, EventArgs e)
        {

        }
    }
}
