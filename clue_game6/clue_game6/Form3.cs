using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clue_game6
{
    public partial class Form3 : Form
    {
        //gina- 네트워크 스트림 관련 변수
        private NetworkStream stream;
        private bool isNetworkMode = false;
        //gina
        GameState gameState;
        Player player;
        int choose;
        int id;

        public Form3(GameState G, Player p, int i, int id_num)
        {
            InitializeComponent();
            player = p;
            choose = i;
            gameState = G;
            id = id_num;

            this.Load += Form3_Load; // Load 이벤트 연결
        }

        ////gina네트워크 모드 생성자 (오버로드)
        public Form3(GameState G, Player p, int i, int id_num, bool isNetMode, NetworkStream netStream)
    : this(G, p, i, id_num)
        {
            isNetworkMode = isNetMode;
            stream = netStream;
        }

        // 콤보박스 선택 시 버튼 활성화
        public void SelectedIndexChanged(object sender, EventArgs e)
        {
            if (manBox.SelectedIndex != -1 && weaponBox.SelectedIndex != -1 && roomBox.SelectedIndex != -1)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }
        // [확인] 버튼 클릭 이벤트
        private void button1_Click(object sender, EventArgs e)
        {
            //if (choose == 1)
            //{
            //    // 1. 추리 로그 전체 플레이어에 출력
            //    string guessLog = $"Player {player.id + 1}: {manBox.Text}가 {roomBox.Text}에서 {weaponBox.Text}로 죽였다.";
            //    gameState.AddLog(guessLog);

            //    player.hasSuggested = true;

            //    // 2. 추리 대상 카드 목록 만들기
            //    List<string> guessedNames = new List<string> { manBox.Text, weaponBox.Text, roomBox.Text };

            //    // 3. 공개 카드에 있는 경우 자동 반박
            //    var openMatch = gameState.openCard.Where(c => c.name == manBox.Text || c.name == weaponBox.Text || c.name == roomBox.Text).ToList();
            //    if (openMatch.Count > 0)
            //    {
            //        string matched = string.Join(", ", openMatch.Select(c => c.name));
            //        gameState.AddLog($"→ 공개된 카드 '{matched}'로 반박됨.");
            //        MessageBox.Show($"공개 카드({matched})로 반박되었습니다.");
            //        this.Close();
            //        return;
            //    }

            //    // 4. 다른 플레이어가 반박할 수 있는지 확인
            //    int totalPlayers = gameState.TotalPlayers;
            //    int current = (player.id + 1) % totalPlayers;
            //    while (current != player.id)
            //    {
            //        var other = gameState.Players[current];
            //        if (!other.isAlive)
            //        {
            //            current = (current + 1) % totalPlayers;
            //            continue;
            //        }

            //        var match = other.hands.Where(card => guessedNames.Contains(card.name)).ToList();
            //        if (match.Count > 0)
            //        {
            //            // 5. 반박할 카드가 여러 장이면 선택 폼 띄우기
            //            SelectCardForm selectForm = new SelectCardForm(match, $"Player {other.id + 1}", $"Player {player.id + 1}");
            //            if (selectForm.ShowDialog() == DialogResult.OK)
            //            {
            //                var revealed = selectForm.SelectedCard;
            //                gameState.AddLog($"→ Player {other.id + 1}가 반박했다.");
            //                MessageBox.Show($"Player {other.id + 1}가 '{revealed.name}' 카드를 보여주었습니다.");
            //            }

            //            this.Close();
            //            return;
            //        }

            //        current = (current + 1) % totalPlayers;
            //    }

            //    //gina
            //    string message = $"player{id + 1}: {manBox.Text}가 {roomBox.Text}에서 {weaponBox.Text}로 죽였다.";

            //    // 仅联机模式广播
            //    if (Application.OpenForms["Form1"] is Form1 gameForm && gameForm.IsNetworkMode())
            //    {
            //        gameForm.SendSuggestion(message);
            //    }


            //    // 6. 아무도 반박하지 못한 경우
            //    gameState.AddLog("→ 아무도 반박하지 못했다.");
            //    MessageBox.Show("아무도 반박하지 못했습니다.");

            //    this.Close();
            //}
            // 일반 추리
            if (choose == 1)
            {
                // 1. 추리 로그 전체 플레이어에 출력
                string guessLog = $"Player {player.id + 1}: {manBox.Text}가 {roomBox.Text}에서 {weaponBox.Text}로 죽였다.";
                gameState.AddLog(guessLog);

                player.hasSuggested = true;

                // 2. 추리 대상 카드 목록 만들기
                List<string> guessedNames = new List<string> { manBox.Text, weaponBox.Text, roomBox.Text };

                // 3. 공개 카드에 있는 경우 자동 반박
                var openMatch = gameState.openCard
                    .Where(c => guessedNames.Contains(c.name))
                    .ToList();

                if (openMatch.Count > 0)
                {
                    string matched = string.Join(", ", openMatch.Select(c => c.name));
                    gameState.AddLog($"→ 공개된 카드 '{matched}'로 반박됨.");
                    MessageBox.Show($"공개 카드({matched})로 반박되었습니다.");

                    // 온라인 모드일 경우 서버에 추리 메시지 보내기
                    if (isNetworkMode && stream != null && stream.CanWrite)
                    {
                        string suggestMsg = $"SUGGEST|{guessLog}\n";
                        byte[] msgData = Encoding.UTF8.GetBytes(suggestMsg);
                        stream.Write(msgData, 0, msgData.Length);
                    }
                    BroadcastControlUpdate();
                    this.Close();
                    return;
                }

                // 4. 다른 플레이어가 반박할 수 있는지 확인
                int totalPlayers = gameState.TotalPlayers;
                int current = (player.id + 1) % totalPlayers;

                while (current != player.id)
                {
                    var other = gameState.Players[current];
                    if (!other.isAlive)
                    {
                        current = (current + 1) % totalPlayers;
                        continue;
                    }

                    var match = other.hands
                        .Where(card => guessedNames.Contains(card.name))
                        .ToList();

                    if (match.Count > 0)
                    {
                        // 5. 반박할 카드가 여러 장이면 선택 폼 띄우기
                        SelectCardForm selectForm = new SelectCardForm(match, $"Player {other.id + 1}", $"Player {player.id + 1}");
                        if (selectForm.ShowDialog() == DialogResult.OK)
                        {
                            var revealed = selectForm.SelectedCard;

                            gameState.AddLog($"→ Player {other.id + 1}가 반박했다.");
                            MessageBox.Show($"Player {other.id + 1}가 '{revealed.name}' 카드를 보여주었습니다.");

                            // 온라인 모드에서 정보의 동기식 표시
                            if (isNetworkMode && stream != null && stream.CanWrite)
                            {
                                string suggestMsg = $"SUGGEST|{guessLog}\n";
                                byte[] msgData = Encoding.UTF8.GetBytes(suggestMsg);
                                stream.Write(msgData, 0, msgData.Length);
                                string replyMsg = $"SUGGEST_REPLY|{other.id}|{player.id}|{revealed.type}|{revealed.name}\n";
                                string noticeMsg = $"SUGGEST_REPLY_NOTICE|{other.id}|{player.id}\n";

                                byte[] replyData = Encoding.UTF8.GetBytes(replyMsg);
                                byte[] noticeData = Encoding.UTF8.GetBytes(noticeMsg);

                                stream.Write(replyData, 0, replyData.Length);
                                stream.Write(noticeData, 0, noticeData.Length);
                            }
                        }
                        BroadcastControlUpdate();

                        this.Close();
                        return;
                    }

                    current = (current + 1) % totalPlayers;
                }

                // 6. 아무도 반박하지 못한 경우
                gameState.AddLog("→ 아무도 반박하지 못했다.");
                MessageBox.Show("아무도 반박하지 못했습니다.");

                //  마지막으로 일반 추측 방송을 추가합니다(모두가 볼 수 있음)
                if (isNetworkMode && stream != null && stream.CanWrite)
                {
                    string suggestMsg = $"SUGGEST|{guessLog}\n";
                    byte[] msgData = Encoding.UTF8.GetBytes(suggestMsg);
                    stream.Write(msgData, 0, msgData.Length);
                }
                BroadcastControlUpdate();
                this.Close();
            }
            // 최종 추리
            else if (choose == 2)
            {
                string finalLog = $"Player {player.id + 1}의 최종 추리: {manBox.Text}, {weaponBox.Text}, {roomBox.Text}";
                gameState.AddLog(finalLog);

                if (isNetworkMode && stream != null && stream.CanWrite)
                {
                    // 온라인 모드: 서버로 전송하여 우승자를 결정합니다.
                    string msg = $"FINAL_SUGGEST|{id}|{manBox.Text}|{weaponBox.Text}|{roomBox.Text}\n";
                    byte[] data = Encoding.UTF8.GetBytes(msg);
                    stream.Write(data, 0, data.Length);
                }
                else
                {
                    // 오프라인 모드: 직접 정답 확인
                    if (gameState.answer[0].name == manBox.Text &&
                        gameState.answer[1].name == weaponBox.Text &&
                        gameState.answer[2].name == roomBox.Text)
                    {
                        gameState.AddLog($"Player {player.id + 1}가 정답을 맞춰서 게임에서 승리했습니다!");
                        MessageBox.Show("정답입니다! 게임에서 승리했습니다!");
                        Application.Exit(); // 또는 승리 화면
                    }
                    else
                    {
                        gameState.AddLog($"Player {player.id + 1}의 최종 추리 실패 — 탈락");
                        MessageBox.Show("틀렸습니다. 당신은 탈락입니다.");
                        player.isAlive = false;
                    }
                }

                BroadcastControlUpdate();
                this.Close();
            }


            // 반박 카드 선택
            else if (choose == 3)
            {
                //for(int i =0;i<player.hands.Count();i++)
                //{
                //    if (manBox.Text == player.hands[i].name)
                //    {
                //    PlayerChoose.AllPlayerForms[gameState.CurrentTurn].textBox1.Text += "player" + id.ToString() + ": " + manBox.Text +"\r\n";
                //    this.Close();
                //    }
                //    else if (weaponBox.Text == player.hands[i].name)
                //    {
                //    PlayerChoose.AllPlayerForms[gameState.CurrentTurn].textBox1.Text += "player" + id.ToString() + ": " + weaponBox.Text + "\r\n";
                //    this.Close();
                //    }
                //    else if(roomBox.Text == player.hands[i].name)
                //    {
                //    PlayerChoose.AllPlayerForms[gameState.CurrentTurn].textBox1.Text += "player" + id.ToString() + ": " + roomBox.Text + "\r\n";
                //    this.Close();
                //    }
                //}

                for (int i = 0; i < player.hands.Count; i++)
                {
                    string selectedCardName = null;
                    string selectedCardType = null;

                    if (manBox.Text == player.hands[i].name)
                    {
                        selectedCardName = player.hands[i].name;
                        selectedCardType = player.hands[i].type;
                    }
                    else if (weaponBox.Text == player.hands[i].name)
                    {
                        selectedCardName = player.hands[i].name;
                        selectedCardType = player.hands[i].type;
                    }
                    else if (roomBox.Text == player.hands[i].name)
                    {
                        selectedCardName = player.hands[i].name;
                        selectedCardType = player.hands[i].type;
                    }

                    if (selectedCardName != null)
                    {

                        if (!isNetworkMode)
                        {
                            if (gameState.CurrentTurn < PlayerChoose.AllPlayerForms.Count)
                            {
                                PlayerChoose.AllPlayerForms[gameState.CurrentTurn].textBox1.Text +=
                                    $"player{id}: {selectedCardName}\r\n";
                            }
                        }
                        else
                        {
                            //  온라인 모드: 서버에 SUGGEST_REPLY 명령을 보냅니다(서버에서 재배포)
                            if (stream != null && stream.CanWrite)
                            {
                                string replyMsg = $"SUGGEST_REPLY|{id}|{gameState.CurrentTurn}|{selectedCardType}|{selectedCardName}\n";
                                byte[] data = Encoding.UTF8.GetBytes(replyMsg);
                                stream.Write(data, 0, data.Length);
                            }
                        }
                        BroadcastControlUpdate();
                        this.Close();
                        break;
                    }
                }


            }
        }

        // UI 갱신 신호 전송
        private void BroadcastControlUpdate()
        {
            if (isNetworkMode && stream != null && stream.CanWrite)
            {
                string updateMsg = $"UPDATE_UI|{player.id}\n";
                byte[] data = Encoding.UTF8.GetBytes(updateMsg);
                stream.Write(data, 0, data.Length);
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
        private void Form3_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;

            if (choose == 1)
            {
                roomBox.Enabled = false;

                // 현재 플레이어가 위치한 좌표에서 방 이름 자동 설정
                string roomName = gameState.GetRoomNameByPosition(player.x, player.y);
                roomBox.Items.Clear();
                roomBox.Items.Add(roomName);
                roomBox.SelectedIndex = 0;
            }
        }
    }
}


