using financeAPI.Repo;
using financeAPI.Data;
using financeAPI.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Quic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

string conString = Environment.GetEnvironmentVariable("ASPNETCORE_ConString");

if (conString != null)
{
    if (Environment.GetEnvironmentVariable("ASPNETCORE_EnvWalletPath") != null)
    {
        string path = Environment.GetEnvironmentVariable("ASPNETCORE_EnvWalletPath");
        if (OracleConfiguration.TnsAdmin != path)
        {
            OracleConfiguration.TnsAdmin = path;
            OracleConfiguration.WalletLocation = path;
        }
    } else 
        OracleConfiguration.OracleDataSources.Add(Environment.GetEnvironmentVariable("ASPNETCORE_TNSName"), Environment.GetEnvironmentVariable("ASPNETCORE_TNSConnectionString"));

    var option = new DbContextOptionsBuilder<DataContext>().UseOracle(conString).Options;
    using var db = new DataContext(option, builder.Configuration);

    //db.Database.EnsureCreated();
    //SeedData.SeedAll(db);

    app.UseSwagger();

    var costCenterRepo = new CostCenterRepo(db);
    var chartOfAccountsRepo = new ChartOfAccountsRepo(db);
    var bankAccountRepo = new BankAccountRepo(db);
    var supplierRepo = new SupplierRepo(db);
    var transactionRepo = new TransactionRepo(db);

    app.MapGet("/CostCenter", () => costCenterRepo.GetAll());
    app.MapGet("/CostCenter/{id}", (int id) => costCenterRepo.Get(id));
    app.MapDelete("/CostCenter/{id}", (int id) => costCenterRepo.Delete(id));
    app.MapPut("/CostCenter/{id}", (int id, CostCenter updatedCostCenter) => costCenterRepo.Update(id, updatedCostCenter));
    app.MapPost("/CostCenter/{id}", (CostCenter costCenter) => costCenterRepo.Create(costCenter));

    app.MapGet("/ChartOfAccounts", () => chartOfAccountsRepo.GetAll());
    app.MapGet("/ChartOfAccounts/{id}", (int id) => chartOfAccountsRepo.Get(id));
    app.MapDelete("/ChartOfAccounts/{id}", (int id) => chartOfAccountsRepo.Delete(id));
    app.MapPut("/ChartOfAccounts/{id}", (int id, ChartOfAccounts updatedChartOfAccounts) => chartOfAccountsRepo.Update(id, updatedChartOfAccounts));
    app.MapPost("/ChartOfAccounts/{id}", (ChartOfAccounts chartOfAccounts) => chartOfAccountsRepo.Create(chartOfAccounts));

    app.MapGet("/BankAccount", () => bankAccountRepo.GetAll());
    app.MapGet("/BankAccount/{id}", (int id) => bankAccountRepo.Get(id));
    app.MapDelete("/BankAccount/{id}", (int id) => bankAccountRepo.Delete(id));
    app.MapPut("/BankAccount/{id}", (int id, BankAccount updatedBankAccount) => bankAccountRepo.Update(id, updatedBankAccount));
    app.MapPost("/BankAccount/{id}", (BankAccount bankAccount) => bankAccountRepo.Create(bankAccount));

    app.MapGet("/Supplier", () => supplierRepo.GetAll());
    app.MapGet("/Supplier/{guid}", (Guid guid) => supplierRepo.Get(guid));
    app.MapDelete("/Supplier/{guid}", (Guid guid) => supplierRepo.Delete(guid));
    app.MapPut("/Supplier/{guid}", (Guid guid, Supplier updatedSupplier) => supplierRepo.Update(guid, updatedSupplier));
    app.MapPost("/Supplier/{guid}", (Supplier supplier) => supplierRepo.Create(supplier));

    app.MapGet("/Transaction", () => transactionRepo.GetAll());
    app.MapGet("/Transaction/{guid}", (Guid guid) => transactionRepo.Get(guid));
    app.MapDelete("/Transaction/{guid}", (Guid guid) => transactionRepo.Delete(guid));
    app.MapPut("/Transaction/{guid}", (Guid guid, Transaction updatedTransaction) => transactionRepo.Update(guid, updatedTransaction));
    app.MapPost("/Transaction/{guid}", (Transaction transaction) => transactionRepo.Create(transaction));

    app.UseSwaggerUI();

    app.Run();
}