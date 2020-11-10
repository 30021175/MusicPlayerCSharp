using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper;
using System.Security.Cryptography;

/*
 * Date: 01/11/2020
 * Student ID: 30021175
 * Name: Willian Bernatzki Woellner
 * Course: Diploma of Software Development
 * Cluster: Programming 3
 * Description: Assessment 3 - Project. 
 * Create an advanced music player that allows the ability to sort and search the songs stored in a binary tree 
 * (any sort and search algorithm you select will have to be approved if it is not merge sort and binary search), 
 * the GUI should display the sorted track list and highlight and play the searched track, 
 * it should save the track list to a csv using a 3rd party library.
 * Version: 1.0
*/

namespace AdvancedMusicPlayer
{
    public partial class MediaPlayerForm : Form
    {
        //Global Variables
        private BinaryTree binaryTree;
        private LinkedList<Song> sortedSongs;
        private Song currentSong;
        private int currentPosition;

        //1 - MediaPlayerForm Method - It is used to initialize the main form.
        public MediaPlayerForm()
        {
            InitializeComponent();

            lblMessage.Visible = false;
            setMediaPlayer();
        }

        //2 - setMediaPlayer Method - It is used to set up the Windows Media Player plugin.
        private void setMediaPlayer()
        {
            wMPlayer.uiMode = "mini";
            wMPlayer.Location = new Point(10, 225);
            wMPlayer.Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - 290);
            wMPlayer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

        }

        //3 - displaySongs Method - It is used to display the tracklist sorted by title in a gridview
        private void displaySongs()
        {
            sortedSongs = binaryTree.display();

            DataTable dt = new DataTable();
            dt.Columns.Add("Title");
            dt.Columns.Add("Artist");
            dt.Columns.Add("Album");
            dt.Columns.Add("Year");

            foreach (var item in sortedSongs)
            {
                dt.Rows.Add(item.title, item.artist, item.album, item.year == 0 ? "Unknown" : item.year.ToString());
            }

            dtGridViewSongs.DataSource = dt;
        }

        //4 - loadSong Method - It is used to load a song usign the Windows Media Player plugin.
        private void loadSong()
        {
            wMPlayer.URL = currentSong.path;
            wMPlayer.Ctlcontrols.play();
        }

