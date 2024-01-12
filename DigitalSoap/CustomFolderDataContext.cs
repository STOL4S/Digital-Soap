using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSoap
{
    public class CustomFolderDataContext
    {
        public string CFTitle { get; set; }

        public string CFPath { get; set; }

        public string CFDescription { get; set; }

        public CustomFolderDataContext()
        {
            this.CFTitle = "NULL";
            this.CFPath = "NULL";
            this.CFDescription = "NULL";
        }

        public CustomFolderDataContext(string _Title, string _Path, string _Description)
        {
            this.CFTitle = _Title;
            this.CFPath = _Path;
            this.CFDescription = _Description;
        }
    }
}
