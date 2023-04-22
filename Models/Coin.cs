using System.Text;

namespace GrpcBillingService.Models
{
    public class Coin
    {
        public Coin(int id, User firstUser)
        {
            CurrentUser = firstUser;
            Id = id;
        }

        public int Id { get; set; }

        public List<User> HistoryOfUsers { get; } = new List<User>();

        public User CurrentUser
        {
            get => HistoryOfUsers.Last();
            set => HistoryOfUsers.Add(value);
        }

        public string GetStringHistory()
        {
            int countOfUsers = HistoryOfUsers.Count;
            StringBuilder stringBuilder = new(countOfUsers);
            stringBuilder.Append($"{countOfUsers} users: ");

            for (int i = 0; i < HistoryOfUsers.Count; i++)
            {
                User user = HistoryOfUsers[i];
                stringBuilder.Append($"#{i + 1} {user.ToString()}; ");
            }
            return stringBuilder.ToString();
        }
    }
}
