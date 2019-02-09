using System;

namespace TQ.Texture
{
    public readonly ref struct Frame
    {
        readonly Span<byte> _data;

        public Frame(Span<byte> data) => _data = data;
    }
}