
using Microsoft.VisualBasic;
using PRG2_T13_08;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Net.Sockets;

//==========================================================
// Student Number : S10267930
// Student Name : Chong Wan Tong
// Partner Name : Teo Qi En
//==========================================================
internal class Program
{

    static Terminal terminal5 = new Terminal();
    static Dictionary<string, Airline> airlineDict = terminal5.Airlines; // key: Airline code
    static Dictionary<string, Flight> flightDict = terminal5.Flights; // key: Flight Number
    static Dictionary<string, BoardingGate> boardingGateDict = terminal5.BoardingGates; // key: Gate Name
    // Flight to boarding gate dictionary
    static Dictionary<string, string> flightToBoardingGateDict = new Dictionary<string, string>(); // Key: flight Number, value: boarding gate name


    private static void Main(string[] args)
    {
        // Load Airline File
        LoadAirlines("airlines.csv");
        // Load BoardingGate File
        LoadBoardingGates("boardinggates.csv");
        // Load Flight File 
        LoadFlightFiles("flights.csv");

        Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");

        while (true) // Keeps looping until break
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
                DisplayBoardingGates();
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
                Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
            }
            else if (option == 6) // Modify Flight Details
            {
                Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
            }
            else if (option == 7) // Display Flight Schedule
            {
                DisplayScheduledFlights();
                Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
            }
            else if (option == 9) // Calculate fee per airline
            {
                CalculateFeesPerAirline();
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
        Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n9. Calculate Fees Per Airline\r\n0. Exit");
    }


    static void LoadAirlines(string filename)
    {
        // read file
        string[] lines = File.ReadAllLines(filename);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(',');
            Airline airline = new Airline(line[0], line[1]); // create airline object
            terminal5.AddAirline(airline); // add airline object to airline dictionary
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
            terminal5.AddBoardingGate(boardingGate); // add boardinggate object to boardinggate dictionary
        }
        Console.WriteLine("Loading Boarding Gates...");
        Console.WriteLine("{0} Boarding Gates Loaded!", boardingGateDict.Count);
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
            string srCode = flight[4]; // special request code
          
            Flight f = new Flight();
            if (srCode == "DDJB")
            {
                f = new DDJBFlight(fn, o, d, e, "On Time"); //Create DDJBFlight object
            }
            else if (srCode == "CFFT")
            {
                f = new CFFTFlight(fn, o, d, e, "On Time"); //Create CFFTFlight object
            }
            else if (srCode == "LWTT")
            {
                f = new LWTTFlight(fn, o, d, e, "On Time"); //Create LWTTFlight object
            }

            else // No special request code
            {
                f = new NORMFlight(fn, o, d, e, "On Time"); //Create NORMFlight object
            }

