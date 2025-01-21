
//==========================================================
// Student Number : S10267930
// Student Name : Chong Wan Tong
// Partner Name : Teo Qi En
//==========================================================

namespace PRG2_T13_08
{
    class NORMFlight : Flight
    {
        public NORMFlight() : base() { }
        public NORMFlight(string fn, string o, string d, DateTime e, string s) : base(fn, o, d, e, s) { }

        public override double CalculateFees()
        {
            if (Origin == "Singapore (SIN)") //Depart from SG
            {
                return 800 + 300; // + basefee
            }
            else //Arrive at SG
            {
                return 500 + 300; // + basefee
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
