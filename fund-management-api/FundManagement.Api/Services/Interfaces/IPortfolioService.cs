using FundManagement.Api.DTOs.Portfolio;

namespace FundManagement.Api.Services.Interfaces
{
    public interface IPortfolioService
    {
        Task BuyUnitsAsync(BuyUnitsDto buyUnitsDto);
        Task SellUnitsAsync(SellUnitsDto sellUnitsDto);
    }
}
