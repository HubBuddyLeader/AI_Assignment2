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
    }
}
