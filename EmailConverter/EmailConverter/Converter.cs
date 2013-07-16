using System;
using System.IO;
using System.Collections;

namespace EmailConverter
{
    class Converter
    {
        public string inputFileName = "";
        public string inputDumpFileName = "";
        public string liveHolder = "";
        public string loadDirctoryPath = AppDomain.CurrentDomain.BaseDirectory + "\\LoadFrom\\";
        public string dumpDirectoryPath = AppDomain.CurrentDomain.BaseDirectory + "\\DumpTo\\";
        public string outlooksTitleBar = "\"Title\",\"First Name\",\"Middle Name\",\"Last Name\",\"Suffix\",\"Company\",\"Department\",\"Job Title\",\"Business Street\",\"Business Street 2\",\"Business Street 3\",\"Business City\",\"Business State\",\"Business Postal Code\",\"Business Country/Region\",\"Home Street\",\"Home Street 2\",\"Home Street 3\",\"Home City\",\"Home State\",\"Home Postal Code\",\"Home Country/Region\",\"Other Street\",\"Other Street 2\",\"Other Street 3\",\"Other City\",\"Other State\",\"Other Postal Code\",\"Other Country/Region\",\"Assistant's Phone\",\"Business Fax\",\"Business Phone\",\"Business Phone 2\",\"Callback\",\"Car Phone\",\"Company Main Phone\",\"Home Fax\",\"Home Phone\",\"Home Phone 2\",\"ISDN\",\"Mobile Phone\",\"Other Fax\",\"Other Phone\",\"Pager\",\"Primary Phone\",\"Radio Phone\",\"TTY/TDD Phone\",\"Telex\",\"Account\",\"Anniversary\",\"Assistant's Name\",\"Billing Information\",\"Birthday\",\"Business Address PO Box\",\"Categories\",\"Children\",\"Directory Server\",\"E-mail Address\",\"E-mail Type\",\"E-mail Display Name\",\"E-mail 2 Address\",\"E-mail 2 Type\",\"E-mail 2 Display Name\",\"E-mail 3 Address\",\"E-mail 3 Type\",\"E-mail 3 Display Name\",\"Gender\",\"Government ID Number\",\"Hobby\",\"Home Address PO Box\",\"Initials\",\"Internet Free Busy\",\"Keywords\",\"Language\",\"Location\",\"Manager's Name\",\"Mileage\",\"Notes\",\"Office Location\",\"Organizational ID Number\",\"Other Address PO Box\",\"Priority\",\"Private\",\"Profession\",\"Referred By\",\"Sensitivity\",\"Spouse\",\"User 1\",\"User 2\",\"User 3\",\"User 4\",\"Web Page\"";
        public bool moreFilesToCheck = true;

        public static void Main(string[] args) // Main thread of execution 
        {
            Converter sm = new Converter();
            sm.welcomePrintOut();
            Hashtable outlookTable = new Hashtable(91);
            Hashtable thunderbirdTable = new Hashtable(36);

            outlookTable = sm.populateOutlookHashtable(outlookTable);
            thunderbirdTable = sm.populateThunderbirdHashtable(thunderbirdTable);

            // Example of foreach'ing over each object in the hashtable. Each object in a hashtable is called a DictionaryEnrty, //
            // which contains the Key & the Value (DictionaryEntry.Key || DictonaryKey.Value)
            //foreach(Object x in outlookTable)
                //Console.WriteLine(x);

            while (sm.moreFilesToCheck)
                sm.checkFileConsole();
        }

        public void readTextFile(string fileName)
        {
            String line;
            StreamReader sr = new StreamReader(loadDirctoryPath + fileName);
            line = sr.ReadLine();

            while (line != null)
            {
                liveHolder += line + "\r\n";
                line = sr.ReadLine();
            }
            Console.WriteLine(liveHolder);

            // REMOVES bad string char values // 
            removeExtraChars();
            // WRITE //
            writeToFile(liveHolder);
        }


        public void removeExtraChars()
        {
            liveHolder = liveHolder.Replace("\"", "");
            liveHolder = liveHolder.Replace("\\", "");
            liveHolder = liveHolder.Replace("/", "");
            liveHolder = liveHolder.Replace("\'", "");
        }

        public string[] delimitCommas() // get
        {
            string[] holder = new string[36];
            char delimiter = ',';

            holder = liveHolder.Split(delimiter);

            int counter = 0;
            foreach (string x in holder)
            {
                holder[counter] = "\"" + x + "\",";
                counter++;
            }

            return holder;
        }

        public void writeToFile(string createMe) // Writes new file to disk in dump directory
        {
            if (!System.IO.File.Exists(dumpDirectoryPath + inputDumpFileName))
                System.IO.File.WriteAllText(@dumpDirectoryPath + inputDumpFileName, createMe);
            else
                Console.WriteLine("The file already has been dumped previously, \r\n *WARNING* skipped file write");
        }

