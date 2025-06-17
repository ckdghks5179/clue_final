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
using System.Net.Sockets;
using System.Threading;
using static System.Windows.Forms.AxHost;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Security.Cryptography;



//https://github.com/ckdghks5179/clue_game
//https://github.com/ckdghks5179/clue_game.git


namespace clue_game6
{
    public partial class Form1 : Form
    {
        Form2 notePad;
        Form3 suggest;

        private int remainingSteps = 0;
        private Point nextLogicalPosition;
        private bool isMoving = false;
        Point targetPosition;
        int stepSize = 4;
        Queue<Point> moveQueue = new Queue<Point>();
        PictureBox movingPlayerBox;

        private Random rand = new Random();
        private Random random = new Random();

        private Image[] diceImages;
        private System.Windows.Forms.Timer diceTimer;
        private int animationCounter;
        private int finalDice1;
        private int finalDice2;

        private Dictionary<Button, System.Windows.Forms.Timer> hoverTimers = new Dictionary<Button, System.Windows.Forms.Timer>();
        private Dictionary<Button, int> originalTops = new Dictionary<Button, int>();
        private Dictionary<Button, int> currentOffsets = new Dictionary<Button, int>();
        private Dictionary<Button, bool> hovering = new Dictionary<Button, bool>();

        // 네트워크 스트림 및 온라인 여부 플래그gina
        private NetworkStream stream;      // 서버와의 통신에 사용됨
        private bool isNetworkMode = false; // 온라인 모드 여부
        // 게임 상태 및 현재 플레이어 정보
        private GameState gameState;
        private int playerId;
        private PictureBox myPlayerBox;
        private Dictionary<int, PictureBox> playerBoxes = new Dictionary<int, PictureBox>();

        private Player[] playerList => gameState.Players;
        private Player player;

