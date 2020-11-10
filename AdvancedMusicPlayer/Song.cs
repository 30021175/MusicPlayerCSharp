using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * Date: 01/11/2020
 * Student ID: 30021175
 * Name: Willian Bernatzki Woellner
 * Course: Diploma of Software Development
 * Cluster: Programming 3
 * Description: Assessment 3 - Project
 * File: Song Class
 * Version: 1.0
 */

namespace AdvancedMusicPlayer
{
    class Song
    {
        //Attributes
        public string title { get; set; }
        public string artist { get; set; }
        public string album { get; set; }
        public int year { get; set; }
        public string path { get; set; }

        //Constructor Method
        public Song(string title, string album, string artist, int year, string path)
        {
            this.title = string.IsNullOrEmpty(title) ? "Unknown" : title;
            this.album = string.IsNullOrEmpty(album) ? "Unknown" : album;
            this.artist = string.IsNullOrEmpty(artist) ? "Unknown" : artist;
            this.year = year;
            this.path = path;
        }

    }
}
