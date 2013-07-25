using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace EmailConverter
{
    class Converter
    {
        public string inputFileName = "";
        public string inputDumpFileName = "";
        public string liveHolder = "";
        public string currentContact = "";
        public string loadDirctoryPath = AppDomain.CurrentDomain.BaseDirectory + "\\LoadFrom\\";
        public string dumpDirectoryPath = AppDomain.CurrentDomain.BaseDirectory + "\\DumpTo\\";
        public string outlooksTitleBar = "\"Title\",\"First Name\",\"Middle Name\",\"Last Name\",\"Suffix\",\"Company\",\"Department\",\"Job Title\",\"Business Street\",\"Business Street 2\",\"Business Street 3\",\"Business City\",\"Business State\",\"Business Postal Code\",\"Business Country/Region\",\"Home Street\",\"Home Street 2\",\"Home Street 3\",\"Home City\",\"Home State\",\"Home Postal Code\",\"Home Country/Region\",\"Other Street\",\"Other Street 2\",\"Other Street 3\",\"Other City\",\"Other State\",\"Other Postal Code\",\"Other Country/Region\",\"Assistant's Phone\",\"Business Fax\",\"Business Phone\",\"Business Phone 2\",\"Callback\",\"Car Phone\",\"Company Main Phone\",\"Home Fax\",\"Home Phone\",\"Home Phone 2\",\"ISDN\",\"Mobile Phone\",\"Other Fax\",\"Other Phone\",\"Pager\",\"Primary Phone\",\"Radio Phone\",\"TTY/TDD Phone\",\"Telex\",\"Account\",\"Anniversary\",\"Assistant's Name\",\"Billing Information\",\"Birthday\",\"Business Address PO Box\",\"Categories\",\"Children\",\"Directory Server\",\"E-mail Address\",\"E-mail Type\",\"E-mail Display Name\",\"E-mail 2 Address\",\"E-mail 2 Type\",\"E-mail 2 Display Name\",\"E-mail 3 Address\",\"E-mail 3 Type\",\"E-mail 3 Display Name\",\"Gender\",\"Government ID Number\",\"Hobby\",\"Home Address PO Box\",\"Initials\",\"Internet Free Busy\",\"Keywords\",\"Language\",\"Location\",\"Manager's Name\",\"Mileage\",\"Notes\",\"Office Location\",\"Organizational ID Number\",\"Other Address PO Box\",\"Priority\",\"Private\",\"Profession\",\"Referred By\",\"Sensitivity\",\"Spouse\",\"User 1\",\"User 2\",\"User 3\",\"User 4\",\"Web Page\"";
        public bool moreFilesToCheck = true;

        public Hashtable thunderbirdTable = new Hashtable();
        public Hashtable outlookTable = new Hashtable();


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


            outlookTable = populateOutlookHashtable(outlookTable);
            thunderbirdTable = populateThunderbirdHashtable(thunderbirdTable);

            while (line != null)
            {
                liveHolder += line + "\r\n";
                line = sr.ReadLine();
            }
            Console.WriteLine(liveHolder);

            // REMOVES bad string char values // 
            removeExtraChars();

            string[] contactList = processSingleContacts(liveHolder);


            foreach (string s in contactList)
            {
                Console.WriteLine(s);
            }

            string outputString = mappingAlgorithm(contactList, thunderbirdTable, outlookTable);
            
            // WRITE TO FILE //
            writeToFile(outputString);
        }

        public void removeExtraChars()
        {
            liveHolder = liveHolder.Replace("\"", "");
            liveHolder = liveHolder.Replace("\\", "");
            liveHolder = liveHolder.Replace("/", "");
            liveHolder = liveHolder.Replace("\'", "");
        }

        public string[] delimitCommas(string intake) // get
        {
            string[] contactInfo = new string[36]; //  this must have more space because it only takes the first 36 comma seperated values. that's bad....
            char delimiter = ',';
            contactInfo = intake.Split(delimiter);

            int counter = 0;
            foreach (string value in contactInfo)
            {
                contactInfo[counter] = "\"" + value + "\",";
                counter++;
            }
            return contactInfo;
        }

        public  string[] processSingleContacts(string intake) // inital parsing of the contact string. 
        {
            string[] contacts = intake.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            return contacts;
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

            outlookTable.Add("Title", "\"\",");
            outlookTable.Add("First Name", "\"\",");
            outlookTable.Add("Middle Name", "\"\",");
            outlookTable.Add("Last Name", "\"\",");
            outlookTable.Add("Suffix", "\"\",");
            outlookTable.Add("Company", "\"\",");
            outlookTable.Add("Department", "\"\",");
            outlookTable.Add("Job Title", "\"\",");
            outlookTable.Add("Business Street", "\"\",");
            outlookTable.Add("Business Street 2", "\"\",");
            outlookTable.Add("Business Street 3", "\"\",");
            outlookTable.Add("Business City", "\"\",");
            outlookTable.Add("Business State", "\"\",");
            outlookTable.Add("Business Postal Code", "\"\",");
            outlookTable.Add("Business Country/Region", "\"\",");
            outlookTable.Add("Home Street", "\"\",");
            outlookTable.Add("Home Street 2", "\"\",");
            outlookTable.Add("Home Street 3", "\"\",");
            outlookTable.Add("Home City", "\"\",");
            outlookTable.Add("Home State", "\"\",");
            outlookTable.Add("Home Postal Code", "\"\",");
            outlookTable.Add("Home Country/Region", "\"\",");
            outlookTable.Add("Other Street", "\"\",");
            outlookTable.Add("Other Street 2", "\"\",");
            outlookTable.Add("Other Street 3", "\"\",");
            outlookTable.Add("Other City", "\"\",");
            outlookTable.Add("Other State", "\"\",");
            outlookTable.Add("Other Postal Code", "\"\",");
            outlookTable.Add("Other Country/Region", "\"\",");
            outlookTable.Add("Assistant's Phone", "\"\",");
            outlookTable.Add("Business Fax", "\"\",");
            outlookTable.Add("Business Phone", "\"\",");
            outlookTable.Add("Business Phone 2", "\"\",");
            outlookTable.Add("Callback", "\"\",");
            outlookTable.Add("Car Phone", "\"\",");
            outlookTable.Add("Company Main Phone", "\"\",");
            outlookTable.Add("Home Fax", "\"\",");
            outlookTable.Add("Home Phone", "\"\",");
            outlookTable.Add("Home Phone 2", "\"\",");
            outlookTable.Add("ISDN", "\"\",");
            outlookTable.Add("Mobile Phone", "\"\",");
            outlookTable.Add("Other Fax", "\"\",");
            outlookTable.Add("Other Phone", "\"\",");
            outlookTable.Add("Pager", "\"\",");
            outlookTable.Add("Primary Phone", "\"\",");
            outlookTable.Add("Radio Phone", "\"\",");
            outlookTable.Add("TTY/TDD Phone", "\"\",");
            outlookTable.Add("Telex", "\"\",");
            outlookTable.Add("Account", "\"\",");
            outlookTable.Add("Anniversary", "\"\",");
            outlookTable.Add("Assistant's Name", "\"\",");
            outlookTable.Add("Billing Information", "\"\",");
            outlookTable.Add("Birthday", "\"\",");
            outlookTable.Add("Business Address PO Box", "\"\",");
            outlookTable.Add("Categories", "\"\",");
            outlookTable.Add("Children", "\"\",");
            outlookTable.Add("Directory Server", "\"\",");
            outlookTable.Add("E-mail Address", "\"\",");
            outlookTable.Add("E-mail Type", "\"\",");
            outlookTable.Add("E-mail Display Name", "\"\",");
            outlookTable.Add("E-mail 2 Address", "\"\",");
            outlookTable.Add("E-mail 2 Type", "\"\",");
            outlookTable.Add("E-mail 2 Display Name", "\"\",");
            outlookTable.Add("E-mail 3 Address", "\"\",");
            outlookTable.Add("E-mail 3 Type", "\"\",");
            outlookTable.Add("E-mail 3 Display Name", "\"\",");
            outlookTable.Add("Gender", "\"\",");
            outlookTable.Add("Government ID Number", "\"\",");
            outlookTable.Add("Hobby", "\"\",");
            outlookTable.Add("Home Address PO Box", "\"\",");
            outlookTable.Add("Initials", "\"\",");
            outlookTable.Add("Internet Free Busy", "\"\",");
            outlookTable.Add("Keywords", "\"\",");
            outlookTable.Add("Language", "\"\",");
            outlookTable.Add("Location", "\"\",");
            outlookTable.Add("Manager's Name", "\"\",");
            outlookTable.Add("Mileage", "\"\",");
            outlookTable.Add("Notes", "\"\",");
            outlookTable.Add("Office Location", "\"\",");
            outlookTable.Add("Organizational ID Number", "\"\",");
            outlookTable.Add("Other Address PO Box", "\"\",");
            outlookTable.Add("Priority", "\"\",");
            outlookTable.Add("Private", "\"\",");
            outlookTable.Add("Profession", "\"\",");
            outlookTable.Add("Referred By", "\"\",");
            outlookTable.Add("Sensitivity", "\"\",");
            outlookTable.Add("Spouse", "\"\",");
            outlookTable.Add("User 1", "\"\",");
            outlookTable.Add("User 2", "\"\",");
            outlookTable.Add("User 3", "\"\",");
            outlookTable.Add("User 4", "\"\",");
            outlookTable.Add("Web Page", "\"\",");
            

            
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

        public string mappingAlgorithm(string[] contactList, Hashtable thunderbirdHash, Hashtable outlookHash)
        {
            string outputString = outlooksTitleBar;

            foreach(string s in contactList)
            {

                string[] contact = delimitCommas(s);

                // convert contact info into ordered hashmap names for cleaner more legiable 1:1 mapping //
                thunderbirdHash["First Name"] = contact[0];
                thunderbirdHash["Last Name"] = contact[1];
                thunderbirdHash["Display Name"] = contact[2];
                thunderbirdHash["Nickname"] = contact[3];
                thunderbirdHash["Primary Email"] = contact[4];
                thunderbirdHash["Secondary Email"] = contact[5];
                thunderbirdHash["Screen Name"] = contact[6];
                thunderbirdHash["Work Phone"] = contact[7];
                thunderbirdHash["Home Phone"] = contact[8];
                thunderbirdHash["Fax Number"] = contact[9];
                thunderbirdHash["Pager Number"] = contact[10];
                thunderbirdHash["Mobile Number"] = contact[11];
                thunderbirdHash["Home Address"] = contact[12];
                thunderbirdHash["Home Address 2"] = contact[13];
                thunderbirdHash["Home City"] = contact[14];
                thunderbirdHash["Home State"] = contact[15];
                thunderbirdHash["Home ZipCode"] = contact[16];
                thunderbirdHash["Home Country"] = contact[17];
                thunderbirdHash["Work Address"] = contact[18];
                thunderbirdHash["Work Address 2"] = contact[19];
                thunderbirdHash["Work City"] = contact[20];
                thunderbirdHash["Work State"] = contact[21];
                thunderbirdHash["Work ZipCode"] = contact[22];
                thunderbirdHash["Work Country"] = contact[23];
                thunderbirdHash["Job Title"] = contact[24];
                thunderbirdHash["Department"] = contact[25];
                thunderbirdHash["Organization"] = contact[26];
                thunderbirdHash["Web Page 1"] = contact[27];
                thunderbirdHash["Web Page 2"] = contact[28];
                thunderbirdHash["Birth Year"] = contact[29];
                thunderbirdHash["Birth Month"] = contact[30];
                thunderbirdHash["Birth Day"] = contact[31];
                thunderbirdHash["Custom 1"] = contact[32];
                thunderbirdHash["Custom 2"] = contact[33];
                thunderbirdHash["Custom 3"] = contact[34];
                thunderbirdHash["Custom 4"] = contact[35];
                thunderbirdHash["Notes"] = contact[36];

                // begin 1:1 mapping

                outlookHash["First Name"] = thunderbirdHash["First Name"]; 
                outlookHash["Last Name"] = thunderbirdHash["Last Name"];
                outlookHash["E-mail Adress"] = thunderbirdHash["Primary Email"];
                outlookHash["E-mail 2 Address"] = thunderbirdHash["Secondary Email"];
                outlookHash["Business Phone"] = thunderbirdHash["Work Phone"];
                outlookHash["Home Phone"] = thunderbirdHash["Home Phone"];
                outlookHash["Business Fax"] = thunderbirdHash["Fax Number"];
                outlookHash["Pager"] = thunderbirdHash["Pager Number"];
                outlookHash["Mobile Phone"] = thunderbirdHash["Mobile Number"];
                outlookHash["Home Street"] = thunderbirdHash["Home Adress"];
                outlookHash["Home City"] = thunderbirdHash["Home City"];
                outlookHash["Home State"] = thunderbirdHash["Home State"];
                outlookHash["Home Postal Code"] = thunderbirdHash["Home Zipcode"];
                outlookHash["Home Country/Region"] = thunderbirdHash["Home Country"];
                outlookHash["Business Street"] = thunderbirdHash["Work Address"];
                outlookHash["Business City"] = thunderbirdHash["Work City"];
                outlookHash["Business State"] = thunderbirdHash["Work State"];
                outlookHash["Business Postal Code"] = thunderbirdHash["Work Zipcode"];
                outlookHash["Business Country/Region"] = thunderbirdHash["Work Country"];
                outlookHash["Job Title"] = thunderbirdHash["Job Title"];
                outlookHash["Department"] = thunderbirdHash["Department"];
                outlookHash["Web Page"] = thunderbirdHash["Web Page 1"];
                outlookHash["Notes"] = thunderbirdHash["Notes"];

                // append to output string

                outputString += outlookHash["Title"];
                outputString += outlookHash["First Name"];
                outputString += outlookHash["Middle Name"];
                outputString += outlookHash["Last Name"];
                outputString += outlookHash["Suffix"];
                outputString += outlookHash["Company"];
                outputString += outlookHash["Department"];
                outputString += outlookHash["Job Title"];
                outputString += outlookHash["Business Street"];
                outputString += outlookHash["Business Street 2"];
                outputString += outlookHash["Business Street 3"];
                outputString += outlookHash["Business City"];
                outputString += outlookHash["Business State"];
                outputString += outlookHash["Business Postal Code"];
                outputString += outlookHash["Business Country/Region"];
                outputString += outlookHash["Home Street"];
                outputString += outlookHash["Home Street 2"];
                outputString += outlookHash["Home Street 3"];
                outputString += outlookHash["Home City"];
                outputString += outlookHash["Home State"];
                outputString += outlookHash["Home Postal Code"];
                outputString += outlookHash["Home Country/Region"];
                outputString += outlookHash["Other Street"];
                outputString += outlookHash["Other Street 2"];
                outputString += outlookHash["Home Street 3"];
                outputString += outlookHash["Home City"];
                outputString += outlookHash["Home State"];
                outputString += outlookHash["Home Postal Code"];
                outputString += outlookHash["Home Country/Region"];
                outputString += outlookHash["Other Street"];
                outputString += outlookHash["Other Street 2"];
                outputString += outlookHash["Other Street 3"];
                outputString += outlookHash["Other City"];
                outputString += outlookHash["Other State"];
                outputString += outlookHash["Other Postal Code"];
                outputString += outlookHash["Other Country/Region"];
                outputString += outlookHash["Assistant's Phone"];
                outputString += outlookHash["Business Fax"];
                outputString += outlookHash["Business Phone"];
                outputString += outlookHash["Business Phone 2"];
                outputString += outlookHash["Callback"];
                outputString += outlookHash["Car Phone"];
                outputString += outlookHash["Company Main Phone"];
                outputString += outlookHash["Home Fax"];
                outputString += outlookHash["Home Phone"];
                outputString += outlookHash["Home Phone 2"];
                outputString += outlookHash["ISDN"];
                outputString += outlookHash["Mobile Phone"];
                outputString += outlookHash["Other Fax"];
                outputString += outlookHash["Other Phone"];
                outputString += outlookHash["Pager"];
                outputString += outlookHash["Primary Phone"];
                outputString += outlookHash["Radio Phone"];
                outputString += outlookHash["TTY/TDD Phone"];
                outputString += outlookHash["Telex"];
                outputString += outlookHash["Account"];
                outputString += outlookHash["Anniversary"];
                outputString += outlookHash["Assistant's Name"];
                outputString += outlookHash["Billing Information"];
                outputString += outlookHash["Birthday"];
                outputString += outlookHash["Business Address PO Box"];
                outputString += outlookHash["Categories"];
                outputString += outlookHash["Children"];
                outputString += outlookHash["Directory Server"];
                outputString += outlookHash["E-mail Address"];
                outputString += outlookHash["E-mail Type"];
                outputString += outlookHash["E-mail Display Name"];
                outputString += outlookHash["E-mail 2 Address"];
                outputString += outlookHash["E-mail 2 Type"];
                outputString += outlookHash["E-mail 2 Display Name"];
                outputString += outlookHash["E-mail 3 Address"];
                outputString += outlookHash["E-mail 3 Type"];
                outputString += outlookHash["E-mail 3 Display Name"];
                outputString += outlookHash["Gender"];
                outputString += outlookHash["Government ID Number"];
                outputString += outlookHash["Hobby"];
                outputString += outlookHash["Home Address PO Box"];
                outputString += outlookHash["Initials"];
                outputString += outlookHash["Internet Free Busy"];
                outputString += outlookHash["Keywords"];
                outputString += outlookHash["Language"];
                outputString += outlookHash["Location"];
                outputString += outlookHash["Manager's Name"];
                outputString += outlookHash["Mileage"];
                outputString += outlookHash["Notes"];
                outputString += outlookHash["Office Location"];
                outputString += outlookHash["Organizational ID Number"];
                outputString += outlookHash["Other Address PO Box"];
                outputString += outlookHash["Priority"];
                outputString += outlookHash["Private"];
                outputString += outlookHash["Profession"];
                outputString += outlookHash["Referred By"];
                outputString += outlookHash["Sensitivity"];
                outputString += outlookHash["Spouse"];
                outputString += outlookHash["User 1"];
                outputString += outlookHash["User 2"];
                outputString += outlookHash["User 3"];
                outputString += outlookHash["User 4"];
                outputString += outlookHash["Web Page"];

                outputString += "\n";

            }
            return outputString;
        }

        public Converter() //empty constructor because science.
        {

        }
    }
}



