﻿
using Microsoft.VisualBasic;
using PRG2_T13_08;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Net.Sockets;
using System.Xml.Linq;

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
    static Queue<Flight> flightQueue = new Queue<Flight>();

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
            try
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
                    while (true) // Repeat if ans is Y
                    {
                        CreateNewFlight("flights.csv");
                        //prompt the user asking if they would like to add another Flight, repeating the previous 5 steps if [Y] or continuing to the next step if [N]
                        Console.WriteLine("Would you like to add another flight? (Y/N)");
                        string? ans = Console.ReadLine();
                        if (string.IsNullOrEmpty(ans)) // Check if ans is empty or null
                        {
                            Console.WriteLine("Invalid Input");
                            break;
                        }
                        else if (ans.ToUpper() == "N") // If ans is "N"
                        {
                            break; 
                        }
                        else
                        {
                            Console.WriteLine("Invalid Input");
                            break; 
                        }
                    }
                    Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
                }
                else if (option == 5) // Display Airline Flights
                {
                    DisplayAirlineFlights();
                    Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
                }
                else if (option == 6) // Modify Flight Details
                {
                    ModifyFlightDetails();
                    Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
                }
                else if (option == 7) // Display Flight Schedule
                {
                    DisplayScheduledFlights();
                    Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
                }
                else if (option == 8)
                {
                    ProcessUnassignedFlights();
                    Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
                }
                else if (option == 9) // Calculate the fees for each airline
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
                    Console.WriteLine("Invalid Input. Please enter again.");
                    Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    static void DisplayMenu() // Menu of options
    {
        Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n8. Process all unassigned flights to boarding gates in bulk\r\n9. Calculate Fees Per Airline\r\n0. Exit");
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
        string[] flights = File.ReadAllLines(file); // Get lines from flight file
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

            flightDict.Add(fn, f); //Add object to terminal 5flight dictionary

            string acode = fn.Split(" ")[0]; // Get airline code
            Airline a = airlineDict[acode]; // Airline object
            a.AddFlight(f); // Add flight into airline object's flight dictionay
        }
        //Loaded Msg
        Console.WriteLine("Loading Flights...");
        Console.WriteLine("{0} Flights Loaded!", flightDict.Count); 
    }


    static void DisplayBasicFlightInfo() // Option 1
    {
        //Display Header
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Flights for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("{0, -17}{1,-23}{2,-25}{3,-25}{4}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

        foreach (KeyValuePair<string, Flight> kvp in flightDict) // Get each flight from flightDict
        {
            Flight f = kvp.Value; // flight
            Airline a = terminal5.GetAirlineFromFlight(f);// Get airline object
            string airlineName = a.Name; // Airline Name
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
                Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-23}", bg.GateName, bg.SupportsDDJB, bg.SupportsCFFT, bg.SupportsLWTT, "Unassigned");
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
            Console.Write("Enter Flight Number: "); // Prompt for flight number
            string? flightNo = Console.ReadLine(); // Flight Number

            if (string.IsNullOrEmpty(flightNo)) // If flight Number is empty or null
            {
                Console.WriteLine("Invalid Input\r\n");
                continue; //Continue asking for valid input, don't exit the loop
            }
            else if (!flightDict.ContainsKey(flightNo)) // Check if flight exsits
            {
                Console.WriteLine("Flight does not exists\r\n");
                continue; // Prompt user again if flight doesn't exist
            }

            Console.Write("Enter Boarding Gate Name: "); // Prompt for Boarding Gate Name
            string? gateName = Console.ReadLine(); // Boarding Gate Name
            if (string.IsNullOrEmpty(gateName)) // If gate name is empty or null
            {
                Console.WriteLine("Invalid Input\r\n");
                continue; // Continue asking for valid inout, don't exit the loop
            }
            else if (!boardingGateDict.ContainsKey(gateName)) // CHeck if boarding gate exists
            {
                Console.WriteLine("Boarding Gate does not exists\r\n");
                continue; // Prompt again if boarding gate does not exists
            }
            

            Flight f = flightDict[flightNo]; // Get Flight object

            BoardingGate bg = boardingGateDict[gateName]; // Get boarding gate object

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
                while (true)
                { 
                    Console.WriteLine("Would you like to update the status of the flight? (Y/N)"); // Prompt user if want to update status
                    string? ans = Console.ReadLine(); // User option

                    if (string.IsNullOrEmpty(ans) || ans.ToUpper() != "N" && ans.ToUpper() != "Y") // Check for valid inot
                    {
                        Console.WriteLine("Invalid Input\r\n");
                        continue; // Continue to prompt user for valid input
                    }
                    else if (ans.ToUpper() == "Y") // update status of the flight
                    {
                        Console.WriteLine("1. Delayed\r\n2. Boarding\r\n3. On Time"); // Display status options
                        Console.Write("Please select the new status of the flight: "); // Prompt user for new status
                        try
                        {
                            int opt = Convert.ToInt32(Console.ReadLine()); // User choice

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
                                continue; // Continue asking for valid input, don't exit the loop
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message); 
                        }
                    }   
                    break;
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
            if (string.IsNullOrEmpty(fNo)) // CHeck if flight numbr is null or empty
            {
                Console.WriteLine("Invalid Input\r\n");
                continue; //Continue asking for valid input, don't exit the loop
            }
            Console.Write("Enter Origin: "); // prompt for origin
            string? o = Console.ReadLine(); // origin
            if (string.IsNullOrEmpty(o)) // Check if origin is null or empty
            {
                Console.WriteLine("Invalid Input");
                continue; //Continue asking for valid input, don't exit the loop
            }
            Console.Write("Enter Destination: "); // Prompt for destination
            string? d = Console.ReadLine(); // destination
            if (string.IsNullOrEmpty(d)) // Check if destination is null or empty
            {
                Console.WriteLine("Invalid Input\r\n");
                continue; //Continue asking for valid input, don't exit the loop
            }

            if (o == "Singapore (SIN)" || d == "Singapore (SIN)" && !flightDict.ContainsKey(fNo)) // Check if either origin or destination is SG and flight is not in flight dictionary
            {
                Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): "); // Prmopt for expected arrival / departure time
                DateTime eTime = Convert.ToDateTime(Console.ReadLine()); // expected arrival or departure timing
                //prompt the user if they would like to enter any additional information, like the Special Request Code
                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                string? specialRC = Console.ReadLine(); // Speacial Request Code
                if (string.IsNullOrEmpty(specialRC) || (specialRC != "CFFT" && specialRC != "DDJB" && specialRC != "LWTT" && specialRC != "None")) // user input is not valid
                {
                    Console.WriteLine("Invalid Input\r\n");
                    continue; //Continue asking for valid input, don't exit the loop
                }
                //create the proper Flight object with the information given
                Flight f = new Flight(fNo, o, d, eTime, "On Time");
                if (specialRC == "DDJB")
                {
                    f = new DDJBFlight(fNo, o, d, eTime, "On Time"); //Create DDJBFlight object
                }
                else if (specialRC == "CFFT")
                {
                    f = new CFFTFlight(fNo, o, d, eTime, "On Time"); //Create CFFTFlight object
                }
                else if (specialRC == "LWTT")
                {
                    f = new LWTTFlight(fNo, o, d, eTime, "On Time"); //Create LWTTFlight object
                }

                else // No special request code
                {
                    f = new NORMFlight(fNo, o, d, eTime, "On Time"); //Create NORMFlight object
                }
                flightDict.Add(fNo, f); //Add object to terminal's flight dictionary
                string acode = fNo.Split(" ")[0]; // Get airline code
                Airline a = airlineDict[acode]; // Airline object
                a.AddFlight(f); // Add flight into airline object's flight dictionay

                //append the new Fligght information to the flights.csv file
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
            else if (flightDict.ContainsKey(fNo)) // Flight already exists
            {
                Console.WriteLine("Flight already exists\r\n");
                break; // break when fligt already exists
            }
            else
            {
                Console.WriteLine("Invalid Input\r\n");
                break; // Break if invalid input
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
           
           string gName = "Unassigned";
           foreach (BoardingGate bg in boardingGateDict.Values) // loops through every iteration to check if a boarding gate is assigned to the flight user input
           {
               if (bg.Flight != null && bg.Flight.FlightNumber == flightNo)
               {
                    gName = bg.GateName;
                    break;
               }

           }
           // display special request code of each flight and boarding gate assigned to each flight 
           if (flight is NORMFlight)
           {
               Console.WriteLine("{0,-16}{1,-20}{2,-20}{3,-20}{4,-35}{5,-23}{6,-20}", flightNo, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime, "None", gName);
           }
           else if (flight is DDJBFlight)
           {
               Console.WriteLine("{0,-16}{1,-20}{2,-20}{3,-20}{4,-35}{5,-23}{6,-20}", flightNo, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime, "DDJB", gName);
           }
           else if (flight is CFFTFlight)
           {
               Console.WriteLine("{0,-16}{1,-20}{2,-20}{3,-20}{4,-35}{5,-23}{6,-20}", flightNo, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime, "CFFT", gName);
           }
           else if (flight is LWTTFlight)
           {
               Console.WriteLine("{0,-16}{1,-20}{2,-20}{3,-20}{4,-35}{5,-23}{6,-20}", flightNo, airlineDict[airlineCode].Name, flight.Origin, flight.Destination, flight.ExpectedTime, "LWTT", gName);
           }

       }

       catch (Exception ex)
       {
           Console.WriteLine(ex.Message);
       }
   }

    
    static void ModifyFlightDetails() // Option 6
    {
        try
        {
            // Display Airlines available
            Console.WriteLine("============================================="); // Title 
            Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine("{0,-16}{1,-20}", "Airline Code", "Airline Name");
            foreach (Airline a in airlineDict.Values) // each iteration retrieves an Airline object
            {
                Console.WriteLine("{0,-16}{1,-20}", a.Code, a.Name); // dislplays the airline code and name 
            }

            string airlineCode; // stores user input in variable named airlineCode
            while (true) // data validation
            {
                Console.WriteLine("Enter Airline Code:"); // prompt user input
                airlineCode = Console.ReadLine().ToUpper();
                if (!airlineDict.ContainsKey(airlineCode)) // if airline code is not in the airlineDict
                {
                    Console.WriteLine("Invalid Airline Number. Please enter again.");
                    continue; // keeps asking for user input when the input is invalid
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
            string flightNo; // stores user input into variable named flightNo
            int option; // stores user input into variable named option
            while (true) // data validation
            {
                Console.WriteLine("Choose an existing Flight to modify or delete: "); // prompt user input
                flightNo = Console.ReadLine();
                if (!flightDict.ContainsKey(flightNo)) // if flightNo is not in flightDIct
                {
                    Console.WriteLine("Invalid Flight Number. Please enter again.");
                    continue; // keeps asking for user input when the input is invalid
                }
                break;
            }
            while (true)
            {
                Console.WriteLine("1. Modifly Flight");
                Console.WriteLine("2. Delete Flight");
                Console.WriteLine("Choose an option: "); // prompt user to choose an option
                option = Convert.ToInt32(Console.ReadLine());
                if (option != 1 && option != 2) // if option is not 1 or 2
                {
                    Console.WriteLine("Invalid option. Please enter again.");
                    continue; // keeps asking for user input when the input is invalid
                }
                break;
            }

            if (option == 1) // if user chooses to modify a flight
            {
                int option2;
                while (true) // data validation
                {
                    Console.WriteLine("1. Modify Basic Information");
                    Console.WriteLine("2. Modify Status");
                    Console.WriteLine("3. Modify Special Request Code");
                    Console.WriteLine("4. Modify Boarding Gate");
                    Console.WriteLine("Choose an option: ");
                    option2 = Convert.ToInt32(Console.ReadLine());
                    if (option2 < 1 && option2 > 4) // if user input is not 1,2,3 or 4
                    {
                        Console.WriteLine("Invalid option. Please enter again.");
                    }
                    break;
                }


                if (option2 == 1) // if user chooses to modify basic information
                {
                    string origin;
                    string destination;

                    while (true)
                    {
                        Console.Write("Enter new Origin: ");
                        origin = Console.ReadLine();
                        Console.Write("Enter new Destination: ");
                        destination = Console.ReadLine();
                        if (origin != "Singapore (SIN)" && destination != "Singapore (SIN)")
                        {
                            Console.WriteLine("Singapore (SIN) has to be either the Origin or the Destination. Please enter again.");
                            continue; // keeps asking for user input until user inputs valid data
                        }
                        break;
                    }
                    Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                    DateTime expectedTime = Convert.ToDateTime(Console.ReadLine());
                    // Updating the flight's basic information
                    flightDict[flightNo].Origin = origin;
                    flightDict[flightNo].Destination = destination;
                    flightDict[flightNo].ExpectedTime = expectedTime;

                }

                else if (option2 == 2) // if user chooses to modify the status of the flight
                {
                    string status;
                    while (true)
                    {

                        Console.Write("Enter new Status: ");
                        status = Console.ReadLine();
                        if (status != "On Time" && status != "Delayed" && status != "Boarding")
                        {
                            Console.WriteLine("Invalid Status. Please enter again.");
                            continue;
                        }
                        break;
                    }
                    flightDict[flightNo].Status = status; // updating the flight's status
                }
                else if (option2 == 3) // if user chooses to modify the special request code of the flight
                {
                    string srCode;
                    while (true)
                    {
                        Console.Write("Enter a new Special Request Code: ");
                        srCode = Console.ReadLine().ToUpper();
                        if (srCode != "CFFT" && srCode != "LWTT" && srCode != "DDJB")
                        {
                            Console.WriteLine("Invalid Special Request Code. Please enter again.");
                            continue;
                        }
                        break;
                    }

                    Flight flight = flightDict[flightNo];
                    // Converting the original Flight subclass object to another Flight subclass object
                    if (srCode.ToUpper() == "DDJB")
                    {
                        // create a new DDJBFlight object and replace the original object with the new one into flightDict
                        DDJBFlight updated = new DDJBFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime, flight.Status);
                        flightDict[flightNo] = updated;

                    }
                    else if (srCode.ToUpper() == "CFFT")
                    {
                        // create a new CFFTFlight object and replace the original object with the new one into flightDict
                        CFFTFlight updated = new CFFTFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime, flight.Status);
                        flightDict[flightNo] = updated;
                    }
                    else if (srCode.ToUpper() == "LWTT")
                    {
                        // create a new LWTTFlight object and replace the original object with the new one into flightDict
                        LWTTFlight updated = new LWTTFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime, flight.Status);
                        flightDict[flightNo] = updated;
                    }

                }
                else if (option2 == 4) // if user chooses to modify the boarding gate of the flight
                {
                    string bg;
                    while (true) // data validation
                    {
                        Console.Write("Enter a new Boarding Gate of the Flight: ");
                        bg = Console.ReadLine();
                        if (!boardingGateDict.ContainsKey(bg))
                        {
                            Console.WriteLine("Invalid Boarding Gate. Please enter again.");
                            continue;
                        }
                        break;
                    }

                    foreach (BoardingGate b in boardingGateDict.Values)
                    {
                        if (b.Flight is not null && b.Flight.FlightNumber == flightNo)
                        {
                            b.Flight = null; // unassign previous boarding gate to the flight
                            boardingGateDict[bg].Flight = flightDict[flightNo]; // assign the new boarding gate to the flight
                        }

                    }

                }
                // displaying updated information
                Flight f = flightDict[flightNo];
                Console.WriteLine("Flight updated");
                Console.WriteLine("Flight Number: {0}", flightNo);
                Console.WriteLine("Airline Name: {0}", airlineDict[airlineCode].Name);
                Console.WriteLine("Origin: {0}", flightDict[flightNo].Origin);
                Console.WriteLine("Destination: {0}", flightDict[flightNo].Destination);
                Console.WriteLine("Expected Departure/Arrival Time: {0}", flightDict[flightNo].ExpectedTime);
                Console.WriteLine("Status: {0}", flightDict[flightNo].Status);
                // display the special request code according to which Flight object they belong to
                if (f is NORMFlight) { Console.WriteLine("Special Request Code: None"); }
                else if (f is CFFTFlight) { Console.WriteLine("Special Request Code: CFFT"); }
                else if (f is DDJBFlight) { Console.WriteLine("Special Request Code: DDJB"); }
                else if (f is LWTTFlight) { Console.WriteLine("Special Request Code: LWTT"); }

                string gName = "Unassigned";
                foreach (BoardingGate b in boardingGateDict.Values) // loops through all the values in boardingGateDict to retrieve and display the updated boarding gate
                {
                    if (b.Flight != null && b.Flight.FlightNumber == flightNo)
                    {
                        gName = b.GateName;
                        // displays the updated boarding gate if the boarding gate is assigned to a flight and if the flight number is the same as the user input
                        Console.WriteLine("Boarding Gate: {0}", gName);
                        break;
                    }
                }
                // if no boarding gate is assigned to the flight display this message
                Console.WriteLine("Boarding Gate: {0}",gName); 

            }
            else if (option == 2) // if user chooses to delete a flight
            {
                Console.WriteLine("Are you sure you want to delete {0}? (Y/N)", flightNo);
                string? confirmation = Console.ReadLine();
                if (confirmation.ToUpper() == "Y")
                {
                    flightDict.Remove(flightNo); // removing flight from flight dictionary 
                    
                    foreach (KeyValuePair<string, string> kvp in flightToBoardingGateDict) // remove flight from the boarding gate dict's value
                    {
                        if (kvp.Key == flightNo)
                        {
                            string gName = kvp.Value; // to retrieve which boarding gate was assigned to the flight
                            boardingGateDict[gName].Flight = null; // to remove the flight object from the previously assigned boarding gate 
                            flightToBoardingGateDict.Remove(kvp.Key); // to remove the previously assigned boarding gate and flight from the flightToBoardingGateDict
                        }
                    }
                    
                    Console.WriteLine("Flight deleted successfully!");
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
                bg = flightToBoardingGateDict[f.FlightNumber]; // Boarding gate where flight is assigned to
            }
            Airline a = terminal5.GetAirlineFromFlight(f); // Get airline object
            string airlineName = a.Name; // Airline Name
            // Display scheduled flight details
            Console.WriteLine("{0,-16}{1,-25}{2,-20}{3,-25}{4,-37}{5,-20}{6}", f.FlightNumber, airlineName, f.Origin, f.Destination, f.ExpectedTime, f.Status, bg);
        }
    }

    static void ProcessUnassignedFlights() // Option 8
    {
        foreach (Flight flight in flightDict.Values)
        {
            bool hasGate = false;

            // check if the flight has an assigned boarding gate
            foreach (BoardingGate bg in boardingGateDict.Values)
            {
                if (bg.Flight != null && bg.Flight.FlightNumber == flight.FlightNumber)
                {
                    hasGate = true;
                    break;
                }
                
            }

            // if no gate is found, add flight to queue
            if (hasGate == false)
            {
                flightQueue.Enqueue(flight);
            }
        }

        /* foreach (Flight f in flightQueue)
        {
            flightQueue.Dequeue();
            if (f is NORMFlight)
            {

            }
        }*/

        Console.WriteLine("Total number of Flights that do not have any Boarding Gate assigned yet: {0}",flightQueue.Count);
        int num = 0; // to count the number of unassigned boarding gates
        foreach (BoardingGate bg in boardingGateDict.Values)
        {
            if (bg.Flight == null)
            {
                num++; // if boarding gate is unassigned add 1 to the variable num
            }
        }
        Console.WriteLine("Total number of Boarding Gates that do not have a Flight Number assigned yet: {0}",num);
    }
    static void CalculateFeesPerAirline() // Option 9
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
        double terminalSubTotal = 0;
        double terminalDiscount = 0;
        foreach (Airline a in airlineDict.Values)
        {
            double discount = 0;
            double totalfee = a.CalculateFees();
            // Calculate discounts
            foreach (Flight f in a.Flights.Values)
            {
                BoardingGate b = boardingGateDict[flightToBoardingGateDict[f.FlightNumber]]; // Get boarding gate object so can use CalculateFees() from BoardingGate Class
                totalfee += b.CalculateFees(); // Add basefee for each flight (300)
                
                if (f.ExpectedTime.Hour < 11 || f.ExpectedTime.Hour > 21) // For flights arriving/departing before 11am or after 9pm
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

            if (a.Flights.Count / 3 >= 1) // For every 3 flights
            {
                discount += (350 * Math.Floor(a.Flights.Count / 3.0));
            }
            if (a.Flights.Count > 5) // For more than 5 flights
            {
                discount += totalfee * 0.3;
            }
            double finalFee = totalfee - discount;

            string airlineName = a.Name;
            // Display fees for each airline
            Console.WriteLine("Airline: {0}", airlineName);
            Console.WriteLine("Subtotal: {0:C2}", totalfee);
            Console.WriteLine("Discount: {0:C2}", discount);
            Console.WriteLine("Final Fee: {0:F2}", finalFee);
            Console.WriteLine();
            
            // Terminal
            terminalSubTotal += totalfee;
            terminalDiscount += discount;
        }
        double terminalFinal = terminalSubTotal - terminalDiscount;
        double percentage = 0;
        if (terminalFinal > 0)
        {
            percentage = terminalDiscount / terminalFinal * 100;
        }
        terminal5.PrintAirlineFees(terminalSubTotal,terminalDiscount,terminalFinal,percentage);
    }

}

