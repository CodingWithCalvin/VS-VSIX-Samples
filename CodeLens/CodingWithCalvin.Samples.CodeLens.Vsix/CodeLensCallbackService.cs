using System.ComponentModel.Composition;
using System.Diagnostics;
using CodingWithCalvin.Samples.CodeLens.Shared;
using Microsoft.VisualStudio.Language.CodeLens;
using Microsoft.VisualStudio.Utilities;

namespace CodingWithCalvin.Samples.CodeLens
{
    [Export(typeof(ICodeLensCallbackListener))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ContentType("CSharp")]
    internal class CodeLensCallbackService : ICodeLensCallbackListener, IMyCodeLensCallbackService
    {
        public int GetVisualStudioPid()
        {
            return Process.GetCurrentProcess().Id;
        }
    }
}
