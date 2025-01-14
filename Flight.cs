using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_08
{
    abstract class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        public Flight() { }
        public Flight(string fn, string o, string d, DateTime e, string s)
        {
            FlightNumber = fn;
            Origin = o;
            Destination = d;
            ExpectedTime = e;
            Status = s;
        }

        public abstract double CalculateFees();

        public override string ToString()
        {
            return "FlightNo: " + FlightNumber + "\tOrigin: " + Origin + "\tDestination: " + Destination + "\tExpected Time: " + ExpectedTime + "\tStatus: " + Status;
        }
    }
}
