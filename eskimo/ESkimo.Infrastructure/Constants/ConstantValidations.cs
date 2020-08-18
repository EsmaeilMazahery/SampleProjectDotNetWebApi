
namespace ESkimo.Infrastructure.Constants
{
    public static class ConstantValidations
    {
        public const int NameLength = 1000;
        public const int TitleLength = 2500;
        public const int DescriptionLength = 4000;
        public const int WebAddressLength = 1000;
        public const int IdentityLength = 1000;
        public const int UsernameLength = 1000;
        public const int NationalCodeLength = 10;
        public const int PasswordLength = 1000;
        public const int AddressLength = 1000;
        public const int MobileLength = 1000;
        public const int EmailLength = 1000;
        public const int ColorLength = 9;

        public const string ColorRegEx = @"^#[0-9A-Fa-f]{3-8}$";
        public const string MobileRegEx = @"^0?9[0-9]{9}$";
        public const string EmailRegEx = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        public const string SUbDomainRegEx = @"^[A-Za-z0-9](?:[A-Za-z0-9\-]{0,61}[A-Za-z0-9])?$";

        public const string amountTypeTable = "decimal(18,4)";
    }
}
