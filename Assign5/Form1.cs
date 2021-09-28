using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assign5
{
    public partial class Form1 : Form
    {
        bool goleft;
        bool goright;
        bool isGameOver;
        bool isGamePause;
        bool isGameStart;
        bool rewardCheck = false;

        int score;
        int ballx;
        int bally;
        int goaly;
        int playerSpeed;
        int level;
        int speed = 8;
        int brickRemoved=0;

        //SplashScreen mainWindow =new SplashScreen();

        PictureBox[] bricksArray;
        List<PictureBox> reward = new List<PictureBox>();
        

        Random rnd = new Random();
        

        public Form1()
        {
            SplashScreen splashScreen = new SplashScreen();
            
            InitializeComponent();
            PlaceBricks();
            isGameStart = true;
            level = 1;
        }

        public void setupGame()
        {
            isGameOver = false;
            score = 0;
            ballx = 5;
            bally = 5;
            playerSpeed = 12;

            ball.Left = 389;
            ball.Top = 341;

            player.Left = 348;

            lbl_score.Text = "Score: " + score;

            timer.Start();

            int count = 0;
            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && (String)x.Tag == "bricks")
                {
                    if(count==0)
                    {
                        x.BackColor = Color.Red;
                        count += 1;
                    }
                    else if(count==1)
                    {
                        x.BackColor = Color.Green;
                        count += 1;
                    }
                    else if(count==2)
                    {
                        x.BackColor = Color.Blue;
                        count = 0;
                    }
                }
            }

        }

        private void GameOver(string message)
        {
            isGameOver = true;
            timer.Stop();
            lbl_message.Text = message;
        }

        private void PlaceBricks()
        {
            int a = 0;
            int top = 40;
            int left = 80;
            bricksArray = new PictureBox[24];
            for(int i=0; i<bricksArray.Length; i++)
            {
                bricksArray[i] = new PictureBox();
                bricksArray[i].Height = 16;
                bricksArray[i].Width = 80;
                bricksArray[i].Tag = "bricks";
                bricksArray[i].BackColor = Color.White;


                if (a == 6)
                {
                    top = top + 40;
                    left = 80;
                    a = 0;
                }
                if (a < 6)
                {
                    a++;
                    bricksArray[i].Left = left;
                    bricksArray[i].Top = top;
                    this.Controls.Add(bricksArray[i]);
                    left = left + 100;

                }
            }
            setupGame();
        }
        private void removeBricks()
        {
            foreach(PictureBox blocks in bricksArray)
            {
                
                this.Controls.Remove(blocks);
                
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lbl_score.Text = "Score: " + score;
            if (goleft == true && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }
            if (goright == true && player.Left < 698)
            {
                player.Left += playerSpeed;
            }

            ball.Left += ballx;
            ball.Top += bally;

            if (ball.Left < 0 || ball.Left > 777)
            {
                ballx = -ballx;
            }
            if (ball.Top < 0)
            {
                bally = -bally;
            }
            

            if (ball.Bounds.IntersectsWith(player.Bounds))
            {
                ball.Top = 400;
                bally = rnd.Next(5, 12) * -1;
                if(ballx < 0)
                {
                    ballx = rnd.Next(5, 12) * -1;
                }
                else
                {
                    ballx = rnd.Next(5, 12);
                }
            }

            if (ball.Bounds.IntersectsWith(panel1.Bounds))
            {
                bally = -bally;  
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (String)x.Tag == "apple")
                {
                    if(x.Bounds.IntersectsWith(player.Bounds))
                    {
                        Controls.Remove(x);
                        score += 1;
                        rewardCheck = false;
                    }
                }
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (String)x.Tag == "bricks")
                {
                    if (ball.Bounds.IntersectsWith(x.Bounds) && x.BackColor == Color.Red)
                    {
                        rewardCheck = true;
                        x.BackColor = Color.Blue;
                        bally = -bally;
                    }
                    else if (ball.Bounds.IntersectsWith(x.Bounds) && x.BackColor == Color.Blue)
                    {
                        x.BackColor = Color.Green;
                        bally = -bally;
                    }
                    else if (ball.Bounds.IntersectsWith(x.Bounds) && x.BackColor == Color.Green)
                    {
                        Controls.Remove(x);
                        brickRemoved += 1;
                        if(rewardCheck == true)
                        {
                            PictureBox apple = new PictureBox();
                            apple.BackgroundImage = Properties.Resources.apple;
                            apple.BackgroundImageLayout = ImageLayout.Zoom;
                            apple.Height = 25;
                            apple.Width = 20;
                            apple.Location = x.Location;
                            apple.Tag = "apple";
                            Controls.Add(apple);
                            reward.Add(apple); 
                        }
                        score += 1;
                        bally = -bally;
                        break;
                    }
                        
                        
                }
                
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (String)x.Tag == "apple")
                {
                     x.Top += speed;
                }
            }
            if (brickRemoved == 24)
            {
                level = 2;
                lbl_level.Text = "Level: " + level;
                PlaceBricks();
                Level2SetUp();
            }

            if (brickRemoved == 49)
            {
                GameOver("You won!!! Press Enter to Play Again");
            }
            if(ball.Top > 494)
            {
                if(level==2)
                {
                    lbl_message.Text = "Press Space to Pause/Play";
                }
                else
                {
                    GameOver("You Lose!!!  Press Enter to Play Again");
                    level = 1;
                }
                
            }

        }

        private void Level2SetUp()
        {
            isGameOver = false;
            score = 25;
            ballx = 5;
            bally = 5;
            playerSpeed = 7;
            timer.Interval = 30;
            ball.Left = 389;
            ball.Top = 341;

            player.Left = 348;

            lbl_score.Text = "Score: " + score;

            timer.Start();
            int count = 0;
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (String)x.Tag == "bricks")
                {
                    if (x is PictureBox && (String)x.Tag == "bricks")
                    {
                        if (count == 0)
                        {
                            x.BackColor = Color.Red;
                            count += 1;
                        }
                        else if (count == 1)
                        {
                            x.BackColor = Color.Green;
                            count += 1;
                        }
                        else if (count == 2)
                        {
                            x.BackColor = Color.Blue;
                            count = 0;
                        }
                        
                    }
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
            if(e.KeyCode == Keys.Enter && isGameOver == true)
            {
                removeBricks();
                PlaceBricks();
            }
            if(e.KeyCode == Keys.Space)
            {
                if(isGameStart==true)
                {
                    timer.Stop();
                    isGamePause = true;
                    isGameStart = false;
                }
                else if(isGamePause==true)
                {
                    timer.Start();
                    isGamePause = false;
                    isGameStart = true;

                }
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                goleft = true;
            }
            
            if(e.KeyCode == Keys.Right)
            {
                goright = true;
            }


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
