using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        static readonly char[] defaultChars =
            "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890".ToCharArray();

        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            MemoryByteChanges(vm);
            MoveMemoryPointer(vm);
            vm.RegisterCommand('.', b => { write((char)b.Memory[b.MemoryPointer]); });
            vm.RegisterCommand(',', b =>
            {
                byte s = (byte)read();
                b.Memory[b.MemoryPointer] = s;
            });
            foreach (var i in defaultChars)
            {
                vm.RegisterCommand(i, b => { b.Memory[b.MemoryPointer] = (byte)i; });
            }
        }

        private static void MemoryByteChanges(IVirtualMachine vm)
        {
            vm.RegisterCommand('+', b =>
            {
                if (b.Memory[b.MemoryPointer] < 255)
                    b.Memory[b.MemoryPointer]++;
                else
                    b.MemoryPointer++;
            });
            vm.RegisterCommand('-', b =>
            {
                if (b.Memory[b.MemoryPointer] > 0)
                    b.Memory[b.MemoryPointer]--;
                else
                    b.Memory[b.MemoryPointer] = 255;
            });
        }

        private static void MoveMemoryPointer(IVirtualMachine vm)
        {
            vm.RegisterCommand('>', b =>
            {
                if (b.MemoryPointer < b.Memory.Length - 1)
                    b.MemoryPointer++;
                else
                    b.MemoryPointer = 0;
            });
            vm.RegisterCommand('<', b =>
            {
                if (b.MemoryPointer > 0)
                    b.MemoryPointer--;
                else
                    b.MemoryPointer = b.Memory.Length - 1;
            });
        }
    }
}