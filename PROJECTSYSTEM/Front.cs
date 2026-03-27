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
    public partial class Front : Form
    {
        public Front()
        {
            InitializeComponent();
        }
        int startpoint = 0;
        private void Fronttimer_Tick(object sender, EventArgs e)
        {
            startpoint += 1;
            progressBar1.Value = startpoint;

            // Display the ProgressBar value in the ProgressStatus label
            ProgressStatus.Text = $"{progressBar1.Value}%";

            if (progressBar1.Value ==1)
            {
                progressBar1.Value = 0;
                Fronttimer.Stop();
                SignUpForm spalsh = new SignUpForm();
                this.Hide();
                spalsh.Show();
            }
        }

        private void Front_Load(object sender, EventArgs e)
        {
            Fronttimer.Start();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void ProgressStatus_Click(object sender, EventArgs e)
        {

        }
    }
}
