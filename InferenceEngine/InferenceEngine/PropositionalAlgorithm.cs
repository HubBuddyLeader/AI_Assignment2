using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public abstract class PropositionalAlgorithm
    {
        public string code;
        public string longName; // Don't know if this is really needed...

        // Add general abstract algorithm methods here...

        // Each Algorithms description is also in the class file, this is just an overview.
        /*
        Standard output is an answer of the form YES or NO, depending on whether the ASK(ed) query q follows
        from the TELL(ed) knowledge base KB. When the method is TT and the answer is YES, it should be
        followed by a colon (:) and the number of models of KB. When the method is FC or BC and the answer is 
        YES, it should be followed by a colon (:) and the list of propositional symbols entailed from KB that has been
        found during the execution of the specified algorithm.
        For example, running iengine with method TT on the example test1.txt file should produce the
        following output:
        > YES: 3
        On the other hand, when I run my implementation of iengine with method FC on the example test1.txt
        it produces the following output:
        > YES: a, b, p2, p3, p1, d
        Note that your implementation might produce a different output and it would still be correct, depending on the
        order of the sentences in the knowledge base. I’ll check that carefully to ensure that you won’t be
        disadvantaged if your results don’t look exactly the same as my results.
        And running iengine with method BC on the example test file above should produce the following output:
        > YES: p2, p3, p1, d 
         */
    }
}
