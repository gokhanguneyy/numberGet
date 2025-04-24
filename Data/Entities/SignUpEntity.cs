using System;

namespace numberGet.Data.Entities
{
    public class SignUpEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword  { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
