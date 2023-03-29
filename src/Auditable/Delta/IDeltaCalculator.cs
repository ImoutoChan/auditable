using Newtonsoft.Json.Linq;

namespace Auditable.Delta;

public interface IDeltaCalculator
{
    JToken? Calculate(string leftJson, string rightJson);
}
