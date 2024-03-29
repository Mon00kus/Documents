namespace Documents.Backend.Data
{
    public class SeedEnsureDb
    {
        private readonly DataContext _dataContext;

        public SeedEnsureDb(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task SeedEnsureAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
        }
    }
}
