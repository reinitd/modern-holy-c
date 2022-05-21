namespace Compiler.Types
{
    internal class Reader
    {
        public string FilePath { get; set; }
        public string OutPath { get; set; }
        public List<string> Contents { get; set; }
        public int CurrentWhitespaceCount { get; set; }

        public Reader(string path, string outp)
        {
            FilePath = path;
            Contents = File.ReadAllLines(FilePath).ToList();
            OutPath = outp;
        }

        public async Task StartRead()
        {
            await File.AppendAllTextAsync(OutPath, "#include <stdio.h>\n");
            await File.AppendAllTextAsync(OutPath, "#include <stdlib.h>\n");

            for(int i = 0; i < Contents.Count; i++)
            {
                if(Contents[i].StartsWith("strct"))
                {
                    //Add logic
                    continue;
                }

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
                        i++;
                    }

                    Method method = new Method(MethodString);

                    foreach(string val in method.CVersion)
                    {
                        Console.WriteLine(val);
                    }

                    await File.AppendAllLinesAsync(OutPath, method.CVersion);
                }
            }
        }
    }
}