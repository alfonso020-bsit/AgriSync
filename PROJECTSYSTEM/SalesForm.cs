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
using MySql.Data.MySqlClient;

namespace PROJECTSYSTEM
{
    public partial class SalesForm : Form
    {

        private PrintDocument printDocument;
        private PrintPreviewDialog printPreviewDialog;

        private static string connectionString = "Server=localhost;Database=db_ricemillmanagement;Uid=root;";

        public SalesForm()
        {
            InitializeComponent();
            cmbProducts.Items.AddRange(new string[] { "Rice", "Pinlid", "Darak" , "Mais"});

            // Initialize PrintDocument and PrintPreviewDialog
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;

            printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string selectedProduct = cmbProducts.SelectedItem?.ToString();
            string selectedYear = txtYear.Text;
            string selectedMonth = txtMonth.Text;
            string selectedDay = txtDay.Text;

            // Construct your SQL query based on the selected criteria
            string query = "SELECT * FROM tbl_products WHERE 1 = 1"; // Always true condition to allow appending conditions

            if (!string.IsNullOrEmpty(selectedProduct))
            {
                query += $" AND PRODUCTNAME = '{selectedProduct}'";
            }

            if (!string.IsNullOrEmpty(selectedYear))
            {
                query += $" AND YEAR(DATE) = {selectedYear}";
            }

            if (!string.IsNullOrEmpty(selectedMonth))
            {
                query += $" AND MONTH(DATE) = {selectedMonth}";
            }

            if (!string.IsNullOrEmpty(selectedDay))
            {
                query += $" AND DAY(DATE) = {selectedDay}";
            }
            Console.WriteLine("Generated Query: " + query);
            // Execute the query and populate the ListView
            try
            {
                FillListView(query);
                CalculateTotalSales();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FillListView(string query)
        {
            lvResults.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        ListViewItem item = new ListViewItem(row["ID"].ToString()); // Adjust column names
                        item.SubItems.Add(row["CUSTOMERNAME"].ToString());
                        item.SubItems.Add(row["PRODUCTNAME"].ToString());
                        item.SubItems.Add(row["TRANSACTIONTYPE"].ToString());
                        item.SubItems.Add(row["QUANTITY"].ToString());
                        item.SubItems.Add(row["PRICE"].ToString());
                        item.SubItems.Add(row["TOTALPRICE"].ToString());
                        item.SubItems.Add(row["DATE"].ToString());

                        lvResults.Items.Add(item);
                    }
                }
            }
        }
        private void CalculateTotalSales()
        {
            double totalSales = 0;

            foreach (ListViewItem item in lvResults.Items)
            {
                // Assuming "TOTALPRICE" is the seventh column, adjust the index accordingly
                totalSales += double.Parse(item.SubItems[6].Text);
            }

            txtTotalSales.Text = totalSales.ToString();
        }

        //Exit Button
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            HomePage homePage = new HomePage();
            homePage.Show();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            using (PrintDialog printDialog = new PrintDialog())
            {
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    // Set the page orientation based on the selected print options
                    printDocument.DefaultPageSettings.Landscape = printDialog.PrinterSettings.DefaultPageSettings.Landscape;

                    // Show print preview dialog
                    printPreviewDialog.ShowDialog();
                }
            }
        }
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            string headerText = "MORTEL RICE MILL";
            Font headerFont = new Font("Tahoma", 12, FontStyle.Bold);
            SizeF headerSize = e.Graphics.MeasureString(headerText, headerFont);
            float headerX = (e.PageBounds.Width - headerSize.Width) / 2;
            e.Graphics.DrawString(headerText, headerFont, Brushes.Black, new PointF(headerX, 10));

            string salesRecordText = "SALES RECORD";
            Font salesRecordFont = new Font("Tahoma", 9, FontStyle.Bold);
            SizeF salesRecordSize = e.Graphics.MeasureString(salesRecordText, salesRecordFont);
            float salesRecordX = (e.PageBounds.Width - salesRecordSize.Width) / 2;
            float salesRecordY = 5 + headerSize.Height + 10; // Add some space (adjust as needed)
            e.Graphics.DrawString(salesRecordText, salesRecordFont, Brushes.Black, new PointF(salesRecordX, salesRecordY));

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
            for (int i = 0; i <= lvResults.Items.Count; i++)
            {
                e.Graphics.DrawLine(gridPen, 30, yPos + i * 20, xPos + 150, yPos + i * 20);
            }

            for (int i = 0; i < 8; i++) // 8 columns in total
            {
                e.Graphics.DrawLine(gridPen, 30 + i * columnWidth, yPos, 30 + i * columnWidth, yPos + lvResults.Items.Count * 20);
            }

            // Code to print ListView items
            foreach (ListViewItem item in lvResults.Items)
            {
                xPos = 30; // Reset starting position for each item
                e.Graphics.DrawString(item.Text, new Font("Arial", 10), Brushes.Black, new Point(xPos, yPos));

                xPos += columnWidth; // Adjust the spacing between columns as needed
                for (int i = 1; i < item.SubItems.Count; i++)
                {
                    e.Graphics.DrawString(item.SubItems[i].Text, new Font("Arial", 10), Brushes.Black, new Point(xPos, yPos));
                    xPos += columnWidth; // Adjust the spacing between columns as needed
                }

                yPos += 20; // Adjust the spacing between items as needed
            }

            // Calculate the right edge of the printed text in the "Date" column
            int dateColumnRightEdge = xPos + e.Graphics.MeasureString("Date", new Font("Arial", 10)).ToSize().Width;

            // Draw the last vertical grid line (after printing items)
            e.Graphics.DrawLine(gridPen, dateColumnRightEdge, 90, dateColumnRightEdge, yPos - 20 + lvResults.Items.Count * 20);

            // Draw a horizontal line below the ListView items
            yPos += 10;
            e.Graphics.DrawLine(gridPen, 30, yPos, dateColumnRightEdge, yPos);

            // Print total sales at the rightmost side
            yPos += 20; // Adjust the spacing between the line and total sales
            string totalSalesText = $"TOTAL SALES: {txtTotalSales.Text}";
            float totalSalesWidth = e.Graphics.MeasureString(totalSalesText, new Font("Arial", 10, FontStyle.Bold)).Width;
            float totalSalesX = e.PageBounds.Width - totalSalesWidth - 30;
            e.Graphics.DrawString(totalSalesText, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new PointF(totalSalesX, yPos));
        }
    }
}

