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
        const string DownloadPage = "https://www.nyaa.se/?page=download&";

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
                index = sourcePage.IndexOf("tid=", index);
                if (index != -1)
                {
                    var endOfTid = sourcePage.IndexOf("\"", index);
                    var difference = endOfTid - index;
                    var tid = sourcePage.Substring(index, difference);

                    torrents.Add(DownloadPage + tid);
                    index++;
                }
            } while (index != -1);

            torrents = torrents.Distinct().ToList();
            return torrents;
        }

        private static void DownloadTorrents(List<string> torrents)
        {
            using (WebClient webClient = new WebClient())
            {
                int count = 1;
                foreach (var torrent in torrents)
                {
                    string name = $"{count}.torrent";
                    webClient.DownloadFile(torrent, name);
                    Process.Start(name);
                    count++;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.nyaa.se/?page=search&cats=1_37&filter=2");
        }
    }
}
