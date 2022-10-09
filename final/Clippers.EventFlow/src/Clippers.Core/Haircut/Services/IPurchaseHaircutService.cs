using Clippers.Core.Haircut.Events;
using Clippers.Core.Haircut.Models;

namespace Clippers.Core.Haircut.Services
{
    public interface IPurchaseHaircutService
    {
        Task<HaircutModel> SaveHaircut(HaircutModel haircut);
        Task<HaircutModel> CreateHaircut(HaircutCreated haircutPurchased);
    }
}
