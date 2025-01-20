
using PRG2_T13_08;
internal class Program
{
    static Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();
    static Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();
    static Dictionary<string, BoardingGate> boardingggateDict = new Dictionary<string, BoardingGate>();

    private static void Main(string[] args)
    {
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

    static void DisplayMenu()
    {
        Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n0. Exit");
    }

    static void LoadFlightFiles(string file)
    {
        string[] flights = File.ReadAllLines(file);
        for (int i = 1; i < flights.Length; i++)
        {
            string[] flight = flights[i].Split(',');
            string fn = flight[0];
            string o = flight[1];
            string d = flight[2];
            DateTime e = Convert.ToDateTime(flight[3]);

            Flight f = new Flight(fn, o, d, e, "On Time");

            flightDict.Add(fn, f);
        }

        Console.WriteLine("Loading Flights...");
        Console.WriteLine("{0} Flights Loaded!",flights.Length-1);
    }
}