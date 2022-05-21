using Compiler.Types;

namespace Compiler
{
    internal class EntryPoint
    {
        public static async Task Main(params string[] args)
        {
            if(args.Length == 0)
            {
                Environment.Exit(0);
            }

            Reader r = new Reader(args[0], args[1]);
            await r.StartRead();       
        }
    }
}