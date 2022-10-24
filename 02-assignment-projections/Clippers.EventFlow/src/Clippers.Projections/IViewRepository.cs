namespace Clippers.Projections
{
    public interface IViewRepository
    {
        Task<View> LoadViewAsync(string name);

        Task<bool> SaveViewAsync(string name, View view);
    }
}