using System;
using System.IO;

namespace METL.InjectorSamples.PE32
{
    class Program
    {
        static void Main(string[] args)
        {
            [PLACEHOLDER]

            Console.WriteLine($"0wn3d by METL on {DateTime.Now}");

            System.IO.File.WriteAllBytes(Path.GetRandomFileName(), Convert.FromBase64String(malSource));
        }
    }
}