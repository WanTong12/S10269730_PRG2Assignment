
using PRG2_T13_08;
internal class Program
{
    static Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();
    static Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();
    static Dictionary<string, BoardingGate> boardingggateDict = new Dictionary<string, BoardingGate>();

    private static void Main(string[] args)
    {
        // Load Flight File
        LoadFlightFiles();

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
}