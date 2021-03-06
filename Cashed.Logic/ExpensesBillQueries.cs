﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model.Base;
using Cashed.DataAccess.Model.Expenses;
using Cashed.Logic.Contract;
using Cashed.Logic.Contract.Models;

namespace Cashed.Logic
{
    public class ExpensesBillQueries : IExpensesBillQueries
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpensesBillQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<ExpenseBillModel>> GetAll(bool includeDeleted = false)
        {
            throw new NotImplementedException();
        }

        public async Task<ExpenseBillsList> GetFiltered(DateTime dateFrom, DateTime dateTo, PaginationArgs args)
        {
            var billsList = new ExpenseBillsList();

            // отбираем нужные счета
            var expenseRepo = _unitOfWork.GetQueryRepository<ExpenseBill>();
            var billsQuery = expenseRepo.Query
                .Where(x => x.DateTime >= dateFrom && x.DateTime < dateTo);
            var totalCount = billsQuery.Count();
            var pagination = PaginationInfo.FromArgs(args, totalCount);
            var bills = await billsQuery
                .OrderByDescending(x => x.DateTime)
                .ThenByDescending(x => x.SumPrice)
                .Skip(pagination.Skipped).Take(pagination.Taken)
                .Select(x => new ExpenseBillModel
                {
                    Id = x.Id,
                    Cost = x.SumPrice,
                    DateTime = x.DateTime
                })
                .ToListAsync();
            var billIds = bills.Select(x => x.Id).ToList();

            var itemsRepo = _unitOfWork.GetQueryRepository<ExpenseItem>();

            // ищем категории, входящие в счет
            var categoriesRepo = _unitOfWork.GetQueryRepository<Category>();
            var billCategoriesQuery =
                from itm in itemsRepo.Query
                join cat in categoriesRepo.Query on itm.CategoryId equals cat.Id
                where billIds.Contains(itm.BillId)
                group cat.Name by itm.BillId
                into catGroup
                select new
                {
                    BillId = catGroup.Key,
                    CategoryNames = catGroup.Distinct().ToList()
                };
            var catNames = await billCategoriesQuery.ToListAsync();
            var catDict = catNames.ToDictionary(x => x.BillId, x => x.CategoryNames);

            // присваиваем каждому счету свой список вошедших в него категорий
            foreach (var bill in bills)
            {
                if (catDict.TryGetValue(bill.Id, out var billCats))
                {
                    var firstTwo = billCats.Take(2);
                    bill.Category = string.Join(", ", firstTwo);
                    if (billCats.Count > 2)
                    {
                        bill.Category += $"... ({billCats.Count})";
                    }
                }
            }

            billsList.List = bills;
            billsList.Pagination = pagination;
            return billsList;
        }

        public async Task<ExpenseBillModel> GetById(int id)
        {
            var repo = _unitOfWork.GetQueryRepository<ExpenseBill>();
            var bill = await repo.GetById(id);
            var model = new ExpenseBillModel
            {
                Id = bill.Id,
                DateTime = bill.DateTime,
                Cost = bill.SumPrice,
                Items = bill.Items.Select(x => new ExpenseItemModel
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    DateTime = x.DateTime,
                    Comment = x.Comment,
                    Cost = x.Price,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };
            await FullfillItemsFields(model);
            return model;
        }

        private async Task FullfillItemsFields(ExpenseBillModel bill)
        {
            var catRepo = _unitOfWork.GetQueryRepository<Category>();
            var prodRepo = _unitOfWork.GetQueryRepository<Product>();
            foreach (var item in bill.Items)
            {
                var product = await prodRepo.GetById(item.ProductId);
                item.Product = product.Name;
                var category = await catRepo.GetById(item.CategoryId);
                item.Category = category.Name;
            }
        }

        public Task<ExpenseBillModel> GetByName(string name, bool includeDeleted = false)
        {
            throw new NotImplementedException();
        }

        public TotalsInfoModel GetTotals(List<ExpenseBillModel> bills)
        {
            if (bills.Count > 0)
            {
                var orderedBills = bills.OrderBy(x => x.DateTime).ToList();
                var totals = new TotalsInfoModel
                {
                    Caption = $"Итог за {orderedBills.FirstOrDefault()?.DateTime: yyyy.MM.dd} - {orderedBills.LastOrDefault()?.DateTime: yyyy.MM.dd}: ",
                    Total = bills.Sum(x => x.Cost)
                };
                return totals;
            }
            return new TotalsInfoModel
            {
                Caption = "Итог:",
                Total = 0m
            };
        }
    }
}
