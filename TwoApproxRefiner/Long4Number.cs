using System;
using System.Security.Cryptography;

namespace TwoApproxRefiner
{
    /// <summary>
    /// Klasa reprezentująca liczbe w systemie czwórkowym.
    /// Indeks 0 reprezentuje bit najmłodszy.  
    /// </summary>
    public class Long4Number
    {
        public byte[] Representation { get; private set; }
        public int Length => Representation.Length;
        
        /// <summary>
        /// Tworzy liczbę 0 o zadanej ilości zanków.
        /// </summary>
        /// <param name="length">Długośc reprezentacji liczby w systemie czwórkowym.</param>
        public Long4Number(int length)
        {
            Representation = new byte[length];
        }
        
        /// <summary>
        /// Zwiększa liczbę o 1.
        /// </summary>
        /// <returns>True - udana inkrementacja, false - overflow</returns>
        public bool Increment()
        {
            int i = 0;
            bool carriage = false;
            do
            {
                Representation[i]++;

                if (Representation[i] >= 4)
                {
                    carriage = true;
                    Representation[i] = 0;
                    i++;
                }
                else
                    carriage = false;
            } while (carriage && i < Representation.Length);

            if (i == Representation.Length)
                return false;
            return true;
        }
        
       
    }
}