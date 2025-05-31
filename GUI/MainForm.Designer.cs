namespace GUI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnlogin = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTinTuc = new System.Windows.Forms.Button();
            this.btnQuyDinh = new System.Windows.Forms.Button();
            this.btnLienHe = new System.Windows.Forms.Button();
            this.btnTrangChu = new System.Windows.Forms.Button();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCover2)).BeginInit();
            this.panelBook1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCover1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(129, 115);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(631, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(341, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "THƯ VIỆN TRỰC TUYẾN";
            // 
            // btnlogin
            // 
            this.btnlogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnlogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnlogin.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnlogin.Location = new System.Drawing.Point(1530, 15);
            this.btnlogin.Name = "btnlogin";
            this.btnlogin.Size = new System.Drawing.Size(154, 38);
            this.btnlogin.TabIndex = 3;
            this.btnlogin.Text = "Đăng nhập";
            this.btnlogin.UseVisualStyleBackColor = false;
            this.btnlogin.Click += new System.EventHandler(this.btnlogin_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnRegister.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnRegister.Location = new System.Drawing.Point(1703, 15);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(118, 38);
            this.btnRegister.TabIndex = 4;
            this.btnRegister.Text = "Đăng ký";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.btnTinTuc);
            this.panel1.Controls.Add(this.btnQuyDinh);
            this.panel1.Controls.Add(this.btnLienHe);
            this.panel1.Controls.Add(this.btnTrangChu);
            this.panel1.Location = new System.Drawing.Point(278, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1133, 62);
            this.panel1.TabIndex = 5;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnTinTuc
            // 
            this.btnTinTuc.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnTinTuc.Location = new System.Drawing.Point(916, 14);
            this.btnTinTuc.Name = "btnTinTuc";
            this.btnTinTuc.Size = new System.Drawing.Size(147, 35);
            this.btnTinTuc.TabIndex = 3;
            this.btnTinTuc.Text = "Tin tức";
            this.btnTinTuc.UseVisualStyleBackColor = true;
            this.btnTinTuc.Click += new System.EventHandler(this.btnTinTuc_Click);
            // 
            // btnQuyDinh
            // 
            this.btnQuyDinh.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnQuyDinh.Location = new System.Drawing.Point(669, 14);
            this.btnQuyDinh.Name = "btnQuyDinh";
            this.btnQuyDinh.Size = new System.Drawing.Size(147, 35);
            this.btnQuyDinh.TabIndex = 2;
            this.btnQuyDinh.Text = "Quy định";
            this.btnQuyDinh.UseVisualStyleBackColor = true;
            this.btnQuyDinh.Click += new System.EventHandler(this.btnQuyDinh_Click);
            // 
            // btnLienHe
            // 
            this.btnLienHe.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnLienHe.Location = new System.Drawing.Point(389, 14);
            this.btnLienHe.Name = "btnLienHe";
            this.btnLienHe.Size = new System.Drawing.Size(147, 35);
            this.btnLienHe.TabIndex = 1;
            this.btnLienHe.Text = "Liên hệ";
            this.btnLienHe.UseVisualStyleBackColor = true;
            this.btnLienHe.Click += new System.EventHandler(this.btnLienHe_Click);
            // 
            // btnTrangChu
            // 
            this.btnTrangChu.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnTrangChu.Location = new System.Drawing.Point(70, 14);
            this.btnTrangChu.Name = "btnTrangChu";
            this.btnTrangChu.Size = new System.Drawing.Size(168, 35);
            this.btnTrangChu.TabIndex = 0;
            this.btnTrangChu.Text = "Trang chủ";
            this.btnTrangChu.UseVisualStyleBackColor = true;
            this.btnTrangChu.Click += new System.EventHandler(this.btnTrangChu_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Teal;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(0, 958);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(2690, 90);
            this.panel2.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(1152, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(272, 25);
            this.label7.TabIndex = 1;
            this.label7.Text = "📞 Hotline: 0336 697 9159";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(343, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(621, 25);
            this.label6.TabIndex = 0;
            this.label6.Text = "📍 Địa chỉ: 41A Đ. Phú Diễn, Phú Diễn, Bắc Từ Liêm, Hà Nội";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(127, 16);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 22);
            this.txtSearch.TabIndex = 7;
            this.txtSearch.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnSearch.Location = new System.Drawing.Point(333, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(106, 41);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.button7_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox2.ForeColor = System.Drawing.Color.Maroon;
            this.groupBox2.Location = new System.Drawing.Point(62, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(470, 300);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Thông báo";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 25;
            this.listBox1.Items.AddRange(new object[] {
            "📌 Thư viện sẽ đóng cửa vào ngày ",
            "       lễ 30/4 & 1/5",
            "",
            "⚠️ Quy định mới về thời gian ",
            "       mượn sách: tối đa 15 ngày",
            "",
            "🏆 Cuộc thi viết review sách ",
            "      nhận quà hấp dẫn"});
            this.listBox1.Location = new System.Drawing.Point(6, 38);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(428, 229);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBox2);
            this.groupBox3.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox3.ForeColor = System.Drawing.Color.Maroon;
            this.groupBox3.Location = new System.Drawing.Point(579, 72);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(528, 300);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tin tức";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // listBox2
            // 
            this.listBox2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 25;
            this.listBox2.Items.AddRange(new object[] {
            "🆕 Thư viện vừa bổ sung 100+ đầu sách ",
            "       mới tháng 3",
            "",
            "🎓 Chương trình hỗ trợ tài liệu học tập ",
            "       miễn phí cho sinh viên",
            "",
            "📚 Top 10 cuốn sách hay nên đọc ",
            "       mùa hè này"});
            this.listBox2.Location = new System.Drawing.Point(18, 38);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(471, 229);
            this.listBox2.TabIndex = 0;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.ForeColor = System.Drawing.Color.Teal;
            this.groupBox1.Location = new System.Drawing.Point(1483, 168);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 251);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Giờ mở cửa";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(13, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(323, 25);
            this.label5.TabIndex = 3;
            this.label5.Text = "Nghỉ: Thứ 7, Chủ nhật, ngày lễ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label4.Location = new System.Drawing.Point(13, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(192, 25);
            this.label4.TabIndex = 2;
            this.label4.Text = "Thứ hai - Thứ sáu";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(13, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(227, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "Chiều: 13h00 - 17h00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(13, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(206, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Sáng: 8h00 - 12h00";
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.panel3);
            this.panelContent.Controls.Add(this.panelBook1);
            this.panelContent.Controls.Add(this.groupBox3);
            this.panelContent.Controls.Add(this.groupBox2);
            this.panelContent.Controls.Add(this.btnSearch);
            this.panelContent.Controls.Add(this.txtSearch);
            this.panelContent.Location = new System.Drawing.Point(88, 162);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1298, 760);
            this.panelContent.TabIndex = 14;
            this.panelContent.Paint += new System.Windows.Forms.PaintEventHandler(this.panel5_Paint);
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
            this.panel3.Location = new System.Drawing.Point(621, 396);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(510, 350);
            this.panel3.TabIndex = 13;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // lblCategory2
            // 
            this.lblCategory2.AutoSize = true;
            this.lblCategory2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblCategory2.Location = new System.Drawing.Point(168, 64);
            this.lblCategory2.Name = "lblCategory2";
            this.lblCategory2.Size = new System.Drawing.Size(94, 26);
            this.lblCategory2.TabIndex = 5;
            this.lblCategory2.Text = "Thể loại:";
            // 
            // lblBorrowCount2
            // 
            this.lblBorrowCount2.AutoSize = true;
            this.lblBorrowCount2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblBorrowCount2.Location = new System.Drawing.Point(168, 248);
            this.lblBorrowCount2.Name = "lblBorrowCount2";
            this.lblBorrowCount2.Size = new System.Drawing.Size(145, 26);
            this.lblBorrowCount2.TabIndex = 4;
            this.lblBorrowCount2.Text = "Số lượt mượn:";
            // 
            // lblYear2
            // 
            this.lblYear2.AutoSize = true;
            this.lblYear2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblYear2.Location = new System.Drawing.Point(168, 182);
            this.lblYear2.Name = "lblYear2";
            this.lblYear2.Size = new System.Drawing.Size(147, 26);
            this.lblYear2.TabIndex = 3;
            this.lblYear2.Text = "Năm xuất bản:";
            // 
            // lblPublisher2
            // 
            this.lblPublisher2.AutoSize = true;
            this.lblPublisher2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblPublisher2.Location = new System.Drawing.Point(168, 124);
            this.lblPublisher2.Name = "lblPublisher2";
            this.lblPublisher2.Size = new System.Drawing.Size(141, 26);
            this.lblPublisher2.TabIndex = 2;
            this.lblPublisher2.Text = "Nhà xuất bản:";
            // 
            // lblAuthor2
            // 
            this.lblAuthor2.AutoSize = true;
            this.lblAuthor2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblAuthor2.Location = new System.Drawing.Point(168, 11);
            this.lblAuthor2.Name = "lblAuthor2";
            this.lblAuthor2.Size = new System.Drawing.Size(85, 26);
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
            this.panelBook1.Location = new System.Drawing.Point(62, 396);
            this.panelBook1.Name = "panelBook1";
            this.panelBook1.Size = new System.Drawing.Size(538, 350);
            this.panelBook1.TabIndex = 12;
            // 
            // lblCategory1
            // 
            this.lblCategory1.AutoSize = true;
            this.lblCategory1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblCategory1.Location = new System.Drawing.Point(168, 64);
            this.lblCategory1.Name = "lblCategory1";
            this.lblCategory1.Size = new System.Drawing.Size(94, 26);
            this.lblCategory1.TabIndex = 5;
            this.lblCategory1.Text = "Thể loại:";
            // 
            // lblBorrowCount1
            // 
            this.lblBorrowCount1.AutoSize = true;
            this.lblBorrowCount1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblBorrowCount1.Location = new System.Drawing.Point(168, 248);
            this.lblBorrowCount1.Name = "lblBorrowCount1";
            this.lblBorrowCount1.Size = new System.Drawing.Size(145, 26);
            this.lblBorrowCount1.TabIndex = 4;
            this.lblBorrowCount1.Text = "Số lượt mượn:";
            // 
            // lblYear1
            // 
            this.lblYear1.AutoSize = true;
            this.lblYear1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblYear1.Location = new System.Drawing.Point(168, 182);
            this.lblYear1.Name = "lblYear1";
            this.lblYear1.Size = new System.Drawing.Size(147, 26);
            this.lblYear1.TabIndex = 3;
            this.lblYear1.Text = "Năm xuất bản:";
            // 
            // lblPublisher1
            // 
            this.lblPublisher1.AutoSize = true;
            this.lblPublisher1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblPublisher1.Location = new System.Drawing.Point(168, 124);
            this.lblPublisher1.Name = "lblPublisher1";
            this.lblPublisher1.Size = new System.Drawing.Size(141, 26);
            this.lblPublisher1.TabIndex = 2;
            this.lblPublisher1.Text = "Nhà xuất bản:";
            // 
            // lblAuthor1
            // 
            this.lblAuthor1.AutoSize = true;
            this.lblAuthor1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblAuthor1.Location = new System.Drawing.Point(168, 11);
            this.lblAuthor1.Name = "lblAuthor1";
            this.lblAuthor1.Size = new System.Drawing.Size(85, 26);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnlogin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = " ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
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

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnlogin;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnTrangChu;
        private System.Windows.Forms.Button btnTinTuc;
        private System.Windows.Forms.Button btnQuyDinh;
        private System.Windows.Forms.Button btnLienHe;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelContent;
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