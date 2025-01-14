using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_08
{
    class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();

        public Airline() { }

        public Airline(string n, string c, Dictionary<string, Flight> f)
        {
            Name = n;
            Code = c;
            Flights = f;
        }

        public bool AddFlight(Flight f)
        {
            return;
        }

        public double CalculateFee()
        {
            return;
        }

        public bool RemoveFlight(Flight f)
        {
            return;
        }

        public override string ToString()
        {
            return "Name: " + Name + "\tCode: " + Code;
        }
    }
}
