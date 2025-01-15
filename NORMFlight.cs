namespace PRG2_T13_08
{
    class NORMFlight : Flight
    {
        public NORMFlight() : base() { }
        public NORMFlight(string fn, string o, string d, DateTime e, string s) : base(fn, o, d, e, s) { }

        public override double CalculateFees()
        {
            double originalFee = 0;
            double discount = 0;
            if (Origin == "Singapore SIN") //Depart from SG
            {
                originalFee = 800; ;
            }
            else if (Destination == "Singapore SIN")  //Arrive at SG
            {
                originalFee = 500;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
