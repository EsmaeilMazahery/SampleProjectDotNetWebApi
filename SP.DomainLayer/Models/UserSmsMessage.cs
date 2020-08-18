
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SP.Infrastructure.Enumerations;

namespace SP.DomainLayer.Models
{
    public class UserSmsMessage
    {
        public int userSmsMessageId { get; set; }


        [Display(Name = "تاریخ ثبت")]
        public DateTime registerDateTime { get; set; }


        [Display(Name = "متن")]
        [Required]
        [MaxLength(1000)]
        public string text { get; set; }


        [Display(Name = "نوع ارسال")]
        public UserSmsMessageSendType sendType { get; set; }


        [Display(Name = "کد پیگیری")]
        [MaxLength(100)]
        public string refId { get; set; }


        [Display(Name = "وضعیت ارسال")]
        public SmsSendStatus sendStatus { get; set; }


        [Display(Name = "شماره ارسال کننده")]
        [MaxLength(100)]
        public string senderNumber { get; set; }


        [Display(Name = "شماره دریافت کننده")]
        [MaxLength(10)]
        public string receiverNumber { get; set; }


        public byte tryCount { get; set; }


        public int adminId { get; set; }


        public virtual User admin { get; set; }
    }
}
