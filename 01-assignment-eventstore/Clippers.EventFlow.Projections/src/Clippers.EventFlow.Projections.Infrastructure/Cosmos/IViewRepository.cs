namespace Clippers.EventFlow.Projections.Infrastructure.Cosmos
{
    public interface IViewRepository
    {
        Task<View> LoadViewAsync(string name);

        Task<bool> SaveViewAsync(string name, View view);
    }
}