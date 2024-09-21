using System;
using System.Threading.Tasks;

namespace CodingWithCalvin.Samples.CodeLens.Shared
{
    public interface IMyCodeLensCallbackService
    {
        DateTime GetCurrentDateTime();
        int GetVisualStudioPid();
        Task InitializeRpcAsync(string dataPointId);
    }
}
