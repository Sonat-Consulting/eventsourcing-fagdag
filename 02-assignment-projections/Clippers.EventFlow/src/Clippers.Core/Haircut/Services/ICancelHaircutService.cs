using Clippers.Core.Haircut.Events;
using Clippers.Core.Haircut.Models;

namespace Clippers.Core.Haircut.Services
{
    public interface ICancelHaircutService
    {
        Task<HaircutModel> CancelHaircut(CancelHaircutCommand cancelHaircutCommand);
    }
}
