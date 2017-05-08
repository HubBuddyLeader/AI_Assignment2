using System;
using System.Collections.Generic;

namespace InferenceEngine
{
    public abstract class PropositionalAlgorithm
    {
        public string code;
        public string longName;
        public List<Sentence> KB;
        public List<Symbol> knownFacts = new List<Symbol>();
        public List<Symbol> returnFacts = new List<Symbol>();

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

        public void OrganiseKBData(List<string> split)
        {
            List<Sentence> SentenceToReturn = new List<Sentence>();

            for (int i = 0; i < split.Count - 1; i++)
            {
                // segment applies to each statment before a semicolon.
                string segment = split[i].Replace(" ", string.Empty);

                // character array stores each character in a segment.
                char[] character = segment.ToCharArray();

                SentenceToReturn.Add(new Sentence());

                for (int j = 0; j < character.Length; j++)
                {
                    // define the current character and next character in array.
                    int currentChar = Char.ToLower(character[j]);
                    int nextChar = 1;

                    // make sure we get an out of bounds exception.
                    if (j < character.Length - 1)
                    {
                        // here we'll store the next character.
                        nextChar = Char.ToLower(character[j + 1]);
                    }

                    // if (currentChar == '=' && nextChar == '>' || currentChar == '&')
                    // check for implication
                    if (currentChar == '=' && nextChar == '>')
                    {
                        // here we'll store operators.
                        SentenceToReturn[i].AddOperator((character[j].ToString() + character[j+1].ToString()).ToString());
                        j++;
                        continue;
                    }

                    if ((currentChar == '>') || (currentChar == '&'))
                    {
                        // need this to store operators.
                        SentenceToReturn[i].AddOperator(character[j].ToString());
                        continue;
                    }

                    // while the next character is not a symbol, build a word.
                    string symbolName = "";
                    int symbolCounter = 0;

                    // for some reason, '&' is not a symbol? 
                    // check if the next character is a symbol
                    while (!(((Char.IsSymbol(character[j + symbolCounter]))) || (character[j + symbolCounter].ToString() == "&"))) 
                    {
                        symbolName = symbolName + character[j + symbolCounter].ToString();
                        symbolCounter++;

                        // we found the end of the array
                        if ((j + symbolCounter) >= (character.Length))
                        {
                            break;
                        }
                    }
                    
                    j = j + symbolCounter - 1 ;

                    if (SentenceToReturn[i].CheckForImplicationSymbol())
                    {
                        SentenceToReturn[i].AddImplications(new Symbol(symbolName));
                    }
                    else
                    {
                        SentenceToReturn[i].AddSymbol(new Symbol(symbolName));
                    }
                }
            }

            SentenceToReturn.Reverse();
            KB =  SentenceToReturn;
        }

        /// <summary>
        /// Using the string in KB, operate on the appropriate values. Allows
        /// definition of operators from strings. Can be expanded as required.
        /// </summary>
        /// <param name="op"></param>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        /// <returns></returns>
        public static bool Operator(string op, bool op1, bool op2)
        {
            switch (op)
            {
                case "&": return op1 && op2;
                case "||": return op1 || op2;
                case "=>": return (op1 && true); // return the value of op1
                default: throw new Exception("Invalid Operator!");
            }
        }

    }
}
