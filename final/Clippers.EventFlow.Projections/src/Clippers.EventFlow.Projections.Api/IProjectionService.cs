namespace Clippers.EventFlow.Projections.Api
{
    public interface IProjectionService
    {
        Task<string> GetView(string name);
        Task<string> GetViews();
    }
}