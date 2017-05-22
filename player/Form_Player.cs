using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace player
{
    public partial class Form_Player : Form
    {
        string curFileFullPath;
        string curMediaFileName;

        List<FileInfo> curMusicList = new List<FileInfo>();

        int curIndex = -1;
        bool isTrackBarScroling = false;

        public Form_Player()
        {
            InitializeComponent();
            timer1.Enabled = true;
        }

        public void SetMusicList(List<FileInfo> musicList)
        {
            curMusicList.Clear();

            foreach (FileInfo item in musicList)
            {
                curMusicList.Add(item);
            }

            curIndex = 0;
            curMediaFileName = curMusicList[curIndex].Name;
            curFileFullPath = curMusicList[curIndex].FullName;

            PrintMusicName();
        }

        private void PrintMusicName()
        {
            string printFolderText = curMediaFileName;
            const int limitLength = 22;
            if (printFolderText.Length > limitLength)
            {
                printFolderText = printFolderText.Remove(limitLength, printFolderText.Length - limitLength);
                printFolderText = printFolderText + "...";
            }

            label1.Text = printFolderText;
        }

        private void PlayMusic(string musicFile, bool loop)
        {
            if (MediaControl.Status() == "playing" || MediaControl.Status() == "paused")
            {
                MediaControl.Close();
            }

            MediaControl.Open(musicFile);
            trackBar1.Maximum = MediaControl.Length();

            MediaControl.Play(loop);

            PrintMusicName();
        }

        private void PlayMusic(string musicFile, bool loop, int seekTime)
        {
            if (MediaControl.Status() == "playing" || MediaControl.Status() == "paused")
            {
                MediaControl.Close();
            }

            MediaControl.Open(musicFile);
            trackBar1.Maximum = MediaControl.Length();

            MediaControl.Play(loop, seekTime);

            PrintMusicName();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            curIndex -= 1;

            if (curIndex <= 0)
            {
                curIndex = 0;
            }

            curMediaFileName = curMusicList[curIndex].Name;

            PlayMusic(curFileFullPath, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MediaControl.Status() == "playing")
            {
                MediaControl.Pause();
            }
            else if (MediaControl.Status() == "paused")
            {
                MediaControl.Resume();
            }
            else if (MediaControl.Status() == "stopped")
            {
                MediaControl.Close();
                PlayMusic(curFileFullPath, true);
            }
            else
            {
                if (curIndex >= 0)
                {
                    PlayMusic(curFileFullPath, true);
                } 
                else
                {
                    MediaControl.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MediaControl.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            curIndex += 1;

            if (curIndex >= curMusicList.Count - 1)
            {
                curIndex = curMusicList.Count - 1;
            }

            curMediaFileName = curMusicList[curIndex].Name;

            PlayMusic(curFileFullPath, true);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MediaControl.Status() == "playing" || MediaControl.Status() == "paused")
            {
                //Console.WriteLine("length : {0} ~ {1}", trackBar1.Minimum, trackBar1.Maximum);

                if (isTrackBarScroling != true)
                    trackBar1.Value = MediaControl.Position();

            }
            else if(MediaControl.Status() == "stopped")
            {
                trackBar1.Value = 0;
            }
        }

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            isTrackBarScroling = true;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            isTrackBarScroling = false;
            PlayMusic(curFileFullPath, true, trackBar1.Value);
        }
    }
}
