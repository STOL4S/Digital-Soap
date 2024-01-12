using DigitalClean;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSoap
{
    public static class Core
    {
        public static void Initialize()
        {
            CreateFolderStructure();
        }

        private static void CreateFolderStructure()
        {
            List<string> ExpectedFolders = new List<string>
            {
                Folders.LOCAL_APPDATA + "\\DigitalSoap",
                Folders.LOCAL_APPDATA + "\\DigitalSoap\\Config",
                Folders.LOCAL_APPDATA + "\\DigitalSoap\\Resources"
            };

            foreach (string Dir in ExpectedFolders)
            {
                if (!Directory.Exists(Dir))
                {
                    Directory.CreateDirectory(Dir);
                }
            }
        }
    }
}
