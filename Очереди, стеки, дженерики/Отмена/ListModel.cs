using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

interface ICommand
{
    void Execute();
    void Undo();
}

class AddItemCommand<TItem> : ICommand
{
    private readonly List<TItem> _items = new List<TItem>();
    private readonly TItem _item;

    public AddItemCommand(List<TItem> items, TItem item)
    {
        _items = items;
        _item = item;
    }

    public void Execute()
    {
        _items.Add(_item);
    }

    public void Undo()
    {
        _items.Remove(_item);
    }
}

class RemoveItemCommand<TItem> : ICommand
{
    private List<TItem> _items;
    private int _index;
    private TItem _deletedItem;

    public RemoveItemCommand(List<TItem> items, int index)
    {
        _items = items;
        _index = index;
        _deletedItem = items[index];
    }

    public void Execute()
    {
        _items.RemoveAt(_index);
    }

    public void Undo()
    {
        _items.Insert(_index, _deletedItem);
    }
}

class ComandController<TItem>
{
    private readonly LimitedSizeStack<ICommand> _commands;
    public int CommandsCount { get { return _commands.Count; } }

    public ComandController(int undoLimit)
    {
        _commands = new LimitedSizeStack<ICommand>(undoLimit);
    }

    public void AddItem(List<TItem> items, TItem item)
    {
        var command = new AddItemCommand<TItem>(items, item);
        _commands.Push(command);
        command.Execute();
    }

    public void RemoveItem(List<TItem> items, int index)
    {
        var command = new RemoveItemCommand<TItem>(items, index);
        _commands.Push(command);
        command.Execute();
    }

    public void Undo(List<TItem> items)
    {
        var command = _commands.Pop();
        command.Undo();
    }
}

public class ListModel<TItem>
{
    public List<TItem> Items { get; }
    public static int UndoLimit;
    private readonly ComandController<TItem> _commandController;

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
    {

    }

    public ListModel(List<TItem> items, int undoLimit)
    {
        Items = items;
        UndoLimit = undoLimit;
        _commandController = new ComandController<TItem>(undoLimit);
    }

    public void AddItem(TItem item)
    {
        _commandController.AddItem(Items, item);
    }

    public void RemoveItem(int index)
    {
        _commandController.RemoveItem(Items, index);
    }

    public void Undo()
    {
        _commandController.Undo(Items);
    }

    public bool CanUndo()
    {
        return _commandController.CommandsCount > 0;
    }
}