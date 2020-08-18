namespace SP.Infrastructure.Extensions
{
    public static class Constants
    {
        public const int NameLength = 100;
        public const int IdentifierNumberLength = 10;
        public const int UsernameLength = 100;
        public const int TitleLength = 500;
        public const int AddressLength = 500;
        public const int ShortDescriptionLength = 1000;
        public const int DescriptionLength = 4000;
        public const int TelLength = 10;
        public const int MobileLength = 10;
        public const int PasswordLength = 100;
        public const int HashPasswordLength = 1000;
        public const int EmailLength = 1000;
        public const int PostalCodeLength = 10;

        public const string UsernameRegex = @"^[a-zA-Z0-9]+$";
        public const string EmailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        public const string MobileRegex = @"^9[0-9]{9}$";
        public const string TelRegex = @"^9[0-9]{9}$";
        public const string EnumRegex = @"^[0-9]+$";

        public static class Roles
        {

        }

        public static class Settings
        {
            public const string SmsPanelNumber = "SmsPanelNumber";
            public const string SmsPanelUsername = "SmsPanelUsername";
            public const string SmsPanelPassword = "SmsPanelPassword";
        }
    }
}
