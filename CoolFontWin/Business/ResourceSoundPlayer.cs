using System;
using System.Media;
using System.IO;
using log4net;

namespace CFW.Business
{
    public static class ResourceSoundPlayer
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void TryToPlay(UnmanagedMemoryStream stream)
        {
            try
            {
                stream.Position = 0;
                SoundPlayer player = new SoundPlayer();
                player.Stream = stream;
                player.Play();
            }
            catch (Exception ex)
            {
                log.Debug("Unable to play sound: " + ex);
            }
        }
    }
}
