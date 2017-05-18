using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class SingleSymbol
    {
        /* A SingleSymbol:
         *      1. Is a string version of a Symbol.
         *      2. Has a boolean value assosiated to it. 
         *      3. Is a truth table object.
        */
        private string name;
        private bool truth;
        private int increment;

        public SingleSymbol(string n, int i)
        {
            name = n;
            truth = false;
            increment = i;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool Truth
        {
            get { return truth; }
            set { truth = value; }
        }

        public int Increment
        {
            get { return increment; }
            set { increment = value; }
        }

        public void FlipState()
        {
            if (truth == true)
                truth = false;
            else
                truth = true;
        }
    }
}
