using Clippers.EventFlow.Projections.Core.Interfaces;

namespace Clippers.EventFlow.Projections.Infrastructure.Cosmos
{
    public interface ICosmosDBProjectionEngine
    {
        void RegisterProjection(IProjection projection);
        Task StartAsync();
        Task StopAsync();
    }
}