using Newtonsoft.Json.Linq;

namespace Auditable.Delta;

public interface IDifferenceEngine
{
    JToken Differences(string left, string right);
}