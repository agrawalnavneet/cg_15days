using System;
using System.Collections.Generic;

public class Solution
{
    public static void Main()
    {
        Dictionary<int, int> employees = new Dictionary<int, int>
        {
            {1, 20000},
            {4, 40000},
            {5, 15000}
        };

        int[] ids = { 1, 4, 5 };
        int totalSalary = 0;

        foreach (int id in ids)
        {
            if (employees.ContainsKey(id))
            {
                totalSalary += employees[id];
            }
        }

        Console.WriteLine(totalSalary);
    }
}