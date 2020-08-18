namespace ESkimo.Infrastructure.Extensions
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

        public static class AdminSessionFields
        {
            public const string Admin = "Admin";
        }

        public static class UserSessionFields
        {
            public const string User = "User";
        }

        public static class MemberSessionFields
        {
            public const string Member = "Member";
            public const string Admin = "Admin";
        }
        public static class ManagementSessionFields
        {
            public const string Admin = "Admin";
        }

        public static class Settings
        {
            public const string SmsPanelNumber = "SmsPanelNumber";
            public const string SmsPanelUsername = "SmsPanelUsername";
            public const string SmsPanelPassword = "SmsPanelPassword";
        }

        public static class String
        {
            public const string DefaulUser_Mobile = "9171111111";
            public const string DefaulUser_FirstName = "نام";
            public const string DefaulUser_LastName = "نام";
            public const string DefaulUser_Password = "123456";
            public const string DefaulUser_Username = "admin";
            public const string DefaulUser_NationalCode = "0000000000";

            public const string ForgetPassword = @"سامانه ارسال 
                                                        نام کاربری: {0}
                                                        رمزعبور: {1}";
        }
    }
}
