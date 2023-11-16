using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private LinkedList<T> MyStack = new LinkedList<T>();
    private int MaxSize;
    public int Count { get; private set; }

    public LimitedSizeStack(int undoLimit)
    {
        MaxSize = undoLimit;
        Count = 0;
    }

    public void Push(T item)
    {
        if (MaxSize > 0)
        {
            if (MyStack.Count == MaxSize)
            {
                MyStack.RemoveFirst();
                Count--;
            }

            MyStack.AddLast(item);
            Count++;
        }
    }

    public T Pop()
    {
        if (MyStack.Count >= 0)
        {
            Count--;
            var result = MyStack.Last.Value;
            MyStack.RemoveLast();
            return result;
        }
        throw new Exception("Stack count less than 0");
    }
}