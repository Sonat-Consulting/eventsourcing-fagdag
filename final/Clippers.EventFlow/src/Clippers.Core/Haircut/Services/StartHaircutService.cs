using Clippers.Core.Haircut.Events;
using Clippers.Core.Haircut.Models;
using Clippers.Core.Haircut.Repository;

namespace Clippers.Core.Haircut.Services
{
    public class StartHaircutService : HaircutServiceBase, IStartHaircutService
    {
        public StartHaircutService(IHaircutRepository haircutRepository) : base(haircutRepository)
        {
        }
        public async Task<HaircutModel> StartHaircut(HaircutStarted haircutStarted)
        {
            var haircut = await LoadHaircut(haircutStarted.HaircutId);
            haircut.Start(haircutStarted.HairdresserId, haircutStarted.StartedAt);
            return await base.SaveHaircut(haircut);
        }
    }
}
