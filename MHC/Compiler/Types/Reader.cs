namespace Compiler.Types
{
    internal class Reader
    {
        public string FilePath { get; set; }
        public List<string> Contents { get; set; }
        public int CurrentWhitespaceCount { get; set; }

        public Reader(string path)
        {
            FilePath = path;
            Contents = File.ReadAllLines(FilePath).ToList();
        }

        public async Task StartRead()
        {
            //Add logic

            await File.AppendAllTextAsync("output.c", "#include <stdio.h>\n");
            await File.AppendAllTextAsync("output.c", "#include <stdlib.h>\n");

            for(int i = 0; i < Contents.Count; i++)
            {
                if(Contents[i].Contains("(") && Contents[i + 1].Contains("{") || Contents[i].Contains("{") && Contents[i].Contains("("))
                {
                    List<string> MethodString = new();
                    for(int x = i; x < Contents.Count; x++)
                    {
                        if(Contents[x] == "}")
                        {
                            break;
                        }

                        MethodString.Add(Contents[x]);
                    }

                    Method method = new Method(MethodString);

                    foreach(string val in method.CVersion)
                    {
                        Console.WriteLine(val);
                    }

                    await File.AppendAllLinesAsync("output.c", method.CVersion);
                }
            }
        }
    }
}