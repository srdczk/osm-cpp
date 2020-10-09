using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace OSM
{
    class Reporter
    {
        // reporter for the number of peds per floor
        private AtomicBool running;
        // new thread for IO
        private Thread outThread;
        // sync queue for output
        private SyncQueue<string> queue;
        // file streams and Stream Writers
        private List<FileStream> fileStreams;
        private List<StreamWriter> writes;

        // file's name, floors to create the file stream
        public Reporter(string fileName, int floors)
        {
            running = new AtomicBool(true);
            outThread = new Thread(ThreadFunc);
            queue = new SyncQueue<string>();
            //  init for output
            fileStreams = new List<FileStream>();
            writes = new List<StreamWriter>();
            // every floor has one text
            for (int i = 0; i <= floors; i++)
            {
                if (i < floors)
                {
                    var path = fileName + i + ".txt";
                    FileStream fileStream;
                    if (!File.Exists(path))
                        fileStream = new FileStream(path, FileMode.CreateNew);
                    else
                        fileStream = new FileStream(path, FileMode.Append);
                    var writer = new StreamWriter(fileStream);
                    fileStreams.Add(fileStream);
                    writes.Add(writer);
                }
                else
                {
                    var path = fileName + "peds.txt";
                    FileStream fileStream;
                    if (!File.Exists(path))
                        fileStream = new FileStream(path, FileMode.CreateNew);
                    else
                        fileStream = new FileStream(path, FileMode.Append);
                    var writer = new StreamWriter(fileStream);
                    fileStreams.Add(fileStream);
                    writes.Add(writer);
                }
            }
            outThread.Start();
        }

        // add the string to this 
        public void AddReport(string line)
        {
            queue.Add(line);
        }

        // the Func of thread
        private void ThreadFunc()
        {
            while (running.Get())
            {
                // take items from queue
                var list = queue.TakeAll(Util.kTimeout);
                if (list == null) continue;
                foreach (var item in list)
                {
                    if (item.Length > 6)
                    {
                        var index = writes.Count - 1;
                        writes[index].WriteLine(item);
                        writes[index].Flush();
                    }
                    else
                    {
                        // output to single files
                        var ss = item.Split(' ');
                        var index = int.Parse(ss[0]);
                        writes[index].WriteLine(ss[1]);
                        writes[index].Flush();
                    }
                }
            }
            if (queue != null && queue.Count() > 0)
            {
                // if not clear all thread
                // output to files
                var list = queue.TakeAll();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item.Length > 6)
                        {
                            var index = writes.Count - 1;
                            writes[index].WriteLine(item);
                            writes[index].Flush();
                        }
                        else
                        {
                            var ss = item.Split(' ');
                            var index = int.Parse(ss[0]);
                            writes[index].WriteLine(ss[1]);
                            writes[index].Flush();
                        }
                    }
                }
            }
            // close all 
            foreach (var writer in writes)
                writer.Close();
            foreach (var fileStream in fileStreams)
                fileStream.Close();
        }

        // stop and let thread running
        public void Stop()
        {
            // if not running
            if (!running.Get()) return;
            // set the atomic bool running to false
            while (!running.GetAndSet(false)) ;
            outThread.Join();
        }

    }
}
