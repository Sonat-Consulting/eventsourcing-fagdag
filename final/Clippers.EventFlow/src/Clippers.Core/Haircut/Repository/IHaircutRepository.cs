using Clippers.Core.Haircut.Models;

namespace Clippers.Core.Haircut.Repository
{
    public interface IHaircutRepository
    {
        Task<HaircutModel> LoadHaircut(string haircutId);
        Task<bool> SaveHaircut(HaircutModel haircut);
    }
}
