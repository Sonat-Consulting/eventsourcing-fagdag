using Clippers.Core.Haircut.Events;
using Clippers.Core.Haircut.Models;
using Clippers.Core.Haircut.Repository;

namespace Clippers.Core.Haircut.Services
{
    public class CancelHaircutService : HaircutServiceBase, ICancelHaircutService
    {
        public CancelHaircutService(IHaircutRepository haircutRepository) : base(haircutRepository)
        {
        }
        public async Task<HaircutModel> CancelHaircut(CancelHaircutCommand cancelHaircutCommand)
        {
            var haircut = await LoadHaircut(cancelHaircutCommand.HaircutId);
            haircut.Cancel(cancelHaircutCommand.CancelledAt);
            return await base.SaveHaircut(haircut);
        }
    }
}
