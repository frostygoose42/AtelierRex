namespace AtelierRex.Core;

public interface IQueryEngine
{
    QueryResult Query(IQuery query, IEnumerable<IFile> files);
}
