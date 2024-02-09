using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            Dictionary<int, int> bracketPairs = FindBracketPairs(vm.Instructions);

            vm.RegisterCommand('[', vmState =>
            {
                if (vmState.Memory[vmState.MemoryPointer] == 0)
                {
                    vmState.InstructionPointer = bracketPairs[vmState.InstructionPointer];
                }
            });

            vm.RegisterCommand(']', vmState =>
            {
                if (vmState.Memory[vmState.MemoryPointer] != 0)
                {
                    vmState.InstructionPointer = bracketPairs[vmState.InstructionPointer];
                }
            });
        }

        private static Dictionary<int, int> FindBracketPairs(string program)
        {
            Dictionary<int, int> bracketPairs = new Dictionary<int, int>();
            Stack<int> openBrackets = new Stack<int>();

            for (int i = 0; i < program.Length; i++)
            {
                if (program[i] == '[')
                {
                    openBrackets.Push(i);
                }
                else if (program[i] == ']')
                {
                    int openBracketIndex = openBrackets.Pop();
                    bracketPairs.Add(openBracketIndex, i);
                    bracketPairs.Add(i, openBracketIndex);
                }
            }
            return bracketPairs;
        }
    }
}
