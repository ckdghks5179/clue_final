using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using clue_game6;

namespace clue_game6
{
    public partial class PlayerChoose : Form
    {
        public static List<Form1> AllPlayerForms = new List<Form1>();
        public PlayerChoose()
        {
            InitializeComponent();
            ApplyDetectiveStyle();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            int playerCount = 0;
            string playerName = comboBox1.Text;
            playerCount = Convert.ToInt32(playerName);

            if (playerCount < 3 || playerCount > 6)
            {
                MessageBox.Show("Please select a number between 3 and 6 players.");
                return;
            }

            // Assuming you have a method to start the game with the selected number of players
            StartGame(playerCount);
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            btnOnline.Enabled = true; // 온라인 모드
        }

        public void StartGame(int playerCount)
        {
            // Logic to start the game with the selected number of players
            // For example, you can create a new game instance and pass the player count to it
            // Game game = new Game(playerCount);
            // game.Start();

            GameState gameState = new GameState
            {
                TotalPlayers = playerCount,
                Players = new Player[playerCount],
                clue_map_point = new Point[25, 24]
            };

            for (int i = 0; i < 25; i++)
                for (int j = 0; j < 24; j++)
                    gameState.clue_map_point[i, j] = new Point(8 + j * 20, 8 + i * 16);

            int[] initialX = { 7, 17, 24, 0, 6, 19 };
            int[] initialY = { 0, 0, 7, 14, 23, 23 };

            for (int i = 0; i < playerCount; i++)
            {
                gameState.Players[i] = new Player
                {
                    id = i,
                    name = $"Player {i + 1}",
                    hands = new List<Card>(), // 카드 분배 로직은 여기서 추가 가능
                    x = initialX[i],
                    y = initialY[i],
                    isTurn = (i == 0)
                };
            }



            //카드 분배 로직 추가
            gameState.InitializeCards(); //카드 초기화 및 봉투에 정답 카드 넣기
            gameState.distributeCards(); //카드 나눠주기

            gameState.initializeNote(); //노트 초기화

            for (int i = 0; i < playerCount; i++)
            {
                Form1 form = new Form1(gameState, i);
                AllPlayerForms.Add(form);
                form.Show();
            }

            // 4. Dialog 종료
            this.Close();  // 또는 this.DialogResult = DialogResult.OK; if needed

        }
        //온라인 모드gina
        public static Form MainFormToRun = null;
        private void btnOnline_Click(object sender, EventArgs e)
        {
            int playerCount = int.Parse(comboBox1.Text);
            LoginForm lgf = new LoginForm(playerCount);
            if (lgf.ShowDialog() == DialogResult.OK)
            {
                
                MainFormToRun = lgf.WaitingFormInstance;  
                this.Close();
            }

        }
        private void ApplyDetectiveStyle()
        {
            this.BackColor = Color.Bisque;
            this.Font = new Font("Georgia", 10, FontStyle.Regular);

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label lbl)
                {
                    lbl.ForeColor = Color.SaddleBrown;
                    lbl.Font = new Font("Georgia", 11, FontStyle.Bold);
                    lbl.BackColor = Color.Transparent;
                }
                else if (ctrl is TextBox tb)
                {
                    tb.BackColor = Color.LemonChiffon;
                    tb.ForeColor = Color.Black;
                    tb.BorderStyle = BorderStyle.FixedSingle;
                    tb.Font = new Font("Courier New", 10);
                }
                else if (ctrl is Button btn)
                {
                    btn.BackColor = Color.SaddleBrown;
                    btn.ForeColor = Color.White;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Font = new Font("Georgia", 9, FontStyle.Bold);
                }
                else if (ctrl is ComboBox cb)
                {
                    cb.BackColor = Color.LemonChiffon;
                    cb.ForeColor = Color.Black;
                    cb.Font = new Font("Georgia", 9);
                }
            }
        }
    }
}
