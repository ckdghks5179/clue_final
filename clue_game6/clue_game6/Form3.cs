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
    public partial class Form3 : Form
    {
        //gina
        private NetworkStream stream;
        private bool isNetworkMode = false;
        //gina
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
        ////gina
        public Form3(GameState G, Player p, int i, int id_num, bool isNetMode, NetworkStream netStream)
    : this(G, p, i, id_num) // 调用原来的构造函数，保留单机逻辑
        {
            isNetworkMode = isNetMode;
            stream = netStream;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(choose == 1)
            {
                foreach (var form in PlayerChoose.AllPlayerForms)
                {
                    form.textBox1.Text += "player" + (id+1).ToString() + ": " + manBox.Text + "가 " + roomBox.Text + "에서 " + weaponBox.Text + "로 죽였다." + "\r\n";//수정
                }
                //gina
                string message = $"player{id + 1}: {manBox.Text}가 {roomBox.Text}에서 {weaponBox.Text}로 죽였다.";

                // 仅联机模式广播
                if (Application.OpenForms["Form1"] is Form1 gameForm && gameForm.IsNetworkMode())
                {
                    gameForm.SendSuggestion(message);
                }
                this.Close();
            }
            else if(choose == 2)
            {
                if (!(gameState.answer[0].name == manBox.Text && gameState.answer[1].name == weaponBox.Text && gameState.answer[2].name == roomBox.Text))
                    player.isAlive = false;
                // gina
                // 联机模式下广播该猜想
                if (Application.OpenForms["Form1"] is Form1 gameForm && gameForm.IsNetworkMode())
                {
                    string msg = $"FINAL_SUGGEST|{id}|{manBox.Text}|{weaponBox.Text}|{roomBox.Text}";
                    gameForm.SendMessage(msg);
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
        }
    }

