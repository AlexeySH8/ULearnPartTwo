using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class VirtualMachine : IVirtualMachine
    {
        public string Instructions { get; }
        public int InstructionPointer { get; set; }
        public byte[] Memory { get; }
        public int MemoryPointer { get; set; }
        public Dictionary<char, Action<IVirtualMachine>> _dicComand =
            new Dictionary<char, Action<IVirtualMachine>>();

        public VirtualMachine(string program, int memorySize)
        {
            Memory = new byte[memorySize];
            Instructions = program;
            MemoryPointer = 0;
            InstructionPointer = 0;
        }

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
        {
            if (execute != null && !_dicComand.ContainsKey(symbol))
                _dicComand.Add(symbol, execute);
        }

        public void Run()
        {
            if (_dicComand.Count > 0)
            {
                for (; InstructionPointer < Instructions.Length; InstructionPointer++)
                {
                    var instruction = Instructions[InstructionPointer];
                    if (_dicComand.ContainsKey(instruction))
                        _dicComand[instruction](this);
                }
            }
        }
    }
}