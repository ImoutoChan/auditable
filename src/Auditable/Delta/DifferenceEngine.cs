using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;

namespace Auditable.Delta;

public class DifferenceEngine : IDifferenceEngine
{
    public JToken Differences(string left, string right)
    {
        if (left == right) return null;

        var jdp = new JsonDiffPatch();
        var leftToken = JToken.Parse(left);
        var rightToken = JToken.Parse(right);

        var patch = jdp.Diff(leftToken, rightToken);

        return patch.HasValues ? patch : null;
    }
}