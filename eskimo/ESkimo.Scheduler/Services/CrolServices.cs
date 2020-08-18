using ESkimo.DomainLayer.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using ESkimo.Infrastructure.Extensions;
using ESkimo.Scheduler.Models;
using System.Threading;
using ESkimo.Infrastructure;

namespace ESkimo.Scheduler.Services
{
    public static class CrolServices
    {
        public static List<CookieModel> Login(string digiUsername, string digiPassword)
        {
            //var digiUsername = "shadlyco@gmail.com";
            //var digiPassword = "Sh@13921219";

            var client = new RestClient("https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
            request.AddHeader("Referer", "https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
            request.AddHeader("Host", "seller.digikala.com");

            IRestResponse response = client.Execute(request);
            var Cookies = response.Cookies.ToList();

            client = new RestClient("https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
            request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
            request.AddHeader("Referer", "https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
            request.AddHeader("Host", "seller.digikala.com");

            foreach (var item in Cookies)
            {
                request.AddCookie(item.Name, item.Value);
            }

            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"login[email]\"\r\n\r\n" + digiUsername + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"login[password]\"\r\n\r\n" + digiPassword + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
            response = client.Execute(request);

            foreach (var i in response.Cookies)
            {
                Cookies.RemoveAll(a => a.Name == i.Name);
                Cookies.Add(i);
            }

            return Cookies.Select(s => new CookieModel() { Name = s.Name, Value = s.Value }).ToList();
        }

        public static (
            string sellerFirstName,
            string sellerLastName,
            string name,
            int sellerCode,
            DateTime contractstartdate,
            DateTime contractenddate) CrolSellerInfo(List<CookieModel> Cookies)
        {
            var client = new RestClient("https://seller.digikala.com/profile/display/");
            var request = new RestRequest(Method.GET);

            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Accept-Language", "en-US,en;q=0.5");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Host", "seller.digikala.com");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");

            foreach (var item in Cookies)
            {
                request.AddCookie(item.Name, item.Value);
            }

            IRestResponse response = client.Execute(request);

            var html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(response.Content);
            var document = html.DocumentNode;

            var sellerCode = document.QuerySelectorAll("form#profile-form .c-card__body--form input#seller-code").First().Attributes["value"].Value.getConvertor().ToInt;
            var contractstartdate = document.QuerySelectorAll("form#profile-form .c-card__body--form input#contract-start-date").First().Attributes["value"].Value;
            var contractenddate = document.QuerySelectorAll("form#profile-form .c-card__body--form input#contract-end-date").First().Attributes["value"].Value;
            var name = document.QuerySelectorAll("form#profile-form .c-card__body--form input#seller-business-name").First().Attributes["value"].Value;
            var sellerFirstName = document.QuerySelectorAll("form#profile-form .c-card__body--form input#seller-first-name").First().Attributes["value"].Value;
            var sellerLastName = document.QuerySelectorAll("form#profile-form .c-card__body--form input#seller-last-name").First().Attributes["value"].Value;

            return (
                    sellerFirstName,
                    sellerLastName,
                    name,
                    sellerCode,
                    PersianDateTime.Parse(contractstartdate).ToDateTime(),
                    PersianDateTime.Parse(contractenddate).ToDateTime()
                    );
        }

        public static (decimal timelySupply, decimal postingCommitment, decimal noReference) CrolSellerRating(List<CookieModel> Cookies)
        {
            var client = new RestClient("https://seller.digikala.com/profile/rating/");
            var request = new RestRequest(Method.GET);

            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Accept-Language", "en-US,en;q=0.5");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Host", "seller.digikala.com");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");

            foreach (var item in Cookies)
            {
                request.AddCookie(item.Name, item.Value);
            }

            IRestResponse response = client.Execute(request);

            var html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(response.Content);
            var document = html.DocumentNode;

            var rates = document.QuerySelectorAll(".c-rating-chart .c-rating-chart__stats .c-rating-chart__stat").ToList();

            var timelySupply = Convert.ToDecimal(rates[0].QuerySelectorAll(".c-rating-chart__stat-value").First().InnerText.Replace("٪", "").TrimName().NumberToEnglish());
            var postingCommitment = Convert.ToDecimal(rates[1].QuerySelectorAll(".c-rating-chart__stat-value").First().InnerText.Replace("٪", "").TrimName().NumberToEnglish());
            var noReference = Convert.ToDecimal(rates[2].QuerySelectorAll(".c-rating-chart__stat-value").First().InnerText.Replace("٪", "").TrimName().NumberToEnglish());

            return (timelySupply, postingCommitment, noReference);
        }

        public static List<categoryModel> CrolCategory(List<CookieModel> Cookies)
        {
            List<categoryModel> categories = new List<categoryModel>();
            categories.Add(new categoryModel() { id = 5966, parent = null, value = "کالای دیجیتال", address = "https://www.digikala.com/main/electronic-devices/" });
            categories.Add(new categoryModel() { id = 5968, parent = null, value = "آرایشی، بهداشتی و سلامت", address = "https://www.digikala.com/main/personal-appliance/" });
            categories.Add(new categoryModel() { id = 8450, parent = null, value = "خودرو، ابزار و اداری", address = "https://www.digikala.com/main/vehicles/" });
            categories.Add(new categoryModel() { id = 8749, parent = null, value = "مد و پوشاک", address = "https://www.digikala.com/search/category-apparel/" });
            categories.Add(new categoryModel() { id = 5967, parent = null, value = "خانه و آشپزخانه", address = "https://www.digikala.com/main/home-and-kitchen/" });
            categories.Add(new categoryModel() { id = 8, parent = null, value = "کتاب، لوازم تحریر و هنر", address = "https://www.digikala.com/main/book-and-media/" });
            categories.Add(new categoryModel() { id = 6741, parent = null, value = "اسباب بازی، کودک و نوزاد", address = "https://www.digikala.com/main/mother-and-child/" });
            categories.Add(new categoryModel() { id = 6124, parent = null, value = "ورزش و سفر", address = "https://www.digikala.com/main/sport-entertainment/" });
            categories.Add(new categoryModel() { id = 8895, parent = null, value = "خوردنی و آشامیدنی", address = "https://www.digikala.com/main/food-beverage/" });

            int index = 0;
            while (categories.Count() > index)
            {
                var client = new RestClient("https://seller.digikala.com/ajax/product/category/");
                var request = new RestRequest(Method.POST);

                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                request.AddHeader("Accept-Encoding", "gzip, deflate, br");
                request.AddHeader("Accept-Language", "en-US,en;q=0.5");
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("Host", "seller.digikala.com");
                request.AddHeader("Upgrade-Insecure-Requests", "1");
                request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("undefined", "id=" + categories[index].id, ParameterType.RequestBody);

                foreach (var item in Cookies)
                {
                    request.AddCookie(item.Name, item.Value);
                }

                IRestResponse response = client.Execute(request);

                var clienta = new RestClient(categories[index].address);
                var requesta = new RestRequest(Method.GET);

                requesta.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                requesta.AddHeader("Accept-Encoding", "gzip, deflate, br");
                requesta.AddHeader("Accept-Language", "en-US,en;q=0.5");
                requesta.AddHeader("Connection", "keep-alive");
                requesta.AddHeader("Host", "www.digikala.com");
                requesta.AddHeader("Upgrade-Insecure-Requests", "1");
                requesta.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");


                IRestResponse responsea = clienta.Execute(requesta);

                var html = new HtmlAgilityPack.HtmlDocument();
                html.LoadHtml(responsea.Content);
                var document = html.DocumentNode;

                var mainrows = document.QuerySelectorAll(".c-category-card__container .c-category-card").ToList();
                //var rows = document.QuerySelectorAll(".c-category-card .c-category-card__list li a").ToList();
                var depthrows = document.QuerySelectorAll(".c-catalog__list--depth .c-catalog__cat-item a.c-catalog__link").ToList();
                var subcategory = document.QuerySelectorAll(".c-catalog__plain-list-subcategory .c-catalog__plain-list-item a.c-catalog__plain-list-link").ToList();



                var category = JsonConvert.DeserializeObject<dynamic>(response.Content);

                if (category.status.Value)
                    foreach (var i in category.data)
                    {
                        string address = null;
                        string address1 = null;
                        string address2 = null;
                        try
                        {
                            address = mainrows.Where(w => w.QuerySelectorAll("a .c-category-card__header div.c-category-card__title").First().InnerText == i.Value.Value).First().Children().First().Attributes["href"].Value;
                        }
                        catch { }
                        try
                        {
                            address1 = depthrows.Where(w => w.InnerText == i.Value.Value).First().Attributes["href"].Value;
                        }
                        catch { }
                        try
                        {
                            address2 = subcategory.Where(w => w.InnerText == i.Value.Value).First().Attributes["href"].Value;
                        }
                        catch { }
                        if (!categories.Any(a => a.id == int.Parse(i.Name) && a.parent == categories[index].id))
                            categories.Add(new categoryModel() { id = int.Parse(i.Name), parent = categories[index].id, value = i.Value.Value, address = "https://www.digikala.com" + (address ?? address1 ?? address2) });
                        else if (categories.Any(a => a.id == int.Parse(i.Name) && a.parent == categories[index].id && a.address == "https://www.digikala.com"))
                        {
                            categories.RemoveAll(a => a.id == int.Parse(i.Name) && a.parent == categories[index].id && a.address == "https://www.digikala.com");
                            categories.Add(new categoryModel() { id = int.Parse(i.Name), parent = categories[index].id, value = i.Value.Value, address = "https://www.digikala.com" + (address ?? address1 ?? address2) });
                        }
                    }

                index++;
            }
            return categories;
        }

        public static List<brandModel> CrolBrand(List<CookieModel> Cookies)
        {
            List<brandModel> brands = new List<brandModel>();

            return brands;
        }

        public static IEnumerable<productModel> CrolProduct(List<CookieModel> Cookies)
        {
            int countPage = 1;
            int index = 1;

            while (countPage >= index)
            {
                var client = new RestClient("https://seller.digikala.com/ajax/product/search/");
                var request = new RestRequest(Method.POST);

                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                request.AddHeader("Accept-Encoding", "gzip, deflate, br");
                request.AddHeader("Accept-Language", "en-US,en;q=0.5");
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("Host", "seller.digikala.com");
                request.AddHeader("Upgrade-Insecure-Requests", "1");
                request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("undefined", "sortColumn=product_moderation_status_score&sortOrder=desc&items=100&page=" + index, ParameterType.RequestBody);

                foreach (var item in Cookies)
                {
                    request.AddCookie(item.Name, item.Value);
                }

                IRestResponse response = client.Execute(request);

                if (response.Content.IndexOf("<h1 class=\"c-new-login__header\">ورود به مرکز فروشندگان</h1>") >= 0)
                    throw new ExceptionAuth();

                if (response.IsSuccessful)
                {
                    var html = new HtmlAgilityPack.HtmlDocument();
                    html.LoadHtml(response.Content);
                    var document = html.DocumentNode;

                    var rows = document.QuerySelectorAll(".c-table-container .table-responsive>table>tbody>tr");//  
                    try
                    {
                        countPage = int.Parse(document.QuerySelectorAll(".pagination-button.pagination-button--end").First().Attributes["data-page"].Value.TrimName().TrimForNumber().NumberToEnglish());
                    }
                    catch { }

                    var p = new productModel();
                    foreach (HtmlAgilityPack.HtmlNode node in rows)
                    {
                        try
                        {
                            var columns = node.QuerySelectorAll("td.table-word-wrap").ToList();

                            p = new productModel()
                            {
                                image = columns[1].QuerySelectorAll("img").First().Attributes["src"].Value,
                                dkpCode = columns[2].QuerySelectorAll("a").First().InnerText.TrimName(),
                                category = columns[3].InnerText.TrimName(),
                                name = columns[4].QuerySelectorAll(".product-table-title tr .fa-title").First().InnerText.TrimName().Replace("عنوان : ", ""),
                                brand = columns[5].InnerText.TrimName()
                            };
                        }
                        catch 
                        {
                            p = null;
                        }

                        yield return p;
                    }
                    index++;
                }
                else
                {
                    Thread.Sleep(10000);
                }
            }
            yield break;
        }

        public static List<productPriceModel> CrolProductPrice(List<CookieModel> Cookies, string dkpCode)
        {
            var client = new RestClient("https://seller.digikala.com/product/edit/" + dkpCode + "/?ref=conf");
            var request = new RestRequest(Method.GET);

            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Accept-Language", "en-US,en;q=0.5");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Host", "seller.digikala.com");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");

            foreach (var item in Cookies)
            {
                request.AddCookie(item.Name, item.Value);
            }

            IRestResponse response = client.Execute(request);

            if (response.Content.IndexOf("<h1 class=\"c-new-login__header\">ورود به مرکز فروشندگان</h1>") >= 0)
                throw new ExceptionAuth();

            if (response.IsSuccessful)
            {
                var html = new HtmlAgilityPack.HtmlDocument();
                html.LoadHtml(response.Content);
                var document = html.DocumentNode;

                var rows = document.QuerySelectorAll("div.product-variant div.product-variant-table div.table-responsive table tbody tr");

                List<productPriceModel> list = new List<productPriceModel>();

                foreach (HtmlAgilityPack.HtmlNode node in rows)
                {
                    var columns = node.QuerySelectorAll("td.table-word-wrap").ToList();

                    list.Add(new productPriceModel
                    {
                        name = columns[1].InnerText.TrimName(),
                        digiCode = columns[2].InnerText.TrimName().getConvertor().ToInt,
                        price = decimal.Parse(columns[7].InnerText.TrimName()),
                        inventory = int.Parse(columns[8].InnerText.TrimName()),
                        countBasket = int.Parse(columns[9].InnerText.TrimName()),
                        sendRange = int.Parse(columns[10].InnerText.TrimName()),
                    });
                }

                return list;
            }
            else
            {
                Thread.Sleep(10000);
                return null;
            }
        }

        public static IEnumerable<productResultModel> CrolProductResult(string dkpCode)
        {
            var client = new RestClient("https://www.digikala.com/product/dkp-" + dkpCode);
            var request = new RestRequest(Method.GET);

            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Accept-Language", "en-US,en;q=0.5");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Host", "www.digikala.com");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");

            //foreach (var item in Cookies)
            //{
            //    request.AddCookie(item.Name, item.Value);
            //}

            IRestResponse response = client.Execute(request);

            //if (response.Content.IndexOf("<h1 class=\"c-new-login__header\">ورود به مرکز فروشندگان</h1>") >= 0)
            //    throw new ExceptionAuth();

            if (response.IsSuccessful)
            {
                var html = new HtmlAgilityPack.HtmlDocument();
                html.LoadHtml(response.Content);
                var document = html.DocumentNode;

                var sellers = document.QuerySelectorAll(CssSellector.CrolProductResult_productResult_sellers).ToList();
                var prductname = document.QuerySelectorAll(CssSellector.CrolProductResult_productResult_prductname).First().InnerText.TrimName();

                if (sellers.Count() > 0)
                    foreach (HtmlAgilityPack.HtmlNode node in sellers)
                    {
                        var columns = node.QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns).ToList();

                        var productname = prductname;
                        var sellername = columns[0].QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_sellername).First().InnerText.TrimName();
                        var sellerlink = columns[0].QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_sellerlink).First().Attributes["href"].Value;
                        var rate = columns[0].QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_rate).First().InnerText.getConvertor().ToDecimal;
                        var timelySupply = columns[0].QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_timelySupply).First().InnerText.TrimForNumber().NumberToEnglish().getConvertor().ToDecimal;
                        var postingCommitment = columns[0].QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_postingCommitment).First().InnerText.TrimForNumber().NumberToEnglish().getConvertor().ToDecimal;
                        var noReference = columns[0].QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_noReference).First().InnerText.TrimForNumber().NumberToEnglish().getConvertor().ToDecimal;
                        var digiDepo = node.QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_digiDepo).First().InnerText.TrimName().Contains("آماده ارسال");
                        var price = node.QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_price).First().Children().First().InnerText.TrimForNumber().getConvertor().ToDecimal;
                        var digiCode = node.QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_digiCode).First().Attributes["href"].Value.Replace("/cart/add/", "").Replace("/1/", "").TrimForNumber().getConvertor().ToInt;
                        var digiCodeBuyBox = node.QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_digiCodeBuyBox).First().Attributes["data-variant"].Value.TrimForNumber().getConvertor().ToInt;

                        var productResultModel = new productResultModel
                        {
                            productname = prductname,
                            sellername = columns[0].QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_sellername).First().InnerText,
                            sellerlink = columns[0].QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_sellerlink).First().Attributes["href"].Value,
                            rate = columns[0].QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_rate).First().InnerText.getConvertor().ToDecimal,
                            timelySupply = columns[0].QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_timelySupply).First().InnerText.TrimForNumber().NumberToEnglish().getConvertor().ToDecimal,
                            postingCommitment = columns[0].QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_postingCommitment).First().InnerText.TrimForNumber().NumberToEnglish().getConvertor().ToDecimal,
                            noReference = columns[0].QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_noReference).First().InnerText.TrimForNumber().NumberToEnglish().getConvertor().ToDecimal,
                            digiDepo = node.QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_digiDepo).First().InnerText.TrimName().Contains("آماده ارسال"),
                            price = node.QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_price).First().Children().First().InnerText.TrimForNumber().getConvertor().ToDecimal,
                            digiCode = node.QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_digiCode).First().Attributes["href"].Value.Replace("/cart/add/", "").Replace("/1/", "").TrimForNumber().getConvertor().ToInt,
                            digiCodeBuyBox = node.QuerySelectorAll(CssSellector.CrolProductResult_productResult_columns_digiCodeBuyBox).First().Attributes["data-variant"].Value.TrimForNumber().getConvertor().ToInt,
                        };

                        var clientl = new RestClient(productResultModel.sellerlink);
                        var requestl = new RestRequest(Method.GET);

                        yield return productResultModel;
                    }
                else
                {
                    productResultModel productResult = new productResultModel();

                    productResult.sellerlink = document.QuerySelectorAll(CssSellector.CrolProductResult_productResult_sellerlink).First().Attributes["href"].Value;

                    var clientl = new RestClient(productResult.sellerlink);
                    var requestl = new RestRequest(Method.GET);

                    productResult.productname = prductname;

                    productResult.sellername = document.QuerySelectorAll(CssSellector.CrolProductResult_productResult_sellername).First().InnerText;
                    productResult.rate = document.QuerySelectorAll(CssSellector.CrolProductResult_productResult_rate).First().InnerText.getConvertor().ToDecimal;

                    productResult.timelySupply = document.QuerySelectorAll(CssSellector.CrolProductResult_productResult_timelySupply).First().InnerText.getConvertor().ToDecimal;
                    productResult.postingCommitment = document.QuerySelectorAll(CssSellector.CrolProductResult_productResult_postingCommitment).First().InnerText.getConvertor().ToDecimal;
                    productResult.noReference = document.QuerySelectorAll(CssSellector.CrolProductResult_productResult_noReference).First().InnerText.getConvertor().ToDecimal;

                    productResult.digiDepo = document.QuerySelectorAll(CssSellector.CrolProductResult_productResult_digiDepo).First().InnerText.Contains("آماده ارسال");
                    decimal.TryParse(document.QuerySelectorAll(CssSellector.CrolProductResult_productResult_Price).First().InnerText, out decimal price);
                    productResult.price = price;
                    productResult.digiCode = document.QuerySelectorAll(CssSellector.CrolProductResult_productResult_digiCode).First().Attributes["data-variant"].Value.getConvertor().ToInt;
                    productResult.digiCodeBuyBox = document.QuerySelectorAll(CssSellector.CrolProductResult_productResult_digiCodeBuyBox).First().Attributes["data-variant"].Value.getConvertor().ToInt;

                    yield return productResult;
                }
            }
            else
            {
                Thread.Sleep(10000);
                yield break;
            }
        }

        public static int ChangeProductPrice(List<CookieModel> Cookies, string product_id, string product_variant_id, decimal newPrice)
        {
            //https://seller.digikala.com/ajax/product/editproductvariant/
            var client = new RestClient("https://seller.digikala.com/ajax/product/editproductvariant/");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
            // request.AddHeader("Referer", "https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
            request.AddHeader("Host", "seller.digikala.com");

            foreach (var item in Cookies)
            {
                request.AddCookie(item.Name, item.Value);
            }
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "product_variant_id=" + product_variant_id, ParameterType.RequestBody);

            var response = client.Execute(request);

            var html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(response.Content);
            var document = html.DocumentNode;

            var sellerPhysicalStock = document.QuerySelectorAll("input[name=\"variant[seller_physical_stock]\"]").First().Attributes["value"].Value;//variant[seller_physical_stock]
            var maximumPerOrder = document.QuerySelectorAll("input[name=\"variant[maximum_per_order]\"]").First().Attributes["value"].Value;
            var leadTime = document.QuerySelectorAll("input[name=\"variant[lead_time]\"]").First().Attributes["value"].Value;

            //قیمت مرجع
            var priceList = document.QuerySelectorAll("input[name=\"variant[price_list]\"]").First().Attributes["value"].Value;



            //https://seller.digikala.com/ajax/product/savevariant/
            client = new RestClient("https://seller.digikala.com/ajax/product/savevariant/");
            request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
            // request.AddHeader("Referer", "https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
            request.AddHeader("Host", "seller.digikala.com");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            foreach (var item in Cookies)
            {
                request.AddCookie(item.Name, item.Value);
            }

            request.AddParameter("undefined", "data=variant%255Bis_active%255D%3Don%26variant%255Bprice_sale%255D%3D" + newPrice + "%26variant%255Bseller_physical_stock%255D%3D" + sellerPhysicalStock + "%26variant%255Bmaximum_per_order%255D%3D" + maximumPerOrder + "%26variant%255Blead_time%255D%3D" + leadTime + "%26variant%255Bsupplier_code%255D%3D%26variant%255Bproduct-variant-id%255D%3D" + product_variant_id + "&product_id=" + product_id, ParameterType.RequestBody);

            response = client.Execute(request);

            return 1;
        }

        public static List<CookieModel> LoginDkms(string DKMSUsername, string DKMSPassword)
        {
            //var digiUsername = "shadlyco@gmail.com";
            //var digiPassword = "Sh@13921219";

            var client = new RestClient("https://dkms.digikala.com/users/login/");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
            // request.AddHeader("Referer", "https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
            request.AddHeader("Host", "dkms.digikala.com");

            IRestResponse response = client.Execute(request);
            var Cookies = response.Cookies.ToList();

            client = new RestClient("https://dkms.digikala.com/users/login/");
            request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
            // request.AddHeader("Referer", "https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
            request.AddHeader("Host", "dkms.digikala.com");

            foreach (var item in Cookies)
            {
                request.AddCookie(item.Name, item.Value);
            }

            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"login[email]\"\r\n\r\n" + DKMSUsername + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"login[password]\"\r\n\r\n" + DKMSPassword + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
            response = client.Execute(request);

            //https://seller.digikala.com/profile/display/

            foreach (var i in response.Cookies)
            {
                Cookies.RemoveAll(a => a.Name == i.Name);
                Cookies.Add(i);
            }

            return Cookies.Select(s => new CookieModel()
            {
                Name = s.Name,
                Value = s.Value
            }).ToList();
        }

        public static IEnumerable<promotionModel> CrolPromotionList(List<CookieModel> Cookies)
        {
            int countPage = 1;
            int index = 1;

            while (countPage >= index)
            {
                var client = new RestClient("https://dkms.digikala.com/promotions/public/?page=" + index);
                var request = new RestRequest(Method.GET);

                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                request.AddHeader("Accept-Encoding", "gzip, deflate, br");
                request.AddHeader("Accept-Language", "en-US,en;q=0.5");
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("Host", "dkms.digikala.com");
                request.AddHeader("Upgrade-Insecure-Requests", "1");
                request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");

                foreach (var item in Cookies)
                {
                    request.AddCookie(item.Name, item.Value);
                }

                IRestResponse response = client.Execute(request);

                if (response.ResponseUri.AbsoluteUri.Contains("/users/login/"))
                    throw new ExceptionAuth();

                if (response.IsSuccessful)
                {
                    var html = new HtmlAgilityPack.HtmlDocument();
                    html.LoadHtml(response.Content);
                    var document = html.DocumentNode;

                    var rows = document.QuerySelectorAll("table.c-table__table .c-table__table-body tr.c-table__table-row");

                    try
                    {
                        countPage = int.Parse(document.QuerySelectorAll("ul.c-table__pager li.c-table__page-container .c-table__page--last").First().Attributes["data-page"].Value.TrimName().TrimForNumber().NumberToEnglish());
                    }
                    catch { }

                    List<promotionModel> list = new List<promotionModel>();
                    foreach (HtmlAgilityPack.HtmlNode node in rows)
                    {
                        var columns = node.QuerySelectorAll("td.c-table__table-cell").ToList();

                        var code = int.Parse(columns[0].InnerText.TrimName().NumberToEnglish());
                        var name = columns[1].InnerText.TrimName();
                        var type = columns[2].InnerText.TrimName();
                        var category = columns[3].InnerText.TrimName();
                        var campaign = columns[4].InnerText.TrimName();

                        var start = columns[6].InnerText.TrimName().NumberToEnglish();
                        var end = columns[7].InnerText.TrimName().NumberToEnglish();
                        var startDateTime = PersianDateTime.Parse(start.Split()[0], start.Split()[1]).ToDateTime();
                        var endDateTime = PersianDateTime.Parse(end.Split()[0], end.Split()[1]).ToDateTime();

                        var deadline = columns[8].InnerText.TrimName()
                            .Replace(" ", "")
                            .Replace("ساعتو", "h")
                            .Replace("روزو", "d")
                            .Replace("دقیقه", "m")
                            .Replace("ساعت", "h")
                            .Replace("روز", "d")
                            .NumberToEnglish();

                        int.TryParse(Regex.Match(deadline, "\\d+m").Value.Replace("m", ""), out int m);
                        int.TryParse(Regex.Match(deadline, "\\d+h").Value.Replace("h", ""), out int h);
                        int.TryParse(Regex.Match(deadline, "\\d+d").Value.Replace("d", ""), out int d);

                        var deadlineDateTime = DateTime.Now.AddDays(d).AddHours(h).AddMinutes(m);

                        var CountVariant = int.Parse(columns[9].InnerText.TrimName().NumberToEnglish());


                        var details = CrolPromotionDetails(Cookies, code);

                        yield return new promotionModel()
                        {
                            campaign = campaign,
                            category = category,
                            CountVariant = CountVariant,
                            deadLine = deadlineDateTime,
                            start = startDateTime,
                            end = endDateTime,
                            name = name,
                            promotionCode = code,
                            type = type,
                            categories = details.categories,
                            minPercentDiscount = details.minPercentDiscount,
                            minDiscount = details.minDiscount
                        };
                    }
                    index++;
                }
                else
                {
                    Thread.Sleep(10000);
                }
            }
            yield break;
        }

        public static IEnumerable<promotionModel> CrolPrivatePromotionList(List<CookieModel> Cookies)
        {
            int countPage = 1;
            int index = 1;

            while (countPage >= index)
            {
                var client = new RestClient("https://dkms.digikala.com/promotions/?page=" + index);
                var request = new RestRequest(Method.GET);

                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                request.AddHeader("Accept-Encoding", "gzip, deflate, br");
                request.AddHeader("Accept-Language", "en-US,en;q=0.5");
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("Host", "dkms.digikala.com");
                request.AddHeader("Upgrade-Insecure-Requests", "1");
                request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");

                foreach (var item in Cookies)
                {
                    request.AddCookie(item.Name, item.Value);
                }

                IRestResponse response = client.Execute(request);

                if (response.ResponseUri.AbsoluteUri.Contains("/users/login/"))
                    throw new ExceptionAuth();

                if (response.IsSuccessful)
                {
                    var html = new HtmlAgilityPack.HtmlDocument();
                    html.LoadHtml(response.Content);
                    var document = html.DocumentNode;

                    var rows = document.QuerySelectorAll("table.c-table__table .c-table__table-body tr.c-table__table-row");

                    try
                    {
                        countPage = int.Parse(document.QuerySelectorAll("ul.c-table__pager li.c-table__page-container .c-table__page--last").First().Attributes["data-page"].Value.TrimName().TrimForNumber().NumberToEnglish());
                    }
                    catch { }

                    List<promotionModel> list = new List<promotionModel>();
                    foreach (HtmlAgilityPack.HtmlNode node in rows)
                    {
                        var columns = node.QuerySelectorAll("td.c-table__table-cell").ToList();

                        var code = node.QuerySelectorAll("td a.c-btn").Where(w => w.InnerText.Contains("جزییات")).First().Attributes["href"].Value.Replace("/promotions/details/", "").NumberToEnglish().TrimForNumber().getConvertor().ToInt;

                        var name = columns[1].InnerText.TrimName();
                        var type = columns[2].InnerText.TrimName();
                        var category = columns[4].QuerySelectorAll("div.c-ui-text").First().InnerText.TrimName();
                        var campaign = columns[5].InnerText.TrimName();

                        var start = columns[7].Children().ToList()[0].InnerText.Replace("شروع", "").TrimName().NumberToEnglish();
                        var end = columns[7].Children().ToList()[2].InnerText.Replace("پایان", "").TrimName().NumberToEnglish();
                        var startDateTime = PersianDateTime.Parse(start.Split()[0], start.Split()[1]).ToDateTime();
                        var endDateTime = PersianDateTime.Parse(end.Split()[0], end.Split()[1]).ToDateTime();

                        var address = columns[8].QuerySelectorAll("a.c-ui-link").First().Attributes["href"].Value.TrimName();

                        var IsPrivate = node.QuerySelectorAll("td a.c-btn").Where(w => w.InnerText.Contains("ویرایش")).Count() > 0;


                        //var deadline = columns[8].InnerText.TrimName()
                        //    .Replace(" ", "")
                        //    .Replace("ساعتو", "h")
                        //    .Replace("روزو", "d")
                        //    .Replace("دقیقه", "m")
                        //    .Replace("ساعت", "h")
                        //    .Replace("روز", "d")
                        //    .NumberToEnglish();

                        //int.TryParse(Regex.Match(deadline, "\\d+m").Value.Replace("m", ""), out int m);
                        //int.TryParse(Regex.Match(deadline, "\\d+h").Value.Replace("h", ""), out int h);
                        //int.TryParse(Regex.Match(deadline, "\\d+d").Value.Replace("d", ""), out int d);

                        //var deadlineDateTime = DateTime.Now.AddDays(d).AddHours(h).AddMinutes(m);

                        //  var CountVariant = int.Parse(columns[9].InnerText.TrimName().NumberToEnglish());

                        var details = CrolPromotionDetails(Cookies, code);

                        yield return new promotionModel()
                        {
                            campaign = campaign,
                            category = category,
                            CountVariant = null,
                            deadLine = null,
                            start = startDateTime,
                            end = endDateTime,
                            name = name,
                            promotionCode = code,
                            type = type,
                            categories = details.categories,
                            minPercentDiscount = details.minPercentDiscount,
                            minDiscount = details.minDiscount,
                            isPrivate = IsPrivate,
                            address = address
                        };
                    }
                    index++;
                }
                else
                {
                    Thread.Sleep(10000);
                }
            }
            yield break;
        }


        public static promotionDetailsModel CrolPromotionDetails(List<CookieModel> Cookies, int promotionCode)
        {
            //https://dkms.digikala.com/promotions/details/90835/

            var client = new RestClient("https://dkms.digikala.com/promotions/details/" + promotionCode + "/");
            var request = new RestRequest(Method.GET);

            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Accept-Language", "en-US,en;q=0.5");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Host", "dkms.digikala.com");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");

            foreach (var item in Cookies)
            {
                request.AddCookie(item.Name, item.Value);
            }

            IRestResponse response = client.Execute(request);

            var html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(response.Content);
            var document = html.DocumentNode;

            var categories = document.QuerySelectorAll(".c-box-main div.container div.row div.c-box__section .c-btn__container .c-btn.c-btn--o-blue-light").ToList().Select(s => s.InnerText).ToList();
            var conditions = document.QuerySelectorAll(".c-box-main div.container div.c-details-box .c-details-box__row .c-details-box__col .c-ui-text--bold").ToList().Select(s => s.InnerText).ToList();

            decimal.TryParse(conditions.Where(w => w.Contains("حداقل درصد تخفیف")).FirstOrDefault() ?? ""
                .Replace("حداقل درصد تخفیف", "").Replace("درصد", "").Replace(":", "").NumberToEnglish(), out decimal minPercentDiscount);

            //به اشتباه درصد نوشتن حذف نشه : حداقل مقدار تخفیف: ۱۰۰۰ درصد 
            decimal.TryParse(conditions.Where(w => w.Contains("حداقل مقدار تخفیف")).FirstOrDefault() ?? ""
                .Replace("حداقل مقدار تخفیف", "").Replace("درصد", "").Replace(":", "").NumberToEnglish(), out decimal minDiscount);


            return new promotionDetailsModel()
            {
                categories = categories,
                minDiscount = minDiscount,
                minPercentDiscount = minPercentDiscount
            };

        }


        //public static LoginCrolModel CrolLogin(LoginCrolModel model)
        //{
        //    var digiUsername = "shadlyco@gmail.com";
        //    var digiPassword = "Sh@13921219";

        //    var client = new RestClient("https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
        //    var request = new RestRequest(Method.GET);
        //    request.AddHeader("cache-control", "no-cache");
        //    request.AddHeader("Connection", "keep-alive");
        //    request.AddHeader("Accept-Encoding", "gzip, deflate");
        //    request.AddHeader("Cache-Control", "no-cache");
        //    request.AddHeader("Accept", "*/*");
        //    request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
        //    request.AddHeader("Referer", "https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
        //    request.AddHeader("Host", "seller.digikala.com");

        //    IRestResponse response = client.Execute(request);
        //    var Cookies = response.Cookies.ToList();

        //    client = new RestClient("https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
        //    request = new RestRequest(Method.POST);
        //    request.AddHeader("cache-control", "no-cache");
        //    request.AddHeader("Connection", "keep-alive");
        //    request.AddHeader("Accept-Encoding", "gzip, deflate");
        //    request.AddHeader("Cache-Control", "no-cache");
        //    request.AddHeader("Accept", "*/*");
        //    request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
        //    request.AddHeader("Referer", "https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
        //    request.AddHeader("Host", "seller.digikala.com");

        //    foreach (var item in Cookies)
        //    {
        //        request.AddCookie(item.Name, item.Value);
        //    }

        //    request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"login[email]\"\r\n\r\n" + digiUsername + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"login[password]\"\r\n\r\n" + digiPassword + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
        //    response = client.Execute(request);

        //    //https://seller.digikala.com/profile/display/

        //    foreach (var i in response.Cookies)
        //    {
        //        Cookies.RemoveAll(a => a.Name == i.Name);
        //        Cookies.Add(i);
        //    }









        //    client = new RestClient("https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
        //    request = new RestRequest(Method.POST);
        //    request.AddHeader("cache-control", "no-cache");
        //    request.AddHeader("Connection", "keep-alive");
        //    request.AddHeader("Content-Type", "multipart/form-data; boundary=--------------------------174033616011756429972833");
        //    request.AddHeader("Accept-Encoding", "gzip, deflate");
        //    request.AddHeader("Cookie", "PHPSESSID=dctjses43a60ongcesbhch3rqo; tracker_global=4rsjcLD4; tracker_session=4rHirVxx");
        //    request.AddHeader("Cache-Control", "no-cache");
        //    request.AddHeader("Accept", "*/*");
        //    request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
        //    request.AddHeader("Referer", "https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
        //    request.AddHeader("Host", "seller.digikala.com");
        //    request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");

        //    request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"login[email]\"\r\n\r\n" + model.digiUsername + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"login[password]\"\r\n\r\n" + model.digiPassword + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
        //    response = client.Execute(request);

        //    //https://seller.digikala.com/profile/display/
        //    model.Cookies = response.Cookies.Select(s => new KeyValuePair<string, string>(s.Name, s.Value)).ToList();

        //    //client = new RestClient("https://seller.digikala.com/profile/display/");
        //    //request = new RestRequest(Method.GET);
        //    //request.AddHeader("cache-control", "no-cache");
        //    //request.AddHeader("Connection", "keep-alive");
        //    //request.AddHeader("Content-Type", "multipart/form-data; boundary=--------------------------174033616011756429972833");
        //    //request.AddHeader("Accept-Encoding", "gzip, deflate");
        //    //request.AddHeader("Cookie", "PHPSESSID=dctjses43a60ongcesbhch3rqo; tracker_global=4rsjcLD4; tracker_session=4rHirVxx");
        //    //request.AddHeader("Cache-Control", "no-cache");
        //    //request.AddHeader("Accept", "*/*");
        //    //request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
        //    //request.AddHeader("Referer", "https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
        //    //request.AddHeader("Host", "seller.digikala.com");

        //    //foreach(var item in model.Cookies)
        //    //{
        //    //    request.AddCookie(item.Key,item.Value);
        //    //}
        //    //response = client.Execute(request);

        //    ////https://seller.digikala.com/profile/rating/
        //    //model.Cookies = response.Cookies.Select(s => new KeyValuePair<string, string>(s.Name, s.Value)).ToList();

        //    //client = new RestClient("https://seller.digikala.com/profile/rating/");
        //    //request = new RestRequest(Method.GET);
        //    //request.AddHeader("cache-control", "no-cache");
        //    //request.AddHeader("Connection", "keep-alive");
        //    //request.AddHeader("Content-Type", "multipart/form-data; boundary=--------------------------174033616011756429972833");
        //    //request.AddHeader("Accept-Encoding", "gzip, deflate");
        //    //request.AddHeader("Cookie", "PHPSESSID=dctjses43a60ongcesbhch3rqo; tracker_global=4rsjcLD4; tracker_session=4rHirVxx");
        //    //request.AddHeader("Cache-Control", "no-cache");
        //    //request.AddHeader("Accept", "*/*");
        //    //request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
        //    //request.AddHeader("Referer", "https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
        //    //request.AddHeader("Host", "seller.digikala.com");

        //    //foreach (var item in model.Cookies)
        //    //{
        //    //    request.AddCookie(item.Key, item.Value);
        //    //}
        //    //response = client.Execute(request);


        //    return model;
        //}

        //public static ProductCrolModel CrolProduct(int dkpCode)
        //{
        //    var client = new RestClient("https://www.digikala.com/product/dkp-" + dkpCode);
        //    client.Proxy = new WebProxy("192.168.1.1", 10);
        //    var request = new RestRequest(Method.GET);
        //    request.AddHeader("cache-control", "no-cache");
        //    request.AddHeader("Connection", "keep-alive");
        //    request.AddHeader("Content-Type", "multipart/form-data; boundary=--------------------------174033616011756429972833");
        //    request.AddHeader("Accept-Encoding", "gzip, deflate");
        //    request.AddHeader("Cookie", "PHPSESSID=dctjses43a60ongcesbhch3rqo; tracker_global=4rsjcLD4; tracker_session=4rHirVxx");
        //    request.AddHeader("Cache-Control", "no-cache");
        //    request.AddHeader("Accept", "*/*");
        //    request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
        //    request.AddHeader("Referer", "https://seller.digikala.com");
        //    request.AddHeader("Host", "seller.digikala.com");
        //    request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");

        //    IRestResponse response = client.Execute(request);
        //    var html = new HtmlDocument();
        //    html.LoadHtml(response.Content);
        //    var document = html.DocumentNode;

        //    ProductCrolModel model = new ProductCrolModel();
        //    model.productPrices = new List<ProductPrice>();

        //    model.name = document.QuerySelectorAll("h1.c-product__title").First().InnerText;

        //    var sellers = document.QuerySelectorAll("div.c-table-suppliers__body div.c-table-suppliers__row");
        //    foreach (HtmlNode node in sellers)
        //    {
        //        ProductPrice productPrice = new ProductPrice();
        //        productPrice.seller = new Seller();
        //        productPrice.seller.sellerEncryptCode = document.QuerySelectorAll(".c-table-suppliers__cell--title div.c-table-suppliers__seller-wrapper .c-table-suppliers__seller-name a").First().Attributes["href"].Value.Replace("/seller/", "").Replace("/", "");

        //        productPrice.price = decimal.Parse(document.QuerySelectorAll(".c-table-suppliers__cell--price .c-price__value").First().Children().First().OuterHtml);

        //    }



        //    return model;
        //}

        //public static List<ProductPriceCrolModel> CrolProductPrice(List<KeyValuePair<string, string>> Cookies, string dkpCode)
        //{
        //    var client = new RestClient("https://seller.digikala.com/product/edit/" + dkpCode + "/?ref=conf");
        //    var request = new RestRequest(Method.GET);
        //    request.AddHeader("cache-control", "no-cache");
        //    request.AddHeader("Connection", "keep-alive");
        //    request.AddHeader("Content-Type", "multipart/form-data; boundary=--------------------------174033616011756429972833");
        //    request.AddHeader("Accept-Encoding", "gzip, deflate");
        //    request.AddHeader("Cookie", "PHPSESSID=dctjses43a60ongcesbhch3rqo; tracker_global=4rsjcLD4; tracker_session=4rHirVxx");
        //    request.AddHeader("Cache-Control", "no-cache");
        //    request.AddHeader("Accept", "*/*");
        //    request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
        //    request.AddHeader("Referer", "https://seller.digikala.com");
        //    request.AddHeader("Host", "seller.digikala.com");
        //    request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");

        //    foreach (var item in Cookies)
        //    {
        //        request.AddCookie(item.Key, item.Value);
        //    }

        //    IRestResponse response = client.Execute(request);

        //    var html = new HtmlDocument();
        //    html.LoadHtml(response.Content);
        //    var document = html.DocumentNode;

        //    var rows = document.QuerySelectorAll("div.product-variant product-variant-table div.table-responsive table tbody tr");
        //    List<ProductPriceCrolModel> ProductPrices = new List<ProductPriceCrolModel>();
        //    foreach (HtmlNode node in rows)
        //    {
        //        var columns = node.QuerySelectorAll("td.table-word-wrap").ToList();

        //        ProductPrices.Add(new ProductPriceCrolModel()
        //        {
        //            name = columns[1].InnerText,
        //            digiCode = columns[2].InnerText,
        //            price = decimal.Parse(columns[7].InnerText),
        //            inventory = int.Parse(columns[8].InnerText),
        //            countBasket = int.Parse(columns[9].InnerText),
        //            sendRange = int.Parse(columns[10].InnerText),
        //        });
        //    }


        //    return ProductPrices;
        //}

    }
}
