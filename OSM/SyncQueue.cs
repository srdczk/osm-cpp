using System.Collections.Generic;
using System.Threading;


namespace OSM
{
    class SyncQueue<T>
    {
        // list to contain
        private List<T> queue;
        // lock to prevent queue
        private AutoResetEvent ars;
        // lock to wait for empty
        private AutoResetEvent waitArs;
        
        public SyncQueue()
        {
            queue = new List<T>();
            ars = new AutoResetEvent(true);
            waitArs = new AutoResetEvent(false);
        }

        // return the count, using ars to prevent
        public int Count()
        {
            int res = 0;
            ars.WaitOne(Timeout.Infinite, true);
            res = queue.Count;
            ars.Set();
            return res;
        }

        // add the val to queue
        public void Add(T t)
        {
            if (t == null) return;
            ars.WaitOne(Timeout.Infinite, true);
            queue.Add(t);
            // the queue not empty, take thread can take
            waitArs.Set();
            ars.Set();
        }
        // take the first item of queue
        public T Take()
        {
            ars.WaitOne(Timeout.Infinite, true);
            while (queue.Count == 0)
            {
                // if queue is empty, wait
                ars.Set();
                waitArs.WaitOne(Timeout.Infinite, true);
                ars.WaitOne(Timeout.Infinite, true);
            }
            T res = queue[0];
            queue.RemoveAt(0);
            ars.Set();
            return res;
        }
        // peek the first item
        public T Peek()
        {
            ars.WaitOne(Timeout.Infinite, true);
            while (queue.Count == 0)
            {
                ars.Set();
                waitArs.WaitOne(Timeout.Infinite, true);
                ars.WaitOne(Timeout.Infinite, true);
            }
            T ret = queue[0];
            ars.Set();
            return ret;
        }
        // take all elements to a queue, add timeout to avoid dead
        public List<T> TakeAll(int timeout = Timeout.Infinite)
        {
            ars.WaitOne(Timeout.Infinite, true);
            while (queue.Count == 0)
            {
                ars.Set();
                waitArs.WaitOne(timeout, true);
                if (queue.Count == 0 && timeout != Timeout.Infinite)
                {
                    // if timeout
                    return null;
                }
                ars.WaitOne(Timeout.Infinite, true);
            }
            var res = new List<T>();
            res.AddRange(queue);

            queue.Clear();
            ars.Set();
            return res;
        }
    }
}
