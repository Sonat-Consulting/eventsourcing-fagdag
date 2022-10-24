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
        public async Task<HaircutModel> StartHaircut(StartHaircutCommand startHaircutCommand)
        {
            var haircut = await LoadHaircut(startHaircutCommand.HaircutId);
            haircut.Start(startHaircutCommand.HairdresserId, startHaircutCommand.StartedAt);
            return await base.SaveHaircut(haircut);
        }
    }
}
