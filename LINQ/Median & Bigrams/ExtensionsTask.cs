using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
    /// <summary>
    /// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
    /// Медиана списка из четного количества элементов — это среднее арифметическое 
    /// двух серединных элементов списка после сортировки.
    /// </summary>
    /// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
    public static double Median(this IEnumerable<double> items)
    {
        var temp = MakeSortedList(items);
        if (temp.Count == 0) throw new InvalidOperationException();
        var halfLength = temp.Count / 2;

        if (temp.Count % 2 == 0)
        {
            return (temp[halfLength - 1] + temp[halfLength]) / 2;
        }
        else return temp[halfLength];
    }
    /// <returns>
    /// Возвращает последовательность, состоящую из пар соседних элементов.
    /// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
    /// </returns>
    public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
    {
        var enumerator = items.GetEnumerator();
        if (!enumerator.MoveNext()) yield break;

        var queue = new Queue<T>();
        queue.Enqueue(enumerator.Current);

        while (enumerator.MoveNext())
        {
            queue.Enqueue(enumerator.Current);
            yield return (queue.Dequeue(), queue.Peek());
        }
    }

    private static List<double> MakeSortedList(IEnumerable<double> items)
    {
        var temp = items.ToList();
        temp.Sort();
        return temp;
    }
}