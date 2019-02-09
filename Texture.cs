using SpanUtils;
using System;
using TQ.Common;

namespace TQ.Texture
{
    public readonly ref struct Texture
    {
        readonly Span<byte> _data;
        public Texture(Span<byte> data)
        {
            _data = data;
            for (int i = 0; i < _magic.Length; i++)
            {
                if (_data[i] != _magic[i])
                { throw new ArgumentException("Wrong magic! Expected file to start with TEX.", nameof(data)); }
            }
        }

        static readonly byte[] _magic = Definitions.Encoding.GetBytes("TEX");
        public ref byte Version => ref _data[_magic.Length];
        public ref uint FPS => ref _data.View<uint>(_magic.Length + sizeof(byte));

        public Enumerator GetEnumerator() => new Enumerator(_data.Slice(_magic.Length + sizeof(byte) + sizeof(int)));
        public ref struct Enumerator
        {
            readonly Span<byte> _data;
            int _offset;

            int _CurrentLength => _data.View<int>(_offset);

            public Enumerator(Span<byte> data)
            {
                _data = data;
                _offset = -1;
            }

            public Frame Current => new Frame(_data.Slice(_offset + sizeof(int), _CurrentLength));

            public bool MoveNext()
            {
                if (_offset == -1) _offset = 0;
                else _offset += sizeof(int) + _CurrentLength;
                switch (_offset.CompareTo(_data.Length))
                {
                    case var x when x < 0: return true;
                    case var x when x == 0: return false;
                    default /* var x when x > 0 */: throw new InvalidOperationException("Tried to move beyond end of data.");
                }
            }

            public void Reset() => _offset = -1;
        }
    }
}
