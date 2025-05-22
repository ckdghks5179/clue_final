using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using clue_game6;


//https://github.com/ckdghks5179/clue_game
//https://github.com/ckdghks5179/clue_game.git


namespace clue_game6
{
    public partial class Form1 : Form
    {
        Form2 notePad;
        Form3 suggest;

        private GameState gameState;
        private int playerId;
        private PictureBox myPlayerBox;
        private Dictionary<int, PictureBox> playerBoxes = new Dictionary<int, PictureBox>();
        //private Point[,] clue_map_point;
        private int[,] clue_map => gameState.clue_map;

        private Player[] playerList => gameState.Players;
        private Player player;
        List<Card> cardList = new List<Card>();

        //int currentTurnPlayer = 0;

        List<string> mans = new List<string>();
        List<string> weapons = new List<string>();
        List<string> rooms = new List<string>();

        string[] man = { "Green", "Mustard", "Peacock", "Plum", "Scarlett", "White" };
        string[] weapon = { "촛대", "파이프", "리볼버", "밧줄", "렌치", "단검" };
        string[] room = { "주방", "공부방", "무도회장", "온실", "식당", "당구장", "서재", "라운지", "홀" };

        private int RollDice()
        {
            Random random = new Random();
            return random.Next(2, 13);
        }

        public Form1(GameState gamestate1, int playerId)
        {
            InitializeComponent();
            //InitializeClueMap_Point();
            //InitializeClueMap();
            //OpenPlayerChooseForm();
            this.gameState = gamestate1;
            this.playerId = playerId;
            this.player = playerList[playerId];
        }

        private void UpdateControlState()
        {
            bool isMyTurn = gameState.CurrentTurn == playerId;
            btnRoll.Enabled = isMyTurn;
            btnTurnEnd.Enabled = isMyTurn;

            btnFinalSug.Enabled = false;
            btnSug.Enabled = false;
            /* btnUp.Enabled = isMyTurn;
             btnDown.Enabled = isMyTurn;
             btnLeft.Enabled = isMyTurn;
             btnRight.Enabled = isMyTurn;
             btnTurnEnd.Enabled = isMyTurn;
            */
        }
        private Color GetPlayerColor(int id)
        {
            Color[] colors = new Color[] { Color.Green, Color.Red, Color.Blue, Color.Purple, Color.Orange, Color.White };
            return colors[id % colors.Length];
        }

        public void UpdatePlayerPositions()
        {
            for (int i = 0; i < gameState.TotalPlayers; i++)
            {
                var p = gameState.Players[i];
                if (playerBoxes.ContainsKey(i))
                {
                    playerBoxes[i].Location = gameState.clue_map_point[p.x, p.y];
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = $"Clue Game - Player {playerId + 1} ({player.name})";
            for(int i =0;i < player.hands.Count(); i++)
            {
                textBox2.Text += "<" + player.hands[i].type + ">" + " " + player.hands[i].name + "\r\n";
            }
            textBox2.Text += "-----public card------\r\n";
            for (int i =0; i< gameState.openCard.Count();i++)
            {
                textBox2.Text += "<" + gameState.openCard[i].type + ">" + " " + gameState.openCard[i].name + "\r\n";
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            for (int i = 0; i < gameState.TotalPlayers; i++)
            {
                Player p = gameState.Players[i];

                PictureBox playerBox = new PictureBox
                {
                    Name = $"playerBox{i}",
                    Size = new Size(20, 20),
                    BackColor = GetPlayerColor(i), // 각 플레이어마다 다른 색상
                    Location = gameState.clue_map_point[p.x, p.y],
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                playerBoxes[i] = playerBox;

                this.Controls.Add(playerBox);


                playerBox.BringToFront();
                //현재 Form이 담당하는 플레이어라면 저장
                if (i == playerId)
                {
                    myPlayerBox = playerBox;
                }
            }
            UpdateControlState();
        }

        private void btnRoll_Click(object sender, EventArgs e)
        {
            int diceValue = RollDice();
            dice1.Text = diceValue.ToString();
            lbRemain.Text = diceValue.ToString();
            btnRoll.Enabled = false;
        }

        private void TryMove(int dx, int dy)
        {
            if (int.Parse(lbRemain.Text) <= 0) return;

            int newX = player.x + dx;
            int newY = player.y + dy;

            if (newX < 0 || newX >= 25 || newY < 0 || newY >= 24) return;
            if (gameState.clue_map[newX, newY] == 1) return;

            foreach (var other in gameState.Players) //player 겹치는거 방지
            {
                if (other != player && other.x == newX && other.y == newY)
                    return;
            }

            if (gameState.clue_map[newX, newY] == 2) //방에 들어온 경우 이동횟수 모두 소모
            {
                player.isInRoom = true;
                btnSug.Enabled = player.isInRoom;
                lbRemain.Text = "1";
            }
            else if(gameState.clue_map[newX, newY] == 5)
            {
                player.isFinalRoom = true;
                btnFinalSug.Enabled = player.isFinalRoom;
                lbRemain.Text = "1";
            }
            else
                player.isInRoom = false;

            //방에서 나온 경우 있었던 위치를 0이 아닌 2로 바꿈
            if (gameState.clue_map[player.x, player.y] == 2)
            {
                gameState.clue_map[player.x, player.y] = 2;
            }
            else
                gameState.clue_map[player.x, player.y] = 0;  //why??

            player.x = newX;
            player.y = newY;
            gameState.clue_map[newX, newY] = 3; //why??
            playerBoxes[playerId].Location = gameState.clue_map_point[newX, newY];
            lbRemain.Text = (int.Parse(lbRemain.Text) - 1).ToString();

            foreach (var form in PlayerChoose.AllPlayerForms)
            {
                form.UpdatePlayerPositions(); //수정
            }
        }


        private void btnUp_Click(object sender, EventArgs e)
        {

            TryMove(-1, 0);
            

        }   
        private void btnDown_Click(object sender, EventArgs e)
        {

            TryMove(1, 0);
           
        }

        private void btnRight_Click(object sender, EventArgs e)
        {

            TryMove(0, 1);
           
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            TryMove(0, -1);
           
        }

        private void btnTurnEnd_Click(object sender, EventArgs e)
        {
            //btnRoll.Enabled = true;
            lbRemain.Text = "0";
            gameState.AdvanceTurn();
            foreach (var form in PlayerChoose.AllPlayerForms)
            {
                form.UpdateControlState();
                form.UpdatePlayerPositions();
            }
        }

        private void btnNote_Click(object sender, EventArgs e)
        {
            notePad = new Form2(player,gameState);
            notePad.Show();
        }

        private void btnSug_Click(object sender, EventArgs e)
        {
            suggest = new Form3(gameState, player, 1, playerId);
            suggest.Show();
        }

        private void btnFinalSug_Click(object sender, EventArgs e)
        {
            suggest = new Form3(gameState, player, 2, playerId);
            suggest.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            suggest = new Form3(gameState, player, 3, playerId);
            suggest.Show();
        }
    }
 }