        public bool checkFile(string filename) // Checks if the file exists on specific load directory
        {
            if (File.Exists(loadDirctoryPath + filename))
                return true; // file 
            else
                return false; // file does not 
        }

        public void checkFileConsole() // Checks to see if the name of the file the user just put in exists on disk
        {
            beginInputOutput();

            if (checkFile(inputFileName))
                readTextFile(inputFileName);
            else
                Console.WriteLine("File " + inputFileName + " was not found.");
            
            bool checker = true;
            while (checker)
                checker = consoleYNLoop();
        }

        public string removeAdditionalCSVName(string checkString)//Allows a user to input the name of the file with or without .csv at the end. (CHROME)
        {
            if (checkString.Length >= 8)
            {
                if (checkString.Substring(checkString.Length - 8).Equals(".csv.csv"))
                    return checkString = checkString.Substring(0, checkString.Length - 4);
                else
                    return checkString;
            }
            else
                return checkString;

           
        }

        public void beginInputOutput() // reads input from user for the desired path of the .csv file, then reads the name of the new file to be created.
        {
            Console.WriteLine(" ");
            Console.WriteLine("Please Enter File Path Of Desired .CSV File");
            inputFileName = Console.ReadLine();
            inputFileName += ".csv";
            inputFileName = removeAdditionalCSVName(inputFileName);
            Console.WriteLine(inputFileName);

            Console.WriteLine("Please specify the name of the name of the new file to be created");
            inputDumpFileName = Console.ReadLine();
            inputDumpFileName += ".csv";
            inputDumpFileName = removeAdditionalCSVName(inputDumpFileName);
            Console.WriteLine(inputDumpFileName);
        }

        public bool consoleYNLoop()//checks for y / n / bad input from user
        {
            Console.WriteLine("More Files To Check?");
            Console.WriteLine("Y/N");
            inputFileName = Console.ReadLine();

            if (inputFileName == "y" || inputFileName == "Y")
            {
                moreFilesToCheck = true;
                return false;
            }
            else if (inputFileName == "n" || inputFileName == "N")
            {
                moreFilesToCheck = false;
                return false;
            }
            else
            {
                Console.WriteLine("Invalid Input, Try Again");
                return true;
            }
        }

