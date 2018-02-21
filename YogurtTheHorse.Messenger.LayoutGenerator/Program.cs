using System;

namespace YogurtTheHorse.Messenger.LayoutGenerator {
    public class Program {
        static void Main(string[] args) {
            if (args.Length == 0) {
                Console.WriteLine("Speciefy template.");
                return;
            }

            Generator gen = new Generator(args[0]);



#if DEBUG
            gen.Generate();
#else
            try {
                gen.Generate();
            } catch (Exception ex) {
                Console.WriteLine("Exception while parsing: {0}", ex);
                Console.ReadLine();
            }
#endif
        }
    }
}
