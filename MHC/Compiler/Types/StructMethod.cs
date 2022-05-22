//Add logic

namespace Compiler.Types
{
    internal class StructMethod
    {
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public List<Variable> Vars { get; set; }
        public List<string> CVersion { get; set; } = new List<string>();
        public int CurrentWhitespaceCount { get; set; }

        public StructMethod(List<string> raw)
        {
            CurrentWhitespaceCount = 1;
        }
    }

    internal struct Struct
    {
        public string Name { get; set; }
        internal List<Variable> vars { get; set; }
    }
}