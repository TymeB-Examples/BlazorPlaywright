using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace BlockchainBlazorWithE2EDemo.Models
{

    public class Blockchain<T>(int initialCapacity = 16) : IEnumerable<Block<T>>, IList<Block<T>>
    {
        private Block<T>[] _chain = new Block<T>[initialCapacity];
        private int _count = 0;

        public int Count => _count;

        public bool IsReadOnly => false;

        public Block<T> this[int index] { get => _chain[index]; set => _chain[index] = value; }

        public void AddBlock(Block<T> block)
        {
            if (_count == _chain.Length)
                ResizeArray(_chain.Length * 2);

            _chain[_count++] = block;
        }

        private void ResizeArray(int newSize)
        {
            var newArr = new Block<T>[newSize];
            Array.Copy(_chain, newArr, _count);
            _chain = newArr;
        }

        public IEnumerator<Block<T>> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
                yield return _chain[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(Block<T> item)
        {
            var comparer = EqualityComparer<Block<T>>.Default;
            for (int i = 0; i < _count; i++)
            {
                if (comparer.Equals(_chain[i], item))
                    return i;
            }
            return -1;
        }

        public void Insert(int index, Block<T> item)
        {
            if (index < 0 || index > _count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (_count == _chain.Length)
                ResizeArray(Math.Max(1, _chain.Length * 2));

            if (index < _count)
                Array.Copy(_chain, index, _chain, index + 1, _count - index);

            _chain[index] = item;
            _count++;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException(nameof(index));

            int moveCount = _count - index - 1;
            if (moveCount > 0)
                Array.Copy(_chain, index + 1, _chain, index, moveCount);

            _chain[--_count] = default!;
        }

        public void Add(Block<T> item)
        {
            AddBlock(item);
        }

        public void Clear()
        {
            // Reset to fresh array of initial capacity to avoid keeping references
            _chain = new Block<T>[initialCapacity];
            _count = 0;
        }

        public bool Contains(Block<T> item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(Block<T>[] array, int arrayIndex)
        {
            ArgumentNullException.ThrowIfNull(array);

            ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);

            if (array.Length - arrayIndex < _count)
                throw new ArgumentException("Destination array is too small.");

            Array.Copy(_chain, 0, array, arrayIndex, _count);
        }

        public bool Remove(Block<T> item)
        {
            int idx = IndexOf(item);
            if (idx >= 0)
            {
                RemoveAt(idx);
                return true;
            }
            return false;
        }
    }
}