        private int RollDice()
        {
            return rand.Next(1, 7);
        }
        private void Init(GameState state, int id)
        {
            this.gameState = state;
            this.playerId = id;
            this.player = state.Players[id];
        }
        public Form1(GameState gamestate1, int playerId)
        {
            InitializeComponent();
            ApplyDetectiveStyle();
            Init(gamestate1, playerId);
            this.isNetworkMode = false;//온라인 모드가 아님
        }
        /// <summary>
        /// 온라인 모드 생성자
        /// </summary>
        public Form1(GameState state, int playerId, NetworkStream stream)
        {
            InitializeComponent();
            ApplyDetectiveStyle();
            Init(state, playerId);
            this.stream = stream;
            this.isNetworkMode = true;
            PlayerChoose.AllPlayerForms.Add(this); // 모든 플레이어 폼 리스트에 등록
            this.gameState = state;
            this.playerId = playerId;
            this.player = playerList[playerId];
        }
        // 서버 메시지 수신 쓰레드 시작
        private void StartListening()
        {
            Thread t = new Thread(new ThreadStart(ReceiveLoop));
            t.IsBackground = true;
            t.Start();
        }
        // 서버로부터 메시지를 계속 수신하여 처리
        private void ReceiveLoop()
        {
            byte[] buffer = new byte[1024];
            StringBuilder incomingData = new StringBuilder();

            while (true) 
            {
                int bytes = stream.Read(buffer, 0, buffer.Length);
                string chunk = Encoding.UTF8.GetString(buffer, 0, bytes);
                incomingData.Append(chunk);
                while (true)
                {
                    string full = incomingData.ToString();
                    int newlineIndex = full.IndexOf('\n');
                    if (newlineIndex == -1) break;

                    string msg = full.Substring(0, newlineIndex).Trim(); 
                    incomingData.Remove(0, newlineIndex + 1); 

                    string[] parts = msg.Split('|');

                    //if (parts[0] == "MOVE")
                    //{
                    //    // 이동 메시지 수신 처리
                    //    int id = int.Parse(parts[1]);
                    //    int x = int.Parse(parts[2]);
                    //    int y = int.Parse(parts[3]);

                    //    gameState.Players[id].x = x;
                    //    gameState.Players[id].y = y;
                    //    this.Invoke((MethodInvoker)(() =>
                    //    {
                    //        foreach (var form in PlayerChoose.AllPlayerForms)
                    //        {
                    //            form.UpdatePlayerPositions();// UI 동기화
                    //        }
                    //    }));
                    //}
                    if (parts[0] == "MOVE")
                    {
                        int id = int.Parse(parts[1]);
                        int x = int.Parse(parts[2]);
                        int y = int.Parse(parts[3]);

                        foreach (var form in PlayerChoose.AllPlayerForms)
                        {
                            form.gameState.Players[id].x = x;
                            form.gameState.Players[id].y = y;
                        }

                        this.Invoke((MethodInvoker)(() =>
                        {
                            foreach (var form in PlayerChoose.AllPlayerForms)
                            {
                                form.UpdatePlayerPositions(); // UI 동기화
                            }
                        }));
                    }
                    else if (parts[0] == "TURN")
                    {
                        // 턴 전달 처리
                        int index = int.Parse(parts[1]);
                       this.Invoke((MethodInvoker)(() =>
                        {
                            SetTurn(index);
                        }));

                    }
                    else if (parts[0] == "SUGGEST_REPLY")
                    {
                        // 추리 응답 처리 (카드 공개)
                        int from = int.Parse(parts[1]);
                        int to = int.Parse(parts[2]);
                        string type = parts[3];
                        string name = parts[4];

                        this.Invoke((MethodInvoker)(() =>
                        {
                            if (playerId == to)
                            {
                                textBox1.AppendText($"플레이어 {from + 1} 이(가) 카드를 보여줬습니다: <{type}> {name}\r\n");
                            }
                            else
                            {
                                textBox1.AppendText($"플레이어 {from + 1} 이(가) 플레이어 {to + 1} 에게 카드를 보여줬습니다.\r\n");
                            }
                        }));
                    }
                    else if (parts[0] == "SUGGEST_REPLY_LOG" && parts.Length >= 3)
                    {
                        int from = int.Parse(parts[1]);
                        int to = int.Parse(parts[2]);

                        BroadcastLogToAllForms($"→ 플레이어 {from + 1} 이(가) 플레이어 {to + 1} 의 추리에 반박했습니다.");

                    }
                    else if (parts[0] == "SUGGEST_NO_REPLY")
                    {
                        int to = int.Parse(parts[1]);
                        BroadcastLogToAllForms($"❗ 플레이어 {to + 1} 의 추리에 아무도 반박하지 못했습니다.");

                    }
                    else if (parts[0] == "FINAL_SUGGEST" && parts.Length == 5)
                    {
                        // 최종 추리 메시지 출력
                        int who = int.Parse(parts[1]);
                        string man = parts[2];
                        string weapon = parts[3];
                        string room = parts[4];

                        string log = $"[최종추리] Player{who + 1}: {man}가 {room}에서 {weapon}으로 범행";
                        BroadcastLogToAllForms(log);

                    }
                    else if (parts[0] == "SUGGEST" && parts.Length >= 2)
                    {
                        // 추리 메시지 수신 시 텍스트창에 출력
                        string suggestionText = msg.Substring("SUGGEST|".Length);
                        BroadcastLogToAllForms("[추리 메시지] " + suggestionText);

                    }
                    else if (parts[0] == "SUGGEST_REPLY_NOTICE")
                    {
                        // 추리 응답 통보만 (카드 정보 없이, 모두에게 공개)
                        int from = int.Parse(parts[1]);
                        int to = int.Parse(parts[2]);
                        BroadcastLogToAllForms($"플레이어 {from + 1} 이(가) 플레이어 {to + 1} 에게 카드를 보여줬습니다.");
                    }
                    else if (parts[0] == "UPDATE_UI")
                    {
                        int who = int.Parse(parts[1]);
                        this.Invoke((MethodInvoker)(() =>
                        {
                            foreach (var form in PlayerChoose.AllPlayerForms)
                            {
                                form.UpdateControlState();
                            }
                        }));
                    }
                    else if (parts[0] == "CHAT" && parts.Length >= 3)
                    {
                        string from = parts[1];
                        string text = parts[2];
                        BroadcastLogToAllForms($"💬 {from}: {text}");
                    }
                    else if (parts[0] == "PLAYER_WIN" && parts.Length == 2)
                    {
                        int winner = int.Parse(parts[1]);
                        this.Invoke((MethodInvoker)(() =>
                        {
                            MessageBox.Show($"🎉 Player {winner + 1} 가 정답을 맞춰 승리했습니다!", "게임 종료");
                            Application.Exit();
                        }));
                    }
                    else if (parts[0] == "PLAYER_FAIL" && parts.Length == 2)
                    {
                        int loser = int.Parse(parts[1]);
                        this.Invoke((MethodInvoker)(() =>
                        {
                            if (playerId == loser)
                            {
                                MessageBox.Show("❌ 당신은 틀렸습니다. 게임에서 탈락했습니다.", "게임 종료");
                                player.isAlive = false;
                            }
                            else
                            {
                                BroadcastLogToAllForms($"⚠️ Player {loser + 1} 가 최종 추리에 실패해 탈락했습니다.");
                            }
                        }));
                    }
                    else if (!msg.Contains("|"))
                    {
                        this.Invoke((MethodInvoker)(() =>
                        {
                            textBox1.AppendText(msg + "\r\n");
                        }));
                    }
                }
            }
        }
        /// ///////////////////
        private void UpdateControlState()
        {

            bool isMyTurn = gameState.CurrentTurn == playerId;
            btnRoll.Enabled = isMyTurn && !player.hasRolled;
            btnTurnEnd.Enabled = isMyTurn;
            btnFinalSug.Enabled = isMyTurn && player.isFinalRoom;
            btnSug.Enabled = isMyTurn && player.isInRoom && !player.hasSuggested;
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
            int cellSize = 36;
            int boxSize = 16;

            for (int i = 0; i < gameState.TotalPlayers; i++)
            {
                var p = gameState.Players[i];

                if (playerBoxes.ContainsKey(i))
                {
                    PictureBox playerBox = playerBoxes[i];

                    Point cellPoint = gameState.clue_map_point[p.x, p.y];
                    playerBox.Location = new Point(
                        cellPoint.X + cellSize / 2 - boxSize / 2,
                        cellPoint.Y - boxSize / 2
                    );
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = $"Clue Game - Player {playerId + 1} ({player.name})";

            // 플레이어 카드 표시
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

            // ===== playerBox 생성 및 위치 설정 =====
            int cellSize = 36;
            int boxSize = 16;

            for (int i = 0; i < gameState.TotalPlayers; i++)
            {
                Player p = gameState.Players[i];

                PictureBox playerBox = new PictureBox
                {
                    Name = $"playerBox{i}",
                    Size = new Size(boxSize, boxSize),
                    BackColor = GetPlayerColor(i),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BorderStyle = BorderStyle.FixedSingle
                };

                Point cellPoint = gameState.clue_map_point[p.x, p.y];
                playerBox.Location = new Point(
                    cellPoint.X + cellSize / 2 - boxSize / 2,
                    cellPoint.Y  - boxSize / 2
                );

                playerBoxes[i] = playerBox;
                this.Controls.Add(playerBox);
                playerBox.BringToFront();

                if (i == playerId)
                {
                    myPlayerBox = playerBox;
                }
            }

            // ===== 주사위 이미지 배열 설정 =====
            diceImages = new Image[]
            {
                Properties.Resources.dice_1_icon,
                Properties.Resources.dice_2_icon,
                Properties.Resources.dice_3_icon,
                Properties.Resources.dice_4_icon,
                Properties.Resources.dice_5_icon,
                Properties.Resources.dice_6_icon
            };

            // ===== 주사위 애니메이션 타이머 설정 =====
            diceTimer = new System.Windows.Forms.Timer();
            diceTimer.Interval = 100;
            diceTimer.Tick += DiceTimer_Tick;

            // ===== 이동 타이머 이벤트 연결
            moveTimer.Tick += moveTimer_Tick;

            // ===== 초기 UI 상태 갱신 =====
            UpdateControlState();
            UpdateCurrentPlayerLabel();

            // ===== 창 크기 및 온라인 수신 시작 =====
            this.ClientSize = new Size(800, 550);
            if (isNetworkMode) StartListening();
        }


        private void btnRoll_Click(object sender, EventArgs e)
        {
            finalDice1 = RollDice();
            finalDice2 = RollDice();
            int total = finalDice1 + finalDice2;
            animationCounter = 0;
            diceTimer.Start();
            btnRoll.Enabled = false;
            btnTurnEnd.Enabled = false;

            remainingSteps = total;
            dice1.Text = finalDice1.ToString();
            labelDice2.Text = finalDice2.ToString();


            btnRoll.Enabled = false;
            pictureBoxDice.Image = GetDiceImage(finalDice1);
            pictureBoxDice2.Image = GetDiceImage(finalDice2);
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.dice_142528);
            player.Play();

            btnTurnEnd.Enabled = true;

            btnUp.Enabled = true;
            btnDown.Enabled = true;
            btnLeft.Enabled = true;
            btnRight.Enabled = true;
        }
        private void DiceTimer_Tick(object sender, EventArgs e)
        {
            animationCounter++;
            int temp1 = RollDice();
            int temp2 = RollDice();

            pictureBoxDice.Image = diceImages[temp1 - 1];
            pictureBoxDice2.Image = diceImages[temp2 - 1];
            if(animationCounter >= 10)
            {
                diceTimer.Stop();
                pictureBoxDice.Image = diceImages[finalDice1 - 1];
                pictureBoxDice2.Image = diceImages[finalDice2 - 1];
            }
        }
        private Image GetDiceImage(int value)
        {
            switch (value)
            {
                case 1: return Properties.Resources.dice_1_icon;
                case 2: return Properties.Resources.dice_2_icon;
                case 3: return Properties.Resources.dice_3_icon;
                case 4: return Properties.Resources.dice_4_icon;
                case 5: return Properties.Resources.dice_5_icon;
                case 6: return Properties.Resources.dice_6_icon;
                default: return null;
            }
        }

        private void TryMove(int dx, int dy)
        {
            Console.WriteLine($"[TryMove] 시작 - remainingSteps: {remainingSteps}, isMoving: {isMoving}");

            if (isMoving)
            {
                Console.WriteLine("[TryMove] 현재 이동 중이므로 이동 불가.");
                return;
            }

            if (remainingSteps <= 0)
            {
                Console.WriteLine("[TryMove] 남은 이동 횟수가 0 이하이므로 이동 불가.");
                return;
            }

            int newX = player.x + dx;
            int newY = player.y + dy;

            if (newX < 0 || newX >= 25 || newY < 0 || newY >= 24)
            {
                Console.WriteLine($"[TryMove] 좌표 범위 초과: ({newX}, {newY})");
                return;
            }

            if (gameState.clue_map[newX, newY] == 1)
            {
                Console.WriteLine($"[TryMove] 벽에 막힘: clue_map[{newX}, {newY}] == 1");
                return;
            }

            foreach (var other in gameState.Players)
            {
                if (other != player && other.x == newX && other.y == newY)
                {
                    Console.WriteLine($"[TryMove] 다른 플레이어가 해당 위치({newX}, {newY})에 있음");
                    return;
                }
            }

            nextLogicalPosition = new Point(newX, newY);
            targetPosition = gameState.clue_map_point[newX, newY];
            isMoving = true;
            movingPlayerBox = playerBoxes[playerId];

            Console.WriteLine($"[TryMove] 이동 시작 - 타겟: {targetPosition}, 논리 위치: ({newX}, {newY})");

            moveTimer.Start();
        }
        /// <summary>
        /// gina온라인 모드 방송 정보
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(string msg)
        {
            if (stream != null && stream.CanWrite)
            {
                byte[] data = Encoding.UTF8.GetBytes(msg + "\n");
                stream.Write(data, 0, data.Length);
                
            }
            else
            {
                Console.WriteLine($"[CLIENT] ❌ ：stream  null ！");
            }
        }
        //gina
        public void MovePlayerExternally(int id, int x, int y)
        {
            if (id >= 0 && id < gameState.TotalPlayers)
            {
                var p = gameState.Players[id];
                p.x = x;
                p.y = y;
                playerBoxes[id].Location = gameState.clue_map_point[x, y];
            }
        }
        //gina
        public void SetTurn(int index)
        {
            gameState.CurrentTurn = index;
            lbRemain.Text = "0"; 


            if (playerId == index)
            {
                player.hasRolled = false;
                btnRoll.Enabled = true;
                player.hasSuggested = false;

                lbRemain.Text = "0";
                MessageBox.Show("당신의 턴입니다!");
                btnRoll.Enabled = true;
                btnTurnEnd.Enabled = false;
            }
            else
            {
                lbRemain.Text = "0";
                btnRoll.Enabled = false;
                btnTurnEnd.Enabled = false;
            }
            
            UpdateControlState();
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
            // Gina온라인 모드
            if (isNetworkMode)
            {
                if (stream != null && stream.CanWrite)
                {
                   
                    Thread.Sleep(50); 
                    SendMessage($"END_TURN|{playerId}");
                }
            }
            else
            {
               
            lbRemain.Text = "0";
            player.hasSuggested = false;

            gameState.AdvanceTurn();
            foreach (var form in PlayerChoose.AllPlayerForms)
                {
                    form.UpdateControlState();
                    form.UpdatePlayerPositions();
                    form.UpdateCurrentPlayerLabel();
                }
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
            // Gina온라인 모드
            if (isNetworkMode)
                suggest = new Form3(gameState, player, 1, playerId, true, stream);
            else
                suggest = new Form3(gameState, player, 1, playerId);

            suggest.Show();
            player.hasSuggested = true;
        }

        private void btnFinalSug_Click(object sender, EventArgs e)
        {
            //  Gina온라인 모드
            if (isNetworkMode)
                suggest = new Form3(gameState, player, 2, playerId, true, stream);
            else
                suggest = new Form3(gameState, player, 2, playerId);
            suggest.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //  Gina온라인 모드
            if (isNetworkMode)
            {
                string chat = text_Chat.Text.Trim();
                if (!string.IsNullOrEmpty(chat))
                {
                    SendMessage($"CHAT|{player.name}|{chat}");
                    text_Chat.Clear(); 
                }
                Console.WriteLine($"💬 [发送测试] {player.name}: {chat}");
            }
            else
            {
                MessageBox.Show("오프라인 모드에서는 채팅이 지원되지 않습니다.");
            }
        }
        //gina
        public void ShowSuggestionMessage(string text)
        {
            textBox1.AppendText(text + "\r\n");
        }
        public bool IsNetworkMode()
        {
            return isNetworkMode;
        }

        public void SendSuggestion(string text)
        {
            SendMessage($"SUGGEST|{text}");
        }
        public static void BroadcastLogToAllForms(string log)
        {
            foreach (var form in PlayerChoose.AllPlayerForms)
            {
                form.Invoke(new Action(() =>
                {
                    form.textBox1.AppendText(log + "\r\n");
                }));
            }
        }
        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            saveFileDialog.FileName = $"ClueGameLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            pictureBox1.Image.Save("extracted_image.png", System.Drawing.Imaging.ImageFormat.Png);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                gameState.SaveLogToFile(saveFileDialog.FileName);
            }
        }
         
