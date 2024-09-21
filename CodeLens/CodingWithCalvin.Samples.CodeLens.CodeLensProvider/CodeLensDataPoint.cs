using System;
using System.Threading;
using System.Threading.Tasks;
using CodingWithCalvin.Samples.CodeLens.Shared;
using Microsoft.VisualStudio.Language.CodeLens;
using Microsoft.VisualStudio.Language.CodeLens.Remoting;
using Microsoft.VisualStudio.Threading;

namespace CodingWithCalvin.Samples.CodeLens.CodeLensProvider
{
    public class CodeLensDataPoint : IAsyncCodeLensDataPoint
    {
        private readonly ICodeLensCallbackService _callbackService;
        public readonly string DataPointId = Guid.NewGuid().ToString();

        public VisualStudioConnection VsConnection;

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
            var currentDateTime = await _callbackService
                .InvokeAsync<DateTime>(
                    this,
                    nameof(IMyCodeLensCallbackService.GetCurrentDateTime),
                    cancellationToken: token
                )
                .ConfigureAwait(false);

            return new CodeLensDataPointDescriptor
            {
                Description = $"The Current DateTime is {currentDateTime.ToLongDateString()}!",
                TooltipText =
                    $"Shows Up On Hover, show it here, too! - Current DateTime = {currentDateTime.ToLongTimeString()}",
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

        public void Refresh() =>
            _ = InvalidatedAsync?.InvokeAsync(this, EventArgs.Empty).ConfigureAwait(false);
    }
}
