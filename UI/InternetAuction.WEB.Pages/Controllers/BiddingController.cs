using InternetAuction.BLL.Contract;
using InternetAuction.BLL.Contract.Validation;
using InternetAuction.BLL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InternetAuction.WEB.Pages.Controllers
{
    /// <summary>
    /// The bidding controller.
    /// </summary>
    [Authorize]
    public class BiddingController : Controller
    {
        private readonly IExpansionGetEmail<UserModel, string> userService;
        private readonly ICrud<LotModel, int> lotService;
        private readonly ICrud<BiddingModel, int> biddingService;
        private readonly ICrud<AutctionModel, int> auctionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiddingController"/> class.
        /// </summary>
        /// <param name="biddingService">The bidding service.</param>
        /// <param name="lotService">The lot service.</param>
        /// <param name="userService">The user service.</param>
        public BiddingController(ICrud<BiddingModel, int> biddingService, ICrud<LotModel, int> lotService, IExpansionGetEmail<UserModel, string> userService, ICrud<AutctionModel, int> auctionService)
        {
            this.biddingService = biddingService;
            this.lotService = lotService;
            this.userService = userService;
            this.auctionService = auctionService;
        }

        // GET: BiddingController
        /// <summary>
        /// Indices the.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        public ActionResult Index()
        {
            var nameUser = HttpContext.User.Identity.Name;
            var user = userService.GetByEmail(nameUser).Result;

            return View(user.Biddings);
        }

        /// <summary>
        /// Details the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>An ActionResult.</returns>
        public ActionResult Details(int id)
        {
            var nameUser = HttpContext.User.Identity.Name;
            var user = userService.GetByEmail(nameUser).Result;
            var result = biddingService.GetByIdAsync(id).Result;
            return View(biddingService.GetByIdAsync(id).Result);
        }

        // GET: BiddingController/Create
        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>An ActionResult.</returns>
        public async Task<ActionResult> Create(int id)
        {
            BiddingModel model = new BiddingModel();
            model.Autction = (await lotService.GetByIdAsync(id)).Autction;
            model.Cost = model.Autction.Lot.CostMin;
            return View(model);
        }

        // POST: BiddingController/Create
        /// <summary>
        /// Creates the async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(int id, BiddingModel collection)
        {
            try
            {
                var nameUser = HttpContext.User.Identity.Name;
                var user = userService.GetByEmail(nameUser).Result;
                collection.Autction = (await lotService.GetByIdAsync(collection.Id)).Autction;
                collection.Id = 0;
                collection.User = user;
                collection.Date = DateTime.Now;

                await biddingService.AddAsync(collection);
                return RedirectToAction(nameof(Index));
            }
            catch (InternetException e) { ModelState.AddModelError("", e.Message); ModelState.AddModelError("", e.Message); return (View(collection)); }
            catch (Exception e) { return RedirectToAction(nameof(Index), id); }
        }

        // GET: BiddingController/Delete/5
        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>An ActionResult.</returns>
        public ActionResult Delete(int id)
        {
            return View(biddingService.GetByIdAsync(id).Result);
        }

        // POST: BiddingController/Delete/5
        /// <summary>
        /// Deletes the async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, IFormCollection collection)
        {
            try
            {
                await biddingService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(id);
            }
        }
    }
}