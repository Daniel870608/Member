using MVCex.Services;
using MVCex.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCex.Controllers
{
    public class MemberController : Controller
    {
        public MemberService memberService = new MemberService();
        public MailSerice mailService = new MailSerice();
        // GET: Member
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(MemberRegisterView ViewData)
        {
            if (ModelState.IsValid)
            {
                ViewData.MemberData.Password = ViewData.Password;
                string AuthCode = mailService.GetAuthCode();
                ViewData.MemberData.AuthCode = AuthCode;
                memberService.RegisterNewMember(ViewData.MemberData);
                //寄送驗證信
                //讀取範本檔
                string TempMail = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/RegisterEmailTemplate.html"));
                UriBuilder ValidateUrl = new UriBuilder(Request.Url)
                {
                    Path = Url.Action("EmailValidate", "Member", new { UserName = ViewData.MemberData.Name, AuthCode = ViewData.MemberData.AuthCode })
                };
                //藉 Service 將使用者資料填入驗證信
                string MailBody = mailService.GetRegisterMailBody(TempMail, ViewData.MemberData.Name, ValidateUrl.ToString().Replace("%3F","?"));
                //藉 Service 寄出認證信
                mailService.SendRegisterMail(MailBody, ViewData.MemberData.Email);
                //使用 TempData 儲存註冊資訊
                TempData["RegisterState"] = "註冊成功，請至註冊信箱收取驗證信！";
                //重新導向
                return RedirectToAction("RegisterResult");
            }
            ViewData.Password = null;
            ViewData.PasswordChek = null;
            return View(ViewData);
        }
    }
}