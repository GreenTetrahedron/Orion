namespace Orion.Models
{
    public class User
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public List<Guid>? DirectCommunicationIds { get; set; }
    }
}
