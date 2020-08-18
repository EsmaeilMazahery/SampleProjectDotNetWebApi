
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ESkimo.Infrastructure.Models;

namespace ESkimo.Infrastructure.Enumerations
{

    [AttributeUsage(AttributeTargets.All)]
    public class ValueAttribute : DescriptionAttribute
    {
        public ValueAttribute(object value)
        {
            this.Value = value;
        }

        public object Value { get; set; }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class NameAttribute : DescriptionAttribute
    {
        public NameAttribute(string Name)
        {
            this.Name = Name;
        }

        public string Name { get; set; }

        public static implicit operator string(NameAttribute c)
        {
            return c.Name == null ? "" : c.Name;
        }
    }

    public static class Extensions
    {
        public static object GetValue(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<ValueAttribute>().Value;
        }

        public static string GetName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<NameAttribute>().Name;
        }
    }

    public static class Enum<T> where T : struct
    {
        /// <summary>
        /// آیا این مقدار در اینام تعریف شده یا نه
        /// </summary>
        public static bool Has(byte value)
        {
            return Enum.IsDefined(typeof(T), value);
        }

    }

    public enum DeviceType : byte
    {
        WebUser = 1,
        WebCustomer = 2,
        WebAdmin = 3,
        Android = 4,
        Ios = 5
    }

    public enum SmsVerifyLookupType : byte
    {
        Sms = 0,
        Call = 1
    }

    public enum SmsTemplate : byte
    {
        [Description("ESkimoRegister")]
        Register = 1,

        [Description("ESkimoForgetPassword")]
        ForgetPassword = 1,
    }

    public enum SettingType : byte
    {
        ZarinPalMerchent = 1,
        CustomerSite = 2,
        ConfirmComment = 3,
        DescriptionPayment = 4,

        WebSiteAddress = 5,
        WebSitePhone = 6,
        WebSiteEmail = 7,
        WebSiteDescription = 8,
        WebSiteTwitter = 9,
        WebSiteInstagram = 10,
        WebSiteFaceBook = 11,

        SmsApiKey = 12,
        SmsSender = 13,

        BannerImage = 14,
        BannerLink = 15,

        BannerImage1 = 16,
        BannerLink1 = 17,

        BannerImage2 = 18,
        BannerLink2 = 19,

        ClipVideo=20,
        ClipLink=21,
        ClipTitle=22
    }

    public enum LogType : byte
    {
        Api_Generic,

        Api_Payment,
        Api_Factor_Pay,

        Client_Generic
    }

    public enum LogLevel : byte
    {
        ALL=0,
        TRACE=1,
        DEBUG=2,
        INFO=3,
        WARN=4,
        ERROR=5,
        FATAL=6,
        OFF=7
    }

    public enum SortDirection : byte
    {
        ASC,
        DESC
    }



    public enum SiteTheme : byte
    {
        [Description("تم 1")]
        [Name("")]
        Theme1 = 1,

        [Description("تم 2")]
        [Name("")]
        Theme2 = 2,

        [Description("تم 3")]
        [Name("")]
        Theme3 = 3,

        [Description("تم 4")]
        [Name("")]
        Theme4 = 4,
    }

    public static class CustomEnumConverter
    {
        /// <summary>
        /// Get description attribute value of this enum item or return null.
        /// </summary>
        public static string GetDescriptionOrNull<T>(this T enumerationValue) where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return null;
        }

