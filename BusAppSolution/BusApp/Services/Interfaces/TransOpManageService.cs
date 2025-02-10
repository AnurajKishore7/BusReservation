using BusApp.DTOs.BusManagement;
using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Services.Implementations;

namespace BusApp.Services.Interfaces
{
    public class TransOpManageService : ITransOpManageService
    {
        private readonly ITransOpManageRepo _transOpManageRepo;

        public TransOpManageService(ITransOpManageRepo transOpManageRepo)
        {
            _transOpManageRepo = transOpManageRepo;
        }

        public async Task<bool> AddBusAsync(BusDto busDto)
        {
            try
            {
                if (busDto == null)
                    throw new ArgumentNullException(nameof(busDto), "Bus data cannot be null.");

                var bus = new Bus
                {
                    BusNo = busDto.BusNo,
                    OperatorId = busDto.OperatorId,
                    BusType = busDto.BusType,
                    TotalSeats = busDto.TotalSeats
                };

                return await _transOpManageRepo.AddBusAsync(bus);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the bus.", ex);
            }
        }

        public async Task<IEnumerable<BusResponseDto>> GetBusesByOperatorIdAsync(int operatorId)
        {
            try
            {
                if (operatorId <= 0)
                    throw new ArgumentException("Invalid operator ID.", nameof(operatorId));

                var buses = await _transOpManageRepo.GetBusesByOperatorIdAsync(operatorId);

                return buses.Select(bus => new BusResponseDto
                {
                    Id = bus.Id,
                    BusNo = bus.BusNo!,
                    BusType = bus.BusType!,
                    TotalSeats = bus.TotalSeats,
                    OperatorId = bus.OperatorId
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving buses.", ex);
            }
        }

        public async Task<BusResponseDto?> GetBusByIdAsync(int busId)
        {
            try
            {
                if (busId <= 0)
                    throw new ArgumentException("Invalid bus ID.", nameof(busId));

                var bus = await _transOpManageRepo.GetBusByIdAsync(busId);
                if (bus == null)
                    throw new KeyNotFoundException($"Bus with ID {busId} not found.");

                return new BusResponseDto
                {
                    Id = bus.Id,
                    BusNo = bus.BusNo!,
                    BusType = bus.BusType!,
                    TotalSeats = bus.TotalSeats,
                    OperatorId = bus.OperatorId
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the bus details.", ex);
            }
        }

        public async Task<bool> UpdateBusAsync(int busId, BusDto busDto)
        {
            try
            {
                if (busId <= 0)
                    throw new ArgumentException("Invalid bus ID.", nameof(busId));
                if (busDto == null)
                    throw new ArgumentNullException(nameof(busDto), "Bus data cannot be null.");

                var existingBus = await _transOpManageRepo.GetBusByIdAsync(busId);
                if (existingBus == null)
                    throw new KeyNotFoundException($"Bus with ID {busId} not found.");

                existingBus.BusNo = busDto.BusNo;
                existingBus.BusType = busDto.BusType;
                existingBus.TotalSeats = busDto.TotalSeats;

                return await _transOpManageRepo.UpdateBusAsync(existingBus);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the bus.", ex);
            }
        }

        public async Task<bool> DeleteBusAsync(int busId)
        {
            try
            {
                if (busId <= 0)
                    throw new ArgumentException("Invalid bus ID.", nameof(busId));

                var deleted = await _transOpManageRepo.DeleteBusAsync(busId);
                if (!deleted)
                    throw new KeyNotFoundException($"Bus with ID {busId} not found or could not be deleted.");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the bus.", ex);
            }
        }
    }

}
