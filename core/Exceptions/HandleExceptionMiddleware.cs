﻿using Microsoft.AspNetCore.Http;
using MISA.CUKCUK.Core.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Exceptions
{
    public class HandleExceptionMiddleware
    {
        private RequestDelegate _next;

        public HandleExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //await _next(context);
            try
            {
                await _next(context);
            }
            catch (MISAValidateException misaEx)
            {
                var errors = new Dictionary<string, List<string>>();
                if (misaEx.FieldName != null && misaEx.Message != null)
                {
                    if (errors.ContainsKey(misaEx.FieldName) == false)
                    {
                        errors.Add(misaEx.FieldName, new List<String> { misaEx.Message });
                    }
                    else if (errors.ContainsKey(misaEx.FieldName) == true)
                    {
                        // kiểm tra key có tồn tại trong errors không
                        if (errors.TryGetValue(misaEx.FieldName, out var messages))
                        {
                            if (!messages.Contains(misaEx.Message))
                            {
                                messages.Add(misaEx.Message);
                            }
                        }
                    }

                    var serviceResult = new
                    {
                        type = "",
                        title = "One or more validation errors occurred.",
                        status = System.Net.HttpStatusCode.BadRequest,
                        traceId = "",
                        errors = errors,
                    };
                    
                    var res = System.Text.Json.JsonSerializer.Serialize(serviceResult);
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(res);
                }
                else
                {
                    // nếu có FieldName hoặc Messange bị null 
                    var serviceResult = new
                    {
                        type = "",
                        title = "One or more validation errors occurred.",
                        status = System.Net.HttpStatusCode.BadRequest,
                        traceId = "",
                        errors = "Dữ liệu không hợp lệ",
                    };

                    var res = System.Text.Json.JsonSerializer.Serialize(serviceResult);
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(res);
                    
                  
                }

            }
            catch (MISACanNotDeleteForeignField exDelete) {
                // Mặc định trả về lỗi 500
                var errors = new Dictionary<string, List<string>>();
                errors.Add("ServerError", new List<string> { exDelete.Message });
                var serviceResult = new
                {
                    type = "",
                    title = "One or more validation errors occurred.",
                    status = System.Net.HttpStatusCode.InternalServerError,
                    traceId = "",
                    errors = errors,
                };


                /*
                    var res = System.Text.Json.JsonSerializer.Serialize(ex);
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                */
                var res = JsonConvert.SerializeObject(serviceResult);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(res);
            }
            catch (Exception ex)
            {
                
                // Mặc định trả về lỗi 500
                var errors = new Dictionary<string, List<string>>();
                errors.Add("ServerError", new List<string> { ex.Message });
                var serviceResult = new
                {
                    type = "",
                    title = "One or more validation errors occurred.",
                    status = System.Net.HttpStatusCode.BadRequest,
                    traceId = "",
                    errors =errors,
                };
    
                    
                /*
                    var res = System.Text.Json.JsonSerializer.Serialize(ex);
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                */
                var res = JsonConvert.SerializeObject(ex);
                await context.Response.WriteAsync(res);
               
   
                }

            }
        }
}
