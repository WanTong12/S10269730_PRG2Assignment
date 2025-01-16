using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_08
{
    class DDJBFlight : Flight
    {
        public double RequestFee { get; set; }

        public DDJBFlight() : base() 
        {
            RequestFee = 300;
        }

        public DDJBFlight(double r, string fn, string o, string d, DateTime e, string s) : base(fn, o, d, e, s)
        {
            RequestFee = r;
        }

        public override double CalculateFees()
        {
            if (Origin == "Singapore (SIN)") //Depart from SG
            {
                return 800 + 300 + RequestFee;
            }
            else //Arrive at SG
            {
                return 500 + 300 + RequestFee;
            }
        }
        
        public override string ToString()
        {
            return base.ToString() + "\tRequest Fee: " + RequestFee;
        }
    }
}
