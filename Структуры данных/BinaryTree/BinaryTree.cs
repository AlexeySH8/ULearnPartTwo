using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace BinaryTrees;

public class BinaryTreeEnumerator<T> : IEnumerator<T>
    where T : IComparable
{
    private BinaryTree<T> tree;
    private T currentItem;
    private Stack<BinaryTree<T>> stack = new();

    public BinaryTreeEnumerator(BinaryTree<T> tree)
    {
        this.tree = tree;
        InitializeStack(tree);
    }

    public T Current => currentItem;

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (stack.Count == 0) return false;

        var node = stack.Pop();
        currentItem = node.Value;

        if (node.Right != null)
        {
            InitializeStack(node.Right);
        }

        return true;
    }

    private void InitializeStack(BinaryTree<T> node)
    {
        while (node != null && tree.Count > 0)
        {
            stack.Push(node);
            node = node.Left;
        }
    }

    public void Dispose()
    {
        stack.Clear();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }
}

public class BinaryTree<T> : IEnumerable<T>
    where T : IComparable
{
    public T Value;
    public BinaryTree<T> Left, Right;
    public int Count = 0;
    public int Size = 1;

    public void Add(T value)
    {
        var current = this;
        while (true)
        {
            current.Size++;
            if (Count == 0)
            {
                this.Value = value;
                break;
            }
            else if (current.Value.CompareTo(value) == 1)
            {
                if (current.Left == null)
                {
                    current.Left = new BinaryTree<T> { Value = value };
                    break;
                }
                else
                    current = current.Left;
            }
            else
            {
                if (current.Right == null)
                {
                    current.Right = new BinaryTree<T> { Value = value };
                    break;
                }
                else
                    current = current.Right;
            }
        }
        Count++;
    }

    public bool Contains(T value)
    {
        if (Count == 0) return false;
        var current = this;
        while (current != null)
        {
            if (current.Value.CompareTo(value) == 0) return true;
            else if (current.Left != null &&
                current.Value.CompareTo(value) == 1)
                current = current.Left;
            else if (current.Right != null)
                current = current.Right;
            else
                return false;
        }
        return false;
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException("Индекс находится за пределами дерева.");
            return GetByIndex(this, index);
        }
    }

    private T GetByIndex(BinaryTree<T> node, int index)
    {
        int leftSize = node.Left?.Size ?? 0;

        if (index < leftSize)
        {
            return GetByIndex(node.Left, index);
        }
        else if (index == leftSize)
        {
            return node.Value;
        }
        else
        {
            return GetByIndex(node.Right, index - leftSize - 1);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new BinaryTreeEnumerator<T>(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}