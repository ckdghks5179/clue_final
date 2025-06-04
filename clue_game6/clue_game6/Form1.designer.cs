namespace clue_game6
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnRoll = new System.Windows.Forms.Button();
            this.btnTurnEnd = new System.Windows.Forms.Button();
            this.dice1 = new System.Windows.Forms.Label();
            this.lbRemain = new System.Windows.Forms.Label();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnNote = new System.Windows.Forms.Button();
            this.btnSug = new System.Windows.Forms.Button();
            this.btnFinalSug = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.labelChat = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSaveLog = new System.Windows.Forms.Button();
            this.labelCurrentPlayer = new System.Windows.Forms.Label();
            this.labelDice2 = new System.Windows.Forms.Label();
            this.moveTimer = new System.Windows.Forms.Timer(this.components);
            this.pictureBoxDice2 = new System.Windows.Forms.PictureBox();
            this.pictureBoxDice = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timerDice = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDice2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRoll
            // 
            this.btnRoll.Location = new System.Drawing.Point(510, 255);
            this.btnRoll.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRoll.Name = "btnRoll";
            this.btnRoll.Size = new System.Drawing.Size(41, 42);
            this.btnRoll.TabIndex = 0;
            this.btnRoll.UseVisualStyleBackColor = true;
            this.btnRoll.Click += new System.EventHandler(this.btnRoll_Click);
            // 
            // btnTurnEnd
            // 
            this.btnTurnEnd.Location = new System.Drawing.Point(510, 301);
            this.btnTurnEnd.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnTurnEnd.Name = "btnTurnEnd";
            this.btnTurnEnd.Size = new System.Drawing.Size(41, 42);
            this.btnTurnEnd.TabIndex = 1;
            this.btnTurnEnd.UseVisualStyleBackColor = true;
            this.btnTurnEnd.Click += new System.EventHandler(this.btnTurnEnd_Click);
            // 
            // dice1
            // 
            this.dice1.AutoSize = true;
            this.dice1.Location = new System.Drawing.Point(549, 228);
            this.dice1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dice1.Name = "dice1";
            this.dice1.Size = new System.Drawing.Size(11, 12);
            this.dice1.TabIndex = 2;
            this.dice1.Text = "1";
            // 
            // lbRemain
            // 
            this.lbRemain.AutoSize = true;
            this.lbRemain.Location = new System.Drawing.Point(504, 228);
            this.lbRemain.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbRemain.Name = "lbRemain";
            this.lbRemain.Size = new System.Drawing.Size(11, 12);
            this.lbRemain.TabIndex = 5;
            this.lbRemain.Text = "0";
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(629, 241);
            this.btnUp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(30, 38);
            this.btnUp.TabIndex = 6;
            this.btnUp.Text = "↑";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(629, 322);
            this.btnDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(30, 38);
            this.btnDown.TabIndex = 7;
            this.btnDown.Text = "↓";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(665, 287);
            this.btnRight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(43, 26);
            this.btnRight.TabIndex = 8;
            this.btnRight.Text = "→";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(577, 287);
            this.btnLeft.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(44, 26);
            this.btnLeft.TabIndex = 9;
            this.btnLeft.Text = "←";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnNote
            // 
            this.btnNote.Location = new System.Drawing.Point(728, 62);
            this.btnNote.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNote.Name = "btnNote";
            this.btnNote.Size = new System.Drawing.Size(44, 36);
            this.btnNote.TabIndex = 13;
            this.btnNote.UseVisualStyleBackColor = true;
            this.btnNote.Click += new System.EventHandler(this.btnNote_Click);
            // 
            // btnSug
            // 
            this.btnSug.Location = new System.Drawing.Point(701, 114);
            this.btnSug.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSug.Name = "btnSug";
            this.btnSug.Size = new System.Drawing.Size(94, 30);
            this.btnSug.TabIndex = 14;
            this.btnSug.Text = "Suggest";
            this.btnSug.UseVisualStyleBackColor = true;
            this.btnSug.Click += new System.EventHandler(this.btnSug_Click);
            // 
            // btnFinalSug
            // 
            this.btnFinalSug.ForeColor = System.Drawing.Color.Red;
            this.btnFinalSug.Location = new System.Drawing.Point(620, 470);
            this.btnFinalSug.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnFinalSug.Name = "btnFinalSug";
            this.btnFinalSug.Size = new System.Drawing.Size(111, 42);
            this.btnFinalSug.TabIndex = 15;
            this.btnFinalSug.Text = "Final\r\nSuggest";
            this.btnFinalSug.UseVisualStyleBackColor = true;
            this.btnFinalSug.Click += new System.EventHandler(this.btnFinalSug_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 430);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(458, 82);
            this.textBox1.TabIndex = 16;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(476, 462);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "messageSend";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(492, 62);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(190, 82);
            this.textBox2.TabIndex = 18;
            // 
            // labelChat
            // 
            this.labelChat.AutoSize = true;
            this.labelChat.Location = new System.Drawing.Point(12, 415);
            this.labelChat.Name = "labelChat";
            this.labelChat.Size = new System.Drawing.Size(58, 12);
            this.labelChat.TabIndex = 19;
            this.labelChat.Text = "message";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(490, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "Card";
            // 
            // btnSaveLog
            // 
            this.btnSaveLog.Location = new System.Drawing.Point(476, 490);
            this.btnSaveLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSaveLog.Name = "btnSaveLog";
            this.btnSaveLog.Size = new System.Drawing.Size(106, 22);
            this.btnSaveLog.TabIndex = 21;
            this.btnSaveLog.Text = "Save Log";
            this.btnSaveLog.UseVisualStyleBackColor = true;
            this.btnSaveLog.Click += new System.EventHandler(this.btnSaveLog_Click);
            // 
            // labelCurrentPlayer
            // 
            this.labelCurrentPlayer.AutoSize = true;
            this.labelCurrentPlayer.Location = new System.Drawing.Point(490, 9);
            this.labelCurrentPlayer.Name = "labelCurrentPlayer";
            this.labelCurrentPlayer.Size = new System.Drawing.Size(45, 12);
            this.labelCurrentPlayer.TabIndex = 22;
            this.labelCurrentPlayer.Text = "Player:";
            // 
            // labelDice2
            // 
            this.labelDice2.AutoSize = true;
            this.labelDice2.Location = new System.Drawing.Point(515, 228);
            this.labelDice2.Name = "labelDice2";
            this.labelDice2.Size = new System.Drawing.Size(0, 12);
            this.labelDice2.TabIndex = 25;
            // 
            // moveTimer
            // 
            this.moveTimer.Interval = 40;
            // 
            // pictureBoxDice2
            // 
            this.pictureBoxDice2.Location = new System.Drawing.Point(534, 217);
            this.pictureBoxDice2.Name = "pictureBoxDice2";
            this.pictureBoxDice2.Size = new System.Drawing.Size(36, 33);
            this.pictureBoxDice2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDice2.TabIndex = 24;
            this.pictureBoxDice2.TabStop = false;
            // 
            // pictureBoxDice
            // 
            this.pictureBoxDice.Location = new System.Drawing.Point(488, 217);
            this.pictureBoxDice.Name = "pictureBoxDice";
            this.pictureBoxDice.Size = new System.Drawing.Size(40, 33);
            this.pictureBoxDice.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDice.TabIndex = 23;
            this.pictureBoxDice.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::clue_game6.Properties.Resources.fullmap;
            this.pictureBox1.Location = new System.Drawing.Point(7, 6);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(478, 400);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 527);
            this.Controls.Add(this.pictureBoxDice);
            this.Controls.Add(this.labelCurrentPlayer);
            this.Controls.Add(this.btnSaveLog);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelChat);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnFinalSug);
            this.Controls.Add(this.btnSug);
            this.Controls.Add(this.btnNote);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.lbRemain);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnTurnEnd);
            this.Controls.Add(this.btnRoll);
            this.Controls.Add(this.pictureBoxDice2);
            this.Controls.Add(this.dice1);
            this.Controls.Add(this.labelDice2);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDice2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRoll;
        private System.Windows.Forms.Button btnTurnEnd;
        private System.Windows.Forms.Label dice1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbRemain;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnNote;
        private System.Windows.Forms.Button btnSug;
        private System.Windows.Forms.Button btnFinalSug;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label labelChat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSaveLog;
        private System.Windows.Forms.Label labelCurrentPlayer;
        private System.Windows.Forms.PictureBox pictureBoxDice;
        private System.Windows.Forms.PictureBox pictureBoxDice2;
        private System.Windows.Forms.Label labelDice2;
        private System.Windows.Forms.Timer moveTimer;
        private System.Windows.Forms.Timer timerDice;
    }
}

