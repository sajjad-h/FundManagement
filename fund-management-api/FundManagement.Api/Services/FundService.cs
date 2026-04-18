using FundManagement.Api.Common.Exceptions;
using FundManagement.Api.Data;
using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.Data.Repositories;
using FundManagement.Api.DTOs.Account;
using FundManagement.Api.DTOs.Fund;
using FundManagement.Api.Models;

namespace FundManagement.Api.Services
{
    public class FundService
    {
        private readonly IFundRepository _fundRepository;

        public FundService(IFundRepository fundRepository)
        {
            _fundRepository = fundRepository;
        }

        public async Task<List<FundResponseDto>> GetAllAsync(string? category)
        {
            var funds = await _fundRepository.GetAllAsync(category);
            if (funds == null || !funds.Any())
                return new List<FundResponseDto>();

            return funds.Select(fund => new FundResponseDto
            {
                Id = fund.Id,
                Name = fund.Name,
                Category = fund.Category,
                NAV = fund.NAV,
                CreatedAt = fund.CreatedAt
            }).ToList();
        }

        public async Task<FundResponseDto?> GetByIdAsync(int id)
        {
            var fund = await _fundRepository.GetByIdAsync(id);
            if (fund == null)
                throw new NotFoundException("Fund not found");

            return new FundResponseDto
            {
                Id = fund.Id,
                Name = fund.Name,
                Category = fund.Category,
                NAV = fund.NAV,
                CreatedAt = fund.CreatedAt
            };
        }

        public async Task<FundResponseDto> AddAsync(CreateFundDto createFundDto)
        {
            var fund = new Fund
            {
                Name = createFundDto.Name,
                Category = createFundDto.Category,
                NAV = createFundDto.NAV
            };

            await _fundRepository.AddAsync(fund);

            return new FundResponseDto
            {
                Id = fund.Id,
                Name = fund.Name,
                Category = fund.Category,
                NAV = fund.NAV,
                CreatedAt = fund.CreatedAt
            };
        }
    }
}
