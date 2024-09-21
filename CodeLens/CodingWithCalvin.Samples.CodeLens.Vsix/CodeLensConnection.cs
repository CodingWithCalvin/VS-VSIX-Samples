﻿using System.IO.Pipes;
using StreamJsonRpc;

namespace CodingWithCalvin.Samples.CodeLens
{
    /// <summary>
    /// Responsible for handling the connections from Visual Studio down to the CodeLens provider
    /// </summary>
    internal class CodeLensConnection
    {
        public JsonRpc Rpc;
        private readonly NamedPipeServerStream _stream;

        public CodeLensConnection(NamedPipeServerStream stream)
        {
            _stream = stream;
            Rpc = JsonRpc.Attach(_stream, this);
        }
    }
}
