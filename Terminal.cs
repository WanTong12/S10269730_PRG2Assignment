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
    internal class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }

        public Dictionary<string, BoardingGate> BoardingGates { get; set; }

        public Dictionary<string, double> GateFees { get; set; }

        public Terminal()
        {
            Airlines = new Dictionary<string, Airline>();
            Flights = new Dictionary<string, Flight>();
            BoardingGates = new Dictionary<string, BoardingGate>();
            GateFees = new Dictionary<string, double>();
        }

        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
        }

        public bool AddAirline(Airline a)
        {
            if (Airlines.ContainsKey(a.Code))
            {
                return false;
            }
            Airlines.Add(a.Code, a);
            return true;
        }

        public bool AddBoardingGate(BoardingGate b)
        {
            if (BoardingGates.ContainsKey(b.GateName))
            {
                return false;
            }
            BoardingGates.Add(b.GateName, b);
            return true;
        }

        public Airline GetAirlineFromFlight(Flight f)
        {
            foreach (Airline a in Airlines.Values) // loop airline values to retrieve the flights dictionary from the Airline class
            {
                if (a.Flights.ContainsKey(f.FlightNumber)) // if the flight number is found in the Airline class's flights dictionary, return the airline
                {
                    return a;
                }
            }
            return null; // return null if the flight number is not found in the Airline class's flights dictionary
        }

        public void PrintAirlineFees()
        {
            Console.WriteLine("{0,-15}{1.-15}","Airlines", "Fees");
            foreach (Airline a in Airlines.Values)
            {
                Console.WriteLine("{0,-15}{1,-15:C2}",a.Name, a.CalculateFee());
            }
        }

        public override string ToString()
        {
            return "Terminal Name: " + TerminalName;
        }
    }
}
