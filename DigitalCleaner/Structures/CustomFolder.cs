using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalClean
{
    public class CustomFolder
    {
        public string Name;
        public string Description;
        public string Path;

        public CustomFolder(string _Name, string _Path)
        {
            this.Name = _Name;
            this.Path = _Path;
            this.Description = "";
        }

        public CustomFolder(string _Name, string _Path, string _Description)
        {
            this.Name = _Name;
            this.Path = _Path;
            this.Description = _Description;
        }
    }
}
