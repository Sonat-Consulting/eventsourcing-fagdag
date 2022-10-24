using Clippers.Core.Haircut.Models;

namespace Clippers.Core.Haircut.Services
{
    public interface IHaircutService
    {
        Task<HaircutModel> LoadHaircut(string haircutId);
        Task<HaircutModel> SaveHaircut(HaircutModel haircut);
    }
}
