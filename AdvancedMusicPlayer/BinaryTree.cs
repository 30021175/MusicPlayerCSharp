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
 * File: BinaryTree Class
 * Version: 1.0
 */

namespace AdvancedMusicPlayer
{
    class BinaryTree
    {
        Node root;

        //Internal class Node
        public class Node
        {
            public Song song { get; set; }
            public Node left { get; set; }
            public Node right { get; set; }

            public Node(Song song)
            {
                this.song = song;
            }
        }

        //1 - addNode Method - It is used to add a new node on the binary tree.
        public void addNode(Song song)
        {
            Node newNode = new Node(song);
            if (root == null)
            {
                root = newNode;
            }
            else
            {
                root = createNode(root, newNode);
            }
        }

        //2 - createNode Method - It is used to create recursively a node.
        private Node createNode(Node current, Node newNode)
        {
            if (current == null)
            {
                current = newNode;
                return current;
            }
            else if (newNode.song.title.CompareTo(current.song.title) < 0)
            {
                current.left = createNode(current.left, newNode);
            }
            else if (newNode.song.title.CompareTo(current.song.title) > 0)
            {
                current.right = createNode(current.right, newNode);
            }
            return current;
        }

        //3 - sort Method - It is used to sort the binary tree in order.
        private void sort(Node current, LinkedList<Song> songs)
        {
            if (current != null)
            {
                sort(current.left, songs);
                songs.AddLast(current.song);
                sort(current.right, songs);
            }
        }

        //4 - display Method - It is used to return a sorted LinkedList with all songs from the binary list. 
        public LinkedList<Song> display()
        {
            LinkedList<Song> songs = null;
            if (root != null)
            {
                songs = new LinkedList<Song>();
                sort(root, songs);
            }
            return songs;
        }

        //5 - search Method - It is used to return a song object by the title target.
        public Song search(string title)
        {
            Node n = searchNode(title, root);

            if (n != null)
                return n.song;
            else
                return null;
        }

        //6 - serachNode Method - It is used to search a node by title on the binary tree using the binary algorithm.
        private Node searchNode(string title, Node current)
        {
            if (current != null)
            {
                if (title.CompareTo(current.song.title) < 0)
                {
                    if (title.Equals(current.song.title, StringComparison.OrdinalIgnoreCase))
                    {
                        return current;
                    }
                    else
                        return searchNode(title, current.left);
                }
                else
                {
                    if (title.Equals(current.song.title, StringComparison.OrdinalIgnoreCase))
                    {
                        return current;
                    }
                    else
                        return searchNode(title, current.right);
                }
            }
            else
                return current;

        }
    }
}
