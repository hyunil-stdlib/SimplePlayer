using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace player
{
    public partial class Form_MusicList : Form
    {
        List<FileInfo> curMusicList = new List<FileInfo>();

        public Form_MusicList()
        {
            InitializeComponent();
        }

        public void setMusicList(List<FileInfo> musicList)
        {
            curMusicList.Clear();
            listBox1.Items.Clear();

            foreach (FileInfo item in musicList)
            {
                curMusicList.Add(item);
                listBox1.Items.Add(item);
            }
        }

        public List<FileInfo> getMusicList()
        {
            return curMusicList;
        }
    }
}
