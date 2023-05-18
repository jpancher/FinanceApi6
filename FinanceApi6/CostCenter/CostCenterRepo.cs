﻿using financeAPI.Data;
using financeAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace financeAPI.Repo
{
    public class CostCenterRepo : IRepo<CostCenter>
    {
        private readonly DataContext _db;

        public CostCenterRepo(DataContext db)
        {
            _db = db;
        }
        public async Task<IResult> Create(CostCenter costCenter)
        {

            _db.CostCenter.Add(costCenter);
            await _db.SaveChangesAsync();            

            return Results.Created($"/CostCenter/{costCenter.Id}", costCenter);
        }

        public async Task<IResult> Delete(int id)
        {
            if (await _db.CostCenter.FindAsync(id) is CostCenter costCenter)
            {
                _db.CostCenter.Remove(costCenter);
                await _db.SaveChangesAsync();                

                return Results.Ok(costCenter);
            }

            return Results.NotFound();
        }

        public async Task<IResult> Get(int id)
        {
            var costCenter = await _db.CostCenter.FindAsync(id);
            return (costCenter!=null ? Results.Ok(costCenter) : Results.NotFound());
        }

        public async Task<IResult> GetAll()
        {            
            return Results.Ok(await _db.CostCenter.ToListAsync());
        }

        public async Task<IResult> Update(int id, CostCenter updatedCostCenter)
        {
            var costCenter = await _db.CostCenter.FindAsync(id);

            if (costCenter is null) 
                return Results.NotFound();
            else
            {       
                costCenter.Name = updatedCostCenter.Name;
            }
            await _db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