        /// <summary>
        /// Get description attribute value of this enum item or string of value.
        /// </summary>
        public static string GetDescriptionOrDefault<T>(this T enumerationValue) where T : struct
        {
            string temp = enumerationValue.GetDescriptionOrNull();
            if (string.IsNullOrWhiteSpace(temp))
                return enumerationValue.ToString();
            else
                return temp;
        }
    }
    public enum AdminSmsMessageSendType : byte
    {
        [Description("فراموشی رمزعبور")]
        RememberPassword = 1
    }
    public enum PropertiseKey : byte
    {
        [DefaultValue("")]
        [Description("شماره پنل پیامک")]
        SmsPanelNumber = 1,
        [DefaultValue("")]
        [Description("نام کاربری پنل پیامک")]
        SmsPanelUsername = 2,
        [DefaultValue("")]
        [Description("گذرواژه پنل پیامک")]
        SmsPanelPassword = 3
    }
    public enum PropertiseType : byte
    {
        [Description("String")]
        String = 1,
        [Description("Int")]
        Int = 2,
        [Description("Float")]
        Float = 3,
        [Description("DateTime")]
        DateTime = 4,
        [Description("DropDown")]
        DropDown = 5,
        [Description("CheckList")]
        CheckList = 6,
        [Description("Radio")]
        Radio = 7,
    }
    public enum FactorStatus : byte
    {
        [Description("ثبت سفارش")]
        RegFactor = 1,
        [Description("پرداخت شده")]
        Payment = 2,
        [Description("ارسال شده")]
        Sent = 3,
        [Description("تکمیل شده")]
        Receive = 4,
    }
    public enum UserCheckStatus : byte
    {
        Reject = 0,
        Verified = 1,
        NewCheck = 2,
    }

    public enum SmsStatus : byte
    {
        Successful = 0,
        NotValid = 1,
        Err = 2,
    }
    public enum JqueryResultType : byte
    {
        Default = 0,
        Success = 1,
        Info = 2,
        Warning = 3,
        Danger = 4
    }

    public enum ErrorType : byte
    {
        Default = 1,
        NotFound = 2,
        ModelState = 3,
        AccountExpired = 4,
    }

    public enum ServiceType : byte
    {
        [Description("شخصی")]
        Personal = 1,
        [Description("شرکت / دفتر")]
        Company = 2,
        [Description("فروشگاه")]
        Store = 3,
        [Description("کارخانه / کارگاه / تولیدی")]
        Factory = 4,
        [Description("سایر")]
        Other = 5,
    }

    public enum MemberAskType : byte
    {
        [Description("سوالات متداول")]
        Question = 1,

        [Description("تماس با ما")]
        Contact = 2,

        [Description("نظرات")]
        FeedBack = 3,
    }

    public enum TroubleUserType
    {
        User_name = 1,
        User_family = 2,
        User_email = 3,
        User_registerDate = 4,
        User_image = 5,

        Service_name = 10,
        Service_serviceType = 11,
        Service_website = 12,
        Service_post = 13,
        Service_description = 14,
        Service_extraDescription = 15,
        Service_major = 16,
        Service_portfolio = 17,
        Service_rating = 18,
        Service_unitOfWork = 19,
        Service_unitOfWorkType = 20,
        Service_registerDate = 21,

        Sample_name = 30,
        Sample_address = 31,
        Sample_description = 32,
        Sample_lat_lng = 33,
        Sample_media = 34,

        Contact_address = 40,
        Contact_description = 41,
        Contact_schedules = 42,
        Contact_lat_lng = 43,
        Contact_fax = 44,
        Contact_phone = 45,

        Price_dateFromDiscount = 50,
        Price_dateToDiscount = 51,
        Price_description = 52,
        Price_desDiscount = 53,
        Price_percentageDiscount = 54,
        Price_priceFrom = 55,
        Price_priceTo = 56,
        Price_priceType = 57,
        Price_registerDate = 58,
        Price_media = 59,

        Cataloge_address = 70,
        Cataloge_title = 71,

        Email_address = 80,
    }

    public enum UserCheckResult
    {
        ComplateReject = 1,
        SmsReject = 2,
        Alert = 3,
    }

    public enum SmsSendStatus : byte
    {
        [Description("ارسال شد")]
        Sent = 1,

        [Description("شماره گیرنده خالی است")]
        ReceiverNumberEmpty = 2,

        [Description("متن پیام خالی است")]
        MessageTextEmpty = 3,

        [Description("نام کاربری یا رمز عبور صحیح نیست")]
        InvalidUsernameOrPassword = 4,

        [Description("اعتبار حساب کافی نیست")]
        CreditLow = 5,

        [Description("حساب کاربر فعال نیست")]
        UserDeactive = 6,

        [Description("شماره گیرنده معتبر نیست")]
        InvalidReceiverNumber = 7,

        [Description("شماره فرستنده معتبر نیست")]
        InvalidSenderNumber = 8,

        [Description("هیچ شماره ای به حساب شما اختصاص نیافته است")]
        InvalidNumber = 9,

        [Description("متن پیام خالی است")]
        MessageTextLenght = 10,

        [Description("خطا در سرویس دهنده")]
        ServiceError = 11,

        [Description("خطا در برقراری ارتباط با سوییچ مخابرات")]
        ErrorCommunicatingToSwitchTelecommunications = 12,

        [Description("شناسه پیام نامعتبر است")]
        InvalidMessageID = 13,

        [Description("آدرس انتقال ترافیک معتبر نیست")]
        AddressIsNotValidTraffic = 14,

        [Description("رمز عبور خالی است")]
        PasswordEmpty = 15,

        [Description("پیام دریافتی معتبر نمی باشد")]
        MessageIsNotValid = 16,

        [Description("شماره گیرنده امکان دریافت پیام تبلیغاتی ندارد")]
        ReceiverNumberNotReceiveAdvertisingSms = 17,

        [Description("شماره فرستنده خالی است")]
        SenderNumberEmpty = 18,

        [Description("نام کاربری پنل پیامک خالی است")]
        UsernameEmpty = 19,

        [Description("خطای ناشناخته")]
        UnknownError = 20,

        [Description("در انتظار ارسال")]
        WaitingForSend = 21,

        [Description("اعتبار پیامک به پایان رسیده")]
        AccountCreditLow = 22,
    }

    public enum LogUserType
    {
        DeleteService = 1,
        EnableService = 2,
        DisableService = 3,

        DeleteUser = 4,
        EnableUser = 5,
        DisableUser = 6,

        WebsiteName = 7,
        DeleteWebsite = 8,
        EnableWebsite = 9,
        DisableWebsite = 10,

        RegisterAlert = 11,
        DeleteAlert = 12,
        Login = 13,
        Edit = 14,
    }

    public enum VerifiedStatus
    {
        Verified = 1,
        Notverified = 2,
        NotChecked = 3,
    }

    public enum PayType
    {
        Factor = 1,
        Month = 2,
        TwoMonth = 3,
        ThreeMonth = 4,
        SixMonth = 5,
        Year
    }

    public enum PasswordScore
    {
        Blank = 0,
        VeryWeak = 1,
        Weak = 2,
        Medium = 3,
        Strong = 4,
        VeryStrong = 5,
        VeryVeryStrong = 6
    }
    public enum ProviderType : byte
    {
        [Description("شخص")]
        Person = 1,
        [Description("شرکت")]
        Company,
        [Description("کارخانه")]
        Factory
    }

    public enum PaymentType : byte
    {
        [Description("فاکتور")]
        Factor = 1,
        [Description("در محل")]
        Person = 2,
        [Description("صورتحساب")]
        Payment = 3,
    }

    public enum SocialNetworkType : byte
    {
        [Description("FaceBook")]
        FaceBook = 1,
        [Description("Twitter")]
        Twitter = 2,
        [Description("Telegram")]
        Telegram = 3,
        [Description("Instagram")]
        Instagram = 4,
        [Description("Linkedin")]
        Linkedin = 5,
        [Description("google-plus")]
        googleplus = 6
    }

    public enum MediaType : byte
    {
        [Description("jpg")]
        jpg = 1,
        [Description("mp4")]
        mp4 = 2,
        [Description("zip")]
        zip = 3,
        [Description("xls")]
        xls = 4,
        [Description("xlsx")]
        xlsx = 5,
        [Description("pdf")]
        pdf = 6,
        [Description("doc")]
        doc = 7,
        [Description("docx")]
        docx = 8,
        [Description("jpeg")]
        jpeg = 9,
        [Description("bmp")]
        bmp = 10,
        [Description("png")]
        png = 11,
        [Description("gif")]
        gif = 12,
        [Description("txt")]
        txt = 13,
        [Description("html")]
        html = 14,
        [Description("htm")]
        htm = 15,
        [Description("css")]
        css = 16,
        [Description("7zip")]
        sevenzip = 17,
        [Description("mp3")]
        mp3 = 18,
        [Description("ogg")]
        ogg = 19,
        [Description("wav")]
        wav = 20,
        [Description("wma")]
        wma = 21,
        [Description("7z")]
        sevenz = 22,
        [Description("pps")]
        pps = 23,
        [Description("ppt")]
        ppt = 24,
        [Description("pptx")]
        pptx = 25,
        [Description("xlr")]
        xlr = 26,
        [Description("ods")]
        ods = 27,
        [Description("odp")]
        odp = 28,
        [Description("3gp")]
        threegp = 29,
        [Description("avi")]
        avi = 30,
        [Description("flv")]
        flv = 31,
        [Description("h264")]
        h264 = 32,
        [Description("mkv")]
        mkv = 33,
        [Description("mov")]
        mov = 34,
        [Description("mpg")]
        mpg = 35,
        [Description("mpeg")]
        mpeg = 36,
        [Description("swf")]
        swf = 37,
        [Description("wmv")]
        wmv = 38,
        [Description("odt")]
        odt = 39,
        [Description("wks")]
        wks = 40,
        [Description("wps")]
        wps = 41,
        [Description("rar")]
        rar = 42
    }

    public enum RolesKey : byte
    {
        [Description("کاربر")]
        [Name("مدیریت کاربران")]
        User = 1,
        [Description("داشبورد")]
        [Name("داشبورد")]
        Dashboard = 2,
    }
    public enum SearchType : byte
    {
        [Description("سرویس ها")]
        Service = 1,
        [Description("نمونه کارها")]
        Sample = 2
    }
}
