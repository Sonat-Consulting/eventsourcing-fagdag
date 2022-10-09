using Newtonsoft.Json.Linq;

namespace Clippers.Projections
{
    public interface IView
    {
        JObject Payload { get; set; }
    }
}