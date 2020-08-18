using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.Scheduler.Models
{
    public class categoryModel
    {
        public int id { set; get; }
        public int? parent { set; get; }
        public string value { set; get; }
        public string address { set; get; }
    }

    public class brandModel
    {
        public int id { set; get; }
        public string value { set; get; }
        public string address { set; get; }
    }

    public class productModel
    {
        public string dkpCode { set; get; }
        public string category { set; get; }
        public string name { set; get; }
        public string brand { set; get; }
        public string image { set; get; }
    }

    public class promotionModel
    {
        public int promotionCode { set; get; }
        public string category { set; get; }
        public string name { set; get; }
        public string type { set; get; }
        public string campaign { set; get; }

        public DateTime start { set; get; }
        public DateTime end { set; get; }

        public bool isPrivate { set; get; }
        public string address { set; get; }

        public DateTime? deadLine { set; get; }
        public int? CountVariant { set; get; }

        public List<string> categories { set; get; }
        public decimal minPercentDiscount { set; get; }
        public decimal minDiscount { set; get; }
    }

    public class promotionDetailsModel
    {
        public List<string> categories { set; get; }
        public decimal minPercentDiscount { set; get; }
        public decimal minDiscount { set; get; }
    }

    public class productPriceModel
    {
        public int digiCode { set; get; }
        public decimal price { set; get; }
        public string name { set; get; }
        public int inventory { set; get; }
        public int countBasket { set; get; }
        public int sendRange { set; get; }
        public string digiColor { set; get; }
    }

    public class SellerModel
    {
        public string digiCode { set; get; }
        public decimal price { set; get; }
        public string name { set; get; }
        public int inventory { set; get; }
        public int countBasket { set; get; }
        public int sendRange { set; get; }
        public string digiColor { set; get; }
    }

    public class productResultModel
    {
        public string productname { set; get; }
        public int digiCode { set; get; }
        public int digiCodeBuyBox { set; get; }
        public string sellername { set; get; }
        public string sellerlink { set; get; }
        public int sellercode { set; get; }
        public decimal rate { set; get; }
        public decimal timelySupply { set; get; }
        public decimal postingCommitment { set; get; }
        public decimal noReference { set; get; }
        public bool digiDepo { set; get; }
        public decimal price { set; get; }
    }
    public class productResultSellerModel
    {
        public int digiCode { set; get; }
        public int digiCodeBuyBox { set; get; }
        public string name { set; get; }
        public string sellerlink { set; get; }
        public decimal rate { set; get; }
        public decimal timelySupply { set; get; }
        public decimal postingCommitment { set; get; }
        public decimal noReference { set; get; }
        public bool digiDepo { set; get; }
        public decimal price { set; get; }
    }

    public class CookieModel
    {
        public string Name { set; get; }
        public string Value { set; get; }
    }
}
