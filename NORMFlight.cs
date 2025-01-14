using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_08
{
    class NORMFlight : Flight
    {
        public NORMFlight():base() { }
        public NORMFlight(string fn, string o, string d, DateTime e, string s) : base(fn, o, d, e, s) { }

        public override double CalculateFees()
        {
            double basefee = 300;
            if (Origin == "Singapore")
            {
                return 800 + basefee;
            }
            else
            {
                return 500 + basefee;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
