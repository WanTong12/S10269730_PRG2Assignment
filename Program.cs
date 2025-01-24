﻿
using Microsoft.VisualBasic;
using PRG2_T13_08;
using System.Diagnostics.Metrics;

//==========================================================
// Student Number : S10267930
// Student Name : Chong Wan Tong
// Partner Name : Teo Qi En
//==========================================================
internal class Program
{
    static Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();
    static Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();
    static Dictionary<string, BoardingGate> boardingGateDict = new Dictionary<string, BoardingGate>();

    private static void Main(string[] args)
    {
        // Load Airline File
        LoadAirlines("airlines.csv");
        // Load BoardingGate File
        LoadBoardingGates("boardinggates.csv");
        // Load Flight File 
        LoadFlightFiles("flights.csv");

        Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");

        while (true)
        {
            DisplayMenu();
            Console.Write("Please select your option: ");
            int option = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            if (option == 1) // List All Flights
            {
                DisplayBasicFlightInfo();
                Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
            }
            else if (option == 2) // Display Boarding Gates
            {
                Console.WriteLine("=============================================");
                Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
                Console.WriteLine("=============================================");
                Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}","Gate Name","DDJB","CFFT","LWTT");
                foreach(BoardingGate b in boardingGateDict.Values)
                {
                    Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}", b.GateName, b.SupportsDDJB, b.SupportsCFFT, b.SupportsLWTT);
                }
                Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
            }
            else if (option == 3) // Assign a Boarding Gate to a Flight
            {
                AssignBoardingGate();
                Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
            }
            else if (option == 4) // Create Flight
            {
                while (true)
                {
                    CreateNewFlight("flights.csv");
                    //prompt the user asking if they would like to add another Flight, repeating the previous 5 steps if [Y] or continuing to the next step if [N]
                    Console.WriteLine("Would you like to add another flight? (Y/N)");
                    string? ans = Console.ReadLine();
                    if (ans.ToUpper() == "N")
                    {
                        break;
                    }
                }
                Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
            }
            else if (option == 5) // Display Airline Flights
            {
                // Display Airlines
                Console.WriteLine("=============================================");
                Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
                Console.WriteLine("=============================================");
                Console.WriteLine("{0,-16}{1,-20}","Airline Code", "Airline Name");
                foreach(Airline a in airlineDict.Values)
                {
                    Console.WriteLine("{0,-16}{1,-20}", a.Code, a.Name);
                }
                Console.Write("Enter Airline Code: "); // prompt user to input airline code
                string? airlineCode = Console.ReadLine();
                // Display Flights from the Airline that user input
                Console.WriteLine("=============================================");
                Console.WriteLine("List of Flights for {0}", airlineDict[airlineCode].Name); // using dictionary, get the airline name
                Console.WriteLine("=============================================");
                Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-32}", "Flight Number","Airline Name","Origin","Destination","Expected Departure/Arrival Time"); // display title
                
