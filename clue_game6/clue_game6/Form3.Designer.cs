namespace clue_game6
{
    partial class Form3
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
            this.manBox = new System.Windows.Forms.ComboBox();
            this.roomBox = new System.Windows.Forms.ComboBox();
            this.weaponBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // manBox
            // 
            this.manBox.FormattingEnabled = true;
            this.manBox.Items.AddRange(new object[] {
            "Green",
            "Mustard",
            "Peacock",
            "Plum",
            "Scarlett",
            "White"});
            this.manBox.Location = new System.Drawing.Point(137, 372);
            this.manBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.manBox.Name = "manBox";
            this.manBox.Size = new System.Drawing.Size(221, 32);
            this.manBox.TabIndex = 0;
            this.manBox.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            // 
            // roomBox
            // 
            this.roomBox.FormattingEnabled = true;
            this.roomBox.Items.AddRange(new object[] {
            "주방",
            "공부방",
            "무도회장",
            "온실",
            "식당",
            "당구장",
            "서재",
            "라운지",
            "홀"});
            this.roomBox.Location = new System.Drawing.Point(522, 372);
            this.roomBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.roomBox.Name = "roomBox";
            this.roomBox.Size = new System.Drawing.Size(221, 32);
            this.roomBox.TabIndex = 1;
            this.roomBox.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            // 
            // weaponBox
            // 
            this.weaponBox.FormattingEnabled = true;
            this.weaponBox.Items.AddRange(new object[] {
            "촛대",
            "파이프",
            "리볼버",
            "밧줄",
            "렌치",
            "단검"});
            this.weaponBox.Location = new System.Drawing.Point(929, 372);
            this.weaponBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.weaponBox.Name = "weaponBox";
            this.weaponBox.Size = new System.Drawing.Size(221, 32);
            this.weaponBox.TabIndex = 2;
            this.weaponBox.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(134, 288);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 46);
            this.label1.TabIndex = 3;
            this.label1.Text = "범인";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(518, 288);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "장소";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(925, 288);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "도구";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1014, 626);
            this.button1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 46);
            this.button1.TabIndex = 6;
            this.button1.Text = "확인";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1486, 890);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.weaponBox);
            this.Controls.Add(this.roomBox);
            this.Controls.Add(this.manBox);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Form3";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox manBox;
        private System.Windows.Forms.ComboBox roomBox;
        private System.Windows.Forms.ComboBox weaponBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}