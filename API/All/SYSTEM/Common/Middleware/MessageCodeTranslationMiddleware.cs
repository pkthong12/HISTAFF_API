﻿using API.All.Services;
using API.Main;
using API.Socket;
using CORE.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace API.All.SYSTEM.Common.Middleware
{

    public class MessageCodeTranslationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MessageCodeTranslationMiddleware> _logger;
        private readonly IHubContext<SignalHub> _hubContext;
        private readonly AppSettings _appSettings;

        public MessageCodeTranslationMiddleware(
        RequestDelegate next,
        ILogger<MessageCodeTranslationMiddleware> logger,
        IHubContext<SignalHub> hubContext,
        IOptions<AppSettings> options
        )
        {
            _next = next;
            _logger = logger;
            _hubContext = hubContext;
            _appSettings = options.Value;
        }

        public async Task Invoke(HttpContext context, ILanguageService languageService)
        {

            // Buffer the response
            var originalBodyStream = context.Response.Body;
            using var responseBodyBuffer = new MemoryStream();
            context.Response.Body = responseBodyBuffer;


            try
            {
                Register();

                // Call the next middleware in the pipeline
                await _next(context);

                // Rewind the response buffer and read the content
                responseBodyBuffer.Seek(0, SeekOrigin.Begin);
                var responseBodyContent = await new StreamReader(responseBodyBuffer).ReadToEndAsync();

                // Modify the response content if needed
                var modifiedResponse = await GetUpdatedJson(responseBodyContent);


                if (!string.IsNullOrEmpty(modifiedResponse))
                {
                    // Write the modified response back to the original response stream
                    context.Response.ContentLength = null; // Reset the content length
                    context.Response.Body = originalBodyStream;
                    await context.Response.WriteAsync(modifiedResponse);
                }
                else
                {
                    // If no modification needed, write the original response back
                    responseBodyBuffer.Seek(0, SeekOrigin.Begin);
                    await responseBodyBuffer.CopyToAsync(originalBodyStream);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var username = context.Request.Typ(_appSettings);

                await _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                {
                    SignalType = "MIDDLEWARE_ERROR",
                    Message = "MessageCodeTranslationMiddleware"
                });
            }
            finally
            {
                // Restore the original response stream
                context.Response.Body = originalBodyStream;
            }

            async Task<string?> GetUpdatedJson(string responseBodyContent)
            {
                if (context.Response.ContentType!.Contains("application/json"))
                {
                    // Update response header based on responseBodyContent
                    if (responseBodyContent.Contains("\"messageCode\":\""))
                    {
                        int x = responseBodyContent.IndexOf("\"messageCode\":\"") + "\"messageCode\":\"".Length;
                        string? y = responseBodyContent[x..];
                        int z = y.IndexOf('\"');
                        string? messageCode = y[..z];
                        string? xLang = context.Request.Headers["X-Language"];
                        string lang = (xLang != null && !string.IsNullOrEmpty(xLang)) ? xLang : "VI";
                        string translation = await languageService.Translate(messageCode, lang) ?? "null";

                        var responseBodyObject = JsonConvert.DeserializeObject<FormatedResponse>(responseBodyContent);
                        responseBodyObject!.TranslatedMessage = translation;
                        var responseBodyContentUpdated = JsonConvert.SerializeObject(responseBodyObject, Formatting.Indented);

                        return responseBodyContentUpdated;
                    }
                    return await Task.Run(() => string.Empty);
                }
                return await Task.Run(() => string.Empty);
            }

            void Register()
            {
                context.Response.OnStarting(state =>
                {
                    Trace.WriteLine("RESPONSE IS BEING STARTED...");
                    return Task.CompletedTask;
                }, context);
            }
        }
    }
}