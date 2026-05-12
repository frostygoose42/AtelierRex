namespace AtelierRex.Core;

public interface IExporter
{
    string TargetFormat { get; }
    Result Export(IFile file, ExportOptions options);
}
