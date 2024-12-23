using NUnit.Framework;
using System.Collections.Generic;

namespace rocket_bot;

public class Channel<T> where T : class
{
    /// <summary>
    /// Возвращает элемент по индексу или null, если такого элемента нет.
    /// При присвоении удаляет все элементы после.
    /// Если индекс в точности равен размеру коллекции, работает как Append.
    /// </summary>
    private readonly List<T> _items = new List<T>();
    public T this[int index]
    {
        get
        {
            lock (this)
            {
                if (index < Count && index >= 0)
                    return _items[index];
                else return null;
            }
        }
        set
        {
            lock (this)
            {
                _items.RemoveRange(index, Count - index);
                if (Count == index) _items.Insert(index, value);
                else _items[index] = value;
            }
        }
    }

    /// <summary>
    /// Возвращает последний элемент или null, если такого элемента нет
    /// </summary>
    public T LastItem()
    {
        lock (this)
        {
            if (Count > 0)
                return _items[Count - 1];
            return null;
        }
    }

    /// <summary>
    /// Добавляет item в конец только если lastItem является последним элементом
    /// </summary>
    public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
    {
        lock (this)
        {
            var last = LastItem();
            if (last == knownLastItem)
                this[Count] = item;
        }
    }

    /// <summary>
    /// Возвращает количество элементов в коллекции
    /// </summary>
    public int Count
    {
        get
        {
            lock (this)
            {
                return _items.Count;
            }
        }
    }
}