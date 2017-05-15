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

        /*
         * When the method is TT and the answer is YES, it should be
         * followed by a colon (:) and the number of models of KB. When the method is FC or BC and the answer is
         * YES, it should be followed by a colon (:) and the list of propositional symbols entailed from KB that has been
         * found during the execution of the specified algorithm.
         * For example, running iengine with method TT on the example test1.txt file should produce the
         * following output:
         * > YES: 3 
         */

        // Datatable?

        public bool CheckEntails(string q)
        {
            return false;
        }

        public bool CheckAll(string q)
        { 
            return false;
        }
    }
}
