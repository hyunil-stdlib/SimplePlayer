using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using System.Windows.Forms;

namespace player
{
    public partial class Form_Main : Form
    {
        public Form_Player fPlayerMode = new Form_Player();
        public Form_Jukebox fJukeboxMode = new Form_Jukebox();
        public Form_MusicList fMusicList = new Form_MusicList();

        bool isMode = false;        // true : 주크박스, false : 음악 플레이어

        public Form_Main()
        {
            InitializeComponent();

            ChangeMode();

            fMusicList.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            fMusicList.Owner = this;
            fMusicList.Show();

            trackBar1.Value = 1000;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeMode();
        }

        public void ChangeMode()
        {
            List<FileInfo> musicItemList = new List<FileInfo>();
            musicItemList = fMusicList.getMusicList();

            if (isMode)
            {
                if (fPlayerMode.Created)
                    fPlayerMode.Close();

                fJukeboxMode = new Form_Jukebox();
                fJukeboxMode.Location = new Point(this.Location.X, this.Location.Y + this.Height);
                fJukeboxMode.Owner = this;
                fJukeboxMode.Show();
                isMode = false;

                if (musicItemList.Count > 0)
                    fJukeboxMode.SetMusicList(musicItemList);

                this.button1.Text = "음악 플레이어로 전환";
            }
            else
            {
                if(fJukeboxMode.Created)
                    fJukeboxMode.Close();

                fPlayerMode = new Form_Player();
                fPlayerMode.Location = new Point(this.Location.X, this.Location.Y + this.Height);
                fPlayerMode.Owner = this;
                fPlayerMode.Show();
                isMode = true;

                if (musicItemList.Count > 0)
                    fPlayerMode.SetMusicList(musicItemList);

                this.button1.Text = "주크박스로 전환";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                string printFolderText = fd.SelectedPath;
                const int limitLength = 15;
                if (printFolderText.Length > limitLength)
                {
                    printFolderText = printFolderText.Remove(0, printFolderText.Length - limitLength);
                    printFolderText = "..." + printFolderText;

                }
                label1.Text = printFolderText;

                DirectoryInfo di = new DirectoryInfo(fd.SelectedPath);
                List<FileInfo> musicItemList = new List<FileInfo>();
                FindMP3Files(di, musicItemList);

                if (fPlayerMode.Created)
                    fPlayerMode.SetMusicList(musicItemList);

                if (fJukeboxMode.Created)
                    fJukeboxMode.SetMusicList(musicItemList);

                fMusicList.setMusicList(musicItemList);
            }
        }

        private void FindMP3Files(DirectoryInfo dirInfo, List<FileInfo> list)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;

            try
            {
                files = dirInfo.GetFiles("*.mp3");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (FileInfo fi in files)
                {
                    list.Add(fi);
                }

                subDirs = dirInfo.GetDirectories();
                foreach (DirectoryInfo di in subDirs)
                {
                    FindMP3Files(di, list);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fPlayerMode.Close();
            fJukeboxMode.Close();
            this.Close();
        }

        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point curMousePos = PointToScreen(new Point(e.X, e.Y));

                this.Location = curMousePos;

                if (fPlayerMode.Created)
                    fPlayerMode.Location = new Point(curMousePos.X, curMousePos.Y + this.Height);

                if (fJukeboxMode.Created)
                    fJukeboxMode.Location = new Point(curMousePos.X, curMousePos.Y + this.Height);

                fMusicList.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            MediaControl.MasterVolume(trackBar1.Value);
        }
    }
}
