using LazyCache;

namespace AccountPortal.Domain.Processors.Interfaces
{
    public interface ICacheProcessor
    {
        IAppCache GetCache();
    }
}
