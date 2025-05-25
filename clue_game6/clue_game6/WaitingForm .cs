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



        public WaitingForm(TcpClient client, string name, int maxPlayers)
        {
            InitializeComponent();
            this.client = client;
            this.myName = name;
            this.stream = client.GetStream();
            txtPlayerName.Text = myName;
            txtServerIP.Text = client.Client.RemoteEndPoint.ToString();
            lblPlayerCount.Text = maxPlayers.ToString();


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

            while (true)
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

                if (bytes == 0) break;

                string msgChunk = Encoding.UTF8.GetString(buffer, 0, bytes);
                incomingData.Append(msgChunk);

                string[] messages = incomingData.ToString().Split('\n');

                incomingData.Clear();
                if (!msgChunk.EndsWith("\n"))
                {
                    incomingData.Append(messages[messages.Length - 1]);
                    messages = messages.Take(messages.Length - 1).ToArray();
                }

                foreach (string rawMsg in messages)
                {
                    if (string.IsNullOrWhiteSpace(rawMsg))
                        continue;
                    string msg = rawMsg.Trim();
                    if (string.IsNullOrEmpty(msg)) continue;

                    //if (msg.StartsWith("GAME_START|"))
                    //{
                    //    string json = msg.Substring("GAME_START|".Length);
                    //    //GameState state = JsonConvert.DeserializeObject<GameState>(json);
                    //    if (state == null)
                    //    {
                    //        MessageBox.Show("게임 상태 로딩 실패");
                    //        return;
                    //    }
                        // 关键：反序列化后同步clue_map_point
                       // state.ConvertNetPointsToLocal();
                        //int myId = -1;
                        //for (int i = 0; i < state.Players.Length; i++)
                        //{
                        //    if (state.Players[i].name == myName)
                        //    {
                        //        myId = i;
                        //        break;
                        //    }
                        //}

                        //if (myId != -1)
                        //{
                        //    GameState capturedState = state;
                        //    int capturedMyId = myId;

                        //    this.Invoke(new MethodInvoker(delegate
                        //    {
                        //        OpenGameForm(capturedState, capturedMyId);
                        //    }));
                        //}

                        //return;
                    //}
                    else if (msg.StartsWith("PLAYER_COUNT|"))
                    {
                        string countStr = msg.Substring("PLAYER_COUNT|".Length);
                        string capturedCountStr = countStr;

                        this.Invoke(new MethodInvoker(delegate
                        {
                            UpdatePlayerCountLabel(capturedCountStr);
                        }));
                    }
                    else if (msg.StartsWith("PLAYER_LIST|"))
                    {
                        string playerListStr = msg.Substring("PLAYER_LIST|".Length);
                        string[] players = playerListStr.Split(',');

                        string[] capturedPlayers = players;

                        this.Invoke(new MethodInvoker(delegate
                        {
                            UpdatePlayerList(capturedPlayers);
                        }));
                    }
                    else
                    {
                        if (!msg.Contains("|"))
                        {
                            string capturedMsg = msg;
                            this.Invoke(new MethodInvoker(delegate
                            {
                                ShowChatMessage(capturedMsg);
                            }));
                        }
                    }
                }
            }
        }
        private void OpenGameForm(GameState state, int myId)
        {
            Form1 gameForm = new Form1(state, myId);
            gameForm.Show();
            this.Hide();
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
    }
}



