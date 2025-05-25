using System.Windows.Forms;
using System;

namespace clue_game6
{
    partial class LoginForm
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
            this.lblName = new Label();
            this.txtName = new TextBox();
            this.lblIP = new Label();
            this.txtIP = new TextBox();
            this.lblMaxPlayers = new Label();
            this.numMaxPlayers = new NumericUpDown();
            this.btnConnect = new Button();
            this.SuspendLayout();

            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(20, 20);
            this.lblName.Text = "닉네임:";

            // txtName
            this.txtName.Location = new System.Drawing.Point(80, 17);
            this.txtName.Size = new System.Drawing.Size(200, 20);

            // lblIP
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(20, 55);
            this.lblIP.Text = "서버 IP:";

            // txtIP
            this.txtIP.Location = new System.Drawing.Point(80, 52);
            this.txtIP.Size = new System.Drawing.Size(200, 20);

            // lblMaxPlayers
            this.lblMaxPlayers.AutoSize = true;
            this.lblMaxPlayers.Location = new System.Drawing.Point(20, 90);
            this.lblMaxPlayers.Text = "최대 인원:";

            // numMaxPlayers
            this.numMaxPlayers.Location = new System.Drawing.Point(80, 88);
            this.numMaxPlayers.Size = new System.Drawing.Size(60, 20);
            this.numMaxPlayers.Minimum = 2;
            this.numMaxPlayers.Maximum = 6;
            this.numMaxPlayers.Value = 4;

            // btnConnect
            this.btnConnect.Location = new System.Drawing.Point(80, 125);
            this.btnConnect.Size = new System.Drawing.Size(200, 30);
            this.btnConnect.Text = "입장하기";
            this.btnConnect.Click += new EventHandler(this.btnConnect_Click);

            // LoginForm
            this.ClientSize = new System.Drawing.Size(320, 180);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.lblMaxPlayers);
            this.Controls.Add(this.numMaxPlayers);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Text = "로그인";
            this.ResumeLayout(false);
            this.PerformLayout();

            this.Load += new EventHandler(this.LoginForm_Load);
        }

        #endregion
    }
}