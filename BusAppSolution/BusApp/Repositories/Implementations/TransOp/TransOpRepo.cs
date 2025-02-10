using BusApp.Models;
using BusApp.Repositories.Interfaces.TransOp;
using BusReservationApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations.TransOp
{
    public class TransOpRepo : ITransOpRepo
    {
        private readonly AppDbContext _context;

        public TransOpRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TransportOperator?> GetOperatorByIdAsync(int operatorId)
        {
            try
            {
                return await _context.TransportOperators
                    .FirstOrDefaultAsync(op => op.Id == operatorId);
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while fetching the transport operator details.");
            }
        }

        public async Task<bool> UpdateOperatorAsync(TransportOperator operatorEntity)
        {
            try
            {
                var existingOperator = await _context.TransportOperators.FindAsync(operatorEntity.Id);
                if (existingOperator == null)
                    throw new KeyNotFoundException("Transport operator not found.");

                existingOperator.Name = operatorEntity.Name;
                existingOperator.Contact = operatorEntity.Contact;
                existingOperator.Email = operatorEntity.Email;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
            catch (DbUpdateException)
            {
                throw new Exception("Database update failed while updating the transport operator.");
            }
            catch (Exception)
            {
                throw new Exception("An unexpected error occurred while updating the transport operator.");
            }
        }

        public async Task<bool> DeleteOperatorAsync(int operatorId)
        {
            try
            {
                var operatorEntity = await _context.TransportOperators.FindAsync(operatorId);
                if (operatorEntity == null)
                    throw new KeyNotFoundException("Transport operator not found.");

                _context.TransportOperators.Remove(operatorEntity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
            catch (DbUpdateException)
            {
                throw new Exception("Database update failed while deleting the transport operator.");
            }
            catch (Exception)
            {
                throw new Exception("An unexpected error occurred while deleting the transport operator.");
            }
        }
    }
}
