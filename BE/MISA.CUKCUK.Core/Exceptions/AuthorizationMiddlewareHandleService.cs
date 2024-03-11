using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using MISA.CUKCUK.Core.DTOs;
using MISA.CUKCUK.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Exceptions
{
    public class AuthorizationMiddlewareHandleService : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler DefaultHandler = new AuthorizationMiddlewareResultHandler();
        public async Task HandleAsync(RequestDelegate next, HttpContext context, 
            AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if(authorizeResult.Challenged)
            {
                var error = new MISAErrorResponse
                {
                    devMsg = MISAAuthResource.UserIsNotAuthenticated,
                    userMsg = MISAAuthResource.PleaseLoginToUseWeb,
                    errorCode = "",
                    moreInfor = "",
                    traceId = ""
                };

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(error);
                return;
            }

            await DefaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
