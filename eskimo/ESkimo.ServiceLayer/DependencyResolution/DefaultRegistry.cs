using ESkimo.DataLayer.Context;
using ESkimo.ServiceLayer.Services;
using StructureMap;
using StructureMap.Pipeline;

public class DefaultRegistry : Registry
{
    public DefaultRegistry()
    {
        For<IAreaRepository>().LifecycleIs(Lifecycles.Container).Use<AreaRepository>();
        For<IAreaPriceRepository>().LifecycleIs(Lifecycles.Container).Use<AreaPriceRepository>();
        For<IBlogCategoryRepository>().LifecycleIs(Lifecycles.Container).Use<BlogCategoryRepository>();
        For<IBlogCommentRepository>().LifecycleIs(Lifecycles.Container).Use<BlogCommentRepository>();
        For<IBlogPostRepository>().LifecycleIs(Lifecycles.Container).Use<BlogPostRepository>();
        For<IBrandRepository>().LifecycleIs(Lifecycles.Container).Use<BrandRepository>();
        For<ICategoryRepository>().LifecycleIs(Lifecycles.Container).Use<CategoryRepository>();
        For<ICommentRepository>().LifecycleIs(Lifecycles.Container).Use<CommentRepository>();
        For<IDiscountCodeRepository>().LifecycleIs(Lifecycles.Container).Use<DiscountCodeRepository>();
        For<IDiscountFactorRepository>().LifecycleIs(Lifecycles.Container).Use<DiscountFactorRepository>();
        For<IFactorItemRepository>().LifecycleIs(Lifecycles.Container).Use<FactorItemRepository>();
        For<IFactorRepository>().LifecycleIs(Lifecycles.Container).Use<FactorRepository>();
        For<IMemberLocationRepository>().LifecycleIs(Lifecycles.Container).Use<MemberLocationRepository>();
        For<IMemberOrderPeriodRepository>().LifecycleIs(Lifecycles.Container).Use<MemberOrderPeriodRepository>();
        For<IMemberRepository>().LifecycleIs(Lifecycles.Container).Use<MemberRepository>();
        For<IMemberAskRepository>().LifecycleIs(Lifecycles.Container).Use<MemberAskRepository>();
        For<IPaymentRepository>().LifecycleIs(Lifecycles.Container).Use<PaymentRepository>();
        For<IPeriodTypeRepository>().LifecycleIs(Lifecycles.Container).Use<PeriodTypeRepository>();
        For<IPocketPostRepository>().LifecycleIs(Lifecycles.Container).Use<PocketPostRepository>();
        For<IProductPriceRepository>().LifecycleIs(Lifecycles.Container).Use<ProductPriceRepository>();
        For<IProductRepository>().LifecycleIs(Lifecycles.Container).Use<ProductRepository>();
        For<IProductTypeRepository>().LifecycleIs(Lifecycles.Container).Use<ProductTypeRepository>();
        For<IRoleRepository>().LifecycleIs(Lifecycles.Container).Use<RoleRepository>();
        For<ISettingRepository>().LifecycleIs(Lifecycles.Container).Use<SettingRepository>();
        For<ISmsRepository>().LifecycleIs(Lifecycles.Container).Use<SmsRepository>();
        For<IUserRepository>().LifecycleIs(Lifecycles.Container).Use<UserRepository>();
        For<IUserSessionUpdateRepository>().LifecycleIs(Lifecycles.Container).Use<UserSessionUpdateRepository>();

        For<ILogRepository>().LifecycleIs(Lifecycles.Container).Use<LogRepository>();

        For<IUnitOfWork>().LifecycleIs(Lifecycles.Container).Use<DatabaseContext>();
    }
}