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
                //Console.Write(symbol.Name.ToUpper() + "\t");
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
                //Console.Write(kvp.Value + "\t");

                counter++;

                if (counter % 11 == 0)
                {
                    //Console.WriteLine();
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

            //Console.WriteLine(symbolColumn[22].ToUpper());
            //Console.WriteLine(symbolTruths[22]);

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

                //Console.Write(outputString + "    ");

                // reset the output string.
                outputString = "";
            }

            List<List<int>> Facts = new List<List<int>>();

            for (int i = 0; i < knownFacts.Count; i++)
            {
                List<int> factReference = new List<int>();

                for (int j = 0; j < symbolColumn.Count; j++)
                {
                    if (knownFacts[i].Name == symbolColumn[j])
                    {
                        factReference.Add(j);
                        //models++;
                        //Console.Write(knownFacts[i].Name + "\t");
                    }
                }
                Facts.Add(factReference);
            }

            int models = 0;

            ///////////////////// REMOVE THIS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            //Console.WriteLine(symbolTruths[Facts[0][1]]);

            for (int i = 0; i < Facts.Count; i++)
            {
                for (int j = 0; j < Facts[i].Count; j++)
                {
                    if (symbolTruths[Facts[0][j]] && symbolTruths[Facts[1][j]] && symbolTruths[Facts[2][j]])
                    {
                        models++;
                    }
                }
            }

            Console.WriteLine(models);
            ///////////////////// REMOVE THIS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

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
