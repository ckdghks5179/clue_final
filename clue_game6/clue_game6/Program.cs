using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clue_game6
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            PlayerChoose chooser = new PlayerChoose();
            chooser.ShowDialog();

            if (PlayerChoose.AllPlayerForms.Count > 0)
            {
                // 첫 번째 플레이어의 Form1을 메인폼으로 실행
                Application.Run(PlayerChoose.AllPlayerForms[0]);
            }
        }
    }
}
