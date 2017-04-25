using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
