﻿using System;
using System.Diagnostics;
using System.Windows.Controls;


namespace CFW.View
{
    /// <summary>
    /// Interaction logic for SocialMediaControl.xaml
    /// </summary>
    public partial class SocialMediaControl : UserControl
    {
        public SocialMediaControl()
        {
            InitializeComponent();
        }

        public void LaunchFacebook(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/CoolFont/");
        }

        public void LaunchTwitter(object sender, EventArgs e)
        {
            Process.Start("https://twitter.com/coolfont");
        }

        public void LaunchYoutube(object sender, EventArgs e)
        {
            Process.Start("https://youtu.be/yUBpnK-y6Nc");
        }

        public void LaunchWeb(object sender, EventArgs e)
        {
            Process.Start("http://www.coolfont.co");
        }

        public void LaunchDiscord(object sender, EventArgs e)
        {
            Process.Start("https://discordapp.com/invite/FNqSYe2");
        }
    }
}