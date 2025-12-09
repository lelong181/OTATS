using BusinessLayer.Model;
using BusinessLayer.Model.API;
using BusinessLayer.Model.Sell;
using BusinessLayer.Repository;
using LSPosMVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket;

namespace LSPosMVC.Controllers
{
    public class HomeController : Controller
    {
        private SellRepository _sellRepository;
        private ManagerProfileRepository _managerProfileRepository;

        public HomeController()
        {
            _sellRepository = new SellRepository();
            _managerProfileRepository = new ManagerProfileRepository();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Order()
        {
            _sellRepository.GetSellB2BData(Global.SiteID, Global.ProfileID, DateTime.Now);
            List<ServiceRateModel> listServicePackageRate = _sellRepository.GetListServicePackageRate();
            return View(listServicePackageRate);
        }


        
    }
}