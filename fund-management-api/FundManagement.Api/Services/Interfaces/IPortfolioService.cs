using FundManagement.Api.DTOs.Account;
using FundManagement.Api.DTOs.Fund;
using FundManagement.Api.DTOs.Portfolio;
using FundManagement.Api.Models;

namespace FundManagement.Api.Services.Interfaces
{
    public interface IPortfolioService
    {
        Task BuyUnitsAsync(BuyUnitsDto buyUnitsDto);
        Task SellUnitsAsync(SellUnitsDto sellUnitsDto);
    }
}
