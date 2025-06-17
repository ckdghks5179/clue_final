namespace clue_game6
{
    partial class Form2
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.manListBox = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.weaponListBox = new System.Windows.Forms.CheckedListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.roomListBox2 = new System.Windows.Forms.CheckedListBox();
            this.roomListBox1 = new System.Windows.Forms.CheckedListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.manListBox);
            this.groupBox1.Location = new System.Drawing.Point(24, 44);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(353, 348);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "범인";
            // 
            // manListBox
            // 
            this.manListBox.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.manListBox.FormattingEnabled = true;
            this.manListBox.Items.AddRange(new object[] {
            "Green",
            "Mustard",
            "Peacock",
            "Plum",
            "Scarlett",
            "White"});
            this.manListBox.Location = new System.Drawing.Point(17, 34);
            this.manListBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.manListBox.Name = "manListBox";
            this.manListBox.Size = new System.Drawing.Size(192, 250);
            this.manListBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.weaponListBox);
            this.groupBox2.Location = new System.Drawing.Point(392, 54);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(353, 336);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "흉기";
            // 
            // weaponListBox
            // 
            this.weaponListBox.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.weaponListBox.FormattingEnabled = true;
            this.weaponListBox.Items.AddRange(new object[] {
            "촛대",
            "단검",
            "파이프",
            "리볼버",
            "밧줄",
            "랜치"});
            this.weaponListBox.Location = new System.Drawing.Point(7, 34);
            this.weaponListBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.weaponListBox.Name = "weaponListBox";
            this.weaponListBox.Size = new System.Drawing.Size(192, 250);
            this.weaponListBox.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.roomListBox2);
            this.groupBox3.Controls.Add(this.roomListBox1);
            this.groupBox3.Location = new System.Drawing.Point(24, 396);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(721, 324);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "장소";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(550, 266);
            this.button1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 46);
            this.button1.TabIndex = 7;
            this.button1.Text = "close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // roomListBox2
            // 
            this.roomListBox2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.roomListBox2.FormattingEnabled = true;
            this.roomListBox2.Items.AddRange(new object[] {
            "주방",
            "서재",
            "라운지",
            "공부방"});
            this.roomListBox2.Location = new System.Drawing.Point(368, 34);
            this.roomListBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.roomListBox2.Name = "roomListBox2";
            this.roomListBox2.Size = new System.Drawing.Size(192, 168);
            this.roomListBox2.TabIndex = 7;
            // 
            // roomListBox1
            // 
            this.roomListBox1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.roomListBox1.FormattingEnabled = true;
            this.roomListBox1.Items.AddRange(new object[] {
            "무도회장",
            "당구장",
            "온실",
            "식당",
            "홀"});
            this.roomListBox1.Location = new System.Drawing.Point(7, 34);
            this.roomListBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.roomListBox1.Name = "roomListBox1";
            this.roomListBox1.Size = new System.Drawing.Size(192, 209);
            this.roomListBox1.TabIndex = 0;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 732);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form2";
            this.Text = "Note";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox manListBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox weaponListBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox roomListBox2;
        private System.Windows.Forms.CheckedListBox roomListBox1;
        private System.Windows.Forms.Button button1;
    }
}