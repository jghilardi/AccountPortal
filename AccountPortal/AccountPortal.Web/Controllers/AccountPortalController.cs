using AccountPortal.Domain.Models;
using AccountPortal.Domain.Processors.Interfaces;
using System.Web.Mvc;
using LazyCache;


namespace AccountPortal.Web.Controllers
{
    public class AccountPortalController : Controller
    {
        private readonly ICacheProcessor _cacheProcessor;
        private readonly IAccountProcessor _accountProcessor;
        private readonly ITransactionProcessor _transactionProcessor;

        public AccountPortalController(ICacheProcessor cacheProcessor, IAccountProcessor accountProcessor, ITransactionProcessor transactionProcessor)
        {
            _cacheProcessor = cacheProcessor;
            _accountProcessor = accountProcessor;
            _transactionProcessor = transactionProcessor;
        }

        public ActionResult Index()
        {
            return View("");
        }

        [Route("GetAccount")]
        public JsonResult GetAccount(Account account, string password)
        {
            var getAccount = _accountProcessor.GetAccount(account, password);

            return new JsonResult
            {
                Data = new { account = getAccount },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        [Route("AddAccount")]
        public JsonResult AddAccount(Account account)
        {
            var newAccount = _accountProcessor.AddAccount(account);

            return new JsonResult
            {
                Data = new { account = newAccount },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }
    }
}