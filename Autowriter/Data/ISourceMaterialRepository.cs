
namespace Autowriter.Data
{
    public interface ISourceMaterialRepository
    {
        void CreateSource(DateTime createdDateTime, string content);
        void DeleteSource(int id);
        SourceMaterialRepository.Source? GetSource(int id);
        IEnumerable<SourceMaterialRepository.Source> GetSources();
    }
}
