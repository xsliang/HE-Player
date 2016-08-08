using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyMp3Player
{
    public partial class FormMain : Form
    {
        List<PlayItem> items = new List<PlayItem>();

        string[] files, path;

        bool isPlaying = false;

        bool NeedDoItAgain = false;

        public FormMain()
        {
            InitializeComponent();
            axWindowsMediaPlayer.PlayStateChange += AxWindowsMediaPlayer_PlayStateChange;
        }

        private void AxWindowsMediaPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            // Test the current state of the player and display a message for each state.
            switch (e.newState)
            {
                case 0:    // Undefined
                    //currentStateLabel.Text = "Undefined";
                    break;
                case 1:    // Stopped
                    if (listBoxPlayerItems.SelectedIndex + 1 < listBoxPlayerItems.Items.Count)
                    {
                        listBoxPlayerItems.SelectedIndex = listBoxPlayerItems.SelectedIndex + 1;
                    }
                    else if (NeedDoItAgain == true)
                    {
                        listBoxPlayerItems_SelectedIndexChanged(null, null);
                    }
                    break;

                case 2:    // Paused
                    //currentStateLabel.Text = "Paused";
                    break;

                case 3:    // Playing
                    if (isPlaying == true)
                    {
                        isPlaying = false;
                        NeedDoItAgain = false;
                    }
                    break;

                case 4:    // ScanForward
                    //currentStateLabel.Text = "ScanForward";
                    break;

                case 5:    // ScanReverse
                    //currentStateLabel.Text = "ScanReverse";
                    break;

                case 6:    // Buffering
                    //currentStateLabel.Text = "Buffering";
                    break;

                case 7:    // Waiting
                    //currentStateLabel.Text = "Waiting";
                    break;

                case 8:    // MediaEnded
                    break;

                case 9:    // Transitioning
                    //currentStateLabel.Text = "Transitioning";
                    break;

                case 10:   // Ready
                    break;

                case 11:   // Reconnecting
                    //currentStateLabel.Text = "Reconnecting";
                    break;

                case 12:   // Last
                    //currentStateLabel.Text = "Last";
                    break;

                default:
                    //currentStateLabel.Text = ("Unknown State: " + e.newState.ToString());
                    break;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void listBoxPlayerItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPlayerItems.SelectedIndex != -1)
            {
                isPlaying = false;
                NeedDoItAgain = true;
                axWindowsMediaPlayer.URL = listBoxPlayerItems.SelectedValue.ToString();
            }
        }

        private void openFolderFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = folderBrowserDialog.SelectedPath;
                string[] files = System.IO.Directory.GetFiles(folderPath);

                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Split('.')[files[i].Split('.').Length - 1] == "mp3")
                    {
                        string name = files[i].Split('\\')[files[i].Split('\\').Length - 1];
                        string path = files[i];
                        items.Add(new PlayItem { Name = name, Path = path });
                    }
                }
                if (items.Count > 0)
                {
                    listBoxPlayerItems.SelectedIndexChanged -= listBoxPlayerItems_SelectedIndexChanged;
                    listBoxPlayerItems.DataSource = null;
                    listBoxPlayerItems.DisplayMember = "Name";
                    listBoxPlayerItems.ValueMember = "Path";
                    listBoxPlayerItems.DataSource = items;
                    listBoxPlayerItems.SelectedIndexChanged += listBoxPlayerItems_SelectedIndexChanged;
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                files = openFileDialog.SafeFileNames;

                path = openFileDialog.FileNames;

                for (int i = 0; i < files.Length; i++)
                {
                    string name = files[i];
                    string path = openFileDialog.FileNames[i];
                    items.Add(new PlayItem { Name = name, Path = path });

                    listBoxPlayerItems.SelectedIndexChanged -= listBoxPlayerItems_SelectedIndexChanged;
                    listBoxPlayerItems.DataSource = null;
                    listBoxPlayerItems.DisplayMember = "Name";
                    listBoxPlayerItems.ValueMember = "Path";
                    listBoxPlayerItems.DataSource = items;
                    listBoxPlayerItems.SelectedIndexChanged += listBoxPlayerItems_SelectedIndexChanged;
                }
            }
        }
    }

    public class PlayItem
    {
        public string Name { get; set; }

        public string Path { get; set; }
    }
}
