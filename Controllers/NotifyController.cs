﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using SampleBot.Helper;

namespace SampleBot.Controllers
{
    [Route("api/notify")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly string _appId;
        private readonly IConversationReferencesHelper _conversationReferenceHelper;

        public NotifyController(IBotFrameworkHttpAdapter adapter, IConfiguration configuration, IConversationReferencesHelper conversationReferencesHelper)
        {
          _adapter = adapter;
          _conversationReferenceHelper = conversationReferencesHelper;
          _appId = configuration["MicrosoftAppId"] ?? string.Empty;
        }

        public async Task<IActionResult> Get()
        {
            foreach (var conversationReference in _conversationReferenceHelper.)
            {
                await ((BotAdapter)_adapter).ContinueConversationAsync(_appId, conversationReference, BotCallback, default(CancellationToken));
            }

            // Let the caller know proactive messages have been sent
            return new ContentResult()
            {
                Content = "<html><body><h1>Proactive messages have been sent.</h1></body></html>",
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        private async Task BotCallback(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync("proactive hello");
        }
    }
}