        private void UpdateCurrentPlayerLabel()
        {
            int current = gameState.CurrentTurn;
            labelCurrentPlayer.Text = $"Current Player: {current + 1}";
            if (current == playerId)
                labelCurrentPlayer.ForeColor = Color.Green;
            else
                labelCurrentPlayer.ForeColor = Color.Black;
        }
        void MovePlayerTo(Point destination)
        {
            targetPosition = destination;
            moveTimer.Start();
        }
        private int moveStepCounter = 0;

        private void moveTimer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("[moveTimer_Tick] 호출됨");

            if (movingPlayerBox == null)
            {
                Console.WriteLine("[moveTimer_Tick] movingPlayerBox가 null → 중지");
                moveTimer.Stop();
                isMoving = false;
                return;
            }

            int dx = targetPosition.X - movingPlayerBox.Left;
            int dy = targetPosition.Y - movingPlayerBox.Top;

            if (Math.Abs(dx) <= stepSize && Math.Abs(dy) <= stepSize)
            {
                Console.WriteLine("[moveTimer_Tick] 목표 위치 도달");

                movingPlayerBox.Left = targetPosition.X;
                movingPlayerBox.Top = targetPosition.Y;
                moveTimer.Stop();

                int newX = nextLogicalPosition.X;
                int newY = nextLogicalPosition.Y;
                player.x = newX;
                player.y = newY;

                // clue_map 업데이트
                if (gameState.clue_map[player.x, player.y] != 2 && gameState.clue_map[player.x, player.y] != 5)
                {
                    gameState.clue_map[player.x, player.y] = 0; // 이전 위치 초기화
                }
                gameState.clue_map[newX, newY] = 3;

                // 방 판정 (열, 행 순서)
                Point dest = new Point(player.y, player.x);
                if (gameState.roomTiles.Contains(dest) || gameState.clue_map[newX, newY] == 2)
                {
                    player.isInRoom = true;
                    player.isFinalRoom = false;
                    Console.WriteLine("[moveTimer_Tick] 일반 방 진입");
                }
                else if (gameState.finalRoomTiles.Contains(dest) || gameState.clue_map[newX, newY] == 5)
                {
                    player.isInRoom = false;
                    player.isFinalRoom = true;
                    Console.WriteLine("[moveTimer_Tick] 최종 방 진입");
                }
                else
                {
                    player.isInRoom = false;
                    player.isFinalRoom = false;
                    Console.WriteLine("[moveTimer_Tick] 방이 아님");
                }

                // 위치 반영 및 UI 갱신
                foreach (var form in PlayerChoose.AllPlayerForms)
                {
                    form.gameState.Players[playerId].x = newX;
                    form.gameState.Players[playerId].y = newY;

                    if (form.playerBoxes.ContainsKey(playerId))
                    {
                        form.playerBoxes[playerId].Location = form.gameState.clue_map_point[newX, newY];
                    }

                    form.UpdatePlayerPositions();
                    form.UpdateControlState(); // 방 진입 여부에 따라 추리 버튼 활성화
                }

                remainingSteps--;
                lbRemain.Text = remainingSteps.ToString();
                Console.WriteLine($"[moveTimer_Tick] 남은 이동: {remainingSteps}");

                if (remainingSteps <= 0)
                {
                    btnUp.Enabled = false;
                    btnDown.Enabled = false;
                    btnLeft.Enabled = false;
                    btnRight.Enabled = false;
                }

                isMoving = false;
                moveStepCounter = 0;
                return;
            }