            flightDict.Add(fn, f); //Add object to flight dictionary

        }

        Console.WriteLine("Loading Flights...");
        Console.WriteLine("{0} Flights Loaded!", flightDict.Count); // -1 bcos of header
    }


    static void DisplayBasicFlightInfo() // Option 1
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

    static void DisplayBoardingGates() // Option 2
    {
        Console.WriteLine("============================================="); // Title
        Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-23}", "Gate Name", "DDJB", "CFFT", "LWTT", "Flight Number Assigned");
        foreach (BoardingGate bg in boardingGateDict.Values) // each iteration retrieves each BoardingGate object 
        {
            // displays the special request code each boarding gate service and the flight number assigned 
            if (bg.Flight == null) // if the boarding gate isn't assigned to a flight, it displays "Nil" under Flight Number Assigned
            {
                Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-23}", bg.GateName, bg.SupportsDDJB, bg.SupportsCFFT, bg.SupportsLWTT, "Nil");
            }
            else // if boarding gate is assigned to a flight, it displays the flight number under Flight Number Assigned
            { 
                Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-23}", bg.GateName, bg.SupportsDDJB, bg.SupportsCFFT, bg.SupportsLWTT, bg.Flight.FlightNumber); 
            }
            
        }
    }

    static void AssignBoardingGate() // Option 3
    {
        //Display Header
        Console.WriteLine("=============================================");
        Console.WriteLine("Assign a Boarding Gate to a Flight");
        Console.WriteLine("=============================================");

        while (true) // Will repeat code until break
        {
            Console.Write("Enter Flight Number: ");
            string? flightNo = Console.ReadLine();

            if (string.IsNullOrEmpty(flightNo))
            {
                Console.WriteLine("Invalid Input\r\n");
                continue; //Continue asking for valid input, don't exit the loop
            }
            else if (!flightDict.ContainsKey(flightNo))
            {
                Console.WriteLine("Flight does not exists\r\n");
                continue; // Prompt user again if flight doesn't exist
            }
            Console.Write("Enter Boarding Gate Name: ");
            string? gateName = Console.ReadLine();
            if (string.IsNullOrEmpty(gateName))
            {
                Console.WriteLine("Invalid Input\r\n");
                continue;
            }
            else if (!boardingGateDict.ContainsKey(gateName))
            {
                Console.WriteLine("Boarding Gate does not exists\r\n");
                continue;
            }

            Flight f = flightDict[flightNo];

            BoardingGate bg = boardingGateDict[gateName]; // get boarding gate object

            if (bg.Flight is null) // If no flight is assigned to the boarding gate
            {
                //Display basic flight info
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

                // Display boarding gate info
                Console.WriteLine("Boarding Gate Name: {0}", bg.GateName);
                Console.WriteLine("Supports DDJB: {0}\r\nSupports CFFT: {1}\r\nSupports LWTT: {2}", bg.SupportsDDJB, bg.SupportsCFFT, bg.SupportsLWTT);
                Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
                string? ans = Console.ReadLine();

                if (string.IsNullOrEmpty(ans) || ans.ToUpper() != "N" && ans.ToUpper() != "Y")
                {
                    Console.WriteLine("Invalid Input\r\n");
                    return;
                }
                else if (ans.ToUpper() == "Y") // update status of the flight
                {
                    Console.WriteLine("1. Delayed\r\n2. Boarding\r\n3. On Time");
                    Console.Write("Please select the new status of the flight: ");
                    try
                    {
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
                        else // none of the options choosen
                        {
                            Console.WriteLine("Invalid Input");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }
                }
            
                bg.Flight = f; // Assign flight to boarding gate
                flightToBoardingGateDict.Add(flightNo, gateName);

                Console.WriteLine("Flight {0} has been assigned to Boarding Gate {1}!", flightNo, bg.GateName);
                // Choose N and breakes out of loop
                break;
            }
            else // Already have flight assigned to boarding gate
            {
                Console.WriteLine("Boarding Gate has already been assigned ");
            }
        }
    }



    static void CreateNewFlight(string file) // Option 4
    {
        while (true)
        {
            // Prompt the user to enter the new Flight
            Console.Write("Enter Flight Number: ");
            string? fNo = Console.ReadLine(); // flight number
            if (string.IsNullOrEmpty(fNo))
            {
                Console.WriteLine("Invalid Input\r\n");
                continue;
            }
            Console.Write("Enter Origin: ");
            string? o = Console.ReadLine(); // origin
            if (string.IsNullOrEmpty(o))
            {
                Console.WriteLine("Invalid Input");
                continue;
            }
            Console.Write("Enter Destination: ");
            string? d = Console.ReadLine(); // destination
            if (string.IsNullOrEmpty(d))
            {
                Console.WriteLine("Invalid Input\r\n");
                continue;
            }    

            if (o == "Singapore (SIN)" || d == "Singapore (SIN)" && !flightDict.ContainsKey(fNo))
            {
                Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                DateTime eTime = Convert.ToDateTime(Console.ReadLine()); // expected arrival or departure timing
                //prompt the user if they would like to enter any additional information, like the Special Request Code
                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                string? specialRC = Console.ReadLine(); // Speacial Request Code
                if (string.IsNullOrEmpty(specialRC) || (specialRC != "CFFT" && specialRC != "DDJB" && specialRC != "LWTT" && specialRC != "None"))
                {
                    Console.WriteLine("Invalid Input\r\n");
                    continue;
                }
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
                break;
            }
            else if (flightDict.ContainsKey(fNo))
            {
                Console.WriteLine("Flight already exists\r\n");
                break;
            }
            else
            {
                Console.WriteLine("Invalid Input\r\n");
                break;
            }
        }
    }


    static void DisplayAirlineFlights() // Option 5
    {
        try
        {
            Console.WriteLine("============================================="); // Title 
            Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine("{0,-16}{1,-20}", "Airline Code", "Airline Name");
            foreach (Airline a in airlineDict.Values) // each iteration retrieves an Airline object
            {
                Console.WriteLine("{0,-16}{1,-20}", a.Code, a.Name); // dislplays the airline code and name 
            }
            string airlineCode;
            while (true)
            {
                // Data Validation
                Console.Write("Enter Airline Code: "); // prompt user to input airline code
                airlineCode = Console.ReadLine().ToUpper();
                if (!airlineDict.ContainsKey(airlineCode))
                {
                    Console.WriteLine("Invalid Airline Code. Please enter again.");
                }
                break;
            }
            Airline airline = airlineDict[airlineCode];
            // Display Flights from the Airline that user input
            Console.WriteLine("=============================================");
            Console.WriteLine("List of Flights for {0}", airline.Name); // retrieves the airline name using airline dictionary
            Console.WriteLine("=============================================");
            Console.WriteLine("{0,-16}{1,-23}{2,-23}", "Flight Number", "Origin", "Destination"); // display title

            foreach (Flight f in flightDict.Values) // each iteration retrieves a Flight object
            {
                if (f.FlightNumber.StartsWith(airlineCode)) //  to retrive the flights from the airline user input
                {
                    // displays the flight number, origin and destination for each flight from the airline user input
                    Console.WriteLine("{0,-16}{1,-23}{2,-23}", f.FlightNumber, f.Origin, f.Destination);

                }
            }

            string flightNo;
            while (true)
            {
                Console.Write("Enter Flight Number: "); // prompt user to select a flight number
                flightNo = Console.ReadLine(); // stores user's input into a variable named flightNo

                if (!flightDict.ContainsKey(flightNo))
                {
                    Console.WriteLine("Invalid Flight Number. Please enter again.");
                }
                break;
            }
            Flight flight = flightDict[flightNo];
            Console.WriteLine("============================================="); // title
            Console.WriteLine("Flight Details for {0}", flightNo);
            Console.WriteLine("=============================================");
            Console.WriteLine("{0,-16}{1,-20}{2,-20}{3,-20}{4,-35}{5,-23}{6,-20}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Special Request Code", "Boarding Gate");
            bool found = false;
            foreach (BoardingGate bg in boardingGateDict.Values) // loops through every iteration to check if a boarding gate is assigned to the flight user input
            {
                if (bg.Flight != null && bg.Flight.FlightNumber == flightNo)
                {    // display special request code of each flight and boarding gate assigned to each flight 
                    if (flight is NORMFlight)
                    {
                        Console.WriteLine("{0,-16}{1,-20}{2,-20}{3,-20}{4,-35}{5,-23}{6,-20}", flightNo, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime, "Nil", bg.GateName);
                    }
                    else if (flight is DDJBFlight)
                    {
                        Console.WriteLine("{0,-16}{1,-20}{2,-20}{3,-20}{4,-35}{5,-23}{6,-20}", flightNo, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime, "DDJB", bg.GateName);
                    }
                    else if (flight is CFFTFlight)
                    {
                        Console.WriteLine("{0,-16}{1,-20}{2,-20}{3,-20}{4,-35}{5,-23}{6,-20}", flightNo, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime, "CFFT", bg.GateName);
                    }
                    else if (flight is LWTTFlight)
                    {
                        Console.WriteLine("{0,-16}{1,-20}{2,-20}{3,-20}{4,-35}{5,-23}{6,-20}", flightNo, airlineDict[airlineCode].Name, flight.Origin, flight.Destination, flight.ExpectedTime, "LWTT", bg.GateName);
                    }
                    found = true;
                    break;
                }


            }
            // if found is false, it means that no boarding gate is assigned to the flight 
            if (!found)
            {
                // displays special request code and boarding gate of the flight user input
                if (flight is NORMFlight)
                {
                    Console.WriteLine("{0,-16}{1,-20}{2,-20}{3,-20}{4,-35}{5,-23}{6,-20}", flightNo, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime, "Nil", "Unassigned");
                }
                else if (flight is DDJBFlight)
                {
                    Console.WriteLine("{0,-16}{1,-20}{2,-20}{3,-20}{4,-35}{5,-23}{6,-20}", flightNo, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime, "DDJB", "Unassigned");
                }
                else if (flight is CFFTFlight)
                {
                    Console.WriteLine("{0,-16}{1,-20}{2,-20}{3,-20}{4,-35}{5,-23}{6,-20}", flightNo, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime, "CFFT", "Unassigned");
                }
                else if (flight is LWTTFlight)
                {
                    Console.WriteLine("{0,-16}{1,-20}{2,-20}{3,-20}{4,-35}{5,-23}{6,-20}", flightNo, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime, "LWTT", "Unassigned");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    

    static void DisplayScheduledFlights() // Option 7
    {
        // Create a list to sort flights by expected departure/arrival time 
        List<Flight> flightsList = new List<Flight>(flightDict.Values);
        flightsList.Sort(); // Sort using IComparable in flight class, earliest first
        //Display header
        Console.WriteLine("=============================================");
        Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        //Display title
        Console.WriteLine("{0,-16}{1,-25}{2,-20}{3,-25}{4,-37}{5,-20}{6}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Status", "Boarding Gate");
        // iterate through flights list 
        foreach (Flight f in flightsList)
        {
            string bg = "Unassigned"; // Default status for boarding gate
            if (flightToBoardingGateDict.ContainsKey(f.FlightNumber)) // check if flight has been assigned to a boarding gate
            {
                bg = flightToBoardingGateDict[f.FlightNumber];
            }
            string airlineCode = f.FlightNumber.Split(' ')[0]; 
            string airlineName = airlineDict[airlineCode].Name;
            // Display scheduled flight details
            Console.WriteLine("{0,-16}{1,-25}{2,-20}{3,-25}{4,-37}{5,-20}{6}", f.FlightNumber, airlineName, f.Origin, f.Destination, f.ExpectedTime, f.Status, bg);
        }
    }

    static void CalculateFeesPerAirline() // Optionn 9
    {
        foreach (Flight f in flightDict.Values)
        {
            // Check if each flight has been assigned to a boarding gate
            if (!flightToBoardingGateDict.ContainsKey(f.FlightNumber)) // Not all flights has been assigned to a boarding gate
            {
                Console.WriteLine("Ensure that all flights has been assigned to a boarding gate");
                return;
            }
        }

        foreach (Airline a in airlineDict.Values)
        {
            double discount = 0;
            // Calculate discounts
            if (flightDict.Count / 3 >= 1) // For every 3 flights
            {
                discount += (350 * Math.Floor(flightDict.Count / 3.0));
            }
            if (flightDict.Count > 5) // For more than 5 flights
            {
                discount += a.CalculateFees() * 0.3;
            }
            foreach (Flight f in flightDict.Values)
            {
               
                if (f.ExpectedTime.Hour < 11 && f.ExpectedTime.Hour > 21) // For flights arriving/departing before 11am or after 9pm
                {
                    discount += 110;
                }
                if (f.Origin == "Dubai (DXB)" || f.Origin == "Bangkok (BKK)" || f.Origin == "Tokyo (NRT)") // For airlines with the Origin of Dubai (DXB), Bangkok (BKK) or Tokyo (NRT)
                {
                    discount += 25;
                }
                if (f is NORMFlight) // For no request fee
                {
                    discount += 50;
                }
            }
            // Display fees
            double finalFee = a.CalculateFees() - discount;
            double percentage = (discount / finalFee) * 100;
            Console.WriteLine("Airline: {0}", a.Name); //Print airline name
            Console.WriteLine("Subtotal: {0}", a.CalculateFees()); // Subtotal of all the fees to be charged
            Console.WriteLine("Discount to be deducted: {0}", discount); // Total discount
            Console.WriteLine("Final Fee: {0}", finalFee); // FInal fee
            Console.WriteLine("Percentage of the subtotal discounts: {0}", percentage); // Percentage of the subtotal discounts over the final total of fees
        }
    }
}

