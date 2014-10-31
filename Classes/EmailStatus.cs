using System;
using System.Text;
using Linka;

namespace Linka
{
    class EmailStatus
    {
        public bool InOracle { get; set; }
        public bool InGoogle { get; set; }
        public string userid { get; set; }

        public Int16 status { get { return this._status(); } }

        private Int16 _status()
        {
            Int16 status = 0;
            if (InOracle == true) status += 1;
            if (InGoogle == true) status += 2;
            return status;
        }
    }
}
