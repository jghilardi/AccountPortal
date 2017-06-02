using LazyCache;

namespace AccountPortal.data
{
    public interface ICacheRepository
    {
        IAppCache GetCache();
    }
}
