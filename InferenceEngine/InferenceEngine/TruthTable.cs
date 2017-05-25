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

        // contains: list of SingleSymbol objects to be converted to strings for later use.
        private List<SingleSymbol> singleSymbols = new List<SingleSymbol>();

        // contains: list of all the symbols in the KB.
        private List<string> symbolNames = new List<string>();

        // contains: list of truths for each symbol in the KB.
        private Dictionary<int, bool> symbolTruths = new Dictionary<int, bool>();

        // contains: organised list with each index representing a single symbol name from the KB.
        private List<string> symbolColumn = new List<string>();

        // contains: list of each clause truth logic. eg: (P2 => P3)<True False...>  (C => E)<True True...>  (P1 => P2)<False False...>
        List<List<bool>> clauseTruthList = new List<List<bool>>();

        // contains: list of know facts for each sentance in the KB.
        List<List<bool>> Facts = new List<List<bool>>();

        private void ListSingleSymbols()
        {
            // get all the symbols and implications.
            foreach (Sentence sentance in KB)
            {
                // all all the symbols from the Symbols list.
                foreach (Symbol symbol in sentance.Symbols)
                {
                    // make sure the symbol doesn't already exist.
                    if (symbolNames.Contains(symbol.Name.ToLower()) == false)
                    {
                        // if it doesn't exist, add it to the list.
                        symbolNames.Add(symbol.Name.ToLower());
                    }
                }

                // do the same for Implications list.
                foreach (Symbol symbol in sentance.Implications)
                {
                    // make sure the symbol doesn't already exist.
                    if (symbolNames.Contains(symbol.Name.ToLower()) == false)
                    {
                        // if it doesn't exist, add it to the list.
                        symbolNames.Add(symbol.Name.ToLower());
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

        public bool CheckAll(string q)
        {
            // perform the single symbols method.
            ListSingleSymbols();

            bool toReturn = false;

            // find if the query exists in the KB.
            foreach (string symbol in symbolNames)
            {
                if (symbol == q.ToLower())
                {
                    // found a solution!
                    toReturn = true;
                }
            }

            // this is because the starting state is repeated at the end otherwise?
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

            // check for all the implications and create separate truth table for that.
            CheckEntails(q);

            /// <summary>
            /// This part was just for the known facts
            /// the list of truth values assigned
            /// once the program works out which
            /// column the truth is in.
            /// </summary>

            // add the sentace truths first.
            foreach(List<bool> truthList in clauseTruthList)
            {
                Facts.Add(truthList);
            }

            // contains: the query truths as a known fact.
            List<bool> qTruths = new List<bool>();

            // add the query q second.
            for(int i = 0; i < symbolColumn.Count; i++)
            {
                if (q == symbolColumn[i])
                {
                    // add the truths to a list so the query becomes a known fact.
                    qTruths.Add(symbolTruths[i]);
                }
            }

            // add the query q to the Facts list.
            Facts.Add(qTruths);

            // add the known facts third.
            for (int i = 0; i < knownFacts.Count; i++)
            {
                List<bool> factReference = new List<bool>();

                for (int j = 0; j < symbolColumn.Count; j++)
                {
                    if (knownFacts[i].Name == symbolColumn[j])
                    {
                        factReference.Add(symbolTruths[j]);
                    }
                }
                Facts.Add(factReference);
            }

            Console.WriteLine("Facts.Count: " + Facts.Count);

            return toReturn;
        }

        private void CheckEntails(string q)
        {
            // contains: list of sentances in string form to be broken down later.
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

            // an array of strings from the sentanceList that will be broken down and
            // put into other lists so it's easy to manage the data.
            string[] newString = { "" };

            ////////////// Implications without & \\\\\\\\\\\\\\\\\\

            // contains: first symbols
            List<string> firstSymbolList = new List<string>();

            // contains: implications
            List<string> implicationList = new List<string>();

            //////////////// Implications with & \\\\\\\\\\\\\\\\\\\

            // contains: first symbols before '&'
            List<string> beforeAndList = new List<string>();

            // contains: first symbols after '&'
            List<string> afterAndList = new List<string>();

            // contains: implications after '&'
            List<string> implicationAndList = new List<string>();

            ///////////////////// List of Lists \\\\\\\\\\\\\\\\\\\\\

            // contains: [0]first symbol list    [1]implies list
            List<List<string>> sentanceImplies = new List<List<string>>();

            // contains: [0]first symbol list    [1]first symbol after '&' list   [2] implies list
            List<List<string>> sentanceAndImplies = new List<List<string>>();

            for (int i = 0; i < sentanceList.Count; i++) // i < 7;
            {
                if (!sentanceList[i].Contains('&')) // doesn't contain &: p2 => p3;
                {
                    newString = sentanceList[i].Split(new char[] { '=', '>' }, StringSplitOptions.RemoveEmptyEntries);

                    firstSymbolList.Add(newString[0].ToLower());
                    implicationList.Add(newString[1].ToLower());
                }

                if (sentanceList[i].Contains('&')) // contains &: a & b => c;
                {
                    newString = sentanceList[i].Split(new char[] { '=', '>', '&' }, StringSplitOptions.RemoveEmptyEntries);

                    beforeAndList.Add(newString[0].ToLower());
                    afterAndList.Add(newString[1].ToLower());
                    implicationAndList.Add(newString[2].ToLower());
                }
            }

            // implies without '&'
            sentanceImplies.Add(firstSymbolList);
            sentanceImplies.Add(implicationList);

            // implies with '&'
            sentanceAndImplies.Add(beforeAndList);
            sentanceAndImplies.Add(afterAndList);
            sentanceAndImplies.Add(implicationAndList);

            /////////////// Implications without '&' \\\\\\\\\\\\\\\\\\\\

            // contains: first symbol truths.
            List<bool> firstSymbolTruths = new List<bool>();

            // contains: implications truths.
            List<bool> implicationTruths = new List<bool>();

            /////////////// Implications with '&' \\\\\\\\\\\\\\\\\\\\\\\

            // contains: before and symbol truths.
            List<bool> beforeAndTruths = new List<bool>();

            // contains: after and symbol truths.
            List<bool> afterAndTruths = new List<bool>();

            // contains: and implication truths.
            List<bool> implicationsAndTruths = new List<bool>();

            /////////////////////// List of Lists \\\\\\\\\\\\\\\\\\\\\\\

            // contains: sentance with no '&' symbol truths.
            List<List<bool>> firstSymbolTruthsList = new List<List<bool>>();
            List<List<bool>> implicationTruthsList = new List<List<bool>>();

            // contains: sentance with '&' symbol truths.
            List<List<bool>> beforeAndTruthsList = new List<List<bool>>();
            List<List<bool>> afterAndTruthsList = new List<List<bool>>();
            List<List<bool>> implicationsAndTruthsList = new List<List<bool>>();

            for (int i = 0; i < sentanceImplies[0].Count; i++) // 4
            {
                firstSymbolTruths = new List<bool>();
                implicationTruths = new List<bool>();

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
                firstSymbolTruthsList.Add(firstSymbolTruths);
                implicationTruthsList.Add(implicationTruths);
            }

            for (int i = 0; i < sentanceAndImplies[0].Count; i++) // 3
            {
                beforeAndTruths = new List<bool>();
                afterAndTruths = new List<bool>();
                implicationsAndTruths = new List<bool>();

                for (int j = 0; j < symbolTruths.Count; j++) // ~22,500
                {
                    if (sentanceAndImplies[0][i] == symbolColumn[j])
                    {
                        beforeAndTruths.Add(symbolTruths[j]);
                    }
                    if (sentanceAndImplies[1][i] == symbolColumn[j])
                    {
                        afterAndTruths.Add(symbolTruths[j]);
                    }
                    if (sentanceAndImplies[2][i] == symbolColumn[j])
                    {
                        implicationsAndTruths.Add(symbolTruths[j]);
                    }
                }
                beforeAndTruthsList.Add(beforeAndTruths);
                afterAndTruthsList.Add(afterAndTruths);
                implicationsAndTruthsList.Add(implicationsAndTruths);
            }

            // contains: list of truths from clause logic. eg: True    True    False   Flase...
            List<bool> clauseTruth = new List<bool>();

            bool truth = false;

            for (int i = 0; i < firstSymbolTruthsList.Count; i++) // 4
            {
                clauseTruth = new List<bool>();

                for (int j = 0; j < firstSymbolTruthsList[i].Count; j++) // 2048
                {
                    if (firstSymbolTruthsList[i][j] && implicationTruthsList[i][j] == false)
                    {
                        truth = false;
                    }
                    else
                    {
                        truth = true;
                    }
                    clauseTruth.Add(truth);
                }
                clauseTruthList.Add(clauseTruth);
            }

            for (int i = 0; i < beforeAndTruthsList.Count; i++) // 3
            {
                clauseTruth = new List<bool>();

                for (int j = 0; j < beforeAndTruthsList[i].Count; j++) // 2048
                {
                    if ((beforeAndTruthsList[i][j] && afterAndTruthsList[i][j]) && implicationsAndTruthsList[i][j] == false)
                    {
                        truth = false;
                    }
                    else
                    {
                        truth = true;
                    }
                    clauseTruth.Add(truth);
                }
                clauseTruthList.Add(clauseTruth);
            }
        }

        public int IsModel()
        {
            /// <summary>
            /// This is the part for the models...
            /// plug in the clause values in here when they work.
            /// </summary>

            Model model = new Model(Facts.Count);

            foreach (List<bool> factList in Facts)
            {
                model.PopulateList(factList);
            }

            int newModels = model.IsModel();

            return newModels;
        }
    }
}