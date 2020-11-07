using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSM
{
    class FileReader
    {
        private FileStream fileStream;
        private StreamReader reader;
        // read file from dir, if not exist, create the file
        public FileReader()
        {
            var path = "scene.txt";
            fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            // create file's reader
            reader = new StreamReader(fileStream);
        }

        // read the file and add to hash map
        public void Process()
        {
            Util.pedsTable.Clear();
            string line;
            
            while ((line = reader.ReadLine()) != null && line.Length != 0)
            {
                var ss = line.Split(' ');
                // ss[0] to add a ped
                int seconds = int.Parse(ss[0]), floor = int.Parse(ss[1]);
                if (Util.pedsTable[seconds] == null) Util.pedsTable[seconds] = new List<int>();
                var list = (List<int>)Util.pedsTable[seconds];
                list.Add(floor);
                Util.kMaxSeconds = Math.Max(int.Parse(ss[0]), Util.kMaxSeconds);
            }
        }
        
        // close reader 
        public void CloseReader()
        {
            reader.Close();
            fileStream.Close();
        }

    }
}
