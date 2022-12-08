using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using FMusicPlay2.Properties;
using AxWMPLib;

namespace FMusicPlay2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox3.Hide();
            barwidth = panel4.Width;
            panel4.Width = 0;
            pictureBox7.Hide();
            pictureBox8.Hide();
            pictureBox10.Hide();
            listBoxSongsRemove.Hide();
            pictureBox13.Hide();
            axWindowsMediaPlayer1.enableContextMenu = false;
            axWindowsMediaPlayer1.Ctlenabled = false;
        }

        List<string> paths = new List<string>();
        List<string> files = new List<string>();
        string IsTheSame = "";
        int barwidth;
        bool repeat = false;
        bool shuffle = false;
        Random rnshuffle = new Random();
        bool remove = false;

       
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //close app
            this.Close();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            IsTheSame = "nothing";
            //code to select songs
            OpenFileDialog ofd = new OpenFileDialog();

            //Code to select multiple files
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    listBoxSongs.Items.Remove(files[i]); //display songs in in listbox
                    listBoxSongsRemove.Items.Remove(files[i]);
                }

                files.AddRange(ofd.SafeFileNames); //save the names of the track in files array
                paths.AddRange(ofd.FileNames); //save the path of the track in path array
                //Display the music titles in listbox
                for (int i = 0; i < files.Count; i++)
                {
                    listBoxSongs.Items.Add(files[i]); //display songs in in listbox
                    listBoxSongsRemove.Items.Add(files[i]);
                }
            }
        }

        public void listBoxSongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (IsTheSame != paths[listBoxSongs.SelectedIndex])
                {
                    //code to play music
                    axWindowsMediaPlayer1.URL = paths[listBoxSongs.SelectedIndex];
                    IsTheSame = paths[listBoxSongs.SelectedIndex];
                }
            }
            catch (Exception)
            {
            }
            //hides play button, shows pause button
            pictureBox2.Hide();
            pictureBox3.Show();
        }

        //PLAY BUTTON
        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            //plays the music and hides this button
            axWindowsMediaPlayer1.Ctlcontrols.play();
            pictureBox2.Hide();
            pictureBox3.Show();
        }

        //PAUSE BUTTON
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //pauses the music and hides this button
            axWindowsMediaPlayer1.Ctlcontrols.pause();
            pictureBox3.Hide();
            pictureBox2.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxSongs.SelectedIndex == files.Count - 1)
                {
                    listBoxSongs.SelectedIndex = 0;
                }
                else
                {
                    listBoxSongs.SelectedIndex = listBoxSongs.SelectedIndex + 1;
                }
            }
            catch (Exception)
            {
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxSongs.SelectedIndex == 0)
                {
                    listBoxSongs.SelectedIndex = files.Count - 1;
                }
                else
                {
                    listBoxSongs.SelectedIndex = listBoxSongs.SelectedIndex - 1;
                }
            }
            catch (Exception)
            {
            }
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                timer1.Start();
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                timer1.Stop();
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                panel4.Width = 0;
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
            label3.Text = axWindowsMediaPlayer1.Ctlcontrols.currentItem.durationString.ToString();
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                //math that calculates position of progress bar
                try
                {
                    panel4.Width = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition * barwidth / Convert.ToInt32(System.Math.Floor(axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration));
                }
                catch (Exception)
                {
                }
            }
            if (barwidth == panel4.Width)
            {
                try
                {
                    if (repeat == true)
                    {
                        axWindowsMediaPlayer1.URL = paths[listBoxSongs.SelectedIndex];
                    }
                    else if (shuffle == true)
                    {
                        int indexCheck = listBoxSongs.SelectedIndex;
                        listBoxSongs.SelectedIndex = rnshuffle.Next(files.Count - 1);
                        if (indexCheck == listBoxSongs.SelectedIndex)
                        {
                            if (listBoxSongs.SelectedIndex == files.Count - 1)
                            {
                                listBoxSongs.SelectedIndex = 0;
                            }
                            else
                            {
                                listBoxSongs.SelectedIndex = listBoxSongs.SelectedIndex + 1;
                            }
                        }
                    }
                    else if (listBoxSongs.SelectedIndex == files.Count - 1)
                    {
                        listBoxSongs.SelectedIndex = 0;
                    }
                    else
                    {
                        listBoxSongs.SelectedIndex = listBoxSongs.SelectedIndex + 1;
                    }
                    timer1.Stop();
                    axWindowsMediaPlayer1.URL = paths[listBoxSongs.SelectedIndex];
                }
                catch (Exception)
                {
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        //DRAG WINDOW
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        //click on progress bar 1
        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                decimal decMClick = ((decimal)e.X / (decimal)barwidth) * (decimal)axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = (double)decMClick;
                //math that calculates position of progress bar
                try
                {
                    panel4.Width = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition * barwidth / Convert.ToInt32(System.Math.Floor(axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration));
                }
                catch (Exception)
                {
                }
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                decimal decMClick = ((decimal)e.X / (decimal)barwidth) * (decimal)axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = (double)decMClick;
                //math that calculates position of progress bar
                try
                {
                    panel4.Width = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition * barwidth / Convert.ToInt32(System.Math.Floor(axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration));
                }
                catch (Exception)
                {
                }
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                decimal decMClick = ((decimal)e.X / (decimal)barwidth) * (decimal)axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = (double)decMClick;
                //math that calculates position of progress bar
                try
                {
                    panel4.Width = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition * barwidth / Convert.ToInt32(System.Math.Floor(axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration));
                }
                catch (Exception)
                {
                }
            }
        }

        //click on progress bar 2
        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                decimal decMClick = ((decimal)e.X / (decimal)barwidth) * (decimal)axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = (double)decMClick;
                //math that calculates position of progress bar
                try
                {
                    panel4.Width = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition * barwidth / Convert.ToInt32(System.Math.Floor(axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration));
                }
                catch (Exception)
                {
                }
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                decimal decMClick = ((decimal)e.X / (decimal)barwidth) * (decimal)axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = (double)decMClick;
                //math that calculates position of progress bar
                try
                {
                    panel4.Width = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition * barwidth / Convert.ToInt32(System.Math.Floor(axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration));
                }
                catch (Exception)
                {
                }
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                decimal decMClick = ((decimal)e.X / (decimal)barwidth) * (decimal)axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = (double)decMClick;
                //math that calculates position of progress bar
                try
                {
                    panel4.Width = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition * barwidth / Convert.ToInt32(System.Math.Floor(axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration));
                }
                catch (Exception)
                {
                }
            }
        }


        public void pictureBox6_Click(object sender, EventArgs e)
        {
            repeat = true;
            pictureBox6.Hide();
            pictureBox7.Show();
        }

        public void pictureBox7_Click(object sender, EventArgs e)
        {
            repeat = false;
            pictureBox6.Show();
            pictureBox7.Hide();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            shuffle = true;
            pictureBox9.Hide();
            pictureBox8.Show();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            shuffle = false;
            pictureBox8.Hide();
            pictureBox9.Show();
        }

        private void listBoxSongsRemove_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (remove == false)
            {
                remove = true;
                listBoxSongsRemove.Show();
                listBoxSongs.Hide();
                pictureBox10.Show();
            }
            else if(remove == true)    
            {
                remove = false;
                listBoxSongsRemove.Hide();
                listBoxSongs.Show();
                pictureBox10.Hide();
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            for (int i = listBoxSongsRemove.Items.Count - 1; i >= 0; i--)
            {
                // clb is the name of the CheckedListBox control
                if (listBoxSongsRemove.GetItemChecked(i))
                {
                    listBoxSongsRemove.Items.Remove(listBoxSongsRemove.Items[i]);
                    listBoxSongs.Items.Remove(listBoxSongs.Items[i]);
                }
            }
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel6_MouseDown(object sender, MouseEventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = e.X;
            panel5.Width = e.X;
            Settings.Default.VolumeSave = axWindowsMediaPlayer1.settings.volume;
            Settings.Default.Save();
        }

        private void panel5_MouseDown(object sender, MouseEventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = e.X;
            panel5.Width = e.X;
            Settings.Default.VolumeSave = axWindowsMediaPlayer1.settings.volume;
            Settings.Default.Save();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            pictureBox12.Show();
            pictureBox13.Hide();
            axWindowsMediaPlayer1.settings.mute = false;
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            pictureBox13.Show();
            pictureBox12.Hide();
            axWindowsMediaPlayer1.settings.mute = true;
        }

        //Load
        private void Form1_Load(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = Settings.Default.VolumeSave;
            panel5.Width = Settings.Default.VolumeSave;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}