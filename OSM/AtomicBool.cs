using System.Threading;

namespace OSM
{
    class AtomicBool
    {
        // value to bool 
        private int value;
        
        // set the init func
        public AtomicBool(bool val)
        {
            value = val ? 1 : 0;
        }
        // deault false
        public AtomicBool(): this(false) { }

        // get the value for this
        public bool Get()
        {
            return value != 0;
        }

        // set by threading's Interlocked
        public void Set(bool val)
        {
            Interlocked.Exchange(ref value, val ? 1 : 0);
        }

        // get and set
        public bool GetAndSet(bool val)
        {
            return Interlocked.Exchange(ref value, val ? 1 : 0) != 0;
        }

    }
}
