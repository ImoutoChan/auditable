using System.IO;
using Auditable.Infrastructure;

namespace Auditable.Writers.File;

public class FileWriterOptions
{
    public GetFileName GetFileName { get; set; } = (id, action) =>
    {
        Code.Require(() => !string.IsNullOrEmpty(id), nameof(id));
        var date = SystemDateTime.UtcNow.ToString("yyyy-MM-dd-H-mm-ss");
        return $"{date}_{id}.auditable";
    };

    public string Folder { get; set; } = Directory.GetCurrentDirectory();
}