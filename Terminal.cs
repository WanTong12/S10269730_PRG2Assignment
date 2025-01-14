using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Airlines.Add(a.Code, a);
            return true;
        }

        public bool AddBoardingGate(BoardingGate b)
        {
            BoardingGates.Add(b.GateName, b);
            return true;
        }

        public Airline GetAirlineFromFlight(Flight f)
        {
            return;
        }

        public void PrintAirlineFees()
        {

        }

        public  
    }
}
