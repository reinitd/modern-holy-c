namespace Compiler.Types
{
    internal class Method
    {
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public string ArgConstruct { get; set; } 
        public List<string> CVersion { get; set; } = new List<string>();
        public int CurrentWhitespaceCount { get; set; }

        public Method(List<string> MethodString)
        {
            Name = MethodString[0].Split(" ")[1].Split("(")[0];
            Console.WriteLine(Name);
            ReturnType = HCToC(MethodString[0].Split(" ")[0]);

            string[] arguments = MethodString[0].Replace(")", "").Split(", ");
            ArgConstruct = ArgConstruct + arguments[arguments.Length - 1];
            for(int i = 2; i < arguments.Length; i++)
            {
                ArgConstruct = ArgConstruct + ", " + arguments[i];
            }

            CVersion.Add($"{ReturnType} {Name}({ArgConstruct})");

            CurrentWhitespaceCount = 1;

            for(int i = 1; i < MethodString.Count; i++)
            {
                string line = MethodString[i].Trim();
                Console.WriteLine(line);

                if(line.Contains("outp"))
                {
                    string[] args = line.Split("(")[1].Replace("(", "").Replace(");", "").Split(", ");
                    string toadd = "";

                    toadd = toadd + BuildWhiteSpace() + $"printf({args[0]}, {args[1]});";
                    CVersion.Add(toadd);
                    continue;
                }

                if(line.StartsWith("\""))
                {
                    CVersion.Add(BuildWhiteSpace() + $"printf(\"{line.Split("\"")[1]}\");");
                    continue;
                }

                if(line.Contains("I64") || line.Contains("I32") || line.Contains("string") || line.Contains("double") || line.Contains("char"))
                {
                    string[] sided = line.Split('=');
                    string[] init = sided[0].Split(' ');

                    Variable var = new Variable();
                    var.Name = init[1].Replace("*", "");
                    var.T = ParseType(init[0]);
                    
                    if(init[1].Contains("*"))
                    {
                        var.IsPointer = true;
                    }

                    if(sided[sided.Length - 1].Contains("(") && !sided[sided.Length - 1].Contains("\""))
                    {
                        var.MethodRet = true;
                    }

                    if(var.MethodRet == false)
                    {
                        switch(var.T)
                        {
                            case MHCType.I64:
                            {
                                var.SharpVal = int.Parse(sided[sided.Length - 1].Trim().Replace(";", ""));
                                var.ParsedValue = sided[1].Trim().Replace(";", "");
                                break;
                            }

                            case MHCType.String:
                            {
                                //Account for if they have '=' in the string
                                string val = "\"" + line.Split("\"")[1] + "\"";
                                var.ParsedValue = val;
                                var.SharpVal = line.Split("\"")[1];
                                break;
                            }

                            case MHCType.I32:
                            {
                                var.SharpVal = float.Parse(sided[sided.Length - 1].Trim().Replace(";", ""));
                                var.ParsedValue = sided[1].Trim();
                                break;
                            }
                        }
                    } else {
                        var.SharpVal = null;
                        var.ParsedValue = sided[sided.Length - 1].Trim().Replace(";", "");
                    }

                    AddVariable(var);
                }

                if(line.Contains("ret"))
                {
                    string toadd = "";
                    toadd = BuildWhiteSpace() + $"return {line.Replace("ret ", "")}";
                    CVersion.Add(toadd);
                }
            }

            CVersion[0] = CVersion[0] + " {";
            CVersion.Add("}");    
        }

        public string BuildWhiteSpace()
        {
            string ret = "";
            for(int i = 0; i <= CurrentWhitespaceCount; i++)
            {
                ret = ret + CBuilder.Indent;
            }

            return ret;
        }
        public void AddVariable(Variable var)
        {
            string toadd = "";

            if(var.MethodRet == false)
            {
                switch(var.T)
                {
                    case MHCType.String:
                    {
                        string val = (string)var.SharpVal;

                        if(!var.IsPointer)
                        {
                            toadd = toadd + BuildWhiteSpace() + $"char {var.Name}[{val.Length}] = {var.ParsedValue};";
                        } else
                        {
                            toadd = toadd + BuildWhiteSpace() + $"char *{var.Name}[{val.Length}] = {var.ParsedValue};";
                        }

                        CVersion.Add(toadd);
                        break;
                    }

                    case MHCType.I64:
                    {
                        int val = (int)var.SharpVal;
                        
                        if(!var.IsPointer)
                        {
                            toadd = toadd + BuildWhiteSpace() + $"int {var.Name} = {var.ParsedValue};";
                        } else 
                        {
                            toadd = toadd + BuildWhiteSpace() + $"int *{var.Name} = {var.ParsedValue};";
                        }

                        CVersion.Add(toadd);
                        break;
                    }

                    case MHCType.I32:
                    {
                        float val = (float)var.SharpVal;

                        if(!var.IsPointer)
                        {
                            toadd = toadd + BuildWhiteSpace() + $"float {var.Name} = {var.ParsedValue}";
                        } else 
                        {
                            toadd = toadd + BuildWhiteSpace() + $"float *{var.Name} = {var.ParsedValue}";
                        }

                        CVersion.Add(toadd);
                        break;
                    }
                }
            } else
            {
                switch(var.T)
                {
                    case MHCType.String:
                    {
                        string val = (string)var.SharpVal;

                        toadd = toadd + BuildWhiteSpace() + $"char *{var.Name} = {var.ParsedValue};";

                        CVersion.Add(toadd);
                        break;
                    }

                    case MHCType.I64:
                    {
                        int val = (int)var.SharpVal;
                        
                        if(!var.IsPointer)
                        {
                            toadd = toadd + BuildWhiteSpace() + $"int {var.Name} = {var.ParsedValue};";
                        } else 
                        {
                            toadd = toadd + BuildWhiteSpace() + $"int *{var.Name} = {var.ParsedValue};";
                        }

                        CVersion.Add(toadd);
                        break;
                    }

                    case MHCType.I32:
                    {
                        float val = (float)var.SharpVal;

                        if(!var.IsPointer)
                        {
                            toadd = toadd + BuildWhiteSpace() + $"float {var.Name} = {var.ParsedValue}";
                        } else 
                        {
                            toadd = toadd + BuildWhiteSpace() + $"float *{var.Name} = {var.ParsedValue}";
                        }

                        CVersion.Add(toadd);
                        break;
                    }
                }
            }
        }

        public string HCToC(string val)
        {
            switch(val)
            {
                case "I64":
                {
                    return "int";
                }
                case "I32":
                {
                    return "float";
                }
                case "string":
                {
                    return "char*";
                }
            }

            return val;
        }

        public MHCType ParseType(string type)
        {
            switch(type)
            {
                case "string":
                {
                    return MHCType.String;
                }
                case "I64":
                {
                    return MHCType.I64;
                }
                case "I32":
                {
                    return MHCType.I32;
                }
                case "double": 
                {
                    return MHCType.Decimal;
                }
                case "char":
                {
                    return MHCType.Char;
                }
            }

            return MHCType.Void;
        }
    }

    internal class Variable
    {
        public string Name { get; set; }
        public MHCType T { get; set; }
        public bool IsPointer { get; set; }
        public object? SharpVal { get; set; }
        public string? ParsedValue { get; set; }
        public bool MethodRet { get; set; } = false;
    }

    public enum MHCType
    {
        I64,
        I32,
        Void,
        Decimal,
        Char,
        String
    }
}