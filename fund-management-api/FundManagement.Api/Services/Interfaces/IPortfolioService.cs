using FundManagement.Api.DTOs.Fund;
using FundManagement.Api.DTOs.Portfolio;

namespace FundManagement.Api.Services.Interfaces
{
    public interface IPortfolioService
    {
        Task<List<PortfolioResponseDto>> GetAllAsync();
        Task BuyUnitsAsync(BuyUnitsDto buyUnitsDto);
        Task SellUnitsAsync(SellUnitsDto sellUnitsDto);
        Task<List<PortfolioSummaryDto>> GetSummaryAsync(int userId);
    }
}
