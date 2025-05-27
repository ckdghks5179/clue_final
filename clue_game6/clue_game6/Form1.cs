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
using System.Security.Cryptography;


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

        private int[,] clue_map => gameState.clue_map;

        private Player[] playerList => gameState.Players;
        private Player player;
        List<Card> cardList = new List<Card>();

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
            btnRoll.Enabled = isMyTurn && !player.hasRolled;
            btnTurnEnd.Enabled = isMyTurn;

            btnFinalSug.Enabled = isMyTurn && player.isFinalRoom;
            btnSug.Enabled = isMyTurn && player.isInRoom && !player.hasSuggested;
            btnSecPass.Enabled = isMyTurn &&
                     gameState.secretPassPoints.ContainsKey(new Point(player.y, player.x)) &&
                     !player.usedSecretPass;
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
            for (int i = 0; i < player.hands.Count(); i++)
            {
                textBox2.Text += "<" + player.hands[i].type + ">" + " " + player.hands[i].name + "\r\n";
            }
            textBox2.Text += "-----public card------\r\n";
            for (int i = 0; i < gameState.openCard.Count(); i++)
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

            this.ClientSize = new Size(800, 550); //창 키울 필요 없게
        }

        private void btnRoll_Click(object sender, EventArgs e)
        {
            int diceValue = RollDice();
            dice1.Text = diceValue.ToString();
            lbRemain.Text = diceValue.ToString();

            player.hasRolled = true;
            btnRoll.Enabled = false;
        }

        private void TryMove(int dx, int dy)
        {
            if (int.Parse(lbRemain.Text) <= 0) return;

            int newX = player.x + dx;
            int newY = player.y + dy;

            if (newX < 0 || newX >= 25 || newY < 0 || newY >= 24) return;
            if (gameState.clue_map[newX, newY] == 1) return; // 벽이면 막음

            // 다른 플레이어가 해당 좌표에 있는 경우 이동 금지
            foreach (var other in gameState.Players)
            {
                if (other != player && other.x == newX && other.y == newY)
                    return;
            }

            //방 진입 , 최종 방 진입 여부 판정
            Point dest = new Point(newY, newX); // 열, 행 순서

            // 방 입구 좌표일 경우 방 진입
            if (gameState.roomTiles.Contains(dest) || gameState.clue_map[newX, newY] == 2)
            {
                player.isInRoom = true;
                player.isFinalRoom = false;
                lbRemain.Text = "1";
            }
            //최종 추리 방 입구
            else if (gameState.finalRoomTiles.Contains(dest) || gameState.clue_map[newX, newY] == 5)
            {
                player.isInRoom = false;
                player.isFinalRoom = true;
                lbRemain.Text = "1";
            }
            else
            {
                player.isInRoom = false;
                player.isFinalRoom = false;
            }


            // 이전 위치가 방이면 clue_map을 2로, 아니면 0으로 되돌림
            if (gameState.clue_map[player.x, player.y] == 2 || gameState.clue_map[player.x, player.y] == 5)
                gameState.clue_map[player.x, player.y] = gameState.clue_map[player.x, player.y]; // 그대로 유지
            else
                gameState.clue_map[player.x, player.y] = 0;

            // 이동 및 좌표 업데이트
            player.x = newX;
            player.y = newY;
            gameState.clue_map[newX, newY] = 3;
            playerBoxes[playerId].Location = gameState.clue_map_point[newX, newY];

            // 이동 횟수 감소
            lbRemain.Text = (int.Parse(lbRemain.Text) - 1).ToString();

            // 위치 및 버튼 상태 갱신
            foreach (var form in PlayerChoose.AllPlayerForms)
            {
                form.UpdatePlayerPositions();
                form.UpdateControlState(); // 버튼 상태 동기화
            }

            Console.WriteLine($"[디버그] Player {playerId + 1} 위치: ({player.x}, {player.y}), clue_map: {gameState.clue_map[player.x, player.y]}");
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
            player.hasRolled = false;
            player.hasSuggested = false;
            player.usedSecretPass = false;

            gameState.AdvanceTurn();
            foreach (var form in PlayerChoose.AllPlayerForms)
            {
                form.UpdateControlState();
                form.UpdatePlayerPositions();
            }
        }

        private void btnNote_Click(object sender, EventArgs e)
        {
            notePad = new Form2(player, gameState);
            notePad.Show();
        }

        private void btnSug_Click(object sender, EventArgs e)
        {
            if (player.hasSuggested)
            {
                MessageBox.Show("이미 추리를 했습니다.");
                return;
            }
            suggest = new Form3(gameState, player, 1, playerId);
            suggest.Show();
            player.hasSuggested = true;
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

        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            saveFileDialog.FileName = $"ClueGameLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                gameState.SaveLogToFile(saveFileDialog.FileName);
            }
        }

        private void btnSecPass_Click(object sender, EventArgs e) //비밀 통로 이동
        {
            if (player.usedSecretPass)
            {
                MessageBox.Show("이 턴에는 이미 비밀 통로를 사용했습니다.");
                return;
            }

            Point current = new Point(player.y, player.x);
            string currentRoom = gameState.GetRoomNameByPosition(player.x, player.y);

            if (gameState.secretPassPoints.ContainsKey(current))
            {
                Point destination = gameState.secretPassPoints[current];
                player.x = destination.Y;
                player.y = destination.X;
                string destiRoom = gameState.GetRoomNameByPosition(player.x, player.y);
                playerBoxes[playerId].Location = gameState.clue_map_point[player.x, player.y];

                gameState.AddLog($"Player {playerId + 1}가 비밀 통로를 통해 {currentRoom}에서 {destiRoom}으로 이동했습니다.");
                lbRemain.Text = "0"; // 비밀 통로 이동 후 그 턴에 다른곳으로 이동 불가능
                player.isInRoom = true;
                player.isFinalRoom = gameState.clue_map[player.x, player.y] == 5;

                player.hasRolled = true; // 비밀 통로 이동 후 주사위 굴리기 불가능
                btnRoll.Enabled = false;
                player.usedSecretPass = true;

                foreach (var form in PlayerChoose.AllPlayerForms)
                {
                    form.UpdatePlayerPositions();
                }
                UpdateControlState();
            }
            else
            {
                MessageBox.Show("현재 방에는 비밀 통로가 없습니다.");
            }
        }
    }
}