            // 애니메이션 이동
            if (dx != 0)
                movingPlayerBox.Left += stepSize * Math.Sign(dx);
            if (dy != 0)
                movingPlayerBox.Top += stepSize * Math.Sign(dy);

            // 안전장치: 너무 오래 반복되면 강제 종료
            moveStepCounter++;
            if (moveStepCounter > 60)
            {
                Console.WriteLine("[moveTimer_Tick] 무한 이동 감지 → 강제 종료");
                moveTimer.Stop();
                isMoving = false;
                moveStepCounter = 0;
            }
        }

        private void ApplyDetectiveStyle()
        {
            //  Общий стиль: 
            this.BackColor = Color.FromArgb(210, 180, 140);

            //  Label: 
            labelCurrentPlayer.Font = new Font("Georgia", 12, FontStyle.Bold);
            labelCurrentPlayer.ForeColor = Color.DarkOliveGreen;
            labelCurrentPlayer.BackColor = Color.Transparent;
            labelChat.Font = new Font("Georgia", 12, FontStyle.Bold);
            labelChat.ForeColor = Color.Black;
            labelChat.BackColor = Color.Transparent;
            label3.Font = new Font("Georgia", 12, FontStyle.Bold);
            label3.ForeColor = Color.Black;
            label3.BackColor = Color.Transparent;


            //  Кнопки
            StyleButton(btnRoll, Properties.Resources.dice_cube);
            StyleButton(btnTurnEnd, Properties.Resources.next);
            StyleButton(btnNote, Properties.Resources.note);
            StyleButton2(btnFinalSug);
            StyleButton2(button1);
            StyleButton2(btnSug);
            StyleButton2(btnSaveLog);


            // TextBox: memo и сообщения
            textBox1.BackColor = Color.Bisque;
            textBox1.ForeColor = Color.Black;
            textBox1.Font = new Font("Courier New", 10, FontStyle.Regular);
            textBox1.BorderStyle = BorderStyle.FixedSingle;

            //  Список карт
            textBox2.BackColor = Color.LemonChiffon;
            textBox2.ForeColor = Color.Black;
            textBox2.Font = new Font("Consolas", 9, FontStyle.Regular);

        }

