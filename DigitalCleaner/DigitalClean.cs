using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DigitalClean
{
    public delegate void ProgressEventHandler();

    public static class DigitalClean
    {
        public static List<CustomFolder> CustomFolders = new List<CustomFolder>();

        public static Dictionary<string, string> ScanData =
            new Dictionary<string, string>();
        public static string ScanParent = string.Empty;
        public static string ScanCategory = string.Empty;
        public static string ScanDirectory = string.Empty;

        public static State Status = State.IDLE;

        private static bool IsInitialized = false;

        /// <summary>
        /// Initializes the Digital Clean module and
        /// loads any custom folders or applications
        /// that have been saved on this device.
        /// </summary>
        public static void Initialize()
        {
            CustomFolders = LoadCustomFolders();
            //SaveCustomFolders();

            IsInitialized = true;
        }

        /// <summary>
        /// Returns directory size in megabytes, file count.
        /// </summary>
        /// <param name="_Path"></param>
        /// <returns></returns>
        public static PointF CalculateDirectorySize(string _Path)
        {
            PointF Sz = new PointF();

            if (_Path != "" && _Path != Folders.USER_ICONS
                && _Path != string.Empty && _Path != null)
            {
                DirectoryInfo CurrentDir = new DirectoryInfo(_Path);

                try
                {
                    FileInfo[] FileInfos = CurrentDir.GetFiles();
                    foreach (FileInfo Info in FileInfos)
                    {
                        try
                        {
                            using (StreamReader Writer = new StreamReader(Info.FullName))
                            {
                                Sz.X += Info.Length;
                                Sz.Y++;
                            }
                        }
                        catch { }
                    }

                    DirectoryInfo[] DirInfos = CurrentDir.GetDirectories();
                    foreach (DirectoryInfo Info in DirInfos)
                    {
                        Sz = new PointF(Sz.X + (CalculateDirectorySize(Info.FullName).X * 1024.0f),
                            Sz.Y + CalculateDirectorySize(Info.FullName).Y);
                    }

                    Sz.X = Sz.X / 1024.0f;
                }
                catch (Exception Ex)
                {
                    return new PointF(-1, -1);
                }
            }
            else
            {
                if (_Path != "")
                {
                    if (File.Exists(_Path))
                    {
                        Sz = new PointF(new FileInfo(_Path).Length / 1024.0f, 1);
                    }
                }
            }

            if (Sz.X < 0)
            {
                Sz.X *= -1;
            }

            return Sz;
        }
        
        /// <summary>
        /// Loads name, path, and description for all custom folders
        /// saved in the configuration file.
        /// </summary>
        /// <returns></returns>
        public static List<CustomFolder> LoadCustomFolders()
        {
            //INITIALIZE RESULT AND FILE PATH
            List<CustomFolder> Result = new List<CustomFolder>();
            string CustomFolderDataPath = Folders.LOCAL_APPDATA + "\\DigitalSoap\\Config\\CustomFolders.ini";

            //CHECK IF CONFIGURATION FILE EXISTS
            if (File.Exists(CustomFolderDataPath))
            {
                //READ RAW DATA FROM STREAM
                string RawData = "";
                using (StreamReader Reader = new StreamReader(CustomFolderDataPath))
                {
                    RawData = Reader.ReadToEnd();
                }

                //SPLIT RAW DATA INTO LINES
                string[] RawLines = RawData.Split(new char[] { '\n', '\r' }, 
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (string Line in RawLines)
                {
                    //SPLIT EACH LINE INTO SEGMENTS BY SYMBOL
                    string[] LineData = Line.Split(new char[] { '|' });

                    //NAME AND PATH DATA
                    if (LineData.Length == 2)
                    {
                        Result.Add(new CustomFolder(LineData[0], LineData[1]));
                    }
                    //NAME, PATH, AND DESCRIPTION DATA
                    else if (LineData.Length == 3)
                    {
                        Result.Add(new CustomFolder(LineData[0], LineData[1], LineData[2]));
                    }
                }
            }

            //RETURN THE CONSTRUCTED RESULT
            return Result;
        }

        /// <summary>
        /// Saves name, path, and description data for all custom
        /// folders configured within the program.
        /// </summary>
        public static void SaveCustomFolders()
        {
            string CustomFolderDataPath = Folders.LOCAL_APPDATA + "\\DigitalSoap\\Config\\CustomFolders.ini";

            using (StreamWriter Writer = new StreamWriter(CustomFolderDataPath))
            {
                foreach (CustomFolder CF in CustomFolders)
                {
                    Writer.Write(CF.Name + "|" + CF.Path);

                    if (CF.Description != null &&
                        CF.Description.Length > 0)
                    {
                        Writer.Write("|" + CF.Description);
                    }

                    Writer.Write('\n');
                }
            }
        }
    }
}