                foreach (Flight f in flightDict.Values) 
                {
                    if (f.FlightNumber.StartsWith(airlineCode)) // to select and display the flights from the airline input by user  
                    { 
                        Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-32}", f.FlightNumber, airlineDict[airlineCode].Name, f.Origin, f.Destination, f.ExpectedTime);

                    }
                }
                    
                
                Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
            }
            else if (option == 6) // Modify Flight Details
            {
                Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
            }
            else if (option == 7) // Display Flight Schedule
            {
                Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
            }
            else if (option == 0) // Exit
            {
                Console.WriteLine("Goodbye!");
                break;
            }
            else // Invalid input
            {
                Console.WriteLine("Invalid Input");
                Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
            }
        }
    }

    static void DisplayMenu() // Menu of options
    {
        Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n0. Exit");
    }


    static void LoadAirlines(string filename)
    {
        // read file
        string[] lines = File.ReadAllLines(filename);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(',');
            Airline airline = new Airline(line[0], line[1]); // create airline object
            airlineDict.Add(airline.Code,airline); // add airline object to airline dictionary
        }
        Console.WriteLine("Loading Airlines...");
        Console.WriteLine("{0} Airlines Loaded!", airlineDict.Count);
    }
    

    static void LoadBoardingGates(string filename)
    {
        // read file
        string[] lines = File.ReadAllLines(filename);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(',');
            bool cfft = Convert.ToBoolean(line[1]);
            bool ddjb = Convert.ToBoolean(line[2]);
            bool lwtt = Convert.ToBoolean(line[3]);
            BoardingGate boardingGate = new BoardingGate(line[0], ddjb, cfft, lwtt, null);// create boardinggate object
            boardingGateDict.Add(boardingGate.GateName,boardingGate); // add boardinggate object to boardinggate dictionary
        }
         Console.WriteLine("Loading Boarding Gates...");
         Console.WriteLine("{0} Boarding Gates Loaded!",boardingGateDict.Count);
    }
    

    static void LoadFlightFiles(string file)
    {
        string[] flights = File.ReadAllLines(file);
        for (int i = 1; i < flights.Length; i++)
        {
            string[] flight = flights[i].Split(',');
            string fn = flight[0]; //flight number
            string o = flight[1]; //Origin
            string d = flight[2]; //Destination
            DateTime e = Convert.ToDateTime(flight[3]); //Expected arrival/ departure time

            Flight? f = null;

            if (flight.Length == 5)
            {
               f = new NORMFlight(fn, o, d, e, "On Time"); //Create NORMFlight object
            }
            else if (flight.Length == 6)
            {
                string? specialRC = flight[4];
                if (specialRC == "DDJB")
                {
                    f = new DDJBFlight(fn, o, d, e, "On Time"); //Create DDJBFlight object
                }
                else if (specialRC == "CFFT")
                {
                    f = new CFFTFlight(fn, o, d, e, "On Time"); //Create CFFTFlight object
                }
                else if (specialRC == "LWTT")
                {
                    f = new LWTTFlight(fn, o, d, e, "On Time"); //Create LWTTFlight object
                }
            }
            if (f != null)
            {
                flightDict.Add(fn, f); //Add object to flight dictionary
            }
        }

        Console.WriteLine("Loading Flights...");
        Console.WriteLine("{0} Flights Loaded!", flights.Length - 1);
    }

    static void DisplayBasicFlightInfo()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Flights for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("{0, -17}{1,-23}{2,-25}{3,-25}{4}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

        foreach (KeyValuePair<string, Flight> kvp in flightDict) // Get each flight from flightDict
        {
            Flight f = kvp.Value; // flight
            string[] flightno = f.FlightNumber.Split(" ");
            string? code = flightno[0]; // Airline code
            string airlineName = airlineDict[code].Name; // Airline Name
            string expectedTime = f.ExpectedTime.ToString(); // Date and time of expected time
            Console.WriteLine("{0, -17}{1,-23}{2,-25}{3,-25}{4}", f.FlightNumber, airlineName, f.Origin, f.Destination, expectedTime); // flight information

        }
    }

    static void AssignBoardingGate()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Assign a Boarding Gate to a Flight");
        Console.WriteLine("=============================================");
        while (true)
        {
            Console.Write("Enter Flight Number: ");
            string? flightNo = Console.ReadLine();
            Console.Write("Enter Boarding Gate Name: ");
            string? boardingGate = Console.ReadLine();

            Flight f = flightDict[flightNo];

            BoardingGate bg = boardingGateDict[boardingGate];

            if (bg.Flight is null)
            {
                Console.WriteLine("Flight Number: {0}", f.FlightNumber);
                Console.WriteLine("Origin: {0}", f.Origin);
                Console.WriteLine("Destination: {0}", f.Destination);
                Console.WriteLine("Expected Time: {0}", f.ExpectedTime);
                //Requet Code
                if (f is NORMFlight)
                {
                    Console.WriteLine("Special Request Code: None");
                }
                else if (f is DDJBFlight)
                {
                    Console.WriteLine("Special Request Code: DDJB");
                }
                else if (f is CFFTFlight)
                {
                    Console.WriteLine("Special Request Code: CFFT");
                }
                else if (f is LWTTFlight)
                {
                    Console.WriteLine("Special Request Code: LWTT");
                }


                Console.WriteLine("Boarding Gate Name: {0}", bg.GateName);
                Console.WriteLine("Supports DDJB: {0}\r\nSupports CFFT: {1}\r\nSupports LWTT: {2}", bg.SupportsDDJB, bg.SupportsCFFT, bg.SupportsLWTT);
                Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
                string? ans = Console.ReadLine();

                if (ans.ToUpper() == "Y")
                {
                    Console.WriteLine("1. Delayed\r\n2. Boarding\r\n3. On Time");
                    Console.Write("Please select the new status of the flight: ");
                    int opt = Convert.ToInt32(Console.ReadLine());

                    if (opt == 1)
                    {
                        f.Status = "Delayed";
                    }
                    else if (opt == 2)
                    {
                        f.Status = "Boarding";
                    }
                    else if (opt == 3)
                    {
                        f.Status = "On Time";
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input");
                    }
                }

                bg.Flight = f;

                Console.WriteLine("Flight {0} has been assigned to Boarding Gate {1}!", flightNo, bg.GateName);
                break;
            }
            else
            {
                Console.WriteLine("Boarding Gate has already been assigned ");
            }
        }
    }

    static void CreateNewFlight(string file)
    {
        // Prompt the user to enter the new Flight
        Console.Write("Enter Flight Number: ");
        string? fNo = Console.ReadLine(); // flight number
        Console.Write("Enter Origin: ");
        string? o = Console.ReadLine(); // origin
        Console.Write("Enter Destination: ");
        string? d = Console.ReadLine(); // destination
        Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
        DateTime eTime = Convert.ToDateTime(Console.ReadLine()); // expected arrival or departure timing
        //	prompt the user if they would like to enter any additional information, like the Special Request Code
        Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
        string? specialRC = Console.ReadLine(); // Speacial Request Code

        //create the proper Flight object with the information given
        Flight f = new Flight(fNo, o, d, eTime, "On Time");
        flightDict.Add(fNo, f); //Add object to flight dictionary

        //append the new Flight information to the flights.csv file

        if (specialRC == "None") //For flights without spreacial request code
        {
            string flightinfo = fNo + "," + o + "," + d + "," + eTime;
            File.AppendAllText(file, flightinfo); //Add flight into flights file
        }
        else // For flights with speacial request code
        {
            string flightinfo = fNo + "," + o + "," + d + "," + eTime + "," + specialRC;
            File.AppendAllText(file, flightinfo); //Add flight into flights file
        }
        //display a message to indicate that the Flight(s) have been successfully added
        Console.WriteLine("Flight {0} has been added!", fNo);
    }
}
