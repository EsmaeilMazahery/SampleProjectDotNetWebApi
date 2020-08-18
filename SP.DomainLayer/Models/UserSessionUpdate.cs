namespace SP.DomainLayer.Models
{
    public class UserSessionUpdate
    {
        public int userSessionUpdateId { get; set; }
        
        public int userId { get; set; }
        
        public virtual User user { get; set; }
    }
}
