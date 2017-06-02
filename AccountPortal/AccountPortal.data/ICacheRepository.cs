using LazyCache;

namespace AccountPortal.Data
{
    public interface ICacheRepository
    {
        IAppCache GetCache();
    }
}
