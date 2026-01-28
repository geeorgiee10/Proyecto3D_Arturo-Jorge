using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static List<int> GetRandomPermutation(int n)
    {
        List<int> numbers = new List<int>();

        // 1️⃣ Crear lista 1..N
        for (int i = 1; i <= n; i++)
            numbers.Add(i);

        // 2️⃣ Mezclar (Fisher–Yates)
        for (int i = numbers.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = numbers[i];
            numbers[i] = numbers[j];
            numbers[j] = temp;
        }

        return numbers;
    }
}
