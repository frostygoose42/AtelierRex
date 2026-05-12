namespace AtelierRex.Core;

public interface IDiffer
{
    DiffResult Diff(IFile left, IFile right, DiffOptions options);
    DiffResult Diff(IChunk left, IChunk right, DiffOptions options);
}
