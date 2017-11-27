using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cashed.Extensions;
using Cashed.Logic.Contract;
using Cashed.Logic.Contract.Models;
using Cashed.View.Models;

namespace Cashed.View.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountQueries _accountQueries;
        private readonly IAccountCommands _accountCommands;

        public AccountsController(IAccountQueries accountQueries, IAccountCommands accountCommands)
        {
            _accountQueries = accountQueries;
            _accountCommands = accountCommands;
        }

        public async Task<ActionResult> Index()
        {
            var viewModel = new AccountListViewModel
            {
                List = await _accountQueries.GetList()
            };
            return View(viewModel);
        }

        public ActionResult Add()
        {
            return View(new EditAccountViewModel { Id = -1 });
        }

        private async Task<ActionResult> AddOrUpdate(EditAccountViewModel model)
        {
            var balance = 0m;
            try
            {
                balance = model.Balance.ParseMoneyInvariant();
            }
            catch (FormatException)
            {
                ModelState.AddModelError(nameof(model.Balance), "Входная строка имела неверный формат");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _accountCommands.Update(new AccountModel
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Balance = balance
                    });
                    return RedirectToAction("Index");
                }
                catch (ArgumentException exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(EditAccountViewModel model)
        {
            return await AddOrUpdate(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var account = await _accountQueries.GetById(id);
            if (account == null) return RedirectToAction("Index");
            return View(new EditAccountViewModel
            {
                Id = account.Id,
                Name = account.Name,
                Balance = account.Balance.ToStandardString()
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditAccountViewModel model)
        {
            return await AddOrUpdate(model);
        }

        public ActionResult Delete()
        {
            return RedirectToAction("Index");
        }
    }
}