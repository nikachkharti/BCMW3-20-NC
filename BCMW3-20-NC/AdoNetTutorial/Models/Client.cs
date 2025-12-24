namespace AdoNetTutorial.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string ClientCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
