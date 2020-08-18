using SP.DataLayer.Context;
using SP.ServiceLayer.Services;
using StructureMap;
using StructureMap.Pipeline;

public class DefaultRegistry : Registry
{
    public DefaultRegistry()
    {
        For<IUserRepository>().LifecycleIs(Lifecycles.Container).Use<UserRepository>();
        For<ICallRepository>().LifecycleIs(Lifecycles.Container).Use<CallRepository>();
        For<IMediaRepository>().LifecycleIs(Lifecycles.Container).Use<MediaRepository>();
        For<IRoleRepository>().LifecycleIs(Lifecycles.Container).Use<RoleRepository>();
        For<IServiceRepository>().LifecycleIs(Lifecycles.Container).Use<ServiceRepository>();
        For<ISettingRepository>().LifecycleIs(Lifecycles.Container).Use<SettingRepository>();
        For<ISmsRepository>().LifecycleIs(Lifecycles.Container).Use<SmsRepository>();
        For<IMemberRepository>().LifecycleIs(Lifecycles.Container).Use<MemberRepository>();
        For<IUserVisitRepository>().LifecycleIs(Lifecycles.Container).Use<UserVisitRepository>();
        For<IMemberVisitRepository>().LifecycleIs(Lifecycles.Container).Use<MemberVisitRepository>();
        For<ILogUserRepository>().LifecycleIs(Lifecycles.Container).Use<LogUserRepository>();

        For<IUnitOfWork>().LifecycleIs(Lifecycles.Container).Use<DatabaseContext>();
    }
}