using System;
using System.Collections.Generic;

namespace NoteApp
{
    public class MementoStack<T>
    {
        private readonly List<T> _items = new List<T>();
        private readonly int _maxSize;

        public int Count => _items.Count;

        public MementoStack(int maxSize = 200)
        {
            if (maxSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxSize));
            _maxSize = maxSize;
        }

        public void Push(T item)
        {
            _items.Add(item);

          
            if (_items.Count > _maxSize)
            {
                _items.RemoveAt(0);
            }
        }

        public T Pop()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Stack is empty.");

            int lastIndex = _items.Count - 1;
            T value = _items[lastIndex];
            _items.RemoveAt(lastIndex);
            return value;
        }

        public T Peek()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Stack is empty.");

            return _items[_items.Count - 1];
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}
