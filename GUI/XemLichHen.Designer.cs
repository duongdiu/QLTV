namespace GUI
{
    partial class XemLichHen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XemLichHen));
            this.label1 = new System.Windows.Forms.Label();
            this.dgvLichHen = new System.Windows.Forms.DataGridView();
            this.btnHuyLich = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichHen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(357, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(327, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "LỊCH HẸN CỦA TÔI";
            // 
            // dgvLichHen
            // 
            this.dgvLichHen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLichHen.Location = new System.Drawing.Point(56, 127);
            this.dgvLichHen.Name = "dgvLichHen";
            this.dgvLichHen.RowHeadersWidth = 51;
            this.dgvLichHen.RowTemplate.Height = 24;
            this.dgvLichHen.Size = new System.Drawing.Size(970, 430);
            this.dgvLichHen.TabIndex = 1;
            this.dgvLichHen.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLichHen_CellClick);
            // 
            // btnHuyLich
            // 
            this.btnHuyLich.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnHuyLich.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnHuyLich.Location = new System.Drawing.Point(420, 577);
            this.btnHuyLich.Name = "btnHuyLich";
            this.btnHuyLich.Size = new System.Drawing.Size(164, 73);
            this.btnHuyLich.TabIndex = 3;
            this.btnHuyLich.Text = "        Hủy lịch";
            this.btnHuyLich.UseVisualStyleBackColor = false;
            this.btnHuyLich.Click += new System.EventHandler(this.btnHuyLich_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(430, 591);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(44, 46);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 94;
            this.pictureBox2.TabStop = false;
            // 
            // XemLichHen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1055, 674);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnHuyLich);
            this.Controls.Add(this.dgvLichHen);
            this.Controls.Add(this.label1);
            this.Name = "XemLichHen";
            this.Text = "XemLichHen";
            this.Load += new System.EventHandler(this.XemLichHen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichHen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvLichHen;
        private System.Windows.Forms.Button btnHuyLich;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}