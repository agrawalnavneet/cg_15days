using System;

class Program
{
    public static T[] MergeSortedArrays<T>(T[] a, T[] b) where T : IComparable<T>
    {
        T[] merged = new T[a.Length + b.Length];

        int i = 0, j = 0, k = 0;

        while (i < a.Length && j < b.Length)
        {
            if (a[i].CompareTo(b[j]) <= 0)
            {
                merged[k++] = a[i++];
            }
            else
            {
                merged[k++] = b[j++];
            }
        }

        while (i < a.Length)
        {
            merged[k++] = a[i++];
        }

        while (j < b.Length)
        {
            merged[k++] = b[j++];
        }

        return merged;
    }

    static void Main()
    {
        int[] a = { 1, 3, 5, 7 };
        int[] b = { 2, 4, 6, 8 };

        int[] result = MergeSortedArrays(a, b);

        foreach (int num in result)
        {
            Console.Write(num + " ");
        }
    }
}