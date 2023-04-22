using Billing;
using Grpc.Core;
using GrpcBillingService.Interfaces;

namespace GrpcBillingService.Services
{
    public class BillingService : Billing.Billing.BillingBase
    {
        private readonly IBillingDataService service;

        public BillingService(IBillingDataService service) => this.service = service;

        public override async Task ListUsers(None request,
            IServerStreamWriter<UserProfile> responseStream,
            ServerCallContext context)
        {
            Models.Coin[] coins = service.GetCoins();
            foreach (var user in service.GetUsers())
            {
                var amount = coins.Where(coin => coin.CurrentUser.Name == user.Name).Count();
                await responseStream.WriteAsync(new UserProfile()
                {
                    Name = user.Name,
                    Amount = amount
                });
            }
        }

        public override Task<Response> CoinsEmission(EmissionAmount request, ServerCallContext context)
        {
            if (request.Amount < service.CountOfUsers())
            {
                return Task.FromResult(new Response
                {
                    Status = Response.Types.Status.Unspecified,
                    Comment = "Not enough coins for all users"
                });
            }

            try
            {
                Models.User[] users = service.GetUsers();

                if (request.Amount == users.Length)
                {
                    foreach (var user in users)
                    {
                        service.AddCoin(new Models.Coin(service.CountOfCoins() + 1, user));
                    }
                }
                else
                {
                    int ratingSum = users.Sum(user => user.Rating);
                    foreach (var user in users)
                    {
                        var coinsCount = request.Amount * (user.Rating / (double)ratingSum);
                        for (int i = 0; i < Math.Floor(coinsCount); i++)
                        {
                            service.AddCoin(new Models.Coin(service.CountOfCoins() + 1, user));
                        }
                    }
                }

                string comment = "Emissing complete!";

                int countOfCoins = service.CountOfCoins();
                if (request.Amount > countOfCoins)
                    comment = $"{countOfCoins} out of {request.Amount} coins were distributed. "
                        + $"{request.Amount - countOfCoins} coin don't distributed";

                return Task.FromResult(new Response
                {
                    Status = Response.Types.Status.Ok,
                    Comment = comment
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Response
                {
                    Status = Response.Types.Status.Failed,
                    Comment = ex.Message
                });
            }
        }

        public override Task<Response> MoveCoins(MoveCoinsTransaction request, ServerCallContext context)
        {
            try
            {
                Models.Coin[] coins = service.GetCoins();
                int sourceUserCoinsCount = coins.Where(coin => coin.CurrentUser.Name == request.SrcUser).Count();

                if (request.Amount > sourceUserCoinsCount)
                    throw new Exception("There is not enough coins on the source user's account");

                int coinsCounter = 0;
                foreach (var coin in coins)
                {
                    if (coin.CurrentUser.Name == request.SrcUser)
                    {
                        if (coinsCounter++ >= request.Amount)
                            break;

                        var user = service.GetUser(request.DstUser);
                        service.UpdateCoin(coin.Id, user);
                    }
                }

                return Task.FromResult(new Response
                {
                    Status = Response.Types.Status.Ok,
                    Comment = "Moving coins success"
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Response
                {
                    Status = Response.Types.Status.Failed,
                    Comment = ex.Message
                });
            }
        }

        public override Task<Coin> LongestHistoryCoin(None request, ServerCallContext context)
        {
            Models.Coin[] coins = service.GetCoins();
            if (coins.Length == 0)
                return Task.FromResult(new Coin());

            int maxHistoryValue = coins.Max(coin => coin.HistoryOfUsers.Count);
            Models.Coin coin = Array.Find(coins, coin => coin.HistoryOfUsers.Count == maxHistoryValue)!;

            Coin resultCoin = new()
            {
                Id = coin.Id,
                History = coin.GetStringHistory()
            };

            return Task.FromResult(resultCoin);
        }
    }
}
