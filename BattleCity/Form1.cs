using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleCity
{
    public partial class Form1 : Form
    {

        Random randNum = new Random();

        KeyboardController keyboardController = new KeyboardController();
        WorldMap map = new WorldMap();

        EntityController entityController;


        private bool updating = false;
        private bool playing = false;

        public Form1()
        {
            InitializeComponent();
            RestartGame();
            entityController = new EntityController(map, keyboardController, this.Controls);
            entityController.initialize();
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            updating = true;
            entityController.update();
            updating = false;
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (updating == false)
                keyboardController.pressKey(e.KeyCode);

            if (e.KeyCode == Keys.U && playing == false)
            {
                entityController.startGame();
                playing = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (updating == false)
                keyboardController.releaseKey(e.KeyCode);
        }


        private void RestartGame()
        {
        }

    }
}
