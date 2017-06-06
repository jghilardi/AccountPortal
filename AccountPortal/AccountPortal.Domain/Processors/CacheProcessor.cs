using AccountPortal.Data;
using AccountPortal.Domain.Processors.Interfaces;
using LazyCache;

namespace AccountPortal.Domain.Processors
{
    public class CacheProcessor : ICacheProcessor
    {
        private readonly ICacheRepository _cacheRepository;
        public CacheProcessor(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public IAppCache GetCache()
        {
            return _cacheRepository.GetCache();
        }
    }
}
