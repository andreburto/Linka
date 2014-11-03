using System;
using System.Text;
using Linka;

namespace Linka
{
    public class EmailStatus
    {
        public bool InOracle { get; set; }
        public bool InGoogle { get; set; }
        public string userid { get; set; }

        public Int16 status()
        {
            Int16 status = 0;
            if (this.InOracle == true) { status += 1; }
            if (this.InGoogle == true) { status += 2; }
            return status;
        }

        public EmailStatus() { }
    }
}
