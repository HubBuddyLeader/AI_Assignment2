using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class BackwardChaning : PropositionalAlgorithm
    {
        public BackwardChaning()
        {
            code = "BC";
            longName = "Backward Chaining";
        }

        /*
         * Running iengine with method BC on the example test file above should produce the following output:
         * > YES: p2, p3, p1, d 
         */

        public bool RunAlgorithm(string q)
        {
            // define "fact" statements
            // must loop backwards as items will be removed
            for (int i = KB.Count - 1; i >= 0; i--)
            //foreach (Sentence s in KB)
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

            // update the other facts accordingly.
            foreach (Sentence s in KB)
            {
                // go through and update "isFact" property
                // this is not needed for this assignement, but may
                // be used later for "extension" work
                s.UpdateFacts(knownFacts);
            }

            List<Symbol> BCSearchFacts = new List<Symbol>();

            // add the query to the return list
            

            // need to search through the KB till query is found in implications.
            for (int i = KB.Count - 1; i >= 0; i--)
            {
                // while there are still implications in that KB sentence
                if (KB[i].Implications.Count > 0)
                {
                    // if those symboles appear in the known facts,
                    // add them to the known fact list and remove from KB
                    for (int j = KB[i].Implications.Count - 1; j >= 0; j--)
                    {
                        // if (KB[i].Symbols[j].Name == knownFacts[p].Name)
                        if (KB[i].Implications[j].Name == q)
                        {
                            // add the symbols to the known facts
                            for (int k = KB[i].Symbols.Count - 1; k >= 0; k--)
                            {
                                bool alreadyAdded = false;

                                // need to check if the symbol has been added already
                                foreach (Symbol s in BCSearchFacts)
                                {
                                    if (s.Name == KB[i].Symbols[k].Name)
                                    {
                                        alreadyAdded = true;
                                        break;
                                    }
                                }

                                // if the symbol has already been added
                                // do no add it. Just remove it.
                                if (alreadyAdded == true)
                                {
                                    KB[i].Symbols.RemoveAt(k);
                                }
                                else
                                {
                                    BCSearchFacts.Add(KB[i].Symbols[k]);
                                    //KB[i].Symbols.RemoveAt(k);
                                }
                            }
                            KB.RemoveAt(i);
                        }
                    }
                }
            }

            //test for branch

            // wait until all the facts have been searched
            while (BCSearchFacts.Count > 0)
            {
                int BCCountStore = 0;
                for (int p = BCSearchFacts.Count - 1; p >= 0; p--)
                {
                    // need to pop the search fact
                    Symbol symbolP = BCSearchFacts[p];
                    BCSearchFacts.RemoveAt(p);

                    // check if item is in final return list
                    bool listCheck = false;
                    foreach (Symbol s in returnFacts)
                    {
                        if (s.Name == symbolP.Name)
                        {
                            listCheck = true;
                        }
                    }

                    // if the item is not in the list, add it
                    //if (!(listCheck))
                    //{
                    //    returnFacts.Add(symbolP);
                    //}

                    for (int i = KB.Count - 1; i >= 0; i--)
                    {
                        // while there are still implications in that KB sentence
                        if (KB[i].Implications.Count > 0)
                        {
                            // if those symboles appear in the known facts,
                            // add them to the known fact list and remove from KB
                            for (int j = KB[i].Implications.Count - 1; j >= 0; j--)
                            {
                                // if (KB[i].Symbols[j].Name == knownFacts[p].Name)
                                if (KB[i].Implications[j].Name == symbolP.Name)
                                {
                                    
                                    bool alreadyAdded = false;

                                    // need to check if the symbol has been added already
                                    //foreach (Symbol s in BCSearchFacts)
                                    //{
                                    //    if (s.Name == KB[i].Symbols[j].Name)
                                    //    {
                                    //        alreadyAdded = true;
                                    //        break;
                                    //    }
                                    //}

                                    for (int k = KB[i].Symbols.Count - 1; k >= 0; k--)
                                    {
                                        // if the symbol has already been added
                                        // do no add it. Just remove it.
                                        if (alreadyAdded == true)
                                        {
                                            KB[i].Symbols.RemoveAt(k);
                                        }
                                        else
                                        {
                                            BCSearchFacts.Add(KB[i].Symbols[k]);
                                            returnFacts.Add(KB[i].Symbols[k]);
                                            BCCountStore++;
                                            KB[i].Symbols.RemoveAt(k);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // The implications are now the onlt things left. Meaning 
                            // the symbols were all known facts. Therefore the implications
                            // are known facts as well.
                            for (int j = KB[i].Symbols.Count - 1; j >= 0; j--)
                            {
                                knownFacts.Add(KB[i].Symbols[j]);
                                KB[i].Symbols.RemoveAt(j);
                            }

                            KB.RemoveAt(i);
                        }
                    }

                    if (BCCountStore == 0)
                    {
                        BCSearchFacts.Add(symbolP);
                        return false;
                    }
                    else
                    {
                        returnFacts.Add(new Symbol(q));
                    }

                }



            }


            // do a check for a solution
            foreach (Symbol knownFact in knownFacts)
            {
                foreach (Symbol searchFact in returnFacts)
                {
                    // check if a known fact is in the search
                    // if it is, a solution has been found.
                    if (knownFact.Name == searchFact.Name)
                    {
                        return true;
                    }
                }
            }

            // test failed
            return false;
        }
    }
}


#region OldCode

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace InferenceEngine
//{
//    public class BackwardChaning : PropositionalAlgorithm
//    {
//        public BackwardChaning()
//        {
//            code = "BC";
//            longName = "Backward Chaining";
//        }

//        /*
//         * Running iengine with method BC on the example test file above should produce the following output:
//         * > YES: p2, p3, p1, d 
//         */

//        public bool RunAlgorithm(string q)
//        {
//            // define "fact" statements
//            // must loop backwards as items will be removed
//            for (int i = KB.Count - 1; i >= 0; i--)
//            //foreach (Sentence s in KB)
//            {
//                // add the facts to the list
//                knownFacts.Insert(0, KB[i].FactCheck());

//                // due to need for the "factcheck" function to return a bool,
//                // we need to remove the "null"
//                if (knownFacts[0] == null)
//                {
//                    knownFacts.RemoveAt(0);
//                }
//                else
//                {
//                    // remove the facts for the knowledge base
//                    KB.RemoveAt(i);
//                }
//            }

//            // update the other facts accordingly.
//            foreach (Sentence s in KB)
//            {
//                // go through and update "isFact" property
//                // this is not needed for this assignement, but may
//                // be used later for "extension" work
//                s.UpdateFacts(knownFacts);
//            }

//            List<Symbol> BCSearchFacts = new List<Symbol>();

//            // add the query to the return list
//            returnFacts.Add(new Symbol(q));

//            // need to search through the KB till query is found in implications.
//            for (int i = KB.Count - 1; i >= 0; i--)
//            {
//                // while there are still implications in that KB sentence
//                if (KB[i].Implications.Count > 0)
//                {
//                    // if those symboles appear in the known facts,
//                    // add them to the known fact list and remove from KB
//                    for (int j = KB[i].Implications.Count - 1; j >= 0; j--)
//                    {
//                        // if (KB[i].Symbols[j].Name == knownFacts[p].Name)
//                        if (KB[i].Implications[j].Name == q)
//                        {
//                            List<Symbol> found = new List<Symbol>();
//                            //start the search through KB and KnownFacts
//                            // add the symbols to the known facts
//                            for (int k = KB[i].Symbols.Count - 1; j >= 0; j--)
//                            {
//                                //Add to the found list
//                                found.Add(KB[i].Symbols[k]);
//                                KB[i].Symbols.RemoveAt(k);
//                            }

//                            if (KB[i].Symbols.Count == 0)
//                            {
//                                bool alreadyAdded = false;
//                                // need to check if the symbol has been added already
//                                foreach (Symbol s in found)
//                                {
//                                    //foreach (Symbol symbol in found )
//                                    //if (s.Name == symbol.Name)
//                                    //{
//                                    //    alreadyAdded = true;
//                                    //    break;
//                                    //}

//                                    //if (!(alreadyAdded))
//                                    //{
//                                    BCSearchFacts.Add(s);

//                                    //}





//                                }

//                                KB.RemoveAt(i);

//                            }

//                        }
//                    }
//                }
//            }

//            // wait until all the facts have been searched
//            while (BCSearchFacts.Count > 0)
//            {
//                for (int p = BCSearchFacts.Count - 1; p >= 0; p--)
//                {
//                    // need to pop the search fact
//                    Symbol symbolP = BCSearchFacts[p];
//                    BCSearchFacts.RemoveAt(p);

//                    // check if item is in final return list
//                    bool listCheck = false;
//                    foreach (Symbol s in returnFacts)
//                    {
//                        if (s.Name == symbolP.Name)
//                        {
//                            listCheck = true;
//                        }
//                    }

//                    // if the item is not in the list, add it
//                    if (!(listCheck))
//                    {
//                        returnFacts.Add(symbolP);
//                    }

//                    for (int i = KB.Count - 1; i >= 0; i--)
//                    {
//                        // while there are still implications in that KB sentence
//                        if (KB[i].Implications.Count > 0)
//                        {
//                            // if those symboles appear in the known facts,
//                            // add them to the known fact list and remove from KB
//                            for (int j = KB[i].Implications.Count - 1; j >= 0; j--)
//                            {
//                                // if (KB[i].Symbols[j].Name == knownFacts[p].Name)
//                                if (KB[i].Implications[j].Name == symbolP.Name)
//                                {
//                                    bool alreadyAdded = false;

//                                    // need to check if the symbol has been added already
//                                    foreach (Symbol s in BCSearchFacts)
//                                    {
//                                        if (s.Name == KB[i].Symbols[j].Name)
//                                        {
//                                            alreadyAdded = true;
//                                            break;
//                                        }
//                                    }

//                                    // if the symbol has already been added
//                                    // do no add it. Just remove it.
//                                    if (alreadyAdded == true)
//                                    {
//                                        KB[i].Symbols.RemoveAt(j);
//                                    }
//                                    else
//                                    {
//                                        BCSearchFacts.Add(KB[i].Symbols[j]);
//                                        returnFacts.Add(KB[i].Symbols[j]);

//                                        KB[i].Symbols.RemoveAt(j);
//                                    }
//                                }
//                            }
//                        }
//                        else
//                        {
//                            // The implications are now the onlt things left. Meaning 
//                            // the symbols were all known facts. Therefore the implications
//                            // are known facts as well.
//                            for (int j = KB[i].Symbols.Count - 1; j >= 0; j--)
//                            {
//                                knownFacts.Add(KB[i].Symbols[j]);
//                                KB[i].Symbols.RemoveAt(j);
//                            }

//                            KB.RemoveAt(i);
//                        }
//                    }
//                }

//                // do a check for a solution
//                foreach (Symbol knownFact in knownFacts)
//                {
//                    foreach (Symbol searchFact in BCSearchFacts)
//                    {
//                        // check if a known fact is in the search
//                        // if it is, a solution has been found.
//                        if (knownFact.Name == searchFact.Name)
//                        {
//                            return true;
//                        }
//                    }
//                }
//            }

//            // test failed
//            return false;
//        }
//    }
//}
#endregion