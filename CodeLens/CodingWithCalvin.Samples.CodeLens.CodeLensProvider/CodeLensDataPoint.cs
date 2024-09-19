using System;
using System.Threading;
using System.Threading.Tasks;
using CodingWithCalvin.Samples.CodeLens.Shared;
using Microsoft.VisualStudio.Language.CodeLens;
using Microsoft.VisualStudio.Language.CodeLens.Remoting;
using Microsoft.VisualStudio.Threading;

namespace CodingWithCalvin.Samples.CodeLens.CodeLensProvider
{
    internal class CodeLensDataPoint : IAsyncCodeLensDataPoint
    {
        private readonly ICodeLensCallbackService _callbackService;

        public CodeLensDataPoint(
            CodeLensDescriptor descriptor,
            ICodeLensCallbackService callbackService
        )
        {
            _callbackService = callbackService;
            Descriptor = descriptor;
        }

        public async Task<CodeLensDataPointDescriptor> GetDataAsync(
            CodeLensDescriptorContext descriptorContext,
            CancellationToken token
        )
        {
            var vsPid = await _callbackService
                .InvokeAsync<int>(
                    this,
                    nameof(IMyCodeLensCallbackService.GetVisualStudioPid),
                    cancellationToken: token
                )
                .ConfigureAwait(false);

            return new CodeLensDataPointDescriptor
            {
                Description = $"The Visual Studio PID is {vsPid}!",
                TooltipText = $"Shows Up On Hover, show it here, too! - VS PID = {vsPid}",
            };
        }

        public Task<CodeLensDetailsDescriptor> GetDetailsAsync(
            CodeLensDescriptorContext descriptorContext,
            CancellationToken token
        )
        {
            // this is what gets triggered when you click a Code Lens entry, and we don't really care about this part for now
            return Task.FromResult<CodeLensDetailsDescriptor>(null);
        }

        public CodeLensDescriptor Descriptor { get; }
        public event AsyncEventHandler InvalidatedAsync;
    }
}
