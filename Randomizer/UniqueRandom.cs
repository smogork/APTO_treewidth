using System;
using System.Collections;

namespace Randomizer
{
    /// <summary>
    /// KLasa losuje liczby całkowite z zakresu od 0 do K - 1 bez powtórzeń.
    /// Przy K + 1 losowaniu zwracany jest wyjątek OverflowException
    /// </summary>
    public class UniqueRandom
    {
        public int K { get; private set; }
        private Random rand;
        private BitArray occurences;
        private int random_counter;
        private int seed;

        public UniqueRandom(int k)
        {
            K = k;
            rand = new Random();
            seed = rand.Next();

            RepetableReset();
        }
        
        public UniqueRandom(int k, int seed)
        {
            K = k;
            this.seed = seed;

            RepetableReset();
        }

        public void RepetableReset()
        {
            rand = new Random(seed);
            NonrepetableReset();
        }

        public void NonrepetableReset()
        {
            occurences = new BitArray(K);
            random_counter = 0;
        }

        public int Next()
        {
            random_counter++;
            if (random_counter > K)
                throw new OverflowException("Randomizer is empty");
            
            int idx = rand.Next(K);

            while (occurences[idx])
                idx = (idx + 1) % K;
            occurences[idx] = true;

            return idx;
        }
        
    }
}