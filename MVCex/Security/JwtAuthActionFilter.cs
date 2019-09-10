using Jose;
using MVCex.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
namespace MVCex.Security
{
    public class JwtAuthActionFilter
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var secret = "Guestbook";
            if (actionContext.Request.Headers.Authorization == null || actionContext.Request.Headers.Authorization.Scheme != "Bearer")
            {
                setErrorResponse(actionContext, "驗證錯誤");
            }
            else
            {
                try
                {
                    var jwtObject = JWT.Decode<JwtObject>(
                        actionContext.Request.Headers.Authorization.Parameter,
                        Encoding.UTF8.GetBytes(secret),
                        JwsAlgorithm.HS512);
                    if (IsTokenExp(jwtObject.Exp.ToString()))
                    {
                        setErrorResponse(actionContext, "Token已過期");
                    }
                }
                catch (Exception ex)
                {
                    setErrorResponse(actionContext, ex.Message);
                    throw;
                }
            }
        }

        private bool IsTokenExp(string datetime)
        {
            return Convert.ToDateTime(datetime) < DateTime.Now;
        }

        private void setErrorResponse(HttpActionContext actionContext, string message)
        {
            var response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, message);
            actionContext.Response = response;
           
        }
    }
}