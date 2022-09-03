using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gavilya.Fps
{
    // Helper class to store frame timestamps
    public class TimestampCollection
    {

        public string Name { get; set; }

        private List<double> timestamps = new List<double>(1001);
        object sync = new object();

        //add value to the collection
        public void Add(double timestamp)
        {
            lock (sync)
            {
                timestamps.Add(timestamp);
                if (timestamps.Count > 1000)
                {
                    timestamps.RemoveAt(0);
                }
            }
        }

        public int QueryCount(double from, double to)
        {
            int count = 0;
            lock (sync)
            {
                foreach (var ts in timestamps)
                {
                    if (ts >= from && ts <= to)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
