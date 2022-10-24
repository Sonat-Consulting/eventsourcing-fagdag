using Clippers.Core.Haircut.Models;
using Clippers.Core.Haircut.Repository;

namespace Clippers.Core.Haircut.Services
{
    public class HaircutServiceBase : IHaircutService
    {
        protected IHaircutRepository HaircutRepository { get; }

        public HaircutServiceBase(IHaircutRepository haircutRepository)
        {
            HaircutRepository = haircutRepository;
        }

        public async Task<HaircutModel> SaveHaircut(HaircutModel haircut)
        {
            _ = await HaircutRepository.SaveHaircut(haircut);
            return await LoadHaircut(haircut.HaircutId);
        }

        public async Task<HaircutModel> LoadHaircut(string haircutId)
        {
            return await HaircutRepository.LoadHaircut(haircutId);
        }
    }
}
