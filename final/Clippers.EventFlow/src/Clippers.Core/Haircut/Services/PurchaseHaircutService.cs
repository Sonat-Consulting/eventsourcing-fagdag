using Clippers.Core.Haircut.Events;
using Clippers.Core.Haircut.Models;
using Clippers.Core.Haircut.Repository;

namespace Clippers.Core.Haircut.Services
{
    public class PurchaseHaircutService : HaircutServiceBase, IPurchaseHaircutService
    {
        public PurchaseHaircutService(IHaircutRepository haircutRepository) : base(haircutRepository)
        {
        }

        public async Task<HaircutModel> CreateHaircut(HaircutCreated haircutPurchased)
        {
            return await base.SaveHaircut(new HaircutModel(
                haircutPurchased.HaircutId,
                haircutPurchased.CustomerId,
                haircutPurchased.DisplayName,
                haircutPurchased.CreatedAt
            ));
        }
    }
}
