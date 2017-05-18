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
        private List<Dictionary<int, bool>> truthsList = new List<Dictionary<int, bool>>();

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
            // Get the tables from the sentances in KB.
            return false;
        }

        public bool CheckAll()
        {
            // perform the single symbols method.
            ListSingleSymbols();

            foreach(SingleSymbol symbol in singleSymbols)
            {
                Console.Write(symbol.Name + "\t");
            }

            Console.WriteLine();

            foreach(SingleSymbol symbol in singleSymbols)
            {
                //symbolTruths.Add(symbol.Name, false);
                //truthsList.Add(symbolTruths);
            }

            // this is because the starting state is repeated at the end?
            singleSymbols[0].FlipState();

            int counter = 1;

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
                    truthsList.Add(symbolTruths);

                    counter++;

                    Console.Write(singleSymbols[j].Truth + "\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine();

            // Set the counter to 0;
            counter = 0;

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

            return false;
        }

        public bool IsModel(string q)
        {
            // Check all the symbols and sentances in KB to determine if it's a model.
            return false;
        }
    }
}
