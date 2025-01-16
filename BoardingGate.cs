using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_08
{
    class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }

        public BoardingGate() { }

        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT, Flight flight)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
            Flight = flight;
        }

        public double CalculateFees() // GateFee
        {
            if (Flight.Origin == "Singapore (SIN)") //Depart from SG
            {
                return  Flight.CalculateFees() + 300 + 800;
            }
            else //Arrive at SG
            {
                return Flight.CalculateFees() + 300 + 500;
            }
        }

        public override string ToString()
        {
            return "Gate Name: " + GateName + "\tSupports CFFT: " + SupportsCFFT + "\tSupports DDJB: " + SupportsDDJB + "\tSupports LWTT: " + SupportsLWTT + "\tFlight: " + Flight;
        }
    }
}
