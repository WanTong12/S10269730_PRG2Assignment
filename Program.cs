
using PRG2_T13_08;
internal class Program
{
    static Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();
    static Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();
    static Dictionary<string, BoardingGate> boardingGateDict = new Dictionary<string, BoardingGate>();

    private static void Main(string[] args)
    {
        // Load Flight File 
        LoadFlightFiles("flights.csv");
        LoadAirlines("airlines.csv");
        LoadBoardingGates("boardinggates.csv");

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
                Console.WriteLine();
            }
            else if (option == 2) // List Boarding Gates
            {

            }
            else if (option == 3) // Assign a Boarding Gate to a Flight
            {

            }
            else if (option == 4) // Create Flight
            {

            }
            else if (option == 5) // Display Airline Flights
            {

            }
            else if (option == 6) // Modify Flight Details
            {

            }
            else if (option == 7) // Display Flight Schedule
            {

            }
            else if (option == 0) // Exit
            {
                Console.WriteLine("Goodbye!");
                break;
            }
            else // Invalid input
            {
                Console.WriteLine("Invalid Input");
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
            BoardingGate boardingGate = new BoardingGate(line[0], cfft, ddjb, lwtt,);// create boardinggate object
            boardingGateDict.Add(boardingGate.GateName,boardingGate); // add boardinggate object to boardinggate dictionary
        }
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

            Flight f = new Flight(fn, o, d, e, "On Time"); //Create Flight object

            flightDict.Add(fn, f); //Add object to flight dictionary
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

            Console.WriteLine("{0, -17}{1,-23}{2,-25}{3,-25}{4}",f.FlightNumber, airlineName, f.Origin,f.Destination, expectedTime); // flight information
        }
    }
}
