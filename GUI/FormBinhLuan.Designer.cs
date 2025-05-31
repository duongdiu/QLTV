namespace GUI
{
    partial class FormBinhLuan
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
            this.richTextBoxNoiDung = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewSach = new System.Windows.Forms.ListView();
            this.tenSach = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tacGia = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ngayMuon = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnDong = new System.Windows.Forms.Button();
            this.btnGuiBinhLuan = new System.Windows.Forms.Button();
            this.lblSachDangChon = new System.Windows.Forms.Label();
            this.lblSachDaChon = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBoxNoiDung
            // 
            this.richTextBoxNoiDung.Location = new System.Drawing.Point(148, 390);
            this.richTextBoxNoiDung.Name = "richTextBoxNoiDung";
            this.richTextBoxNoiDung.Size = new System.Drawing.Size(765, 183);
            this.richTextBoxNoiDung.TabIndex = 11;
            this.richTextBoxNoiDung.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(142, 335);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(258, 32);
            this.label2.TabIndex = 10;
            this.label2.Text = "Nội dung bình luận:";
            this.label2.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(418, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 35);
            this.label1.TabIndex = 9;
            this.label1.Text = "BÌNH LUẬN SÁCH";
            // 
            // listViewSach
            // 
            this.listViewSach.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.tenSach,
            this.tacGia,
            this.ngayMuon});
            this.listViewSach.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.listViewSach.HideSelection = false;
            this.listViewSach.Location = new System.Drawing.Point(148, 124);
            this.listViewSach.Name = "listViewSach";
            this.listViewSach.Size = new System.Drawing.Size(765, 189);
            this.listViewSach.TabIndex = 8;
            this.listViewSach.UseCompatibleStateImageBehavior = false;
            this.listViewSach.View = System.Windows.Forms.View.Details;
            this.listViewSach.SelectedIndexChanged += new System.EventHandler(this.listViewSach_SelectedIndexChanged);
            // 
            // tenSach
            // 
            this.tenSach.Text = "Tên sách";
            this.tenSach.Width = 200;
            // 
            // tacGia
            // 
            this.tacGia.Text = "Tác giả";
            this.tacGia.Width = 150;
            // 
            // ngayMuon
            // 
            this.ngayMuon.Text = "Ngày mượn";
            this.ngayMuon.Width = 150;
            // 
            // btnDong
            // 
            this.btnDong.BackColor = System.Drawing.Color.White;
            this.btnDong.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnDong.ForeColor = System.Drawing.Color.Red;
            this.btnDong.Location = new System.Drawing.Point(965, 3);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(78, 35);
            this.btnDong.TabIndex = 7;
            this.btnDong.Text = "❌";
            this.btnDong.UseVisualStyleBackColor = false;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // btnGuiBinhLuan
            // 
            this.btnGuiBinhLuan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnGuiBinhLuan.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnGuiBinhLuan.Location = new System.Drawing.Point(496, 593);
            this.btnGuiBinhLuan.Name = "btnGuiBinhLuan";
            this.btnGuiBinhLuan.Size = new System.Drawing.Size(80, 48);
            this.btnGuiBinhLuan.TabIndex = 13;
            this.btnGuiBinhLuan.Text = "GỬI";
            this.btnGuiBinhLuan.UseVisualStyleBackColor = false;
            this.btnGuiBinhLuan.Click += new System.EventHandler(this.btnGuiBinhLuan_Click);
            // 
            // lblSachDangChon
            // 
            this.lblSachDangChon.AutoSize = true;
            this.lblSachDangChon.Location = new System.Drawing.Point(407, 349);
            this.lblSachDangChon.Name = "lblSachDangChon";
            this.lblSachDangChon.Size = new System.Drawing.Size(0, 16);
            this.lblSachDangChon.TabIndex = 14;
            // 
            // lblSachDaChon
            // 
            this.lblSachDaChon.AutoSize = true;
            this.lblSachDaChon.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblSachDaChon.Location = new System.Drawing.Point(588, 342);
            this.lblSachDaChon.Name = "lblSachDaChon";
            this.lblSachDaChon.Size = new System.Drawing.Size(180, 25);
            this.lblSachDaChon.TabIndex = 15;
            this.lblSachDaChon.Text = "Sách đang chọn:";
            // 
            // FormBinhLuan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1055, 674);
            this.Controls.Add(this.lblSachDaChon);
            this.Controls.Add(this.lblSachDangChon);
            this.Controls.Add(this.btnGuiBinhLuan);
            this.Controls.Add(this.richTextBoxNoiDung);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewSach);
            this.Controls.Add(this.btnDong);
            this.Name = "FormBinhLuan";
            this.Text = "FormBinhLuan";
            this.Load += new System.EventHandler(this.FormBinhLuan_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBoxNoiDung;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listViewSach;
        private System.Windows.Forms.ColumnHeader tenSach;
        private System.Windows.Forms.ColumnHeader tacGia;
        private System.Windows.Forms.ColumnHeader ngayMuon;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Button btnGuiBinhLuan;
        private System.Windows.Forms.Label lblSachDangChon;
        private System.Windows.Forms.Label lblSachDaChon;
    }
}