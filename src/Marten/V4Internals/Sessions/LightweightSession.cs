using Marten.Services;
using Marten.Storage;

namespace Marten.V4Internals.Sessions
{
    public class LightweightSession: NewDocumentSession
    {
        public LightweightSession(IDocumentStore store, IManagedConnection database, ISerializer serializer, ITenant tenant, IProviderGraph provider, StoreOptions options) : base(store, database, serializer, tenant, options)
        {
        }

        protected override IDocumentStorage<T> selectStorage<T>(DocumentProvider<T> provider)
        {
            return provider.Lightweight;
        }
    }
}
