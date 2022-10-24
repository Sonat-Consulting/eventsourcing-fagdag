using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clippers.Core.Haircut.Commands
{
    public class CreateHaircutCommand
    {
        public DateTime CreatedAt { get; set; }
        public string CustomerId { get; set; }
        public string DisplayName { get; set; }
    }
}
