using System;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;
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
        public static readonly ConcurrentDictionary<string, CodeLensConnection> Connections =
            new ConcurrentDictionary<string, CodeLensConnection>();

        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        public int GetVisualStudioPid()
        {
            return Process.GetCurrentProcess().Id;
        }

        public async Task InitializeRpcAsync(string dataPointId)
        {
            var stream = new NamedPipeServerStream(
                RpcPipeNames.ForCodeLens(Process.GetCurrentProcess().Id),
                PipeDirection.InOut,
                NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous
            );

            await stream.WaitForConnectionAsync().ConfigureAwait(false);

            var connection = new CodeLensConnection(stream);
            Connections[dataPointId] = connection;
        }

        /// <summary>
        /// Refresh a SPECIFIC CodeLens datapoint through RPC
        /// </summary>
        public static async Task RefreshCodeLensDataPointAsync(string dataPointId)
        {
            if (!Connections.TryGetValue(dataPointId, out var connectionHandler))
            {
                throw new InvalidOperationException(
                    $"CodeLens data point {dataPointId} was not registered."
                );
            }

            await connectionHandler
                .Rpc.InvokeAsync(nameof(IRemoteCodeLens.Refresh))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// All RPC connections to the CodeLens datapoints are tracked, therefore
        /// we can trigger them ALL to refresh using this.
        /// </summary>
        public static async Task RefreshAllCodeLensDataPointsAsync() =>
            await Task.WhenAll(Connections.Keys.Select(RefreshCodeLensDataPointAsync))
                .ConfigureAwait(false);
    }
}
