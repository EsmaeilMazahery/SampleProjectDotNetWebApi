using ESkimo.DataLayer.Context;
using ESkimo.ServiceLayer.Services;
using StructureMap;
using StructureMap.Pipeline;

public class SchedulerRegistry : Registry
{
    public SchedulerRegistry()
    {
        For<IAreaRepository>().LifecycleIs(Lifecycles.Singleton).Use<AreaRepository>();
        For<IAreaPriceRepository>().LifecycleIs(Lifecycles.Singleton).Use<AreaPriceRepository>();
        For<IBrandRepository>().LifecycleIs(Lifecycles.Singleton).Use<BrandRepository>();
        For<IBlogCategoryRepository>().LifecycleIs(Lifecycles.Singleton).Use<BlogCategoryRepository>();
        For<IBlogCommentRepository>().LifecycleIs(Lifecycles.Singleton).Use<BlogCommentRepository>();
        For<IBlogPostRepository>().LifecycleIs(Lifecycles.Singleton).Use<BlogPostRepository>();
        For<ICategoryRepository>().LifecycleIs(Lifecycles.Singleton).Use<CategoryRepository>();
        For<ICommentRepository>().LifecycleIs(Lifecycles.Singleton).Use<CommentRepository>();
        For<IDiscountCodeRepository>().LifecycleIs(Lifecycles.Singleton).Use<DiscountCodeRepository>();
        For<IDiscountFactorRepository>().LifecycleIs(Lifecycles.Singleton).Use<DiscountFactorRepository>();
        For<IFactorItemRepository>().LifecycleIs(Lifecycles.Singleton).Use<FactorItemRepository>();
        For<IFactorRepository>().LifecycleIs(Lifecycles.Singleton).Use<FactorRepository>();
        For<IMemberLocationRepository>().LifecycleIs(Lifecycles.Singleton).Use<MemberLocationRepository>();
        For<IMemberOrderPeriodRepository>().LifecycleIs(Lifecycles.Singleton).Use<MemberOrderPeriodRepository>();
        For<IMemberRepository>().LifecycleIs(Lifecycles.Singleton).Use<MemberRepository>();
        For<IMemberAskRepository>().LifecycleIs(Lifecycles.Singleton).Use<MemberAskRepository>();
        For<IPaymentRepository>().LifecycleIs(Lifecycles.Singleton).Use<PaymentRepository>();
        For<IPeriodTypeRepository>().LifecycleIs(Lifecycles.Singleton).Use<PeriodTypeRepository>();
        For<IPocketPostRepository>().LifecycleIs(Lifecycles.Singleton).Use<PocketPostRepository>();
        For<IProductPriceRepository>().LifecycleIs(Lifecycles.Singleton).Use<ProductPriceRepository>();
        For<IProductRepository>().LifecycleIs(Lifecycles.Singleton).Use<ProductRepository>();
        For<IProductTypeRepository>().LifecycleIs(Lifecycles.Singleton).Use<ProductTypeRepository>();
        For<IRoleRepository>().LifecycleIs(Lifecycles.Singleton).Use<RoleRepository>();
        For<ISettingRepository>().LifecycleIs(Lifecycles.Singleton).Use<SettingRepository>();
        For<ISmsRepository>().LifecycleIs(Lifecycles.Singleton).Use<SmsRepository>();
        For<IUserRepository>().LifecycleIs(Lifecycles.Singleton).Use<UserRepository>();
        For<IUserSessionUpdateRepository>().LifecycleIs(Lifecycles.Singleton).Use<UserSessionUpdateRepository>();

        For<ILogRepository>().LifecycleIs(Lifecycles.Singleton).Use<LogRepository>();


        For<IUnitOfWork>().LifecycleIs(Lifecycles.Singleton).Use<DatabaseContext>();
    }
}