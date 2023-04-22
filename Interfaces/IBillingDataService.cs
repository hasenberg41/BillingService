using GrpcBillingService.Models;

namespace GrpcBillingService.Interfaces
{
    public interface IBillingDataService
    {
        User[] GetUsers();
        User GetUser(string userName);
        int CountOfUsers();

        Coin[] GetCoins();
        int CountOfCoins();
        void AddCoin(Coin newCoin);
        Coin UpdateCoin(int coinId, User currentUser);
    }
}
