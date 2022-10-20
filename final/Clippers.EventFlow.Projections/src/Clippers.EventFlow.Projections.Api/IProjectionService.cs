namespace Clippers.EventFlow.Projections.Api
{
    public interface IProjectionService
    {
        Task<string> GetViews();
    }
}