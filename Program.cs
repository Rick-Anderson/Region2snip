﻿public class RegionReplacer
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine($"Usage:{AppDomain.CurrentDomain.FriendlyName} c# file path");
            return;
        }
        var fileCopy = args[0] + ".cs";
        Console.WriteLine(args[0]);

        string[] lines = File.ReadAllLines(args[0]);

        var convertedLines = ConvertRegions(lines);

        File.WriteAllLines(fileCopy, convertedLines);
    }

    public static List<string> ConvertRegions(string[] lines)
    {
        List<string> result = new List<string>();
        Stack<string> regionNames = new Stack<string>();

        foreach (var line in lines)
        {
            if (line.Trim().StartsWith("#region"))
            {
                var snippet = line.Split(new[] { "#region " }, StringSplitOptions.None);
                if (snippet.Length > 1)
                {
                    var regionName = snippet[1].Trim();
                    regionNames.Push(regionName);
                    result.Add($"// <{regionName}>");
                }
            }
            else if (line.Trim().StartsWith("#endregion"))
            {
                if (regionNames.Count > 0)
                {
                    var regionName = regionNames.Pop();
                    result.Add($"// </{regionName}>");
                }
            }
            else
            {
                result.Add(line);
            }
        }

        return result;
    }
}
