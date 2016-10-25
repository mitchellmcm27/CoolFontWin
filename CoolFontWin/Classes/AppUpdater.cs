using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Squirrel;

namespace CoolFont.AppWinForms
{ 
    public static class AppUpdater
    {
        private static UpdateManager updateManagerInstance;

        public static UpdateManager AppUpdateManager
        {
            get
            {
                if (updateManagerInstance != null)
                    return updateManagerInstance;

                var updateManagerTask = UpdateManager.GitHubUpdateManager("https://github.com/mitchellmcm27/CoolFontWin");
                updateManagerTask.Wait(TimeSpan.FromMinutes(1));
                updateManagerInstance = updateManagerTask.Result;
                return updateManagerInstance;
            }
        }

        public static void Dispose()
        {
            if (updateManagerInstance != null)
            {
                updateManagerInstance.Dispose();
            }
        }

        public static void CreateShortcutForThisExe()
        {
            AppUpdateManager.CreateShortcutForThisExe();
        }

        public static void RemoveShortcutForThisExe()
        {
            AppUpdateManager.RemoveShortcutForThisExe();
        }
    }
}
