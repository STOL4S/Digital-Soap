using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DigitalClean
{
    public static class Folders
    {
        //META
        public static string LOCAL_APPDATA = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string ROAMING_APPDATA = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string USER_PROFILE = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static string SYSTEM_FOLDER = Environment.GetFolderPath(Environment.SpecialFolder.System);

        //SYSTEM
        public static string WIN_THUMBNAIL = LOCAL_APPDATA + "\\Microsoft\\Windows\\Explorer";
        public static string USER_THUMBNAIL = USER_PROFILE + "\\.thumbnails";
        public static string WIN_DOWNLOADS = USER_PROFILE + "\\Downloads";
        public static string WIN_MEMDUMP = "C:\\Windows\\Minidump";
        public static string WIN_EVENTLOGS = SYSTEM_FOLDER + "\\winevt\\Logs";
        public static string WIN_SYSLOGS = SYSTEM_FOLDER + "\\Logs";
        public static string WIN_LOGS = SYSTEM_FOLDER + "\\LogFiles";
        public static string WIN_REPORTS = "%ProgramData%\\Microsoft\\Windows\\WER";

        //WINDOWS EXPLORER
        public static string USER_CRASHDUMP = LOCAL_APPDATA + "\\CrashDumps";
        public static string USER_ICONS = LOCAL_APPDATA + "\\IconCache.db";
        public static string USER_RECENTS = ROAMING_APPDATA + "\\Microsoft\\Windows\\Recent";
        public static string USER_HISTORY = LOCAL_APPDATA + "\\Microsoft\\Windows\\History";
        public static string USER_NOTIFICATIONS = LOCAL_APPDATA + "\\Microsoft\\Windows\\Notifications";
        public static string USER_SEARCH = USER_PROFILE + "\\Searches";
        public static string USER_TEMPORARY = LOCAL_APPDATA + "\\Temp";
        public static string USER_RECYCLE = "C:\\$Recycle.Bin";
        public static string USER_WEB = LOCAL_APPDATA + "\\Microsoft\\Windows\\WebCache";

        //INTERNET EXPLORER
        public static string INET_AUTO = LOCAL_APPDATA + "\\Microsoft\\Windows\\INetCache\\dummy";
        public static string INET_COOKIES = LOCAL_APPDATA + "\\Microsoft\\Windows\\INetCookies";
        public static string INET_HISTORY = LOCAL_APPDATA + "\\Microsoft\\Windows\\INetCache";
        public static string INET_IMAGES = LOCAL_APPDATA + "\\Microsoft\\Internet Explorer\\imagestore";
        public static string INET_DATABASE = LOCAL_APPDATA + "\\Microsoft\\Internet Explorer\\Indexed DB";
        public static string INET_PASSWORD = LOCAL_APPDATA + "\\Microsoft\\Windows\\INetCache\\dummy2";
        public static string INET_TEMP = LOCAL_APPDATA + "\\Microsoft\\Internet Explorer\\CacheStorage";

        //MICROSOFT EDGE
        public static string ENET_AUTO;
        public static string ENET_COOKIES;
        public static string ENET_HISTORY;
        public static string ENET_IMAGES;
        public static string ENET_DATABASE;
        public static string ENET_SESSION;
        public static string ENET_PASSWORD;
        public static string ENET_TEMP;

        public static string ParentNameFromPath(string _Path)
        {
            if (_Path == WIN_THUMBNAIL)
            {
                return "Windows Explorer";
            }
            else if (_Path == WIN_MEMDUMP)
            {
                return "System";
            }
            else if (_Path == USER_NOTIFICATIONS)
            {
                return "System";
            }
            else if (_Path == WIN_EVENTLOGS)
            {
                return "System";
            }
            else if (_Path == WIN_SYSLOGS)
            {
                return "System";
            }
            else if (_Path == WIN_LOGS)
            {
                return "System";
            }
            else if (_Path == WIN_REPORTS)
            {
                return "System";
            }
            else if (_Path == USER_CRASHDUMP)
            {
                return "System";
            }
            else if (_Path == USER_RECYCLE)
            {
                return "System";
            }
            else if (_Path == USER_WEB)
            {
                return "System";
            }
            else if (_Path == USER_ICONS)
            {
                return "Windows Explorer";
            }
            else if (_Path == USER_RECENTS)
            {
                return "Windows Explorer";
            }
            else if (_Path == USER_HISTORY)
            {
                return "Windows Explorer";
            }
            else if (_Path == USER_SEARCH)
            {
                return "Windows Explorer";
            }
            else if (_Path == USER_TEMPORARY)
            {
                return "System";
            }
            else if (_Path == USER_THUMBNAIL)
            {
                return "Windows Explorer";
            }
            else if (_Path == WIN_DOWNLOADS)
            {
                return "Windows Explorer";
            }
            else if (_Path == INET_AUTO)
            {
                return "Internet Explorer";
            }
            else if (_Path == INET_COOKIES)
            {
                return "Internet Explorer";
            }
            else if (_Path == INET_HISTORY)
            {
                return "Internet Explorer";
            }
            else if (_Path == INET_IMAGES)
            {
                return "Internet Explorer";
            }
            else if (_Path == INET_DATABASE)
            {
                return "Internet Explorer";
            }
            else if (_Path == INET_PASSWORD)
            {
                return "Internet Explorer";
            }
            else if (_Path == INET_TEMP)
            {
                return "Internet Explorer";
            }
            else if (_Path == ENET_AUTO)
            {
                return "Microsoft Edge";
            }
            else if (_Path == ENET_COOKIES)
            {
                return "Microsoft Edge";
            }
            else if (_Path == ENET_DATABASE)
            {
                return "Microsoft Edge";
            }
            else if (_Path == ENET_HISTORY)
            {
                return "Microsoft Edge";
            }
            else if (_Path == ENET_IMAGES)
            {
                return "Microsoft Edge";
            }
            else if (_Path == ENET_SESSION)
            {
                return "Microsoft Edge";
            }
            else if (_Path == ENET_PASSWORD)
            {
                return "Microsoft Edge";
            }
            else if (_Path == ENET_TEMP)
            {
                return "Microsoft Edge";
            }
            return "NULL";
        }

        public static string NodeNameFromPath(string _Path)
        {
            if (_Path == WIN_THUMBNAIL)
            {
                return "Thumbnail Cache";
            }
            else if (_Path == USER_THUMBNAIL)
            {
                return "User Thumbnail Cache";
            }
            else if (_Path == WIN_DOWNLOADS)
            {
                return "Downloaded Files";
            }
            else if (_Path == WIN_MEMDUMP)
            {
                return "Memory Dumps";
            }
            else if (_Path == USER_NOTIFICATIONS)
            {
                return "Notifications";
            }
            else if (_Path == WIN_EVENTLOGS)
            {
                return "Event Log Files";
            }
            else if (_Path == WIN_SYSLOGS)
            {
                return "System Log Files";
            }
            else if (_Path == WIN_LOGS)
            {
                return "Windows Log Files";
            }
            else if (_Path == WIN_REPORTS)
            {
                return "Windows Report Files";
            }
            else if (_Path == USER_CRASHDUMP)
            {
                return "Windows Error Reports";
            }
            else if (_Path == USER_ICONS)
            {
                return "Icon Cache";
            }
            else if (_Path == USER_RECENTS)
            {
                return "Recents Cache";
            }
            else if (_Path == USER_HISTORY)
            {
                return "History Cache";
            }
            else if (_Path == USER_SEARCH)
            {
                return "Search Cache";
            }
            else if (_Path == USER_TEMPORARY)
            {
                return "Temporary Files";
            }
            else if (_Path == USER_RECYCLE)
            {
                return "Recycle Bin";
            }
            else if (_Path == USER_WEB)
            {
                return "Web Cache";
            }
            else if (_Path == WIN_REPORTS)
            {
                return "Error Reports";
            }
            else if (_Path == INET_AUTO)
            {
                return "Autocomplete History";
            }
            else if (_Path == INET_COOKIES)
            {
                return "Cookies";
            }
            else if (_Path == INET_HISTORY)
            {
                return "History";
            }
            else if (_Path == INET_IMAGES)
            {
                return "Image Cache";
            }
            else if (_Path == INET_DATABASE)
            {
                return "Indexed Database Files";
            }
            else if (_Path == INET_PASSWORD)
            {
                return "Saved Passwords";
            }
            else if (_Path == INET_TEMP)
            {
                return "Temporary Files";
            }
            else if (_Path == ENET_AUTO)
            {
                return "Autocomplete History";
            }
            else if (_Path == ENET_COOKIES)
            {
                return "Cookies";
            }
            else if (_Path == ENET_DATABASE)
            {
                return "Indexed Database Files";
            }
            else if (_Path == ENET_HISTORY)
            {
                return "History";
            }
            else if (_Path == ENET_IMAGES)
            {
                return "Image Cache";
            }
            else if (_Path == ENET_SESSION)
            {
                return "Session";
            }
            else if (_Path == ENET_PASSWORD)
            {
                return "Saved Passwords";
            }
            else if (_Path == ENET_TEMP)
            {
                return "Temporary Files";
            }
            return "NULL";
        }
    }
}
