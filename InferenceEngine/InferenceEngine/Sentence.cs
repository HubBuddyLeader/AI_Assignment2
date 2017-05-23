using System;
using System.Collections.Generic;

namespace InferenceEngine
{
    public class Sentence
    {
        // Sentence structs 
        // [Symbol] [Operator] [Symbol] => [Implication]

        List<Symbol> symbols;       //  left side of clause
        List<string> operators;     //  operators within clause (left side only)
        List<Symbol> implications;  //  right side of clause

        public Sentence()
        {
            symbols = new List<Symbol>();
            operators = new List<string>();
            implications = new List<Symbol>();
        }

        /// <summary>
        /// Push a symbol onto the stack
        /// </summary>
        /// <param name="s"></param>
        public void AddSymbol (Symbol s)
        {
            symbols.Add(s);
        }

        /// <summary>
        /// Push operator onto the stack
        /// </summary>
        /// <param name="op"></param>
        public void AddOperator (string op)
        {
            operators.Add(op);
        }
        
        /// <summary>
        /// Push implication onto the stack
        /// </summary>
        /// <param name="s"></param>
        public void AddImplications(Symbol s)
        {
            implications.Add(s);
        }

        public bool CheckForImplicationSymbol()
        {
            if (operators.Contains("=>"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Symbols require read only access. Does not require setter access.
        /// </summary>
        public List<Symbol> Symbols
        {
            get { return symbols; }
            set { symbols = value;  }
        }

        /// <summary>
        /// Symbols require read only access. Does not require setter access.
        /// </summary>
        public List<string> Operators
        {
            get { return operators; }

        }
        /// <summary>
        /// Symbols require read only access. Does not require setter access.
        /// </summary>
        public List<Symbol> Implications
        {
            get { return implications; }
        }

        public void UpdateFacts(List<Symbol> knownFacts)
        {
            // update implications
            foreach (Symbol s in implications)
            {
                foreach (Symbol a in knownFacts)
                {
                    if (s.Name == a.Name)
                    {
                        s.IsFact = true;
                    }
                }
            }

            // update the values in symbols
            foreach (Symbol s in symbols)
            {
                foreach (Symbol a in knownFacts)
                {
                    if (s.Name == a.Name)
                    {
                        s.IsFact = true;
                    }
                }
            }
        }

        public Symbol FactCheck()
        {
            /* defines a "fact":
             *   Not implications
             *   No Operators 
             *   1 Symbol  
             */
            if ((implications.Count == 0) && (operators.Count == 0) && (symbols.Count == 1))
            {
                // define symbol as fact. Avoid exceptions.
                foreach (Symbol s in symbols)
                {
                    // set fact
                    s.IsFact = true;
                    return s;
                }
            }
            return null;
        }
    }
}
