﻿using System;
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

        static void Main(string[] args)
        {
            //location of the file to read in.
            string fileLocation = "";
            string line = "";
            //string[] infixSplit;
            List<string> infixSplit;

            // Split each line further into arugments and commands within each ';' split.
            // Testing
            List<string> arguments;
            List<string> commands;

            string testEnumCast = "TT";
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
                        infixSplit = line.Split(';').ToList();

                        OrganiseKBData(infixSplit);
                    }
                    //break for now, this is just for testing.
                    //will read next line to determine query
                    else if (line.StartsWith("ASK"))
                    {
                        break;
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

            //wait for the user to close the program.
            Console.WriteLine("Press 'Enter' to continue...");
            Console.ReadLine();
        }

        static void OrganiseKBData(List<string> split)
        {
            // TESTING
            for (int i = 0; i < split.Count - 1; i++)
            {
                // segment applies to each statment before a semicolon.
                string segment = split[i].Replace(" ", string.Empty);

                // character array stores each character in a segment.
                char[] character = segment.ToCharArray();

                for (int j = 0; j < character.Length; j++)
                {
                    // define the current character and next character in array.
                    int currentChar = Char.ToLower(character[j]);
                    int nextChar = 1;

                    // make sure we get an out of bounds exception.
                    if (j < character.Length - 1)
                    {
                        // here we'll store arguments.
                        nextChar = Char.ToLower(character[j + 1]);
                    }

                    if (currentChar == '=' && nextChar == '>' || currentChar == '&')
                    {
                        // here we'll store operators.
                        Console.WriteLine();
                    }

                    // just for testing.
                    Console.Write(character[j]);

                    if (currentChar == '>' || currentChar == '&')
                    {
                        // might need this to store operators too.
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
            }

            throw new NotImplementedException();
        }
    }
}
