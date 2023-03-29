using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;

namespace Auditable.Delta;

internal class DeltaCalculator : IDeltaCalculator
{
    public JToken? Calculate(string leftJson, string rightJson)
    {
        if (leftJson == rightJson) 
            return null;

        var jdp = new JsonDiffPatch();
        var leftToken = JToken.Parse(leftJson);
        var rightToken = JToken.Parse(rightJson);

        var patch = jdp.Diff(leftToken, rightToken);

        return patch.HasValues ? patch : null;
    }
}