        //5 - setLabel Method - It is used to set up the error or success label.
        private void setLabel(Enum.TypeMessage typeMessage, string message)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = typeMessage == Enum.TypeMessage.Error ? Color.Red : Color.Green;
            lblMessage.Visible = true;
        }

        //6 - btnAddSong_Click Method - It is used to add songs to play.
        private void btnAddSong_Click(object sender, EventArgs e)
        {
            //Settings the open dialog 
            openFileDialog.Filter = "Sound files (*.aa; *.aax; *.aac; *.aiff; *.ape; *.dsf; *.flac; *.m4a; *.m4b; *.m4p; *.mp3; *.mpc; *.mpp; *.ogg; *.oga; *.wav; *.wma; *.wv; *.webm)|*.aa; *.aax; *.aac; *.aiff; *.ape; *.dsf; *.flac; *.m4a; *.m4b; *.m4p; *.mp3; *.mpc; *.mpp; *.ogg; *.oga; *.wav; *.wma; *.wv; *.webm";
            openFileDialog.Multiselect = true;
            openFileDialog.DefaultExt = "mp3";

            try
            {
                lblMessage.Visible = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    binaryTree = new BinaryTree(); //create a binary tree

                    foreach (string file in openFileDialog.FileNames) //add nodes
                    {
                        TagLib.File fileTag = TagLib.File.Create(file);
                        Song song = new Song(
                            fileTag.Tag.Title,
                            fileTag.Tag.Album,
                            fileTag.Tag.FirstPerformer,
                            (int)fileTag.Tag.Year,
                            file
                            );
                        binaryTree.addNode(song);
                    }

                    displaySongs();
                    //set up to play the first song
                    currentPosition = 0;
                    dtGridViewSongs.Rows[currentPosition].Selected = true;
                    currentSong = sortedSongs.First();
                    loadSong();
                }
            }
            catch
            {
                setLabel(Enum.TypeMessage.Error, "Error to add songs on the tracklist.");
            }
        }

        //7 - btnFirst_Click Method - It is used to play the first song.
        private void btnFirst_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Visible = false;
                currentPosition = 0;
                dtGridViewSongs.Rows[currentPosition].Selected = true;
                currentSong = sortedSongs.First();
                loadSong();
            }
            catch
            {
                setLabel(Enum.TypeMessage.Error, "The first song cannot be played.");
            }
        }

        //8 - btnPrevious_Click Method - It is used to play the previous song.
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Visible = false;

                LinkedListNode<Song> previousNode = sortedSongs.Find(currentSong).Previous;

                if (previousNode != null)
                {
                    currentPosition--;
                    dtGridViewSongs.Rows[currentPosition].Selected = true;
                    currentSong = previousNode.Value;
                    loadSong();
                }
                else
                {
                    setLabel(Enum.TypeMessage.Error, "There is no previous song to play.");
                }
            }
            catch
            {
                setLabel(Enum.TypeMessage.Error, "The previous song cannot be played.");
            }
        }

        //9 - btnNext_Click Method - It is used to play the next song.
        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Visible = false;

                LinkedListNode<Song> nextSong = sortedSongs.Find(currentSong).Next;

                if (nextSong != null)
                {
                    currentPosition++;
                    dtGridViewSongs.Rows[currentPosition].Selected = true;
                    currentSong = nextSong.Value;
                    loadSong();
                }
                else
                {
                    setLabel(Enum.TypeMessage.Error, "There is no next song to play.");
                }
            }
            catch
            {
                setLabel(Enum.TypeMessage.Error, "The next song cannot be played.");
            }
        }

        //10 - btnLast_Click Method - It is used to play the last song.
        private void btnLast_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Visible = false;
                currentPosition = sortedSongs.Count() - 1;
                dtGridViewSongs.Rows[currentPosition].Selected = true;
                currentSong = sortedSongs.Last();
                loadSong();
            }
            catch
            {
                setLabel(Enum.TypeMessage.Error, "The last song cannot be played.");
            }
        }

        //10 - btnSearch_Click Method - It is used to search a song and play it.
        private void btnSearch_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                if (binaryTree != null)
                {
                    Song song = binaryTree.search(txtSearch.Text);

                    if (song != null)
                    {
                        foreach (DataGridViewRow row in dtGridViewSongs.Rows)
                        {
                            if (row.Cells[0].Value.ToString() == song.title)
                            {
                                row.Selected = true;
                                currentPosition = row.Index;
                                break;
                            }
                        }

                        currentSong = song;
                        loadSong();
                    }
                    else
                    {
                        setLabel(Enum.TypeMessage.Error, "Song not found.");
                    }
                }
                else
                {
                    setLabel(Enum.TypeMessage.Error, "No songs added.");
                }
            }
            else
            {
                setLabel(Enum.TypeMessage.Error, "Search field is required.");
            }
        }

        //11 - btnSave_Click Method - It is used to save the tracklist in a csv file using the csvhelper library.
        private void btnSave_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            if (binaryTree != null)
            {
                //get the csv path to the appconfig file.
                string trackListPath = ConfigurationSettings.AppSettings["trackListPath"].ToString();

                if (!string.IsNullOrEmpty(trackListPath))
                {
                    try
                    {
                        StreamWriter writer = new StreamWriter(trackListPath);

                        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            List<Song> songsToCSV = new List<Song>();

                            foreach (var song in binaryTree.display().ToList())
                            {
                                songsToCSV.Add(
                                    new Song(song.title,
                                    song.album,
                                    song.artist,
                                    song.year,
                                    MD5Encrypt(song.path)));
                            }

                            csv.WriteRecords(songsToCSV);
                            setLabel(Enum.TypeMessage.Success, "Tracklist has been saved.");
                        }
                    }
                    catch
                    {
                        setLabel(Enum.TypeMessage.Error, "Error to save the tracklist.");
                    }
                }
                else
                {
                    setLabel(Enum.TypeMessage.Error, "AppSettings not configurated.");
                }
            }
            else
            {
                setLabel(Enum.TypeMessage.Error, "No songs added.");
            }
        }

        //12 - MD5Encrypt Method - It is used to encrypt the password using MD5 method
        private string MD5Encrypt(string path)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(path));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();

        }

    }
}