        private void StyleButton(Button btn, Image backgroundImage = null)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.BackColor = Color.Transparent;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Georgia", 10, FontStyle.Bold);


            btn.Cursor = Cursors.Hand;

            if (backgroundImage != null)
            {
                btn.BackgroundImage = backgroundImage;
                btn.BackgroundImageLayout = ImageLayout.Stretch;
            }
            if (!originalTops.ContainsKey(btn))
                originalTops[btn] = btn.Top;

            currentOffsets[btn] = 0;
            hovering[btn] = false;

            btn.MouseEnter -= btn_MouseEnter;
            btn.MouseLeave -= btn_MouseLeave;
            btn.MouseEnter += btn_MouseEnter;
            btn.MouseLeave += btn_MouseLeave;

            if (!hoverTimers.ContainsKey(btn))
            {
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 15;
                timer.Tick += (s, e) => AnimateButton(btn);
                hoverTimers[btn] = timer;
                timer.Start();
            }
        }
        private void StyleButton2(Button btn)
        {
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.BackColor = Color.SaddleBrown;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Georgia", 9, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            currentOffsets[btn] = 0;
            hovering[btn] = false;

            btn.MouseEnter -= btn_MouseEnter;
            btn.MouseLeave -= btn_MouseLeave;
            btn.MouseEnter += btn_MouseEnter;
            btn.MouseLeave += btn_MouseLeave;

            if (!hoverTimers.ContainsKey(btn))
            {
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 15;
                timer.Tick += (s, e) => AnimateButton(btn);
                hoverTimers[btn] = timer;
                timer.Start();
            }
        }

        private void btn_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                hovering[btn] = true;
            }
        }
        private void btn_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                hovering[btn] = false;
            }
        }
        private void AnimateButton(Button btn)
        {
            if (!originalTops.ContainsKey(btn)) return;

            int maxOffset = 5;
            int speed = 1;

            int targetOffset = hovering[btn] ? -maxOffset : 0;
            int currentOffset = currentOffsets[btn];

            if (currentOffset != targetOffset)
            {
                int direction = (targetOffset > currentOffset) ? 1 : -1;
                currentOffset += speed * direction;

                if ((direction == -1 && currentOffset < targetOffset) || (direction == 1 && currentOffset > targetOffset))
                    currentOffset = targetOffset;

                btn.Top = originalTops[btn] + currentOffset;
                currentOffsets[btn] = currentOffset;
            }
        }
    }
}

