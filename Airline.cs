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
            if (Flights.ContainsKey(f.FlightNumber))
            {
                return false;
            }
           
            Flights.Add(f.FlightNumber, f);
            return true;
        }

        public double CalculateFee() // Airlines total flight fee
        {
            double fees = 0;
            foreach (KeyValuePair<string, Flight> kvp in Flights) 
            {
                Flight f = kvp.Value; // Each flight
                fees += f.CalculateFees(); // Add all the flight fees together
            }
            
           
            return fees;
        }

        public bool RemoveFlight(Flight f)
        {
            if (Flights.ContainsKey(f.FlightNumber))
            {
                Flights.Remove(f.FlightNumber);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return "Name: " + Name + "\tCode: " + Code;
        }
    }
}
