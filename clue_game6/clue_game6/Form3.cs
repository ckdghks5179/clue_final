using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clue_game6
{
    public partial class Form3 : Form
    {
        GameState gameState;
        Player player;
        int choose;
        int id;
        public Form3(GameState G,Player p, int i, int id_num)
        {
            InitializeComponent();
            player = p;
            choose = i;
            gameState = G;
            id = id_num;
            

        }  

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

        private void button1_Click(object sender, EventArgs e)
        {
            if (choose == 1)
            {
                // 1. 추리 로그 전체 플레이어에 출력
                string guessLog = $"Player {player.id + 1}: {manBox.Text}가 {roomBox.Text}에서 {weaponBox.Text}로 죽였다.";
                gameState.AddLog(guessLog);

                player.hasSuggested = true;

                // 2. 추리 대상 카드 목록 만들기
                List<string> guessedNames = new List<string> { manBox.Text, weaponBox.Text, roomBox.Text };

                // 3. 공개 카드에 있는 경우 자동 반박
                var openMatch = gameState.openCard.Where(c => c.name == manBox.Text || c.name == weaponBox.Text || c.name == roomBox.Text).ToList();
                if (openMatch.Count > 0)
                {
                    string matched = string.Join(", ", openMatch.Select(c => c.name));
                    gameState.AddLog($"→ 공개된 카드 '{matched}'로 반박됨.");
                    MessageBox.Show($"공개 카드({matched})로 반박되었습니다.");
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

                    var match = other.hands.Where(card => guessedNames.Contains(card.name)).ToList();
                    if (match.Count > 0)
                    {
                        // 5. 반박할 카드가 여러 장이면 선택 폼 띄우기
                        SelectCardForm selectForm = new SelectCardForm(match, $"Player {other.id + 1}", $"Player {player.id + 1}");
                        if (selectForm.ShowDialog() == DialogResult.OK)
                        {
                            var revealed = selectForm.SelectedCard;
                            gameState.AddLog($"→ Player {other.id + 1}가 반박했다.");
                            MessageBox.Show($"Player {other.id + 1}가 '{revealed.name}' 카드를 보여주었습니다.");
                        }

                        this.Close();
                        return;
                    }

                    current = (current + 1) % totalPlayers;
                }

                // 6. 아무도 반박하지 못한 경우
                gameState.AddLog("→ 아무도 반박하지 못했다.");
                MessageBox.Show("아무도 반박하지 못했습니다.");
                this.Close();
            }

            else if (choose == 2)
            {
                string finalLog = $"Player {player.id + 1}의 최종 추리: {manBox.Text}, {weaponBox.Text}, {roomBox.Text}";
                gameState.AddLog(finalLog);

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

                this.Close();
            }



            else if(choose ==3)
            {
                    for(int i =0;i<player.hands.Count();i++)
                    {
                        if (manBox.Text == player.hands[i].name)
                        {
                        PlayerChoose.AllPlayerForms[gameState.CurrentTurn].textBox1.Text += "player" + id.ToString() + ": " + manBox.Text +"\r\n";
                        this.Close();
                        }
                        else if (weaponBox.Text == player.hands[i].name)
                        {
                        PlayerChoose.AllPlayerForms[gameState.CurrentTurn].textBox1.Text += "player" + id.ToString() + ": " + weaponBox.Text + "\r\n";
                        this.Close();
                        }
                        else if(roomBox.Text == player.hands[i].name)
                        {
                        PlayerChoose.AllPlayerForms[gameState.CurrentTurn].textBox1.Text += "player" + id.ToString() + ": " + roomBox.Text + "\r\n";
                        this.Close();
                        }
                    }
                   
                }
            }

        private void Form3_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
        }
    }
    }

