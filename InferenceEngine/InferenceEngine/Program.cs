using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class Program
    {
        /// <summary>
        /// Use this enumeration for the search methods.
        /// </summary>
        public enum algorithmType { TT, FC, BC };

        //public enum operations { }
        static void Main(string[] args)
        {
            //location of the file to read in.
            string fileLocation = "";
            string line = "";
            //string[] infixSplit;
            List<string> infixSplit;
            ForwardChaining firstRun = new ForwardChaining();
            // Split each line further into arugments and commands within each ';' split.
            // Testing


            string testEnumCast = "TT";
            bool resultOfQuery = false;
            algorithmType selectedAlgorithm;

            //Prompt for the location of the file

            //Console.WriteLine("Please enter the location of the file...");
            //fileLocation = Console.ReadLine();

            fileLocation = "test1.txt";

            //for the final version of this code, uncomment this.
            //this allows the final arguements to be passed into the program.
            //fileLocation = args[0];

            //cast algorithm type to enum
            //try catch to detect incorrect text entry.
            try
            {
                //cast the arguement to the algorithm type
                //uncomment this in the final verison
                //selectedAlgorithm = (algorithmType)Enum.Parse(typeof(algorithmType), args[1].ToString().ToUpper(), false);

                //for debug
                selectedAlgorithm = (algorithmType)Enum.Parse(typeof(algorithmType), testEnumCast, true);
            }
            catch
            {
                Console.WriteLine("Invalid algorithm. Please try again.");
                Environment.Exit(0);
            }

            //reading the file in is where something may go wrong. 
            //catch errors for this.
            try
            {
                //Create the streamreader object to read the file.
                //for dubug, the file has been added to the resources of this project.
                //the file has been moved to the debug folder. This gets the location of the file for testing.
                string debugPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"test1.txt");

                System.IO.StreamReader file = new System.IO.StreamReader(debugPath);

                //for final implementation
                //System.IO.StreamReader file = new System.IO.StreamReader(fileLocation);

                //while the file is not finished
                bool nextLineAsk = false;
                while ((line = file.ReadLine()) != null)
                {
                    /*first, check the start character of the line
                     * 
                     *  'TELL' indicates the line should not be read.
                     *  'ASK' indicates the line should not be read.
                     *   Any other character defines a data line. String Split if
                     *   require and read data.
                     * 
                    */

                    /* This is just so we don't have keep reading the PDF for a description
                     * Feel free to delete.
                     * 
                     * Slightly more general is the Horn clause, which is a disjunction of literals of which at
                     * HORN CLAUSE most one is positive. So all deﬁnite clauses are Horn clauses, as are clauses with no positive literals; 
                     * these are called goal clauses. Horn clauses are closed under resolution: if youre solve GOAL CLAUSES two Horn clauses, 
                     * you get back a Horn clause.
                     * 
                     */

                    //check if the line starts with ASK or TELL.
                    //Do nothing in this case.

                    if (!((line.StartsWith("ASK")) || (line.StartsWith("TELL"))))
                    {
                        if (!nextLineAsk)
                        {
                            infixSplit = line.Split(';').ToList();
                            firstRun.OrganiseKBData(infixSplit);
                            nextLineAsk = true;
                        }
                        else
                        {
                            resultOfQuery = firstRun.RunAlgorithm(line.ToString());
                        }
                    }
                }
                
                //close the file
                file.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("The selected file could not be opened. Please try executing the program again.");
                Console.WriteLine(ex.Message);
                //close the program
                Console.WriteLine("Press 'Enter' to continue...");
                Console.ReadLine();
                Environment.Exit(0);
            }

            string resultString  = "";
            //create a string of the search results
            foreach (Symbol s in firstRun.returnFacts)
            {
                resultString += s.Name + ", ";
            }

            Console.WriteLine(resultOfQuery.ToString() + " : " + resultString);

            Console.WriteLine("Press 'Enter' to continue...");
            Console.ReadLine();
        }
      
    }
}
