using LazyCache;

namespace AccountPortal.data
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IAppCache _cache;

        public CacheRepository(IAppCache cache)
        {
            _cache = cache;
        }

        public IAppCache GetCache()
        {
            return _cache;
        }
    }
}
