using Clippers.Core.Haircut.Events;
using Clippers.Core.Haircut.Models;
using Clippers.Core.Haircut.Repository;

namespace Clippers.Core.Haircut.Services
{
    public class CreateHaircutService : HaircutServiceBase, ICreateHaircutService
    {
        public CreateHaircutService(IHaircutRepository haircutRepository) : base(haircutRepository)
        {
        }

        public async Task<HaircutModel> CreateHaircut(CreateHaircutCommand createHaircutCommand)
        {
            return await base.SaveHaircut(new HaircutModel(
                Guid.NewGuid().ToString(),
                createHaircutCommand.CustomerId,
                createHaircutCommand.DisplayName,
                createHaircutCommand.CreatedAt
            ));
        }
    }
}
