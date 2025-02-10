using BusApp.DTOs.TransOP;
using BusApp.Repositories.Interfaces.TransOp;
using BusApp.Services.Interfaces.TransOp;

namespace BusApp.Services.Implementations.TransOp
{
    public class TransOpService : ITransOpService
    {
        private readonly ITransOpRepo _transOpRepo;

        public TransOpService(ITransOpRepo transOpRepo)
        {
            _transOpRepo = transOpRepo;
        }

        public async Task<TransOpResponseDto?> GetOperatorByIdAsync(int operatorId)
        {
            try
            {
                var operatorEntity = await _transOpRepo.GetOperatorByIdAsync(operatorId);
                if (operatorEntity == null)
                    throw new KeyNotFoundException("Transport operator not found.");

                return new TransOpResponseDto
                {
                    Id = operatorEntity.Id,
                    Name = operatorEntity.Name,
                    Email = operatorEntity.Email,
                    Contact = operatorEntity.Contact
                };
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while fetching the transport operator details.");
            }
        }

        public async Task<bool> UpdateOperatorAsync(int operatorId, TransOpUpdateDto updateDto)
        {
            try
            {
                var existingOperator = await _transOpRepo.GetOperatorByIdAsync(operatorId);
                if (existingOperator == null)
                    throw new KeyNotFoundException("Transport operator not found.");

                existingOperator.Name = updateDto.Name;
                existingOperator.Contact = updateDto.Contact;

                return await _transOpRepo.UpdateOperatorAsync(existingOperator);
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while updating the transport operator.");
            }
        }

        public async Task<bool> DeleteOperatorAsync(int operatorId)
        {
            try
            {
                return await _transOpRepo.DeleteOperatorAsync(operatorId);
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while deleting the transport operator.");
            }
        }
    }
}
