namespace PROJECTSYSTEM
{
    partial class DarakPinlidForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DarakPinlidForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDarak = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPName = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lvDarakPinlid = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PRODUCTNAME = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.QUANTITYPERSACK = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PRICEPERSACK = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.DarakTimer = new System.Windows.Forms.Timer(this.components);
            this.undoDarakPinlid = new System.Windows.Forms.PictureBox();
            this.btnLoadDarakPinlid = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.undoDarakPinlid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.ForestGreen;
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 100);
            this.panel1.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.ForestGreen;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI Black", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(1170, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(30, 29);
            this.button2.TabIndex = 5;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(118, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(145, 91);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(344, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(625, 50);
            this.label2.TabIndex = 1;
            this.label2.Text = "RICE MILL MANAGEMENT SYSTEM";
            // 
            // btnDarak
            // 
            this.btnDarak.BackColor = System.Drawing.Color.Bisque;
            this.btnDarak.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDarak.BackgroundImage")));
            this.btnDarak.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDarak.Location = new System.Drawing.Point(44, 106);
            this.btnDarak.Name = "btnDarak";
            this.btnDarak.Size = new System.Drawing.Size(178, 128);
            this.btnDarak.TabIndex = 12;
            this.btnDarak.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(244, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(224, 40);
            this.label3.TabIndex = 13;
            this.label3.Text = "DARAK/PINLID";
            // 
            // txtID
            // 
            this.txtID.Enabled = false;
            this.txtID.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtID.Location = new System.Drawing.Point(261, 298);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(302, 33);
            this.txtID.TabIndex = 0;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuantity.Location = new System.Drawing.Point(261, 430);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(302, 33);
            this.txtQuantity.TabIndex = 2;
            this.txtQuantity.TextChanged += new System.EventHandler(this.txtQuantity_TextChanged);
            // 
            // txtPrice
            // 
            this.txtPrice.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrice.Location = new System.Drawing.Point(261, 488);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(302, 33);
            this.txtPrice.TabIndex = 3;
            this.txtPrice.TextChanged += new System.EventHandler(this.txtPrice_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(48, 306);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 25);
            this.label6.TabIndex = 31;
            this.label6.Text = "ID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(48, 433);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(174, 25);
            this.label4.TabIndex = 32;
            this.label4.Text = "Quantity PER Sack";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(48, 496);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 25);
            this.label1.TabIndex = 33;
            this.label1.Text = "Price PER Sack";
            // 
            // cmbPName
            // 
            this.cmbPName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPName.FormattingEnabled = true;
            this.cmbPName.Items.AddRange(new object[] {
            "Darak",
            "Pinlid",
            "Mais"});
            this.cmbPName.Location = new System.Drawing.Point(261, 363);
            this.cmbPName.Name = "cmbPName";
            this.cmbPName.Size = new System.Drawing.Size(302, 32);
            this.cmbPName.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(48, 365);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 25);
            this.label5.TabIndex = 35;
            this.label5.Text = "Product Name";
            // 
            // lvDarakPinlid
            // 
            this.lvDarakPinlid.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.PRODUCTNAME,
            this.QUANTITYPERSACK,
            this.PRICEPERSACK});
            this.lvDarakPinlid.FullRowSelect = true;
            this.lvDarakPinlid.GridLines = true;
            this.lvDarakPinlid.HideSelection = false;
            this.lvDarakPinlid.Location = new System.Drawing.Point(609, 165);
            this.lvDarakPinlid.Name = "lvDarakPinlid";
            this.lvDarakPinlid.Size = new System.Drawing.Size(558, 530);
            this.lvDarakPinlid.TabIndex = 36;
            this.lvDarakPinlid.UseCompatibleStateImageBehavior = false;
            this.lvDarakPinlid.View = System.Windows.Forms.View.Details;
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 100;
            // 
            // PRODUCTNAME
            // 
            this.PRODUCTNAME.Text = "PRODUCTNAME";
            this.PRODUCTNAME.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PRODUCTNAME.Width = 150;
            // 
            // QUANTITYPERSACK
            // 
            this.QUANTITYPERSACK.Text = "QUANTITY PER SACK";
            this.QUANTITYPERSACK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.QUANTITYPERSACK.Width = 150;
            // 
            // PRICEPERSACK
            // 
            this.PRICEPERSACK.Text = "PRICE PER SACK";
            this.PRICEPERSACK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PRICEPERSACK.Width = 150;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.LawnGreen;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(105, 554);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(84, 39);
            this.btnAdd.TabIndex = 37;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.Gold;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(251, 554);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(84, 39);
            this.btnUpdate.TabIndex = 38;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Red;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(401, 554);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(84, 39);
            this.btnDelete.TabIndex = 39;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.ForestGreen;
            this.panel2.Location = new System.Drawing.Point(0, 788);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1203, 19);
            this.panel2.TabIndex = 40;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.Gold;
            this.btnBack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBack.BackgroundImage")));
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.Location = new System.Drawing.Point(0, 678);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(68, 46);
            this.btnBack.TabIndex = 23;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(698, 126);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(378, 33);
            this.txtSearch.TabIndex = 41;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LawnGreen;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(1082, 126);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(84, 34);
            this.btnSearch.TabIndex = 42;
            this.btnSearch.Text = "SEARCH";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // DarakTimer
            // 
            this.DarakTimer.Tick += new System.EventHandler(this.DarakTimer_Tick);
            // 
            // undoDarakPinlid
            // 
            this.undoDarakPinlid.BackColor = System.Drawing.Color.Moccasin;
            this.undoDarakPinlid.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("undoDarakPinlid.BackgroundImage")));
            this.undoDarakPinlid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.undoDarakPinlid.Cursor = System.Windows.Forms.Cursors.Default;
            this.undoDarakPinlid.Location = new System.Drawing.Point(525, 264);
            this.undoDarakPinlid.Name = "undoDarakPinlid";
            this.undoDarakPinlid.Size = new System.Drawing.Size(26, 28);
            this.undoDarakPinlid.TabIndex = 63;
            this.undoDarakPinlid.TabStop = false;
            this.undoDarakPinlid.Click += new System.EventHandler(this.undoDarakPinlid_Click);
            // 
            // btnLoadDarakPinlid
            // 
            this.btnLoadDarakPinlid.BackColor = System.Drawing.Color.Orange;
            this.btnLoadDarakPinlid.FlatAppearance.BorderSize = 0;
            this.btnLoadDarakPinlid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadDarakPinlid.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadDarakPinlid.Location = new System.Drawing.Point(210, 624);
            this.btnLoadDarakPinlid.Name = "btnLoadDarakPinlid";
            this.btnLoadDarakPinlid.Size = new System.Drawing.Size(160, 31);
            this.btnLoadDarakPinlid.TabIndex = 64;
            this.btnLoadDarakPinlid.Text = "LOAD RECORDS";
            this.btnLoadDarakPinlid.UseVisualStyleBackColor = false;
            this.btnLoadDarakPinlid.Click += new System.EventHandler(this.btnLoadDarakPinlid_Click);
            // 
            // DarakPinlidForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1200, 736);
            this.Controls.Add(this.btnLoadDarakPinlid);
            this.Controls.Add(this.undoDarakPinlid);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvDarakPinlid);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbPName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnDarak);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DarakPinlidForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DarakPinlidForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.undoDarakPinlid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDarak;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView lvDarakPinlid;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader PRODUCTNAME;
        private System.Windows.Forms.ColumnHeader QUANTITYPERSACK;
        private System.Windows.Forms.ColumnHeader PRICEPERSACK;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Timer DarakTimer;
        private System.Windows.Forms.PictureBox undoDarakPinlid;
        private System.Windows.Forms.Button btnLoadDarakPinlid;
    }
}