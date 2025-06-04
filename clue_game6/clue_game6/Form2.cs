using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clue_game6
{
    public partial class Form2 : Form
    {
        GameState gameState;
        Player player;
        public Form2(Player p, GameState G)
        {
            InitializeComponent();
            ApplyDetectiveStyle();
            player = p;
            gameState = G;
            openForm2(p, G);
        }
        public void openForm2(Player p, GameState G)
        {
            for (int i = 0; i < player.manBox.Length; i++)
            {
                if (player.manBox[i] == true)
                    manListBox.SetItemChecked(i, true);
                else
                    manListBox.SetItemChecked(i, false);
            }

            for (int i = 0; i < player.weaponBox.Length; i++)
            {
                if (player.weaponBox[i] == true)
                    weaponListBox.SetItemChecked(i, true);
                else
                    weaponListBox.SetItemChecked(i, false);
            }

            for (int i = 0; i < 5; i++)
            {
                if (player.roomBox[i] == true)
                    roomListBox1.SetItemChecked(i, true);
                else
                    roomListBox1.SetItemChecked(i, false);
            }

            for (int i = 0; i < 4; i++)
            {
                if (player.roomBox[i + 5] == true)
                    roomListBox2.SetItemChecked(i, true);
                else
                    roomListBox2.SetItemChecked(i, false);
            }
        }
        public void closeForm2()
        {
            for (int i = 0; i < manListBox.Items.Count; i++)
            {
                if (manListBox.GetItemChecked(i))
                    player.manBox[i] = true;
                else
                    player.manBox[i] = false;
            }
            for (int i = 0; i < weaponListBox.Items.Count; i++)
            {
                if (weaponListBox.GetItemChecked(i))
                    player.weaponBox[i] = true;
                else
                    player.weaponBox[i] = false;
            }
            for (int i = 0; i < roomListBox1.Items.Count; i++)
            {
                if (roomListBox1.GetItemChecked(i))
                    player.roomBox[i] = true;
                else
                    player.roomBox[i] = false;
            }
            for (int i = 0; i < roomListBox2.Items.Count; i++)
            {
                if (roomListBox2.GetItemChecked(i))
                    player.roomBox[i + 5] = true;
                else
                    player.roomBox[i + 5] = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            closeForm2();
            Close();
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