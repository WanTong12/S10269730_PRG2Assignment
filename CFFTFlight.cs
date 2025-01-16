using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_08
{
    class CFFTFlight : Flight
    {
        public double RequestFee { get; set; }

        public CFFTFlight() : base() 
        {
            RequestFee = 150;
        }

        public CFFTFlight(double r, string fn, string o, string d, DateTime e, string s) : base(fn, o, d, e, s)
        {
            RequestFee = r;
        }

        public override double CalculateFees()
        {
            if (Origin == "Singapore (SIN)") //Depart from SG
            {
                return 800 + 300 + RequestFee; // + basefee + requestfee
            }
            else //Arrive at SG
            {
                return 500 + 300 + RequestFee; // + basefee + requestfee
            }
        }
        public override string ToString()
        {
            return base.ToString() + "\tRequest Fee: " + RequestFee;
        }
    }
}
