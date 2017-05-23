using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class TruthTable : PropositionalAlgorithm
    {
        public TruthTable()
        {
            code = "TT";
            longName = "Truth Table";
            algorithm = algorithmType.TT;
        }

        private List<SingleSymbol> singleSymbols = new List<SingleSymbol>();
        private List<string> symbolNames = new List<string>();

        private Dictionary<int, bool> symbolTruths = new Dictionary<int, bool>();
        private List<string> symbolColumn = new List<string>();

        private void ListSingleSymbols()
        {
            // get all the symbols and implications.
            foreach (Sentence sentance in KB)
            {
                // all all the symbols from the Symbols list.
                foreach (Symbol symbol in sentance.Symbols)
                {
                    // make sure the symbol doesn't already exist.
                    if (symbolNames.Contains(symbol.Name) == false)
                    {
                        // if it doesn't exist, add it to the list.
                        symbolNames.Add(symbol.Name);
                    }
                }

                // do the same for Implications list.
                foreach (Symbol symbol in sentance.Implications)
                {
                    // make sure the symbol doesn't already exist.
                    if (symbolNames.Contains(symbol.Name) == false)
                    {
                        // if it doesn't exist, add it to the list.
                        symbolNames.Add(symbol.Name);
                    }
                }
            }

            // sort the list alphabetically for sanity.
            symbolNames.Sort();

            int count = 1;

            // add each symbol from a list of strings to a list of SingleSymbols.
            foreach (string symbol in symbolNames)
            {
                SingleSymbol singleSymbol = new SingleSymbol(symbol, count);
                singleSymbols.Add(singleSymbol);

                count = count * 2;
            }
        }

        /*
         * When the method is TT and the answer is YES, it should be
         * followed by a colon (:) and the number of models of KB. When the method is FC or BC and the answer is
         * YES, it should be followed by a colon (:) and the list of propositional symbols entailed from KB that has been
         * found during the execution of the specified algorithm.
         * For example, running iengine with method TT on the example test1.txt file should produce the
         * following output:
         * > YES: 3 
         */

        public bool CheckEntails(string q)
        {
            // get the tables from the sentances in KB.
            return false;
        }

        public bool CheckAll()
        {
            // perform the single symbols method.
            ListSingleSymbols();

            foreach(SingleSymbol symbol in singleSymbols)
            {
                Console.Write(symbol.Name.ToUpper() + "\t");
            }

            Console.WriteLine();

            // this is because the starting state is repeated at the end?
            singleSymbols[0].FlipState();

            int counter = 0;

            // flip the truths depending on the significants on the bit. (little endian)
            for (int i = 0; i < Math.Pow(2, singleSymbols.Count()); i++)
           {
                // this is just for readability.
                int size = singleSymbols.Count();

                for (int j = size - 1; j >= 0; j--)
                {
                    if (i % singleSymbols[j].Increment == 1)
                    {
                        // flip the state if the remainder of i divided by the symbols increment is equal to 0.
                        singleSymbols[j].FlipState();
                    }
                    if (singleSymbols[j].Increment == 1)
                    {
                        // flip the state if symbols increment equals 1.
                        singleSymbols[j].FlipState();
                    }
                    symbolTruths.Add(counter, singleSymbols[j].Truth);

                    counter++;
                }
            }

            // Set the counter to 0 to cycle through all the dictionary keys.
            counter = 0;

            // diplay each truth value for every combination and add them to a dictionary.
            foreach (KeyValuePair<int, bool> kvp in symbolTruths)
            {
                Console.Write(kvp.Value + "\t");

                counter++;

                if (counter % 11 == 0)
                {
                    Console.WriteLine();
                }
            }

            Console.WriteLine();

            // set the values for each column at an index and add them to a list.
            int count = symbolTruths.Count;
            string current;
            int length = singleSymbols.Count;

            for (int i = 0; i < count; i++)
            {
                if (i < length)
                    current = singleSymbols[i].Name;
                else
                    current = singleSymbols[i % length].Name;

                symbolColumn.Add(current);
            }

            for (int i = KB.Count - 1; i >= 0; i--)
            {
                // add the facts to the list
                knownFacts.Insert(0, KB[i].FactCheck());

                // due to need for the "factcheck" function to return a bool,
                // we need to remove the "null"
                if (knownFacts[0] == null)
                {
                    knownFacts.RemoveAt(0);
                }
                else
                {
                    // remove the facts for the knowledge base
                    KB.RemoveAt(i);
                }
            }

            List<string> sentanceList = new List<string>();

            string outputString = "";

            foreach (Sentence s in KB)
            {
                // symbol count will always be >= implication count.
                for (int i = 0; i <= s.Symbols.Count - 1; i++)
                {
                    // add the first symbol
                    outputString += s.Symbols[i].Name;

                    // check if we go outside the list.
                    if ((i < s.Operators.Count - 1) && (s.Operators.Count > 0))
                    {
                        outputString += "&";
                    }
                }

                // check for implications (facts don't have any)
                if (s.Implications.Count > 0)
                {
                    // get the final output string
                    outputString += "=>" + s.Implications[0].Name; // should only be one implication, might want to use loop for safety.
                }

                sentanceList.Add(outputString);

                // reset the output string.
                outputString = "";
            }

            ///////////// FROM HERE //////////////////////////////////////////////////////////

            /// <summary>
            /// This is the part i've been slaving over...
            /// it's the list of truths after the implications have been
            /// worked out.
            /// The '&' symbol part hasn't been implemented as of yet
            /// but as soon as the regular => is implmented it should
            /// be similar.
            /// </summary>

            string[] newString = { "" };

            // contains: first symbols
            List<string> firstSymbolList = new List<string>();

            // contains: first symbols after '&'
            List<string> afterAndList = new List<string>();

            // contains: implications
            List<string> implicationList = new List<string>();

            // contains: [0]first symbol list    [1]implies list
            List<List<string>> sentanceImplies = new List<List<string>>();

            // contains: [0]first symbol list    [1]first symbol after '&' list   [2] implies list
            List<List<string>> sentanceAndImplies = new List<List<string>>();

            Console.WriteLine("Sentance List: " + sentanceList.Count + "\n");

            for (int i = 0; i < sentanceList.Count; i++) // i < 7;
            {
                if (!sentanceList[i].Contains('&'))
                {
                    newString = sentanceList[i].Split(new char[] { '=', '>' }, StringSplitOptions.RemoveEmptyEntries);

                    firstSymbolList.Add(newString[0]);
                    implicationList.Add(newString[1]);
                }
            }

            sentanceImplies.Add(firstSymbolList);
            sentanceImplies.Add(implicationList);

            Console.WriteLine("Sentance Implies Lists: " + sentanceImplies.Count);      // expected: 2
            Console.WriteLine("Single Symbol List: " + sentanceImplies[0].Count);       // expected: 4
            Console.WriteLine("Implication List: " + sentanceImplies[1].Count + "\n");  // expected: 4

            //                   firstSymbolList[3]              implicationsList[3]
            //                                   p2     =>                        p3
            Console.WriteLine(sentanceImplies[0][3] + " => " + sentanceImplies[1][3] + "\n");

            Console.WriteLine("sentanceImplies[0].Count: " + sentanceImplies[0].Count);
            Console.WriteLine("symbolNames.Count: " + symbolColumn.Count);
            Console.WriteLine();

            // contains: list of truths from clause logic. eg: True    True    False   Flase...
            List <bool> clauseTruth = new List<bool>();

            // contains: list of each clause truth logic. eg: (P2 => P3)<True False...>  (C => E)<True True...>  (P1 => P2)<False False...>
            List<List<bool>> clauseTruthList = new List<List<bool>>();

            // contains: first symbol truths.
            List<bool> firstSymbolTruths = new List<bool>();

            // contains: implications truths.
            List<bool> implicationTruths = new List<bool>();

            // contains: sentance with no '&' symbol truths.
            List<List<bool>> sentanceTruths = new List<List<bool>>();

            for (int i = 0; i < sentanceImplies[0].Count; i++) // 4
            {
                for (int j = 0; j < symbolTruths.Count; j++) // ~22,500
                {
                    if (sentanceImplies[0][i] == symbolColumn[j])
                    {
                        firstSymbolTruths.Add(symbolTruths[j]);
                    }
                    if (sentanceImplies[1][i] == symbolColumn[j])
                    {
                        implicationTruths.Add(symbolTruths[j]);
                    }
                }
                sentanceTruths.Add(firstSymbolTruths);
                sentanceTruths.Add(implicationTruths);
            }

            Console.WriteLine(sentanceTruths.Count);

            bool truth = false;

            //for (int i = 0; i < sentanceTruths.Count; i++)
            {
                for (int j = 0; j < sentanceTruths[0].Count; j++)
                {
                    if (sentanceTruths[4][j] && sentanceTruths[5][j] == false)
                    {
                        truth = false;
                    } else {
                        truth = true;
                    }
                    //Console.WriteLine(truth);
                }
            }


            //////// TO HERE IS FUCKED ////////////////////////////////////////////////


            
            
            /// <summary>
            /// This part was just for the known facts
            /// the list of truth values assigned
            /// once the program works out which
            /// column the truth is in.
            /// </summary>

            List<List<bool>> Facts = new List<List<bool>>();

            for (int i = 0; i < knownFacts.Count; i++)
            {
                List<bool> factReference = new List<bool>();

                for (int j = 0; j < symbolColumn.Count; j++)
                {
                    if (knownFacts[i].Name == symbolColumn[j])
                    {
                        factReference.Add(symbolTruths[j]);
                        //Console.WriteLine(symbolColumn[j]);
                        //models++;
                        //Console.Write(knownFacts[i].Name + "\t");
                    }
                }
                Facts.Add(factReference);
            }

            //Console.WriteLine();
            //Console.WriteLine(Facts[0][3]);

            int models = 0;

            ///////////////////// REMOVE THIS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            /// <summary>
            /// This part was just here to test the models and was
            /// how I was going to fall back on if shit hit the fan
            /// before the due date.
            /// </summary>
            //Console.WriteLine(symbolTruths[Facts[0][1]]);

            for (int i = 0; i < Facts[0].Count; i++)
            {
                if (Facts[0][i] && Facts[1][i] && Facts[2][i])
                {
                    models++;
                }
            }

            Console.WriteLine(models);
            ///////////////////// REMOVE THIS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


            /// <summary>
            /// This is the part for the models...
            /// plug in the clause values in here when they work.
            /// </summary>
            Model model = new Model(Facts.Count);

            foreach (List<bool> factList in Facts)
            {
                model.PopulateList(factList);
            }
            //model.PopulateList(Facts[0]);
            int newModels = model.IsModel();

            Console.WriteLine(newModels);
            //Console.WriteLine(Facts[0].Count);

            Console.WriteLine();
            Console.WriteLine();

            return true;
        }

        public bool IsModel(string q)
        {
            // check all the symbols and sentances in KB to determine if it's a model.
            return false;
        }
    }
}