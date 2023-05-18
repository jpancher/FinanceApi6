﻿using financeAPI.Data;
using financeAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace financeAPI.Repo
{
    public class BankAccountRepo : IRepo<BankAccount>
    {
        private readonly DataContext _db;

        public BankAccountRepo(DataContext db)
        {
            _db = db;
        }
        public async Task<IResult> Create(BankAccount bankAccount)
        {

            _db.BankAccount.Add(bankAccount);
            await _db.SaveChangesAsync();

            return Results.Created($"/BankAccount/{bankAccount.Id}", bankAccount);
        }

        public async Task<IResult> Delete(int id)
        {
            if (await _db.BankAccount.FindAsync(id) is BankAccount bankAccount)
            {
                _db.BankAccount.Remove(bankAccount);
                await _db.SaveChangesAsync();

                return Results.Ok(bankAccount);
            }

            return Results.NotFound();
        }

        public async Task<IResult> Get(int id)
        {
            var bankAccount = await _db.BankAccount.FindAsync(id);
            return (bankAccount != null ? Results.Ok(bankAccount) : Results.NotFound());
        }

        public async Task<IResult> GetAll()
        {
            return Results.Ok(await _db.BankAccount.ToListAsync());
        }

        public async Task<IResult> Update(int id, BankAccount updatedBankAccount)
        {
            var bankAccount = await _db.BankAccount.FindAsync(id);

            if (bankAccount is null)
                return Results.NotFound();
            else
            {
                bankAccount.Name = updatedBankAccount.Name;
                bankAccount.Agency = updatedBankAccount.Agency;
                bankAccount.AccountNumber= updatedBankAccount.AccountNumber;
                bankAccount.BankNumber = updatedBankAccount.BankNumber;
                bankAccount.BankName = updatedBankAccount.BankName;
                bankAccount.Observation = updatedBankAccount.Observation;
            }
            await _db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}