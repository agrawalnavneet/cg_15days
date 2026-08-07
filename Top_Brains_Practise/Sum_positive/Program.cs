using System;

public class Solution
{
    public static void Main()
    {
        int n = int.Parse(Console.ReadLine());
        int[] nums = new int[n];

        for (int i = 0; i < n; i++)
        {
            nums[i] = int.Parse(Console.ReadLine());
        }

        int sum = 0;

        foreach (int num in nums)
        {
            if (num == 0)
                break;

            if (num < 0)
                continue;

            sum += num;
        }

        Console.WriteLine(sum);
    }
}