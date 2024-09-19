using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.CodeLens;
using Microsoft.VisualStudio.Language.CodeLens.Remoting;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Utilities;

namespace CodingWithCalvin.Samples.CodeLens.CodeLensProvider
{
    [Export(typeof(IAsyncCodeLensDataPointProvider))]
    [Name("CodeLensDataPointProvider")]
    [ContentType("csharp")]
    [Priority(200)]
    internal class CodeLensDataPointProvider : IAsyncCodeLensDataPointProvider
    {
        private readonly Lazy<ICodeLensCallbackService> _callbackService;

        [ImportingConstructor]
        public CodeLensDataPointProvider(Lazy<ICodeLensCallbackService> callbackService)
        {
            _callbackService = callbackService;
        }

        public Task<bool> CanCreateDataPointAsync(
            CodeLensDescriptor descriptor,
            CodeLensDescriptorContext descriptorContext,
            CancellationToken token
        )
        {
            var methodsOnly = descriptor.Kind == CodeElementKinds.Method;
            return Task.FromResult(methodsOnly);
        }

        public Task<IAsyncCodeLensDataPoint> CreateDataPointAsync(
            CodeLensDescriptor descriptor,
            CodeLensDescriptorContext descriptorContext,
            CancellationToken token
        )
        {
            return Task.FromResult<IAsyncCodeLensDataPoint>(
                new CodeLensDataPoint(descriptor, _callbackService.Value)
            );
        }
    }
}
