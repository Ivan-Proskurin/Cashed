using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model.Base;
using Cashed.Logic.Contract;
using Cashed.Logic.Contract.Models;

namespace Cashed.Logic
{
    public class AccountQueries : IAccountQueries
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private static AccountModel ToModel(Account account)
        {
            if (account == null) return null;
            return new AccountModel
            {
                Id = account.Id,
                Name = account.Name,
                Balance = account.Balance
            };
        }

        public async Task<List<AccountModel>> GetAll(bool includeDeleted = false)
        {
            var accounts = await _unitOfWork.GetQueryRepository<Account>().Query.ToListAsync();
            return accounts.Select(ToModel).ToList();
        }

        public async Task<AccountList> GetList()
        {
            var accounts = await GetAll();
            return new AccountList
            {
                Accounts = accounts,
                Totals = new TotalsInfoModel
                {
                    Caption = "Суммарный баланс:",
                    Total = accounts.Sum(x => x.Balance)
                }
            };
        }

        public async Task<AccountModel> GetById(int id)
        {
            return ToModel(await _unitOfWork.GetQueryRepository<Account>().GetById(id));
        }

        public async Task<AccountModel> GetByName(string name, bool includeDeleted = false)
        {
            var account = await _unitOfWork.GetNamedModelQueryRepository<Account>().GetByName(name);
            if (account == null || account.IsDeleted && !includeDeleted) return null;
            return ToModel(account);
        }
    }
}