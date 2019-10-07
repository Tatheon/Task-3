using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;


namespace simulation_game
{
    public partial class Form1 : Form
    {
        GameEngine game;
        Timer round = new Timer();
        

        public Form1()
        {
            InitializeComponent();
            
            //DrawMap();
            //ShowUnits();
            //initTimer();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            game = new GameEngine();//create the engine
            ResetWorld();//creates starting world
            initTimer();//creates timer
        }

        void initTimer()
        {
            round.Interval = 1000;
            round.Tick += TimerTickHandeler;
        }

        void TimerTickHandeler(object sender, EventArgs e)//every tick on the timer, this must happen
        {
            game.Run();//game logic will do their tasks
            game.world.UpdateWorld();//update map
            UpdateUI();//show the map

            if (game.gameOver)
            {
                round.Stop();
                lblMap.Text += "Winning team is" + game.WinningTeam;
                btnPause.Enabled = false;
                btnReset.Enabled = true;
            }

        }

        public void UpdateUI()
        {
            //display map
            lblMap.Text = "";
            for (int y = 0; y < game.world.map.GetLength(1); y++)
            {
                for (int x = 0; x < game.world.map.GetLength(0); x++)
                {
                    lblMap.Text += game.world.map[x, y];
                }
                lblMap.Text += "\n";
            }
            lblPlayerStats.Text = "Rounds: "+game.Rounds + "\n";

            //display unit and building stats

            foreach (Building building in game.world.buildings)
            {
                lblPlayerStats.Text += building.ToString() + "\n";
            }

            foreach (Unit unit in game.world.players)
            {
                lblPlayerStats.Text += unit.ToString() + "\n";
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
             
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            round.Start();
            btnLoad.Enabled = false;
            btnSave.Enabled = false;
            btnReset.Enabled = false;
        }

        private void btnPause_Click_1(object sender, EventArgs e)
        {
            round.Stop();
            btnSave.Enabled = true;
            btnReset.Enabled = true;
            btnLoad.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            game.world.SaveWorld(game.Rounds);
            lblMap.Text += "Game saved <<<>>>";
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            game.Rounds = game.world.LoadWorld();
            UpdateUI();
            lblMap.Text += "Map loaded <<<>>>";
        }

        public string[] ShowMyDialogBox()//method that uses an InputBox to collect the users choice on map size
        {
            object InputMenu;
            string defaultResponse = "20,20";
            InputMenu = Interaction.InputBox("Please enter a map size, WIDTH comma HEIGHT","Map size editor", defaultResponse);

            if ((string)InputMenu == "")
            {
                return defaultResponse.Split(',');
            }
            else
            {
                return InputMenu.ToString().Split(',');
            }  
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            btnLoad.Enabled = true;
            btnSave.Enabled = true;
            btnPause.Enabled = true;
            btnStart.Enabled = true;
            ResetWorld();
        }

        public void ResetWorld()
        {
            game.Rounds = 1;
            game.world.setWorld(8, 4, ShowMyDialogBox());
            UpdateUI();
        }
    }
}
