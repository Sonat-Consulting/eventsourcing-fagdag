using Newtonsoft.Json.Linq;

namespace Clippers.EventFlow.Projections.Core.Interfaces
{
    public interface IView
    {
        JObject Payload { get; set; }
    }
}