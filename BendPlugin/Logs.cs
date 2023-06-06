using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace BendPlugin
{
    internal class Logs
    {
        private string path;
        private string name;
        private StringBuilder textForSave;
        public string Path
        {
            get
            {
                return path;
            }
            private set
            {
                path = value;
            }
        }

        public Logs(string path)
        {
            this.path = path;
            textForSave = new StringBuilder();
            name = "log.txt";
        }
        
        public void ClearFile()
        {
            File.WriteAllText(path + name, "");
        }
        public void Remember(string text)
        {
            textForSave.Append(text); 
        }
        public void SaveToFile()
        {
            File.AppendAllText(path + name, textForSave.ToString());
            textForSave.Clear();
        }
        public void WriteAndSave(string text)
        {
            File.AppendAllText(path + name, text);
        }


    }
}
