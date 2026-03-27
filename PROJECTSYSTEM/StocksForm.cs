using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJECTSYSTEM
{
    public partial class StocksForm : Form
    {
        public StocksForm()
        {
            InitializeComponent();
        }

        private void btnRice_Click(object sender, EventArgs e)
        {
            this.Hide();
            RiceForm Rform = new RiceForm();
            Rform.ShowDialog();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            HomePage homefrm = new HomePage();
            homefrm.ShowDialog();
        }

        private void btnDarak_Click(object sender, EventArgs e)
        {
            this.Hide();
            DarakPinlidForm darakPinlidForm = new DarakPinlidForm();
            darakPinlidForm.ShowDialog();
        }

        // X button
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void StocksForm_Load(object sender, EventArgs e)
        {

        }
    }
}
