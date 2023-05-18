using financeAPI.Data;
using financeAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace financeAPI.Repo
{
    public class SupplierRepo : IRepoGuid<Supplier>
    {
        private readonly DataContext _db;

        public SupplierRepo(DataContext db)
        {
            _db = db;
        }
        public async Task<IResult> Create(Supplier supplier)
        {
            if (supplier.Guid == Guid.Empty)
            {
                supplier.Guid = Guid.NewGuid();
            }

            _db.Supplier.Add(supplier);
            await _db.SaveChangesAsync();

            return Results.Created($"/Supplier/{supplier.Guid}", supplier);
        }

        public async Task<IResult> Delete(Guid guid)
        {
            if (await _db.Supplier.FindAsync(guid) is Supplier supplier)
            {
                _db.Supplier.Remove(supplier);
                await _db.SaveChangesAsync();

                return Results.Ok(supplier);
            }

            return Results.NotFound();
        }

        public async Task<IResult> Get(Guid guid)
        {
            var supplier = await _db.Supplier.FindAsync(guid);
            return (supplier != null ? Results.Ok(supplier) : Results.NotFound());
        }

        public async Task<IResult> GetAll()
        {
            return Results.Ok(await _db.Supplier.ToListAsync());
        }

        public async Task<IResult> Update(Guid guid, Supplier updatedSupplier)
        {
            var supplier = await _db.Supplier.FindAsync(guid);

            if (supplier is null)
                return Results.NotFound();
            else
            {
                supplier.Name = updatedSupplier.Name;
                supplier.Document = updatedSupplier.Document;
                supplier.Phone = updatedSupplier.Phone;
                supplier.Address = updatedSupplier.Address;
                supplier.PostalCode = updatedSupplier.PostalCode;
                supplier.City = updatedSupplier.City;
                supplier.County = updatedSupplier.County;
                supplier.State = updatedSupplier.State;
                supplier.Country = updatedSupplier.Country;
            }
            await _db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}