        public void welcomePrintOut() //(CHROME)
        {
            Console.WriteLine("  ___            ______    ");
            Console.WriteLine(" / _ \\    ___    |  ___|   ");
            Console.WriteLine("/ /_\\ \\  ( _ )   | |_      ");
            Console.WriteLine("|  _  |  / _ \\/\\ |  _|     ");
            Console.WriteLine("| | | | | (_>  < | |       ");
            Console.WriteLine("\\_| |_/  \\___/\\/ \\_|       ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" _____                           _            ");
            Console.WriteLine("/  __ \\                         | |           ");
            Console.WriteLine("| /  \\/ ___  _ ____   _____ _ __| |_ ___ _ __ ");
            Console.WriteLine("| |    / _ \\| '_ \\ \\ / / _ \\ '__| __/ _ \\ '__|");
            Console.WriteLine("| \\__/\\ (_) | | | \\ V /  __/ |  | ||  __/ |   ");
            Console.WriteLine(" \\____/\\___/|_| |_|\\_/ \\___|_|   \\__\\___|_|   ");
        }

        public Hashtable populateOutlookHashtable(Hashtable outlookTable) // Populates the hardcoded keys for the outlookhashtable values are ""
        {

            outlookTable.Add("Title", "");
            outlookTable.Add("First Name", "");
            outlookTable.Add("Middle Name", "");
            outlookTable.Add("Last Name", "");
            outlookTable.Add("Suffix", "");
            outlookTable.Add("Company", "");
            outlookTable.Add("Department", "");
            outlookTable.Add("Job Title", "");
            outlookTable.Add("Business Street", "");
            outlookTable.Add("Business Street 2", "");
            outlookTable.Add("Business Street 3", "");
            outlookTable.Add("Business City", "");
            outlookTable.Add("Business State", "");
            outlookTable.Add("Business Postal Code", "");
            outlookTable.Add("Business Country/Region", "");
            outlookTable.Add("Home Street", "");
            outlookTable.Add("Home Street 2", "");
            outlookTable.Add("Home Street 3", "");
            outlookTable.Add("Home City", "");
            outlookTable.Add("Home State", "");
            outlookTable.Add("Home Postal Code", "");
            outlookTable.Add("Home Country/Region", "");
            outlookTable.Add("Other Street", "");
            outlookTable.Add("Other Street 2", "");
            outlookTable.Add("Other Street 3", "");
            outlookTable.Add("Other City", "");
            outlookTable.Add("Other State", "");
            outlookTable.Add("Other Postal Code", "");
            outlookTable.Add("Other Country/Region", "");
            outlookTable.Add("Assistant's Phone", "");
            outlookTable.Add("Business Fax", "");
            outlookTable.Add("Business Phone", "");
            outlookTable.Add("Business Phone 2", "");
            outlookTable.Add("Callback", "");
            outlookTable.Add("Car Phone", "");
            outlookTable.Add("Company Main Phone", "");
            outlookTable.Add("Home Fax", "");
            outlookTable.Add("Home Phone", "");
            outlookTable.Add("Home Phone 2", "");
            outlookTable.Add("ISDN", "");
            outlookTable.Add("Mobile Phone", "");
            outlookTable.Add("Other Fax", "");
            outlookTable.Add("Other Phone", "");
            outlookTable.Add("Pager", "");
            outlookTable.Add("Primary Phone", "");
            outlookTable.Add("Radio Phone", "");
            outlookTable.Add("TTY/TDD Phone", "");
            outlookTable.Add("Telex", "");
            outlookTable.Add("Account", "");
            outlookTable.Add("Anniversary", "");
            outlookTable.Add("Assistant's Name", "");
            outlookTable.Add("Billing Information", "");
            outlookTable.Add("Birthday", "");
            outlookTable.Add("Business Address PO Box", "");
            outlookTable.Add("Categories", "");
            outlookTable.Add("Children", "");
            outlookTable.Add("Directory Server", "");
            outlookTable.Add("E-mail Address", "");
            outlookTable.Add("E-mail Type", "");
            outlookTable.Add("E-mail Display Name", "");
            outlookTable.Add("E-mail 2 Address", "");
            outlookTable.Add("E-mail 2 Type", "");
            outlookTable.Add("E-mail 2 Display Name", "");
            outlookTable.Add("E-mail 3 Address", "");
            outlookTable.Add("E-mail 3 Type", "");
            outlookTable.Add("E-mail 3 Display Name", "");
            outlookTable.Add("Gender", "");
            outlookTable.Add("Government ID Number", "");
            outlookTable.Add("Hobby", "");
            outlookTable.Add("Home Address PO Box", "");
            outlookTable.Add("Initials", "");
            outlookTable.Add("Internet Free Busy", "");
            outlookTable.Add("Keywords", "");
            outlookTable.Add("Language", "");
            outlookTable.Add("Location", "");
            outlookTable.Add("Manager's Name", "");
            outlookTable.Add("Mileage", "");
            outlookTable.Add("Notes", "");
            outlookTable.Add("Office Location", "");
            outlookTable.Add("Organizational ID Number", "");
            outlookTable.Add("Other Address PO Box", "");
            outlookTable.Add("Priority", "");
            outlookTable.Add("Private", "");
            outlookTable.Add("Profession", "");
            outlookTable.Add("Referred By", "");
            outlookTable.Add("Sensitivity", "");
            outlookTable.Add("Spouse", "");
            outlookTable.Add("User 1", "");
            outlookTable.Add("User 2", "");
            outlookTable.Add("User 3", "");
            outlookTable.Add("User 4", "");
            outlookTable.Add("Web Page", "");

            

            
            return outlookTable;
        }

        public Hashtable populateThunderbirdHashtable(Hashtable outlookTable) // Populates the hardcoded keys for the thunderbirdhashtable values are ""
        {
            outlookTable.Add("First Name", "");
            outlookTable.Add("Last Name", "");
            outlookTable.Add("Display Name", "");
            outlookTable.Add("Nickname", "");
            outlookTable.Add("Primary Email", "");
            outlookTable.Add("Secondary Email", "");
            outlookTable.Add("Screen Name", "");
            outlookTable.Add("Work Phone", "");
            outlookTable.Add("Home Phone", "");
            outlookTable.Add("Fax Number", "");
            outlookTable.Add("Pager Number", "");
            outlookTable.Add("Mobile Number", "");
            outlookTable.Add("Home Address", "");
            outlookTable.Add("Home Address 2", "");
            outlookTable.Add("Home City", "");
            outlookTable.Add("Home State", "");
            outlookTable.Add("Home ZipCode", "");
            outlookTable.Add("Home Country", "");
            outlookTable.Add("Work Address", "");
            outlookTable.Add("Work Address 2", "");
            outlookTable.Add("Work City", "");
            outlookTable.Add("Work State", "");
            outlookTable.Add("Work ZipCode", "");
            outlookTable.Add("Work Country", "");
            outlookTable.Add("Job Title", "");
            outlookTable.Add("Department", "");
            outlookTable.Add("Organization", "");
            outlookTable.Add("Web Page 1", "");
            outlookTable.Add("Web Page 2", "");
            outlookTable.Add("Birth Year", "");
            outlookTable.Add("Birth Month", "");
            outlookTable.Add("Birth Day", "");
            outlookTable.Add("Custom 1", "");
            outlookTable.Add("Custom 2", "");
            outlookTable.Add("Custom 3", "");
            outlookTable.Add("Custom 4", "");
            outlookTable.Add("Notes", "");

            return outlookTable;
        }
       
        public Converter() //empty constructor because science.
        {

        }
    }
}

