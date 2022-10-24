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
        public async Task<HaircutModel> CompleteHaircut(CompleteHaircutCommand completeHaircutCommand)
        {
            var haircut = await LoadHaircut(completeHaircutCommand.HaircutId);
            haircut.Complete(completeHaircutCommand.CompletedAt);
            return await base.SaveHaircut(haircut);
        }
    }
}
