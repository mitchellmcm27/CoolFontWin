using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace PocketStrafe.View
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

        public void LaunchReddit(object sender, EventArgs e)
        {
            Process.Start("https://www.reddit.com/r/pocketstrafe");
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
            Process.Start("http://www.pocketstrafe.com");
        }

        public void LaunchDiscord(object sender, EventArgs e)
        {
            Process.Start("https://discordapp.com/invite/FNqSYe2");
        }
    }
}