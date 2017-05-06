using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace TorrentBrowser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void downloadCurrentSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sourcePage = webBrowser1.DocumentText;

            List<string> torrents = GetTorrents(sourcePage);
            DownloadTorrents(torrents);
        }

        private static List<string> GetTorrents(string sourcePage)
        {
            List<string> torrents = new List<string>();
            int index = 0;
            do
            {
                index = sourcePage.IndexOf("magnet:?", index);
                if (index != -1)
                {
                    var endOfMagnetLink = sourcePage.IndexOf("\"", index);
                    var difference = endOfMagnetLink - index;
                    var magnetLink = sourcePage.Substring(index, difference);

                    torrents.Add(magnetLink);
                    index++;
                }
            } while (index != -1);

            torrents = torrents.Distinct().ToList();
            return torrents;
        }

        private static void DownloadTorrents(List<string> torrents)
        {
            foreach (var torrent in torrents)
            {
                Process.Start(torrent);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://nyaa.pantsu.cat/search?c=3_5&s=&sort=torrent_id&order=desc&max=300&q=");
        }
    }
}
