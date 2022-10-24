using Clippers.Core.Haircut.Events;
using Clippers.Core.Haircut.Models;

namespace Clippers.Core.Haircut.Services
{
    public interface IStartHaircutService
    {
        Task<HaircutModel> StartHaircut(StartHaircutCommand startHaircutCommand);
    }
}
