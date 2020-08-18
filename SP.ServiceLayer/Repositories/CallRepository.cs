
namespace SP.ServiceLayer.Services
{
    public interface ICallRepository
    {
        bool SendVerifyCode(string VerifyCode);
    }

    public class CallRepository : ICallRepository
    {
        public CallRepository()
        {

        }

        public bool SendVerifyCode(string VerifyCode)
        {
            return true;
        }
    }
}
