using MyCloa.Common.Ioc;

namespace ApiServerSample
{
    /// <summary>
    /// Ioc服务实现
    /// </summary>
    public class ResolveService : ResolveBase
    {
        private IServiceProvider serviceCollection;
        public ResolveService(IServiceProvider service) : base(default)
        {
            this.serviceCollection = service;
        }
        public override T Resolve<T>()
        {
            return this.serviceCollection.GetService<T>();
        }
    }
}
