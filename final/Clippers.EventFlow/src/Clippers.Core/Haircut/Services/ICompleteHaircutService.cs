using Clippers.Core.Haircut.Events;
using Clippers.Core.Haircut.Models;

namespace Clippers.Core.Haircut.Services
{
    public interface ICompleteHaircutService
    {
        Task<HaircutModel> CompleteHaircut(CompleteHaircutCommand completeHaircutCommand);
    }
}
