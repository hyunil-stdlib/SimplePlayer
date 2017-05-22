using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace player
{
    public partial class Form_Jukebox : Form
    {
        string curFileFullPath;
        string curMediaFileName;

        List<FileInfo> curMusicList = new List<FileInfo>();

        int playCount = 1;

        public Form_Jukebox()
        {
            InitializeComponent();

            if(curMusicList.Count > 0)
                PlayMusic(false);

            label2.Text = playCount.ToString();

            timer1.Enabled = true;
        }

        public void SetMusicList(List<FileInfo> musicList)
        {
            curMusicList.Clear();

            foreach (FileInfo item in musicList)
            {
                curMusicList.Add(item);
            }

            PlayMusic(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (playCount > 0)
                PlayMusic(false);
        }

        private void PlayMusic(bool loop)
        {
            MediaControl.Close();

            if (curMusicList.Count > 0 && playCount > 0)
            {
                int rand = new Random().Next(curMusicList.Count);

                curFileFullPath = curMusicList[rand].FullName;
                curMediaFileName = curMusicList[rand].Name;

                MediaControl.Open(curFileFullPath);
                MediaControl.Play(loop);
                playCount--;

                button1.Text = curMediaFileName;
                label2.Text = playCount.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            playCount++;
            label2.Text = playCount.ToString();

            if (MediaControl.Status() != "playing")
            {
                PlayMusic(false);
            }
        }

        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                playCount++;
                label2.Text = playCount.ToString();

                if (MediaControl.Status() != "playing")
                {
                    PlayMusic(false);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MediaControl.Status() != "playing")
            {
                PlayMusic(false);
            }
            else
            {
                return ;
            }
        }
    }
}
