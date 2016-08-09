using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MyMp3Player
{
    public partial class FormMain : Form
    {
        List<PlayItem> items = new List<PlayItem>();

        string[] files, path;

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
                case (int)WMPLib.WMPPlayState.wmppsStopped:
                        PlayItem();
                    break;

                case 2:    // Paused
                    //currentStateLabel.Text = "Paused";
                    break;

                case (int)WMPLib.WMPPlayState.wmppsPlaying:
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

                case (int)WMPLib.WMPPlayState.wmppsMediaEnded:
                    if (listBoxPlayerItems.SelectedIndex + 1 < listBoxPlayerItems.Items.Count)
                    {
                        listBoxPlayerItems.SelectedIndex = listBoxPlayerItems.SelectedIndex + 1;
                    }
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

        private void listBoxPlayerItems_DoubleClick(object sender, EventArgs e)
        {
            PlayItem();
        }

        private void PlayItem()
        {
            if (listBoxPlayerItems.SelectedIndex != -1)
            {
                axWindowsMediaPlayer.URL = listBoxPlayerItems.SelectedValue.ToString();
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
