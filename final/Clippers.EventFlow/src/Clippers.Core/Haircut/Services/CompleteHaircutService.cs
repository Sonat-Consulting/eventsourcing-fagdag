using Clippers.Core.Haircut.Events;
using Clippers.Core.Haircut.Models;
using Clippers.Core.Haircut.Repository;

namespace Clippers.Core.Haircut.Services
{
    public class CompleteHaircutService : HaircutServiceBase, ICompleteHaircutService
    {
        public CompleteHaircutService(IHaircutRepository haircutRepository) : base(haircutRepository)
        {
        }
        public async Task<HaircutModel> CompleteHaircut(HaircutCompleted haircutCompleted)
        {
            var haircut = await LoadHaircut(haircutCompleted.HaircutId);
            haircut.Complete(haircutCompleted.CompletedAt);
            return await base.SaveHaircut(haircut);
        }
    }
}
