using System;
using System.IO;

namespace Auditable.Writers.File;

public class FileWriterOptions
{
    public Func<string, string, string> GetFileName { get; set; } = (id, action) =>
    {
        var date = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd-H-mm-ss");
        return $"{date}_{id}.auditable";
    };

    public string Folder { get; set; } = Directory.GetCurrentDirectory();
}
