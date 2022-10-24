using Clippers.Core.Haircut.Commands;
using Clippers.Core.Haircut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clippers.Core.Haircut.Services
{
    public interface ICreateHaircutService
    {
        Task<HaircutModel> CreateHaircut(CreateHaircutCommand createHaircutCommand);
    }
}
