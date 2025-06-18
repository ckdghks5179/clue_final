using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace clue_game6
{
    public partial class WaitingForm: Form
    {
        public WaitingForm()
        {
            InitializeComponent();
        }
        private TcpClient client;
        private NetworkStream stream;
        private string myName;
        private bool stopReceiving = false; //  控制是否继续读



        public WaitingForm(TcpClient client, string name, int maxPlayers)
        {
            InitializeComponent();
            this.client = client;
            this.myName = name;
            this.stream = client.GetStream();
            txtPlayerName.Text = myName;
            txtServerIP.Text = client.Client.RemoteEndPoint.ToString();
            //lblPlayerCount.Text = maxPlayers.ToString();


        }

        private void StartListening()
        {
            Thread t = new Thread(new ThreadStart(ReceiveLoop));
            t.IsBackground = true;
            t.Start();
        }


        private void ReceiveLoop()
        {
            byte[] buffer = new byte[8192];
            StringBuilder incomingData = new StringBuilder();

            while (!stopReceiving)
            {
                int bytes = 0;
                try
                {
                    bytes = stream.Read(buffer, 0, buffer.Length);
                }
                catch
                {
                    break;
                }

                if (bytes == 0|| stopReceiving) break;

                string chunk = Encoding.UTF8.GetString(buffer, 0, bytes);
                incomingData.Append(chunk);
                //Console.WriteLine("[watainClient Chunk] " + chunk);

                while (true)
                {
                    string allData = incomingData.ToString();
                    int newlineIndex = allData.IndexOf('\n');
                    if (newlineIndex == -1) break;

                    // 拿出完整一条消息
                    string msg = allData.Substring(0, newlineIndex).Trim();
                    incomingData.Remove(0, newlineIndex + 1);

                    if (string.IsNullOrWhiteSpace(msg)) continue;

                    // --------- 原来处理每条 msg 的代码保持不变 ---------
                    if (msg.StartsWith("GAME_START|"))
                    {
                        //Console.WriteLine("收到原始 GAME_START 消息：" + msg);
                        stopReceiving = true;
                        try
                        {
                            string json = msg.Substring("GAME_START|".Length);
                            var serializer = new DataContractJsonSerializer(typeof(NetworkGameState));
                            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                            {
                                NetworkGameState netState = (NetworkGameState)serializer.ReadObject(ms);
                                GameState state = ConvertToOriginalGameState(netState);

                                state.clue_map_point = new Point[25, 24];
                                for (int i = 0; i < 25; i++)
                                {
                                    for (int j = 0; j < 24; j++)
                                    {
                                        state.clue_map_point[i, j] = new Point(8 + j * 20, 8 + i * 16);
                                    }
                                }

                                int myId = state.Players.FirstOrDefault(p => p.name == myName)?.id ?? 0;

                                this.Invoke(new Action(() =>
                                {
                                    OpenGameForm(state, myId);
                                }));
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("게임 시작 중 오류 발생: " + ex.Message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    
                    else if (msg.StartsWith("PLAYER_COUNT|"))
                    {
                        string countStr = msg.Substring("PLAYER_COUNT|".Length);
                        this.Invoke(new MethodInvoker(delegate
                        {
                            UpdatePlayerCountLabel(countStr);
                        }));
                    }
                    else if (msg.StartsWith("PLAYER_LIST|"))
                    {
                        string playerListStr = msg.Substring("PLAYER_LIST|".Length);
                        string[] players = playerListStr.Split(',');

                        this.Invoke(new MethodInvoker(delegate
                        {
                            UpdatePlayerList(players);
                        }));
                    }
                    else
                    {
                        if (!msg.Contains("|"))
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                ShowChatMessage(msg);
                            }));
                        }
                    }
                }
            }
        }

        private void OpenGameForm(GameState state, int myId)
        {
            this.Hide();
            Form1 gameForm = new Form1(state, myId,stream);
            gameForm.Owner = this;
            gameForm.Show();
        }

        private void UpdatePlayerCountLabel(string countStr)
        {
            lblPlayerCount.Text = countStr;
        }

        private void UpdatePlayerList(string[] players)
        {
            lstPlayers.Clear();
            for (int i = 0; i < players.Length; i++)
            {
                lstPlayers.AppendText(players[i] + "\r\n");
            }
        }

        private void ShowChatMessage(string msg)
        {
            txtStatus.AppendText(msg + "\r\n");
        }

        private void WaitingForm_Load(object sender, EventArgs e)
        {
            // 启动接收线程
            StartListening();
            txtStatus.AppendText("서버에 연결 중...\r\n");
        }

        //转换函数
        private GameState ConvertToOriginalGameState(NetworkGameState net)
        {
            GameState original = new GameState();
            original.TotalPlayers = net.TotalPlayers;
            original.Players = new Player[net.Players.Count];

            for (int i = 0; i < net.Players.Count; i++)
            {
                var np = net.Players[i];
                Player p = new Player();
                p.id = np.id;
                p.name = np.name;
                p.x = np.x;
                p.y = np.y;
                p.isTurn = np.isTurn;

                p.hands = new List<Card>();
                foreach (var c in np.hands)
                {
                    p.hands.Add(new Card(c.type, c.name));
                }

                original.Players[i] = p;
            }

            original.openCard = new List<Card>();
            foreach (var c in net.openCard)
            {
                original.openCard.Add(new Card(c.type, c.name));
            }

            return original;
        }

    }
}



