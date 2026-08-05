using System;
using System.Collections.Generic;

public class TreeNode
{
    public string Value { get; set; }
    public List<TreeNode> Children { get; } = new List<TreeNode>();

    public TreeNode(string value) => Value = value;

    public TreeNode AddChild(TreeNode child)
    {
        Children.Add(child);
        return this;
    }
}

public static class TreeFlattener
{
    public static List<string> FlattenTree(params TreeNode[] roots)
    {
        var result = new List<string>();

        void Traverse(TreeNode node, ref int depth)
        {
            Console.WriteLine($"{node.Value}: depth {depth}");
            result.Add(node.Value);

            foreach (var child in node.Children)
            {
                int childDepth = depth + 1;
                Traverse(child, ref childDepth);
            }
        }

        foreach (var root in roots)
        {
            int depth = 0;
            Traverse(root, ref depth);
        }

        return result;
    }
}

class Program
{
    static void Main()
    {
        var root1 = new TreeNode("A");
        root1.AddChild(new TreeNode("A1"));
        root1.AddChild(new TreeNode("A2"));

        var b1 = new TreeNode("B1");
        b1.AddChild(new TreeNode("B1a"));
        b1.AddChild(new TreeNode("B1b"));
        var root2 = new TreeNode("B");
        root2.AddChild(b1);

        var root3 = new TreeNode("C");

        var flattened = TreeFlattener.FlattenTree(root1, root2, root3);

        Console.WriteLine("[" + string.Join(", ", flattened) + "]");
    }
}