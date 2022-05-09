using Autowriter.Data.Models;

namespace Autowriter.Data
{
    public interface ICreateSourceMaterials
    {
        public Task<SourceMaterial> CreateSourceAsync(DateTime created, string content, CancellationToken cancellationToken);
    }

    public interface ICountSourceMaterials
    {
        public int SourceMaterialCount();
    }

    public interface IReadSourceMaterial
    {
        public SourceMaterial? GetSourceMaterial(int id);
    }

    public interface IReadSourceMaterials
    {
        public IReadOnlyList<SourceMaterial> GetSources();
    }

    public interface IDeleteSourceMaterial
    {
        public Task DeleteSourceAsync(int id);
    }

    public class SourceMaterialRepository : ICreateSourceMaterials, ICountSourceMaterials, IReadSourceMaterial, IReadSourceMaterials, IDeleteSourceMaterial
    {
        private readonly DataContext _dataContext;

        public SourceMaterialRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<SourceMaterial> CreateSourceAsync(DateTime created, string content, CancellationToken cancellationToken)
        {
            var newSource = new SourceMaterial
            {
                Created = created,
                Content = content,
            };

            _dataContext.SourceMaterials?.Add(newSource);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return newSource;
        }

        public int SourceMaterialCount()
        {
            return _dataContext.SourceMaterials!.Count();
        }

        public SourceMaterial? GetSourceMaterial(int id)
        {
            return _dataContext.SourceMaterials!.Where(source => source.Id == id).FirstOrDefault();
        }

        public IReadOnlyList<SourceMaterial> GetSources()
        {
            return _dataContext.SourceMaterials!.ToList();
        }

        public async Task DeleteSourceAsync(int id)
        {
            var source = await _dataContext.SourceMaterials!.FindAsync(id);

            _dataContext.SourceMaterials!.Remove(source!);
            await _dataContext.SaveChangesAsync();
        }
    }
}
