namespace GUI
{
    partial class FormDocGiaChinh
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
            this.flowLayoutPanelBinhLuan = new System.Windows.Forms.FlowLayoutPanel();
            this.lblBinhLuan = new System.Windows.Forms.Label();
            this.lblSachDuocMuon = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblCategory2 = new System.Windows.Forms.Label();
            this.lblBorrowCount2 = new System.Windows.Forms.Label();
            this.lblYear2 = new System.Windows.Forms.Label();
            this.lblPublisher2 = new System.Windows.Forms.Label();
            this.lblAuthor2 = new System.Windows.Forms.Label();
            this.pbCover2 = new System.Windows.Forms.PictureBox();
            this.panelBook1 = new System.Windows.Forms.Panel();
            this.lblCategory1 = new System.Windows.Forms.Label();
            this.lblBorrowCount1 = new System.Windows.Forms.Label();
            this.lblYear1 = new System.Windows.Forms.Label();
            this.lblPublisher1 = new System.Windows.Forms.Label();
            this.lblAuthor1 = new System.Windows.Forms.Label();
            this.pbCover1 = new System.Windows.Forms.PictureBox();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCover2)).BeginInit();
            this.panelBook1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCover1)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanelBinhLuan
            // 
            this.flowLayoutPanelBinhLuan.Location = new System.Drawing.Point(99, 485);
            this.flowLayoutPanelBinhLuan.Name = "flowLayoutPanelBinhLuan";
            this.flowLayoutPanelBinhLuan.Size = new System.Drawing.Size(862, 183);
            this.flowLayoutPanelBinhLuan.TabIndex = 12;
            this.flowLayoutPanelBinhLuan.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanelBinhLuan_Paint);
            // 
            // lblBinhLuan
            // 
            this.lblBinhLuan.AutoSize = true;
            this.lblBinhLuan.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblBinhLuan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblBinhLuan.Location = new System.Drawing.Point(93, 432);
            this.lblBinhLuan.Name = "lblBinhLuan";
            this.lblBinhLuan.Size = new System.Drawing.Size(451, 32);
            this.lblBinhLuan.TabIndex = 11;
            this.lblBinhLuan.Text = "BÌNH LUẬN SÁCH CỦA ĐỘC GIẢ";
            // 
            // lblSachDuocMuon
            // 
            this.lblSachDuocMuon.AutoSize = true;
            this.lblSachDuocMuon.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblSachDuocMuon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblSachDuocMuon.Location = new System.Drawing.Point(93, 6);
            this.lblSachDuocMuon.Name = "lblSachDuocMuon";
            this.lblSachDuocMuon.Size = new System.Drawing.Size(378, 32);
            this.lblSachDuocMuon.TabIndex = 10;
            this.lblSachDuocMuon.Text = "SÁCH ĐƯỢC MƯỢN NHIỀU";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.lblCategory2);
            this.panel3.Controls.Add(this.lblBorrowCount2);
            this.panel3.Controls.Add(this.lblYear2);
            this.panel3.Controls.Add(this.lblPublisher2);
            this.panel3.Controls.Add(this.lblAuthor2);
            this.panel3.Controls.Add(this.pbCover2);
            this.panel3.Location = new System.Drawing.Point(47, 79);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(510, 328);
            this.panel3.TabIndex = 15;
            // 
            // lblCategory2
            // 
            this.lblCategory2.AutoSize = true;
            this.lblCategory2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblCategory2.Location = new System.Drawing.Point(168, 64);
            this.lblCategory2.Name = "lblCategory2";
            this.lblCategory2.Size = new System.Drawing.Size(82, 22);
            this.lblCategory2.TabIndex = 5;
            this.lblCategory2.Text = "Thể loại:";
            // 
            // lblBorrowCount2
            // 
            this.lblBorrowCount2.AutoSize = true;
            this.lblBorrowCount2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblBorrowCount2.Location = new System.Drawing.Point(168, 248);
            this.lblBorrowCount2.Name = "lblBorrowCount2";
            this.lblBorrowCount2.Size = new System.Drawing.Size(125, 22);
            this.lblBorrowCount2.TabIndex = 4;
            this.lblBorrowCount2.Text = "Số lượt mượn:";
            // 
            // lblYear2
            // 
            this.lblYear2.AutoSize = true;
            this.lblYear2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblYear2.Location = new System.Drawing.Point(168, 182);
            this.lblYear2.Name = "lblYear2";
            this.lblYear2.Size = new System.Drawing.Size(123, 22);
            this.lblYear2.TabIndex = 3;
            this.lblYear2.Text = "Năm xuất bản:";
            // 
            // lblPublisher2
            // 
            this.lblPublisher2.AutoSize = true;
            this.lblPublisher2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblPublisher2.Location = new System.Drawing.Point(168, 124);
            this.lblPublisher2.Name = "lblPublisher2";
            this.lblPublisher2.Size = new System.Drawing.Size(118, 22);
            this.lblPublisher2.TabIndex = 2;
            this.lblPublisher2.Text = "Nhà xuất bản:";
            // 
            // lblAuthor2
            // 
            this.lblAuthor2.AutoSize = true;
            this.lblAuthor2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblAuthor2.Location = new System.Drawing.Point(168, 11);
            this.lblAuthor2.Name = "lblAuthor2";
            this.lblAuthor2.Size = new System.Drawing.Size(75, 22);
            this.lblAuthor2.TabIndex = 1;
            this.lblAuthor2.Text = "Tác giả:";
            // 
            // pbCover2
            // 
            this.pbCover2.Location = new System.Drawing.Point(8, 11);
            this.pbCover2.Name = "pbCover2";
            this.pbCover2.Size = new System.Drawing.Size(154, 171);
            this.pbCover2.TabIndex = 0;
            this.pbCover2.TabStop = false;
            // 
            // panelBook1
            // 
            this.panelBook1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBook1.Controls.Add(this.lblCategory1);
            this.panelBook1.Controls.Add(this.lblBorrowCount1);
            this.panelBook1.Controls.Add(this.lblYear1);
            this.panelBook1.Controls.Add(this.lblPublisher1);
            this.panelBook1.Controls.Add(this.lblAuthor1);
            this.panelBook1.Controls.Add(this.pbCover1);
            this.panelBook1.Location = new System.Drawing.Point(602, 79);
            this.panelBook1.Name = "panelBook1";
            this.panelBook1.Size = new System.Drawing.Size(538, 328);
            this.panelBook1.TabIndex = 14;
            // 
            // lblCategory1
            // 
            this.lblCategory1.AutoSize = true;
            this.lblCategory1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblCategory1.Location = new System.Drawing.Point(168, 64);
            this.lblCategory1.Name = "lblCategory1";
            this.lblCategory1.Size = new System.Drawing.Size(82, 22);
            this.lblCategory1.TabIndex = 5;
            this.lblCategory1.Text = "Thể loại:";
            // 
            // lblBorrowCount1
            // 
            this.lblBorrowCount1.AutoSize = true;
            this.lblBorrowCount1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblBorrowCount1.Location = new System.Drawing.Point(168, 248);
            this.lblBorrowCount1.Name = "lblBorrowCount1";
            this.lblBorrowCount1.Size = new System.Drawing.Size(125, 22);
            this.lblBorrowCount1.TabIndex = 4;
            this.lblBorrowCount1.Text = "Số lượt mượn:";
            // 
            // lblYear1
            // 
            this.lblYear1.AutoSize = true;
            this.lblYear1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblYear1.Location = new System.Drawing.Point(168, 182);
            this.lblYear1.Name = "lblYear1";
            this.lblYear1.Size = new System.Drawing.Size(123, 22);
            this.lblYear1.TabIndex = 3;
            this.lblYear1.Text = "Năm xuất bản:";
            // 
            // lblPublisher1
            // 
            this.lblPublisher1.AutoSize = true;
            this.lblPublisher1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblPublisher1.Location = new System.Drawing.Point(168, 124);
            this.lblPublisher1.Name = "lblPublisher1";
            this.lblPublisher1.Size = new System.Drawing.Size(118, 22);
            this.lblPublisher1.TabIndex = 2;
            this.lblPublisher1.Text = "Nhà xuất bản:";
            // 
            // lblAuthor1
            // 
            this.lblAuthor1.AutoSize = true;
            this.lblAuthor1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblAuthor1.Location = new System.Drawing.Point(168, 11);
            this.lblAuthor1.Name = "lblAuthor1";
            this.lblAuthor1.Size = new System.Drawing.Size(75, 22);
            this.lblAuthor1.TabIndex = 1;
            this.lblAuthor1.Text = "Tác giả:";
            // 
            // pbCover1
            // 
            this.pbCover1.Location = new System.Drawing.Point(8, 11);
            this.pbCover1.Name = "pbCover1";
            this.pbCover1.Size = new System.Drawing.Size(154, 171);
            this.pbCover1.TabIndex = 0;
            this.pbCover1.TabStop = false;
            // 
            // FormDocGiaChinh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1255, 674);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panelBook1);
            this.Controls.Add(this.flowLayoutPanelBinhLuan);
            this.Controls.Add(this.lblBinhLuan);
            this.Controls.Add(this.lblSachDuocMuon);
            this.Name = "FormDocGiaChinh";
            this.Text = "FormDocGiaChinh";
            this.Load += new System.EventHandler(this.FormDocGiaChinh_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCover2)).EndInit();
            this.panelBook1.ResumeLayout(false);
            this.panelBook1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCover1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelBinhLuan;
        private System.Windows.Forms.Label lblBinhLuan;
        private System.Windows.Forms.Label lblSachDuocMuon;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblCategory2;
        private System.Windows.Forms.Label lblBorrowCount2;
        private System.Windows.Forms.Label lblYear2;
        private System.Windows.Forms.Label lblPublisher2;
        private System.Windows.Forms.Label lblAuthor2;
        private System.Windows.Forms.PictureBox pbCover2;
        private System.Windows.Forms.Panel panelBook1;
        private System.Windows.Forms.Label lblCategory1;
        private System.Windows.Forms.Label lblBorrowCount1;
        private System.Windows.Forms.Label lblYear1;
        private System.Windows.Forms.Label lblPublisher1;
        private System.Windows.Forms.Label lblAuthor1;
        private System.Windows.Forms.PictureBox pbCover1;
    }
}