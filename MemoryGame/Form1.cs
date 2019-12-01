using System;
using System.Windows.Forms;
using System.Threading;

namespace MemoryGame
{
    public partial class Form1 : Form
    {
        private PictureBox LastDisplayedPicture;
        private bool IsPictureDisplayed = false;
        private int RemainingPictures;
        private int RemainingHints;
        private int TimeLeft;

        private readonly string RemainingTime = "Remaining time: ";
        private readonly string DoYouWantToTryAgain = "Do yo want to try again?";
        private readonly string TheTimeIsOver = "The time is over!";
        private readonly string GameOver = "Game over!";
        private readonly string Congratulations = "Congratulations!";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            ShowImage((sender as PictureBox));

            PictureBox currentSelectedPicture = (sender as PictureBox);

            if (!this.IsPictureDisplayed)
            {
                this.LastDisplayedPicture = currentSelectedPicture;
                this.IsPictureDisplayed = true;
            }
            else if (this.LastDisplayedPicture != currentSelectedPicture)
            {
                HidePictureIfMatch(this.LastDisplayedPicture, currentSelectedPicture);
                this.IsPictureDisplayed = false;
            }
        }

        private void Hint_Click(object sender, EventArgs e)
        {
            ShowAvailableImages();
            Application.DoEvents();
            Thread.Sleep(2000);

            HidePictures();

            if (--this.RemainingHints == 0)
                hintButton.Enabled = false;

            hintButton.Text = "Hint (" + RemainingHints + ")";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (--this.TimeLeft > 0)
                this.time.Text = this.RemainingTime + this.TimeLeft;
            else
            {
                ShowAvailableImages();
                SetEnabledPropertyForAllPictureBox(false);

                this.time.Text = this.TheTimeIsOver;
                DialogResult result = ShowMessageBox(this.GameOver, $"{this.TheTimeIsOver}\n\n{this.DoYouWantToTryAgain}", MessageBoxButtons.YesNo);

                if(result == DialogResult.Yes)
                    StartNewGame();
            }
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void ReturnMenu_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
            this.Close();
        }

        private void HidePictures()
        {
            foreach(Control x in this.Controls)
                if(x is PictureBox)
                    (x as PictureBox).Image = Properties.Resources._0;
        }

        private void ShowAvailableImages()
        {
            foreach (Control x in this.Controls)
                if (x is PictureBox)
                    ShowImage(x as PictureBox);
        }

        private void ShowImage(PictureBox box)
        {
            switch(box.Tag)
            {
                case "1":
                    box.Image = Properties.Resources._1;
                    break;
                case "2":
                    box.Image = Properties.Resources._2;
                    break;
                case "3":
                    box.Image = Properties.Resources._3;
                    break;
                case "4":
                    box.Image = Properties.Resources._4;
                    break;
                case "5":
                    box.Image = Properties.Resources._5;
                    break;
                case "6":
                    box.Image = Properties.Resources._6;
                    break;
                case "7":
                    box.Image = Properties.Resources._7;
                    break;
                case "8":
                    box.Image = Properties.Resources._8;
                    break;
                default:
                    box.Image = Properties.Resources._0;
                    break;
            }
        }
 
        private void ShufflePictures()
        {
            Random random = new Random();
            int[] tags = new int[16];
            int index = 0;

            while (index < 16)
            {
                int valueSelectedRandom = random.Next(1, 17);
                if(Array.IndexOf(tags, valueSelectedRandom) == -1)
                {
                    tags[index] = valueSelectedRandom;
                    index++;
                }
            }

            for(index = 0; index < 16; index++)
                if (tags[index] > 8)
                    tags[index] -= 8;

            index = 0;
            foreach(Control x in this.Controls)
                if(x is PictureBox)
                {
                    (x as PictureBox).Tag = tags[index].ToString();
                    index++;
                }
        }

        private void HidePictureIfMatch(PictureBox previous, PictureBox current)
        {
            if(string.Equals(previous.Tag.ToString(), current.Tag.ToString()))
            {
                Application.DoEvents();
                Thread.Sleep(500);

                previous.Visible = false;
                current.Visible = false;

                if(--RemainingPictures > 0)
                    this.remaining.Text = "Image left: " + RemainingPictures;
                else
                {
                    this.remaining.Text = this.Congratulations;
                    DialogResult playerAnswer = ShowMessageBox(this.GameOver, $"{this.Congratulations}\n{this.DoYouWantToTryAgain}", MessageBoxButtons.YesNo);

                    if(playerAnswer == DialogResult.Yes)
                        StartNewGame();
                }
            }
            else
            {
                Application.DoEvents();
                Thread.Sleep(500);
                previous.Image = Properties.Resources._0;
                current.Image = Properties.Resources._0;
            }
        }

        private void SetPictureBoxVisible()
        {
            foreach (Control x in this.Controls)
                if (x is PictureBox)
                    (x as PictureBox).Visible = true;
        }

        private void SetEnabledPropertyForAllPictureBox(bool value)
        {
            foreach (Control x in this.Controls)
                if (x is PictureBox)
                    (x as PictureBox).Enabled = value;
        }

        private DialogResult ShowMessageBox(string title, string message, MessageBoxButtons messageBoxButtons)
        {
            this.timer.Enabled = false;
            this.hintButton.Enabled = false;
            return MessageBox.Show(message, title, messageBoxButtons);
        }

        private void StartNewGame()
        {
            this.RemainingPictures = 8;
            this.RemainingHints = 3;
            this.TimeLeft = 60;
            this.IsPictureDisplayed = false;

            this.hintButton.Enabled = true;
            this.hintButton.Text = "Hint (" + this.RemainingHints + ")";
            this.remaining.Text = "Remaining images: " + this.RemainingPictures;

            this.time.Text = this.RemainingTime + this.TimeLeft;
            this.timer.Enabled = true;

            ShufflePictures();
            SetPictureBoxVisible();
            HidePictures();
            SetEnabledPropertyForAllPictureBox(true);
        }
    }
}
