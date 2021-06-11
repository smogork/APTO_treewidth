using System;
using CommonCode;
using CommonCode.Converters;
using Randomizer.GraphRandomizers;

namespace Randomizer
{
    class Program
    {
        static void Main(string[] args)
        {
            PaceOutputGraph output = new PaceOutputGraph();
            SimpleGraphRandomizer randomizer = new SimpleGraphRandomizer(4, 6);

            output.Write(Console.OpenStandardOutput(), randomizer.Randomize());

            UniqueRandom rand = new UniqueRandom(10);

            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());

            rand.NonrepetableReset();
            Console.WriteLine("Reset");
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            Console.WriteLine(rand.Next());
            try
            {
                Console.WriteLine(rand.Next());
            }
            catch (OverflowException e)
            {
                Console.WriteLine("Overflow exception occured");
            }
        }
    }
}