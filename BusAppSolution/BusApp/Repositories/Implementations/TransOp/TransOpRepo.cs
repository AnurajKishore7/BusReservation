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

                //Updating fields
                existingOperator.Name = operatorEntity.Name;
                existingOperator.Contact = operatorEntity.Contact;
                existingOperator.Email = operatorEntity.Email;

                // Find the corresponding user in the Users table
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == existingOperator.Email);
                if (user != null)
                {
                    user.Name = operatorEntity.Name; // Update the name in Users table
                    _context.Users.Update(user);
                }

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

                if (operatorEntity != null)
                {
                    operatorEntity.IsDeleted = true;  // Mark user as deleted
                    _context.TransportOperators.Update(operatorEntity);
                }
                await _context.SaveChangesAsync();

                // Find corresponding user in Users table
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == operatorEntity.Email);
                if (user != null)
                {
                    user.IsDeleted = true;  // Mark user as deleted
                    _context.Users.Update(user);
                }

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
