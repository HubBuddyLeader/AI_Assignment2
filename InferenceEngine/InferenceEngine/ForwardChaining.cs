using System;
using System.Collections.Generic;

namespace InferenceEngine
{
    public class ForwardChaining : PropositionalAlgorithm
    {
        public ForwardChaining()
        {
            code = "FC";
            longName = "Forward Chaining";
        }

        /*
         * Standard output is an answer of the form YES or NO, depending on whether the ASK(ed) query q follows
         * from the TELL(ed) knowledge base KB.
         * 
         * On the other hand, when I run my implementation of iengine with method FC on the example test1.txt
         * it produces the following output:
         * > YES: a, b, p2, p3, p1, d
         * Note that your implementation might produce a different output and it would still be correct, depending on the
         * order of the sentences in the knowledge base. I’ll check that carefully to ensure that you won’t be
         * disadvantaged if your results don’t look exactly the same as my results. 
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

            
            // if the known facts are 0, then there is no solution
            while ((knownFacts.Count > 0) )
            {
                // loop through the known facts
                for (int p = knownFacts.Count - 1; p >= 0; p--)
                {
                    Symbol symbolP = knownFacts[p];
                    knownFacts.RemoveAt(p);

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
                    if (!(listCheck))
                    {
                        returnFacts.Add(symbolP);
                    }

                    // checks for solution
                    //if (knownFacts[p].Name == q)
                    if (symbolP.Name == q)
                    {
                        // solution found
                        return true;
                    }
                    else
                    {
                        // loop the KB (backwards)
                        for (int i = KB.Count - 1; i >= 0; i-- )
                        {
                            // while there are still symbols in that KB sentence
                            if (KB[i].Symbols.Count > 0)
                            {
                                // if those symboles appear in the known facts,
                                // add them to the known fact list and remove from KB
                                for (int j = KB[i].Symbols.Count - 1; j >= 0 ; j--)
                                {
                                    // if (KB[i].Symbols[j].Name == knownFacts[p].Name)
                                    if (KB[i].Symbols[j].Name == symbolP.Name)
                                    {
                                        // add to the known facts
                                        knownFacts.Add(KB[i].Symbols[j]);
                                        // remove from the KB Symbols
                                        KB[i].Symbols.RemoveAt(j);
                                    }
                                }
                            }
                            else
                            {
                                // The implications are now the onlt things left. Meaning 
                                // the symbols were all known facts. Therefore the implications
                                // are known facts as well.
                                knownFacts.Add(KB[i].Implications[0]);
                                KB.RemoveAt(i);
                            }
                        }
                    }
                }
            }
            // test failed
            return false;
        }
    }
}
