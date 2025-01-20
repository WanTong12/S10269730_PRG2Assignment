using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_08
{
    class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        public Flight() { } 
        public Flight(string fn, string o, string d, DateTime e, String s)
        {
            FlightNumber = fn;
            Origin = o;
            Destination = d;
            ExpectedTime = e;
            Status = s;
        }

        public double CalculateFees()
        {
            if(Origin == "Singapore (SIN)") //Depart from SG
            {
                return 800;
            }
            else //Arrive at SG
            {
                return 500;
            }
        }

        public override string ToString()
        {
            return "FlightNo: " + FlightNumber + "\tOrigin: " + Origin + "\tDestination: " + Destination + "\tExpected Time: " + ExpectedTime + "\tStatus: " + Status;
        }
    }
}
