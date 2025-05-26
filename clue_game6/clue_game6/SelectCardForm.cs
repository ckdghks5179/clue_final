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
    public partial class SelectCardForm : Form
    {
        public Card SelectedCard { get; private set; }

        public SelectCardForm(List<Card> matchingCards, string responderName, string guesserName)
        {
            InitializeComponent();
            this.Text = $"{responderName}가 {guesserName}에게";
            cmbCardList.DataSource = matchingCards;
            cmbCardList.DisplayMember = "name";
        }

        private void btnChoice_Click(object sender, EventArgs e)
        {
            if (cmbCardList.SelectedItem is Card selected)
            {
                SelectedCard = selected;
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
