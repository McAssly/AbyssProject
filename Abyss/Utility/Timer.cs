using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Utility
{
    internal class Timer
    {
        private double max;
        private double i;
        
        public Timer(double max)
        {
            this.max = max;
            i = 0;
        }

        public void Start(double _override = 0) 
        {
            if (_override == 0)
                i = max;
            else
                i = _override;
        }

        public void Update(double delta) { if (i > 0) i -= delta; }

        public bool IsRunning() { return i > 0; }

        public override string ToString()
        {
            return i + "";
        }

        internal void Add(double v)
        {
            if (this.IsRunning())
            {
                this.i += v;
            }
        }
    }
}
