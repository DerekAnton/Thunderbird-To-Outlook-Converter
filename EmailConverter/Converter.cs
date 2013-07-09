using System;
using System.IO;

// how to get current directory 
// AppDomain.CurrentDomain.BaseDirectory


namespace EmailConverter
{
    class Converter
    {
        public string inputFileName = "";
        public string inputDumpFileName = "";
        public string liveHolder = "";
        public bool moreFilesToCheck = true;
        public string loadDirctoryPath = AppDomain.CurrentDomain.BaseDirectory + "\\LoadFrom\\";
        public string dumpDirectoryPath = AppDomain.CurrentDomain.BaseDirectory + "\\DumpTo\\";

        public static void Main(string[] args)
        {
            Converter sm = new Converter();
            sm.welcomePrintOut();

            while (sm.moreFilesToCheck)
            {
                sm.checkFileConsole();
            }
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
            writeToFile(liveHolder);
        }

        public void writeToFile(string createMe)
        {
            if (!System.IO.File.Exists(dumpDirectoryPath + inputDumpFileName))
                System.IO.File.WriteAllText(@dumpDirectoryPath + inputDumpFileName, createMe);
            else
                Console.WriteLine("The file already has been dumped previously, \r\n *WARNING* skipped file write");
        }

        public bool checkFile(string filename)
        {
            if (File.Exists(loadDirctoryPath + filename))
                return true; // file 
            else
                return false; // file does not 
        }

        public void checkFileConsole()
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

        public string removeAdditionalCSVName(string checkString)
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

        public void beginInputOutput()
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

        public bool consoleYNLoop()
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

        public void welcomePrintOut()
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

        public Converter()
        {

        }
    }
}

