using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingWithCalvin.Samples.CodeLens.Shared
{
    public static class RpcPipeNames
    {
        // Pipe needs to be scoped by PID so multiple VS instances don't compete for connecting CodeLenses.
        public static string ForCodeLens(int pid) => $@"CodeLensSample\vs\{pid}";
    }
}
