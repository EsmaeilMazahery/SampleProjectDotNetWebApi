
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using KavenegarCore.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ESkimo.ServiceLayer.Services
{
    public interface ISmsRepository : IGenericRepository<SmsLog>
    {
        SmsStatus Send(string Message, string receptor, int? memberId = null);

        SmsStatus Verify(string token, string receptor, SmsTemplate template, SmsVerifyLookupType? type = null, int? memberId = null);
    }

    public class SmsRepository : GenericRepository<SmsLog>, ISmsRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Setting>> _settings;
        Lazy<DbSet<SmsLog>> _smsLogs;
        private string apikey;
        private string sender;
        private bool IsSetup = false;

        public SmsRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _settings = new Lazy<DbSet<Setting>>(() => _uow.Set<Setting>());
            _smsLogs = new Lazy<DbSet<SmsLog>>(() => _uow.Set<SmsLog>());
        }

        private bool Setup()
        {
            apikey = _settings.Value.AsQueryable<Setting>().Where(w => w.SettingType == SettingType.SmsApiKey).FirstOrDefault().Value;
            sender = _settings.Value.AsQueryable<Setting>().Where(w => w.SettingType == SettingType.SmsSender).FirstOrDefault().Value;

            if (!string.IsNullOrEmpty(apikey))
            {
                IsSetup = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// ارسال پیام تکی
        /// </summary>
        /// <param name="Message">پیام</param>
        /// <param name="receptor">شماره تماس با صفر</param>
        /// <returns></returns>

        public SmsStatus Send(string message, string receptor,int? memberId=null)
        {
            try
            {
                if (!IsSetup)
                    Setup();

                if (!checkValidSend(receptor))
                    return SmsStatus.NotValid;

                KavenegarCore.KavenegarApi api = new KavenegarCore.KavenegarApi(apikey);
                var result = api.Send(sender, receptor, message);

                InsertLog(message, receptor, DateTime.Now, result.Status, memberId);

                _uow.Save();

                return SmsStatus.Successful;
            }
            catch (KavenegarCore.Exceptions.ApiException ex)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                // Console.Write("Message : " + ex.Message);
            }
            catch (KavenegarCore.Exceptions.HttpException ex)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                //  Console.Write("Message : " + ex.Message);
            }
            catch (Exception ex)
            {

            }

            return SmsStatus.Err;
        }

        public bool checkValidSend(string receptor)
        {
            return AsQueryable().Where(w => w.receptor == receptor && w.dateTime > DateTime.Now.AddDays(-1)).Count()>5?false :
                AsQueryable().Where(w => w.receptor == receptor && w.dateTime > DateTime.Now.AddMinutes(-10)).Count() > 3? false : true;
        }

        public SmsStatus Verify(string token, string receptor, SmsTemplate template, SmsVerifyLookupType? type = null, int? memberId = null)
        {
            try
            {
                if (!IsSetup)
                    Setup();

                if (!checkValidSend(receptor))
                    return SmsStatus.NotValid;

                KavenegarCore.KavenegarApi api = new KavenegarCore.KavenegarApi(apikey);
                var result = type.HasValue ? api.VerifyLookup(receptor, token, template.GetDescriptionOrNull(), (VerifyLookupType)(int)type.Value) : api.VerifyLookup(receptor, token, template.GetDescriptionOrNull());

                InsertLog(result.Message, receptor, DateTime.Now, result.Status, memberId);
                _uow.Save();
                // foreach (var r in result)
                //{
                //    Console.Write(result.Messageid.ToString());
                // }
                return SmsStatus.Successful;
            }
            catch (KavenegarCore.Exceptions.ApiException ex)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                // Console.Write("Message : " + ex.Message);
            }
            catch (KavenegarCore.Exceptions.HttpException ex)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                //  Console.Write("Message : " + ex.Message);
            }
            catch (Exception ex)
            {

            }

            return SmsStatus.Err;
        }

        public void InsertLog(string message, string receptor, DateTime dateTime, int status, int? memberId = null)
        {
            SmsLog smsLog = new SmsLog()
            {
                dateTime = dateTime,
                message = message,
                receptor = receptor,
                status = status,
                memberId= memberId
            };

            ChangeState(smsLog, EntityState.Added);
        }

        public void UpdateStatus(int smsLogId, int status)
        {
            Update(w => w.smsLogId == smsLogId, u => new SmsLog() { status = status });
        }
    }
}
