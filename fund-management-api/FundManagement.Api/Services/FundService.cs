using FundManagement.Api.Common.Exceptions;
using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.DTOs.Fund;
using FundManagement.Api.Models;
using FundManagement.Api.Services.Interfaces;

namespace FundManagement.Api.Services
{
    public class FundService : IFundService
    {
        private readonly IFundRepository _fundRepository;
        private readonly IFundNAVHistoryRepository _fundNAVHistoryRepository;

        public FundService(IFundRepository fundRepository, IFundNAVHistoryRepository fundNAVHistoryRepository)
        {
            _fundRepository = fundRepository;
            _fundNAVHistoryRepository = fundNAVHistoryRepository;
        }

        public async Task<List<FundResponseDto>> GetAllAsync(string? category, bool? curNAVGreaterThan30FilterOn = false)
        {
            var funds = await _fundRepository.GetAllAsync(category, curNAVGreaterThan30FilterOn);
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

        public async Task<FundResponseDto> UpdateAsync(int id, UpdateFundDto dto)
        {
            var fund = await _fundRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Fund not found");

            fund.Name = dto.Name;
            fund.Category = dto.Category;
            fund.NAV = dto.NAV;

            var updated = await _fundRepository.UpdateAsync(fund);

            return new FundResponseDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Category = updated.Category,
                NAV = updated.NAV,
                CreatedAt = updated.CreatedAt
            };
        }

        public async Task DeleteAsync(int id)
        {
            var fund = await _fundRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Fund not found");

            await _fundRepository.DeleteAsync(fund);
        }

        public async Task<List<NAVRecordDto>> GetFundNAVHistoryByFundIdAsync(int fundId)
        {
            var histories = await _fundNAVHistoryRepository.GetByFundIdAsync(fundId);

            return histories.Select(h => new NAVRecordDto
            {
                Date = h.Date,
                NAV = h.NAV
            }).ToList();
        }

        public async IAsyncEnumerable<NAVRecordDto> GetStreamFundNAVHistoryByFundIdAsync(int fundId)
        {
            await foreach (var item in _fundNAVHistoryRepository.GetStreamByFundIdAsync(fundId))
            {
                // simulate slow data
                //await Task.Delay(300); 

                yield return new NAVRecordDto
                {
                    Date = item.Date,
                    NAV = item.NAV
                };
            }
        }
    }
}
