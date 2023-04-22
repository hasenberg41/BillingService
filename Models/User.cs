namespace GrpcBillingService.Models
{
    public class User
    {
        public User(string name, int rating)
        {
            Name = name;
            Rating = rating;
        }

        public string Name { get; set; }

        public int Rating { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Rating: {Rating}";
        }
    }
}
