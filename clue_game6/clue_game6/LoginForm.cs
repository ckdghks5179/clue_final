using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clue_game6
{
    public partial class LoginForm: Form
    {
        private Label lblName;
        private TextBox txtName;
        private Label lblIP;
        private TextBox txtIP;
        private Label lblMaxPlayers;
        private NumericUpDown numMaxPlayers;
        private Button btnConnect;
        public WaitingForm WaitingFormInstance { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
        }
        private int playerCount;

        public LoginForm(int playerCount)
        {

            InitializeComponent();
            this.playerCount = playerCount;
            numMaxPlayers.Value = playerCount;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        txtIP.Text = ip.ToString();
                        break;
                    }
                }
            }
            catch
            {
                txtIP.Text = "127.0.0.1";
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // 获取用户输入
            string nickname = txtName.Text.Trim();
            string ip = txtIP.Text.Trim();
            int maxPlayers = (int)numMaxPlayers.Value;

            // 输入验证
            if (nickname == "" || ip == "")
            {
                MessageBox.Show("닉네임과 IP를 입력하세요.");
                return;
            }

            try
            {
                // 尝试连接服务器
                TcpClient client = new TcpClient();
                client.Connect(ip, 5000);

                // 发送昵称到服务器
                NetworkStream stream = client.GetStream();
                string msgName = $"NICKNAME|{nickname}\n";
                // 发送最大玩家数到服务器
                string msgMaxPlayers = $"MAX_PLAYERS|{maxPlayers}\n";
                // 发送游戏开始消息到服务器
                string msgStart = "GAME_START\n";

                byte[] data = Encoding.UTF8.GetBytes(msgName);
                stream.Write(data, 0, data.Length);

                data = Encoding.UTF8.GetBytes(msgMaxPlayers);
                stream.Write(data, 0, data.Length);

                data = Encoding.UTF8.GetBytes(msgStart);
                stream.Write(data, 0, data.Length);
                //20250520
                WaitingForm wait = new WaitingForm(client, nickname, maxPlayers);
                this.WaitingFormInstance = wait;

                // 不要直接 Show()，让 Main() 统一管理
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("서버 연결 실패: " + ex.Message);
            }
        }
    }
}
