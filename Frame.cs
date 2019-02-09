using System;

namespace TQ.Texture
{
    public readonly ref struct Frame
    {
        public Span<byte> Data { get; }

        public Frame(Span<byte> data) => Data = data;
    }
}