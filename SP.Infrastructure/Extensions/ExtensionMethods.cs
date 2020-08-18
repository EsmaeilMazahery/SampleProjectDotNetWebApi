using SP.Infrastructure.Enumerations;
using SP.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SP.Infrastructure.Extensions
{
    public static class ExtensionMethods
    {

        public static bool DataTimeTryParseNullable(string str,out DateTime? result)
        {
            result = null;
            if (!string.IsNullOrEmpty(str) && DateTime.TryParse(str, out DateTime lastCheckRobotServiceTemp))
            {
                result = lastCheckRobotServiceTemp;
                return true;
            }
            return false;
        }


        public static IEnumerable<TSource> Func<TSource>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TSource>> func)
        {
            return func(source);
        }

        /// <summary>
        /// مرتب سازی توسط نام فیلد - حداکثر تا یک نقطه را پشتیبانی میکند
        /// </summary>
        /// <param name="query">کوئری</param>
        /// <param name="propertyName">نام پروپرتی که قرار است بر اساس آن مرتب سازی کنیم</param>
        /// <param name="direction">نوع مرتب سازی</param>
        /// <param name="modelsNamespace">نیم اسپیس مدل ها - برای پیدا کردن مدل ها لازم میشود</param>
        /// <param name="assemblyName">اسمبلی نیم کل پروژه - برای پیدا کردن مدل ها لازم میشود</param>
        /// <param name="sortEquals">برای فیلد هایی که سورت آنها در وب گرید باگ دارد اینجا میتوان یک نام دیگر برای فیلد مشکل دار تعریف کرد. مثال: "FakeName|RealName"</param>
        public static IOrderedQueryable<TSource> OrderByPropertyName<TSource>(this IQueryable<TSource> query, string propertyName, string direction, params string[] sortEquals)
        {
            try
            {
                // این دو متغیر در هر پروژه باید تغییر کنند
                string modelsNamespace = "ExpertsPortal.DataLayer.Models";
                string assemblyName = "ExpertsPortal.DataLayer";

                // convert sort equals
                string sortEqualItem = sortEquals.FirstOrDefault(f => f.Contains(propertyName + "|"));
                if (!string.IsNullOrWhiteSpace(sortEqualItem))
                    propertyName = sortEqualItem.Split('|')[1];

                string[] splits = propertyName.Split('.');

                Type entityType = typeof(TSource);
                ParameterExpression arg = Expression.Parameter(entityType, "x");
                PropertyInfo propertyInfo = null;
                MemberExpression property = null;
                LambdaExpression selector = null;

                if (splits.Length == 1) // Create x=>x.PropName
                {
                    propertyInfo = entityType.GetProperty(splits[0]);
                    property = Expression.Property(arg, splits[0]);
                    selector = Expression.Lambda(property, new ParameterExpression[] { arg });
                }
                else if (splits.Length == 2) // Create x=>x.Type.PropName
                {
                    propertyInfo = entityType.GetProperty(splits[0]);
                    property = Expression.Property(arg, splits[0]);
                    property = Expression.Property(property, splits[1]);
                    selector = Expression.Lambda(property, new ParameterExpression[] { arg });
                    Type secondType = Type.GetType(modelsNamespace + "." + splits[0] + ", " + assemblyName);
                    propertyInfo = secondType.GetProperty(splits[1]);
                }
                else // این متد توان مرتب سازی فیلدی با دو جوین تو در تو را ندارد. Example --> Member.Group.Name
                    throw new Exception("PropertyName in invalid.");

                //Get System.Linq.Queryable.OrderBy() method.
                var enumarableType = typeof(Queryable);
                var method = enumarableType.GetMethods()
                    .Where(m => m.Name == (direction.ToLower() == "asc" ? "OrderBy" : "OrderByDescending") && m.IsGenericMethodDefinition)
                    .Where(m =>
                    {
                        var parameters = m.GetParameters().ToList();
                        //Put more restriction here to ensure selecting the right overload                
                        return parameters.Count == 2;//overload that has 2 parameters
                    }).Single();
                //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
                MethodInfo genericMethod = method
                    .MakeGenericMethod(entityType, propertyInfo.PropertyType);

                /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
                Note that we pass the selector as Expression to the method and we don't compile it.
                By doing so EF can extract "order by" columns and generate SQL for it.*/
                var newQuery = (IOrderedQueryable<TSource>)genericMethod
                    .Invoke(genericMethod, new object[] { query, selector });
                return newQuery;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// صفحه بندی کردن  یک کوئری - برای استفاده راحت تر از متد های اسکیپ و تِیک
        /// </summary>
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, int pageSize, int pageIndex)
        {
            return query.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        }


        //public static string ToTraceQuery<T>(this IQueryable<T> query)
        //{
        //    ObjectQuery<T> objectQuery = GetQueryFromQueryable(query);

        //    var result = objectQuery.ToTraceString();
        //    foreach (var parameter in objectQuery.Parameters)
        //    {
        //        var name = "@" + parameter.Name;
        //        var value = "'" + parameter.Value.ToString() + "'";
        //        result = result.Replace(name, value);
        //    }

        //    return result;
        //}

        public static IEnumerable<T> ToListStep<T>(this IQueryable<T> query, int step)
        {
            int skip = 0;
            while (true)
            {
                bool check = false;
                foreach (var item in query.Skip(skip).Take(step).ToList())
                {
                    check = true;
                    yield return item;
                }
                if (!check)
                    break;
                skip += step;
            }
        }

        /// <summary>
        /// تبدیل عدد فلوت به رشته قابل نمایش
        /// </summary>
        /// <param name="decimalSeparator">جدا کننده اعشار</param>
        public static string ToDisplayString(this float input, char decimalSeparator = '.')
        {
            char separator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            return input.ToString().Replace(separator, decimalSeparator);
        }

        /// <summary>
        /// آیا این رشته به فلوت تبدیل میشود؟
        /// </summary>
        /// <param name="decimalSeparators">کاراکترهای مجاز جدا کننده اعشار</param>
        public static bool TryParseToFloat(this string input, out float output, params char[] decimalSeparators)
        {
            char separator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            foreach (char i in decimalSeparators)
                input = input.Replace(i, separator);
            return float.TryParse(input, out output);
        }

        /// <summary>
        /// تبدیل رشته به فلوت
        /// </summary>
        /// <param name="decimalSeparators">کاراکترهای مجاز جدا کننده اعشار</param>
        public static float ToFloat(this string input, params char[] decimalSeparators)
        {
            char separator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            foreach (char i in decimalSeparators)
                input = input.Replace(i, separator);
            return float.Parse(input);
        }

        public static string TrimStart(this string text, string trim)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            if (text.StartsWith(trim))
                text = text.Substring(trim.Length);

            return text;
        }

        public static string TrimEnd(this string text, string trim)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            if (text.EndsWith(trim))
                text = text.Substring(0, text.Length - trim.Length);

            return text;
        }

        /// <summary>
        /// زیر رشته برمیگرداند - اگر ایندکس شروع یا طول از خود رشته بزرگتر بود خطا نمیدهد
        /// </summary>
        public static string SubstringWithoutLengthException(this string text, int startIndex, int length)
        {
            if (text.Length >= length + startIndex)
                return text.Substring(startIndex, length);
            else if (startIndex == 0)
                return text;
            else if (startIndex > 0 && startIndex < text.Length)
                return text.Substring(startIndex);
            else
                return string.Empty;
        }

        /// <summary>
        /// زیر رشته برمیگرداند - اگر ایندکس شروع یا طول از خود رشته بزرگتر بود خطا نمیدهد
        /// </summary>
        public static string SubstringWithoutLengthException(this string text, int length)
        {
            return text.SubstringWithoutLengthException(0, length);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// تبدیل یک رشته در فرمت "سلسله مراتبی" به لیست
        /// </summary>
        public static List<int> ToOrderList(this string input)
        {
            string[] splits = input.Split('-');
            List<int> list = new List<int>();
            foreach (string item in splits)
            {
                if (item.Length < 4)
                    throw new Exception("ToOrderList Exception: Wronge input string order format.");
                if (item.Length > 4)
                    throw new Exception("ToOrderList Exception: Maximum value of order item is 9999.");
                list.Add(Convert.ToInt32(item));
            }
            return list;
        }

        /// <summary>
        /// تبدیل یک لیست به رشته "سلسله مراتبی"
        /// </summary>
        public static string ToOrderString(this List<int> input)
        {
            if (input.Count <= 0)
                throw new Exception("ToHierarchyString Exception: Input list must have at least an int.");

            string temp = "";
            foreach (int item in input)
                temp += item.ToString("0000") + "-";
            return temp.TrimEnd("-");
        }

        /// <summary>
        /// تبدیل بولین به بله خیر
        /// </summary>
        public static string ToPersianString(this bool input)
        {
            if (input)
                return "بله";
            return "خیر";
        }

        /// <summary>
        /// داخلی ترین پیغام خطا را برمیگرداند
        /// </summary>
        public static string GetLastInnerExceptionMessage(this Exception ex)
        {
            if (ex == null)
                return null;
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex.Message;
        }
        
        public static PasswordScore PasswordCheckStrength(this string password)
        {
            if (password.Length >= 4)
                return PasswordScore.VeryVeryStrong;


            int score = 0;

            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 6)
                return PasswordScore.VeryWeak;

            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.Match(password, @"\d+", RegexOptions.ECMAScript).Success)
                score++;

            //فارسی
            if (Regex.Match(password, @"/[\u0600-\u06FF\s]/", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"/[a-z]/", RegexOptions.ECMAScript).Success &&
              Regex.Match(password, @"/[A-Z]/", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"/[a-z]/", RegexOptions.ECMAScript).Success ||
              Regex.Match(password, @"/[A-Z]/", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"/.[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]/", RegexOptions.ECMAScript).Success)
                score++;

            return (PasswordScore)score;
        }
    }
}
