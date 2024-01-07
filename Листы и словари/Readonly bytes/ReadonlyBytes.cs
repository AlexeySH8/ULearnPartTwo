using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private byte[] _items;
        private int _hash;
        public int Length { get; }
        public byte this[int index] { get { return _items[index]; } }

        public ReadonlyBytes(params byte[] numbers)
        {
            if (numbers != null)
            {
                _items = numbers;
                _hash = GenerateHashCode();
                Length = numbers.Length;
            }
            else
                throw new ArgumentNullException();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = (ReadonlyBytes)obj;
            if (other.Length != _items.Length) return false;

            for (int i = 0; i < Length; i++)
                if (other[i] != this[i]) return false;

            return true;
        }

        private int GenerateHashCode()
        {
            int prime = 16777619;
            unchecked
            {
                foreach (var item in _items)
                {
                    _hash *= prime;
                    _hash ^= item;
                }
            }
            return _hash;
        }

        public override int GetHashCode()
        {
            return _hash;
        }

        public override string ToString()
        {
            var result = "[" + string.Join(", ", _items) + "]";
            return result;
        }

        public virtual IEnumerator<byte> GetEnumerator()
        {
            return new ReadOnlyEnumerator(_items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ReadOnlyEnumerator : IEnumerator<byte>
    {
        private readonly byte[] _items;
        private int _index = 0;

        public ReadOnlyEnumerator(byte[] items)
        {
            _items = items;
        }

        public byte Current
        {
            get
            {
                return _items[_index++];
            }
        }

        public bool MoveNext()
        {
            return _index < _items.Length;
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        {

        }

        public void Reset()
        {

        }
    }
}