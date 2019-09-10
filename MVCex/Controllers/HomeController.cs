using MVCex.Models;
using MVCex.Services;
using MVCex.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCex.Controllers
{
    public class HomeController : Controller
    {
        public GuestbookService guestbookService = new GuestbookService();

        /*public ActionResult Index(string Search = "", int page = 1)
        {
            Guestbook IndexViewModel = new Guestbook();
            //全部資料
            IndexViewModel.DataList = guestbookService.GetAllGuestbooks();
            //加入搜尋
            IndexViewModel.DataList = guestbookService.GetAllGuestbooks(Search);
            //加入搜尋和分頁
            IndexViewModel.ForPaging = new ForPaging(page);
            IndexViewModel.DataList = guestbookService.GetAllGuestbooks(IndexViewModel.ForPaging, Search);
            return View(IndexViewModel);
        }*/

        #region Index 包含搜尋和分頁(未含Ajax)
            /*
        public ActionResult Index(string Search = "", int page = 1)
        {
            Guestbook IndexViewModel = new Guestbook();
            IndexViewModel.ForPaging = new ForPaging(page);
            IndexViewModel.DataList = guestbookService.GetAllGuestbooks(IndexViewModel.ForPaging, Search);
            return View(IndexViewModel);
        }
        */
        #endregion

        #region Index 包含搜尋和分頁(含Ajax)
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult GetDataList(string Search = "", int page = 1)
        {
            Guestbook IndexViewModel = new Guestbook();
            IndexViewModel.Search = Search;
            IndexViewModel.ForPaging = new ForPaging(page);
            IndexViewModel.DataList = guestbookService.GetAllGuestbooks(IndexViewModel.ForPaging, Search);
            return PartialView(IndexViewModel);
        }
        [HttpPost]
        public ActionResult GetDataList([Bind(Include = "Search")]Guestbook IndexView)
        {
            return RedirectToAction("GetDataList", new { Search = IndexView.Search });
        }
        
        #endregion

        public ActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Add(Gbook Data)
        {
            guestbookService.InsertNewGuestbook(Data);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            Gbook Data = guestbookService.GetDataById(Id);
            return View(Data);
        }
        [HttpPost]
        public ActionResult Edit(Gbook EditData)
        {
            guestbookService.EditGuestbook(EditData);
            return RedirectToAction("Index");
        }

        public ActionResult Reply(int Id)
        {
            Gbook Data = guestbookService.GetDataById(Id);
            return View(Data);
        }
        [HttpPost]
        public ActionResult Reply(Gbook ReplyData)
        {
            guestbookService.ReplyGuestbook(ReplyData);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            guestbookService.DeleteGuestbook(Id);
            return RedirectToAction("Index");
        }
    }
}