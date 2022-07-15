using InternetAuction.BLL.Contract;
using InternetAuction.BLL.DTO;
using InternetAuction.WEB.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InternetAuction.WEB.Pages.Controllers
{
    /// <summary>
    /// The lot controller.
    /// </summary>
    [Authorize]
    public class LotController : Controller
    {
        private readonly IExpansionGetEmail<UserModel, string> userService;
        private readonly ICrud<LotModel, int> lotService;
        private readonly ICrud<LotCategoryModel, int> lotCategoryModel;
        private readonly ICrud<AutctionStatusModel, int> auctionStatusService;

        private readonly ICrud<AutctionModel, int> auctionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LotController"/> class.
        /// </summary>
        /// <param name="lotService">The lot service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="lotCategoryModel">The lot category model.</param>
        /// <param name="auctionStatusService">The auction status service.</param>
        /// <param name="auctionService">The auction service.</param>
        public LotController(ICrud<LotModel, int> lotService, IExpansionGetEmail<UserModel, string> userService, ICrud<LotCategoryModel, int> lotCategoryModel, ICrud<AutctionStatusModel, int> auctionStatusService, ICrud<AutctionModel, int> auctionService)
        {
            this.lotService = lotService;
            this.userService = userService;
            this.lotCategoryModel = lotCategoryModel;
            this.auctionStatusService = auctionStatusService;
            this.auctionService = auctionService;
        }

        // GET: LotController
        /// <summary>
        /// Indices the async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<ActionResult> IndexAsync()
        {
            return View(await lotService.GetAllAsync());
        }

        public async Task<ActionResult> YourLot()
        {
            var nameUser = HttpContext.User.Identity.Name;
            var user = userService.GetByEmail(nameUser).Result;

            var collection = user.Lots.Select(x => lotService.GetByIdAsync(x.Id).Result).ToList();

            return View(collection);
        }

        public async Task<ActionResult> Details(int id)
        {
            var lot = await lotService.GetByIdAsync(id);
            if (lot != null)
            {
                return View(lot);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LotController/Create
        public async Task<ActionResult> Create()
        {
            LotInfo lotInfo = new LotInfo();
            lotInfo.End = DateTime.Now.AddDays(5).Date;
            lotInfo.Start = DateTime.Now.Date;
            lotInfo.CostMin = 0;
            var collection = (await lotCategoryModel.GetAllAsync()).ToList();
            lotInfo.SelectedLotCategory.AddRange(collection.Select(x => x.NameCategory));
            return View(lotInfo);
        }

        // POST: LotController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LotInfo collection)
        {
            try
            {
                var nameUser = HttpContext.User.Identity.Name;

                LotModel lotModel = new LotModel();
                lotModel.Category = (await lotCategoryModel.GetAllAsync()).First(x => x.NameCategory == collection.SelectedLotCategory.First());
                lotModel.CostMin = collection.CostMin;
                lotModel.Description = collection.Description;
                lotModel.Name = collection.Name;
                lotModel.Author = userService.GetByEmail(nameUser).Result;
                AutctionModel autctionModel = new AutctionModel();
                autctionModel.Start = collection.Start;
                autctionModel.End = collection.End;
                autctionModel.Status = auctionStatusService.GetAllAsync().Result.Last();
                lotModel.Autction = autctionModel;
                await lotService.AddAsync(lotModel);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: LotController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var lot = await lotService.GetByIdAsync(id);
            if (lot != null)
            {
                LotInfo lotInfo = new LotInfo();
                lotInfo.Id = lot.Id;
                lotInfo.CostMin = lot.CostMin;
                lotInfo.Start = lot.Autction.Start;
                lotInfo.End = lot.Autction.End;
                var collection = (await lotCategoryModel.GetAllAsync()).ToList();
                lotInfo.SelectedLotCategory.AddRange(collection.Select(x => x.NameCategory));
                var statusModels = (await auctionStatusService.GetAllAsync()).ToList();

                lotInfo.SelecteStatus.AddRange(statusModels.Select(x => x.NameStatus));

                lotInfo.Description = lot.Description;
                lotInfo.Name = lot.Name;

                return View(lotInfo);
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Edits the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, LotInfo collection)
        {
            try
            {
                var lot = await lotService.GetByIdAsync(id);

                lot.Category = (await lotCategoryModel.GetAllAsync()).First(x => x.NameCategory == collection.SelectedLotCategory.First());
                lot.CostMin = collection.CostMin;
                lot.Description = collection.Description;
                lot.Name = collection.Name;

                var auction = await auctionService.GetByIdAsync(lot.Autction.Id);
                auction.Start = collection.Start;
                auction.End = collection.End;
                auction.Status = auctionStatusService.GetAllAsync().Result.Last();
                lot.Autction = auction;
                await lotService.UpdateAsync(lot);
                await auctionService.UpdateAsync(auction);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Edit), id);
            }
        }
    }
}