using GrpcBillingService.Interfaces;
using GrpcBillingService.Models;

namespace GrpcBillingService
{
    public class DefaultData : IBillingDataService
    {
        private User[] users = new User[]
        {
            new User("boris", 5000),
            new User("maria", 1000),
            new User("oleg", 800)
        };

        private List<Coin> coins = new();

        public User[] GetUsers() => users;
        public User GetUser(string userName) => users.First(user => user.Name == userName);
        public int CountOfUsers() => users.Length;

        public Coin[] GetCoins() => coins.ToArray();
        public int CountOfCoins() => coins.Count;
        public void AddCoin(Coin newCoin) => coins.Add(newCoin);
        public Coin UpdateCoin(int coinId, User currentUser)
        {
            coins[coinId - 1].CurrentUser = currentUser;
            return coins[coinId - 1];
        }
    }
}
