namespace clue_game6
{
    partial class SelectCardForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCardList = new System.Windows.Forms.ComboBox();
            this.btnChoice = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(243, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "상대에게 보여줄 카드";
            // 
            // cmbCardList
            // 
            this.cmbCardList.FormattingEnabled = true;
            this.cmbCardList.Location = new System.Drawing.Point(290, 202);
            this.cmbCardList.Name = "cmbCardList";
            this.cmbCardList.Size = new System.Drawing.Size(160, 32);
            this.cmbCardList.TabIndex = 2;
            // 
            // btnChoice
            // 
            this.btnChoice.Location = new System.Drawing.Point(290, 278);
            this.btnChoice.Name = "btnChoice";
            this.btnChoice.Size = new System.Drawing.Size(166, 54);
            this.btnChoice.TabIndex = 3;
            this.btnChoice.Text = "확인";
            this.btnChoice.UseVisualStyleBackColor = true;
            this.btnChoice.Click += new System.EventHandler(this.btnChoice_Click);
            // 
            // SelectCardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnChoice);
            this.Controls.Add(this.cmbCardList);
            this.Controls.Add(this.label1);
            this.Name = "SelectCardForm";
            this.Text = "SelectCardForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCardList;
        private System.Windows.Forms.Button btnChoice;
    }
}