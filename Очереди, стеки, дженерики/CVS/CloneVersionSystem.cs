using System;
using System.Collections.Generic;


namespace Clones;

public class Clone
{
    public MyStack LearnedProgram = new MyStack();
    public MyStack CanceledProgram = new MyStack();
    public Clone()
    {
        LearnedProgram.Push("basic");
    }
}

public class CloneVersionSystem : ICloneVersionSystem
{
    private Dictionary<string, Clone> _cloneData = new Dictionary<string, Clone>();

    public string Execute(string query)
    {
        var array = query.Split(' ');
        string currentCommand = array[0];
        string cloneNum = array[1];
        string program = array.Length > 2 ? array[2] : null;

        switch (currentCommand)
        {
            case "learn":
                if (!_cloneData.ContainsKey(cloneNum))
                {
                    var clone = new Clone();
                    _cloneData.Add(cloneNum, clone);
                }
                Learn(_cloneData[cloneNum], program);
                break;
            case "rollback":
                Rollback(cloneNum);
                break;
            case "relearn":
                Relearn(cloneNum);
                break;
            case "clone":
                Clone(cloneNum);
                break;
            case "check":
                return Check(cloneNum);

        }
        return null;
    }

    private string Learn(Clone clone, string program)
    {
        clone.LearnedProgram.Push(program);
        return null;
    }

    private string Rollback(string cloneNum)
    {
        var clone = _cloneData[cloneNum];
        string oldProgram = clone.LearnedProgram.Pop();
        clone.CanceledProgram.Push(oldProgram);
        return null;
    }

    private string Relearn(string cloneNum)
    {
        var clone = _cloneData[cloneNum];
        var oldProgram = _cloneData[cloneNum].CanceledProgram.Pop();
        clone.LearnedProgram.Push(oldProgram);
        return null;
    }

    private string Clone(string oldCloneNum)
    {
        int key = _cloneData.Count + 1;
        string newKey = key.ToString();
        if (_cloneData.ContainsKey(oldCloneNum))
        {
            var programForNewClone = new MyStack();
            programForNewClone.Head = _cloneData[oldCloneNum].LearnedProgram.Head;

            var canceledProgramForNewClone = new MyStack();
            canceledProgramForNewClone.Head = _cloneData[oldCloneNum].CanceledProgram.Head;

            var cloneAnotherClone = new Clone();
            cloneAnotherClone.LearnedProgram = programForNewClone;
            cloneAnotherClone.CanceledProgram = canceledProgramForNewClone;

            _cloneData.Add(newKey, cloneAnotherClone);
        }
        else
        {
            _cloneData.Add(newKey, new Clone());
        }
        return null;
    }

    private string Check(string cloneNum)
    {
        var clone = new Clone();
        if (_cloneData.ContainsKey(cloneNum))
        {
            clone = _cloneData[cloneNum];
        }
        else
            _cloneData.Add(cloneNum, clone);
        return clone.LearnedProgram.Peek();
    }
}

public class MyStackItem
{
    public string Value { get; set; }
    public MyStackItem Next { get; set; }
}

public class MyStack
{
    public MyStackItem Head;

    public void Push(string value)
    {
        var item = new MyStackItem { Value = value, Next = Head };
        Head = item;
    }


    public string Pop()
    {
        if (Head == null) throw new InvalidOperationException();
        var result = Head.Value;
        Head = Head.Next;
        return result;
    }

    public string Peek()
    {
        return Head.Value;
    }
}

