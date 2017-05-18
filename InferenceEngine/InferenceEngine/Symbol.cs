using System;
using System.Collections.Generic;

namespace InferenceEngine
{
    public class Symbol
    {
        /* A symbol knows: 
         *     1. order it was added to the stack
         *     2. if it is fact
         *     3. its "string" name
         */

        // good practice to keep variables private 
        private int orderAdded;
        private bool isFact;
        private string name;

        public Symbol(string n)
        {
            name = n;
            isFact = false;
            orderAdded = 0;
        }

        /// <summary>
        /// Setter/Getter for Name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Setter/Getter for IsFact
        /// </summary>
        public bool IsFact
        {
            get { return isFact; }
            set { isFact = value; }
        }

        /// <summary>
        /// Setter/Getter for Order
        /// </summary>
        public int Order
        {
            get { return orderAdded; }
            set { orderAdded = value; }
        }
    }
}
