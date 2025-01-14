namespace PRG2_T13_08
{
    class NORMFlight : Flight
    {
        public NORMFlight() : base() { }
        public NORMFlight(string fn, string o, string d, DateTime e, string s) : base(fn, o, d, e, s) { }

        public override double CalculateFees()
        {
            double basefee = 300;
            double fee = 0;

            if (Origin == "Singapore (SIN)") // Departing Flights
            {
                fee = 800 + basefee - 50;
            }
            else // Arriving Flights
            {
                fee = 500 + basefee - 50;
            }

            if (ExpectedTime)

            if (Origin == "Dubai (DXB)" || Origin == "Bangkok (BKK)" || Origin == "Tokyo (NRT)") // Promotional Conditions 4
            {
                fee -= 25;
            }

            return fee;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
