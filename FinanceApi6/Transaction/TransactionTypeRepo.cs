using financeAPI.Data;
using financeAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace financeAPI.Repo
{
    public class TransactionTypeRepo : IRepo<TransactionType>
    {
        private readonly DataContext _db;

        public TransactionTypeRepo(DataContext db)
        {
            _db = db;
        }

        public async Task<IResult> Create(TransactionType transactionType)
        {

            _db.TransactionType.Add(transactionType);
            await _db.SaveChangesAsync();

            return Results.Created($"/TransactionType/{transactionType.Id}", transactionType);
        }

        public async Task<IResult> Delete(int id)
        {
            if (await _db.TransactionType.FindAsync(id) is TransactionType transactionType)
            {
                _db.TransactionType.Remove(transactionType);
                await _db.SaveChangesAsync();

                return Results.Ok(transactionType);
            }

            return Results.NotFound();
        }

        public async Task<IResult> Get(int id)
        {
            var transactionType = await _db.TransactionType.FindAsync(id);
            return (transactionType != null ? Results.Ok(transactionType) : Results.NotFound());
        }

        public async Task<IResult> GetAll()
        {
            return Results.Ok(await _db.TransactionType.ToListAsync());
        }

        public async Task<IResult> Update(int id, TransactionType updatedTransactionType)
        {
            var transactionType = await _db.TransactionType.FindAsync(id);

            if (transactionType is null)
                return Results.NotFound();
            else
            {
                transactionType.Name = updatedTransactionType.Name;
            }
            await _db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
