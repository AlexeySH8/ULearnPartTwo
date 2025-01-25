using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree;

public class Root
{
    public Dictionary<string, Root> Items = new Dictionary<string, Root>();
}

public class DiskTreeTask
{
    public static List<string> Solve(List<string> input)
    {
        var roots = CreateRoots(input);
        return CreateListFromNode(roots, "");
    }

    private static Root CreateRoots(List<string> input)
    {
        var roots = new Root();
        foreach (var path in input)
        {
            var folders = path.Split(new[] { '\\' },
                StringSplitOptions.RemoveEmptyEntries);
            var root = roots;
            for (int i = 0; i < folders.Length; i++)
            {
                if (!root.Items.ContainsKey(folders[i]))
                    root.Items[folders[i]] = new Root();
                root = root.Items[folders[i]];
            }
        }
        return roots;
    }

    public static List<string> CreateListFromNode(Root roots, string prefix)
    {
        var result = new List<string>();

        if (roots.Items.Count == 0)
            return result;

        var listOfRoots = roots.Items.Keys.ToList();
        listOfRoots.Sort((first, second) => string.CompareOrdinal(first, second));

        foreach (var root in listOfRoots)
        {
            result.Add(prefix + root);
            result.AddRange(
                CreateListFromNode(roots.Items[root], prefix + " "));
        }

        return result;
    }
}
