using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10267930
// Student Name : Chong Wan Tong
// Partner Name : Teo Qi En
//==========================================================

namespace PRG2_T13_08
{
    class LWTTFlight : Flight
    {
        public double RequestFee {  get; set; }

        public LWTTFlight():base() 
        {
            RequestFee = 500;
        }

        public LWTTFlight(double r, string fn, string o, string d, DateTime e, string s) : base(fn, o, d, e, s)
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
