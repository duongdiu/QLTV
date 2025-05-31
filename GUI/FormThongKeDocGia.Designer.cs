namespace GUI
{
    partial class FormThongKeDocGia
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormThongKeDocGia));
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboThongKeDocGia = new System.Windows.Forms.ComboBox();
            this.btnThongKeDG = new System.Windows.Forms.Button();
            this.btnTimKiemDG = new System.Windows.Forms.Button();
            this.dgvThongKeDocGia = new System.Windows.Forms.DataGridView();
            this.txtTimKiemDG = new System.Windows.Forms.TextBox();
            this.btnLamMoiDG = new System.Windows.Forms.Button();
            this.btnXuatExcelDG = new System.Windows.Forms.Button();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPage = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongKeDocGia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(603, 642);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(48, 46);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 105;
            this.pictureBox4.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(682, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(465, 35);
            this.label1.TabIndex = 96;
            this.label1.Text = "BÁO CÁO THỐNG KÊ ĐỘC GIẢ";
            // 
            // cboThongKeDocGia
            // 
            this.cboThongKeDocGia.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.cboThongKeDocGia.FormattingEnabled = true;
            this.cboThongKeDocGia.Location = new System.Drawing.Point(331, 123);
            this.cboThongKeDocGia.Name = "cboThongKeDocGia";
            this.cboThongKeDocGia.Size = new System.Drawing.Size(251, 34);
            this.cboThongKeDocGia.TabIndex = 102;
            // 
            // btnThongKeDG
            // 
            this.btnThongKeDG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnThongKeDG.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnThongKeDG.BackgroundImage")));
            this.btnThongKeDG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnThongKeDG.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThongKeDG.Location = new System.Drawing.Point(614, 114);
            this.btnThongKeDG.Name = "btnThongKeDG";
            this.btnThongKeDG.Size = new System.Drawing.Size(66, 57);
            this.btnThongKeDG.TabIndex = 101;
            this.btnThongKeDG.UseVisualStyleBackColor = false;
            this.btnThongKeDG.Click += new System.EventHandler(this.btnThongKeDG_Click_1);
            // 
            // btnTimKiemDG
            // 
            this.btnTimKiemDG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnTimKiemDG.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTimKiemDG.BackgroundImage")));
            this.btnTimKiemDG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTimKiemDG.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnTimKiemDG.Location = new System.Drawing.Point(1563, 114);
            this.btnTimKiemDG.Name = "btnTimKiemDG";
            this.btnTimKiemDG.Size = new System.Drawing.Size(64, 57);
            this.btnTimKiemDG.TabIndex = 100;
            this.btnTimKiemDG.UseVisualStyleBackColor = false;
            this.btnTimKiemDG.Click += new System.EventHandler(this.btnTimKiemDG_Click_1);
            // 
            // dgvThongKeDocGia
            // 
            this.dgvThongKeDocGia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThongKeDocGia.Location = new System.Drawing.Point(174, 226);
            this.dgvThongKeDocGia.Name = "dgvThongKeDocGia";
            this.dgvThongKeDocGia.RowHeadersWidth = 51;
            this.dgvThongKeDocGia.RowTemplate.Height = 24;
            this.dgvThongKeDocGia.Size = new System.Drawing.Size(1463, 336);
            this.dgvThongKeDocGia.TabIndex = 98;
            // 
            // txtTimKiemDG
            // 
            this.txtTimKiemDG.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtTimKiemDG.Location = new System.Drawing.Point(1244, 127);
            this.txtTimKiemDG.Name = "txtTimKiemDG";
            this.txtTimKiemDG.Size = new System.Drawing.Size(278, 34);
            this.txtTimKiemDG.TabIndex = 99;
            // 
            // btnLamMoiDG
            // 
            this.btnLamMoiDG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnLamMoiDG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLamMoiDG.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnLamMoiDG.Location = new System.Drawing.Point(1098, 628);
            this.btnLamMoiDG.Margin = new System.Windows.Forms.Padding(4);
            this.btnLamMoiDG.Name = "btnLamMoiDG";
            this.btnLamMoiDG.Size = new System.Drawing.Size(185, 72);
            this.btnLamMoiDG.TabIndex = 97;
            this.btnLamMoiDG.Text = "         Làm mới";
            this.btnLamMoiDG.UseVisualStyleBackColor = false;
            this.btnLamMoiDG.Click += new System.EventHandler(this.btnLamMoiDG_Click_1);
            // 
            // btnXuatExcelDG
            // 
            this.btnXuatExcelDG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnXuatExcelDG.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXuatExcelDG.Location = new System.Drawing.Point(592, 628);
            this.btnXuatExcelDG.Name = "btnXuatExcelDG";
            this.btnXuatExcelDG.Size = new System.Drawing.Size(212, 72);
            this.btnXuatExcelDG.TabIndex = 95;
            this.btnXuatExcelDG.Text = "        Xuất Excel";
            this.btnXuatExcelDG.UseVisualStyleBackColor = false;
            this.btnXuatExcelDG.Click += new System.EventHandler(this.btnXuatExcelDG_Click_1);
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(1110, 640);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(53, 48);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox5.TabIndex = 106;
            this.pictureBox5.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.Location = new System.Drawing.Point(169, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 25);
            this.label3.TabIndex = 107;
            this.label3.Text = "Thống kê:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(1093, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 25);
            this.label2.TabIndex = 108;
            this.label2.Text = "Tìm kiếm:";
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblPage.Location = new System.Drawing.Point(917, 588);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(20, 22);
            this.lblPage.TabIndex = 111;
            this.lblPage.Text = "0";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(1023, 587);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 110;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(836, 587);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(75, 23);
            this.btnPrev.TabIndex = 109;
            this.btnPrev.Text = "<<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // FormThongKeDocGia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1874, 763);
            this.Controls.Add(this.lblPage);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboThongKeDocGia);
            this.Controls.Add(this.btnThongKeDG);
            this.Controls.Add(this.btnTimKiemDG);
            this.Controls.Add(this.dgvThongKeDocGia);
            this.Controls.Add(this.txtTimKiemDG);
            this.Controls.Add(this.btnLamMoiDG);
            this.Controls.Add(this.btnXuatExcelDG);
            this.Name = "FormThongKeDocGia";
            this.Text = "FormThongKeDocGia";
            this.Load += new System.EventHandler(this.FormThongKeDocGia_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongKeDocGia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboThongKeDocGia;
        private System.Windows.Forms.Button btnThongKeDG;
        private System.Windows.Forms.Button btnTimKiemDG;
        private System.Windows.Forms.DataGridView dgvThongKeDocGia;
        private System.Windows.Forms.TextBox txtTimKiemDG;
        private System.Windows.Forms.Button btnLamMoiDG;
        private System.Windows.Forms.Button btnXuatExcelDG;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
    }
}