
namespace Grocery.Core.Models
{
    public partial class Client : Model
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Role EnumRole { get; set; }
        public Client(int id, string name, string emailAddress, string password, Role userRole = Role.None) : base(id, name)
        {
            EmailAddress=emailAddress;
            Password=password;
            EnumRole=userRole;
        }
    }
}
