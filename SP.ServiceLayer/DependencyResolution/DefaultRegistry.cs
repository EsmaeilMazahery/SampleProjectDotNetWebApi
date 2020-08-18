using SP.DataLayer.Context;
using SP.ServiceLayer.Services;
using StructureMap;
using StructureMap.Pipeline;

public class DefaultRegistry : Registry
{
    public DefaultRegistry()
    {
        For<IAreaRepository>().LifecycleIs(Lifecycles.Container).Use<AreaRepository>();
        For<IAdminRepository>().LifecycleIs(Lifecycles.Container).Use<UserRepository>();
        For<IBookmarkRepository>().LifecycleIs(Lifecycles.Container).Use<BookmarkRepository>();
        For<ICallRepository>().LifecycleIs(Lifecycles.Container).Use<CallRepository>();
        For<IBusinessRepository>().LifecycleIs(Lifecycles.Container).Use<BusinessRepository>();
        For<IContactRepository>().LifecycleIs(Lifecycles.Container).Use<ContactRepository>();
        For<IInfoVerifyRepository>().LifecycleIs(Lifecycles.Container).Use<InfoVerifyRepository>();
        For<ILocationRepository>().LifecycleIs(Lifecycles.Container).Use<LocationRepository>();
        For<IMediaRepository>().LifecycleIs(Lifecycles.Container).Use<MediaRepository>();
        For<IPriceRepository>().LifecycleIs(Lifecycles.Container).Use<PriceRepository>();
        For<IPriceGroupRepository>().LifecycleIs(Lifecycles.Container).Use<PriceGroupRepository>();
        For<IProjectRepository>().LifecycleIs(Lifecycles.Container).Use<ProjectRepository>();
        For<IRoleRepository>().LifecycleIs(Lifecycles.Container).Use<RoleRepository>();
        For<ISampleRepository>().LifecycleIs(Lifecycles.Container).Use<SampleRepository>();
        For<ISampleGroupRepository>().LifecycleIs(Lifecycles.Container).Use<SampleGroupRepository>();
        For<IServiceRepository>().LifecycleIs(Lifecycles.Container).Use<ServiceRepository>();
        For<ISettingRepository>().LifecycleIs(Lifecycles.Container).Use<SettingRepository>();
        For<ISiteRepository>().LifecycleIs(Lifecycles.Container).Use<SiteRepository>();
        For<ISmsRepository>().LifecycleIs(Lifecycles.Container).Use<SmsRepository>();
        For<IUserRepository>().LifecycleIs(Lifecycles.Container).Use<MemberRepository>();
        For<IUserVisitRepository>().LifecycleIs(Lifecycles.Container).Use<UserVisitRepository>();
        For<IMemberVisitRepository>().LifecycleIs(Lifecycles.Container).Use<MemberVisitRepository>();
        For<IRep_DailyChartRepository>().LifecycleIs(Lifecycles.Container).Use<Rep_DailyChartRepository>();
        For<ILogUserRepository>().LifecycleIs(Lifecycles.Container).Use<LogUserRepository>();

        For<IKeywordRepository>().LifecycleIs(Lifecycles.Container).Use<KeywordRepository>();
        For<IKeywordServiceRepository>().LifecycleIs(Lifecycles.Container).Use<KeywordServiceRepository>();
        For<IKeywordSampleRepository>().LifecycleIs(Lifecycles.Container).Use<KeywordSampleRepository>();
        For<IKeywordPriceRepository>().LifecycleIs(Lifecycles.Container).Use<KeywordPriceRepository>();
        For<IKeywordCordinateRepository>().LifecycleIs(Lifecycles.Container).Use<KeywordCordinateRepository>();
        For<IKeywordLoactionRepository>().LifecycleIs(Lifecycles.Container).Use<KeywordLoactionRepository>();
        For<IKeywordExceptionRepository>().LifecycleIs(Lifecycles.Container).Use<KeywordExceptionRepository>();
        For<IKeywordTranslateRepository>().LifecycleIs(Lifecycles.Container).Use<KeywordTranslateRepository>();

        For<ICommentRepository>().LifecycleIs(Lifecycles.Container).Use<CommentRepository>();

        For<ILikeRepository>().LifecycleIs(Lifecycles.Container).Use<LikeRepository>();

        For<IUnitOfWork>().LifecycleIs(Lifecycles.Container).Use<DatabaseContext>();
    }
}