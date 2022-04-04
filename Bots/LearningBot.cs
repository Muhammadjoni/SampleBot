// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Configuration;
using SampleBot.DB;
using SampleBot.Helper;
using SampleBot.Models;

namespace SampleBot.Bots
  {
    public class LearningBot : TeamsActivityHandler
    {
      ApplicationDbContext context;
      public ApplicationDbContext Context { get { return context; } }
      public LearningBot()
        {
          context = new ApplicationDbContext();
        }

  // Message to send to users when the bot receives a Conversation Update event
      private const string WelcomeMessage = "Welcome to the Proactive Bot sample. <a href=\"http://localhost:3978/api/notify\"> Proactively message everyone who has previously messaged this bot.";

      // Dependency injected dictionary for storing ConversationReference objects used in NotifyController to proactively message users
      private readonly IConversationReferencesHelper _conversationReferenceHelper;
      // private readonly IRequestsHelper _requestsHelper;
      // private readonly IApprovalHelper _approvalHelper;
      private readonly IConfiguration _configuration;

      public LearningBot(IConversationReferencesHelper conversationReferencesHelper, IConfiguration configuration)
        {
          _conversationReferenceHelper = conversationReferencesHelper;
          // _requestsHelper = RequestsHelper;
          _configuration = configuration;
          // _approvalHelper = approvalHelper;
        }

      // private void AddConversationReference(Activity activity)
      // {
      //     var conversationReference = activity.GetConversationReference();
      //     _conversationReferences.AddOrUpdate(conversationReference.User.Id, conversationReference, (key, newValue) => conversationReference);
      // }

      protected override async Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
          ConversationReference botConRef = turnContext.Activity.GetConversationReference();
          var currentMember = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
          await _conversationReferenceHelper.AddorUpdateConversationRefrenceAsync(botConRef, currentMember);
        }

      protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
          var teamConversationData = turnContext.Activity.GetChannelData<TeamsChannelData>();
          var tenantId = teamConversationData.Tenant.Id;
          foreach (var member in membersAdded)
          {
            if (member.Id != turnContext.Activity.Recipient.Id)
            {
              var userId = member.Id;
              // TODO: Store details

              // var user = new ConvRef.UPN();

              // try
              // {
              //   Context.Users.Add(user);
              //   Context.SaveChanges();
              // }
              // catch (Exception e)
              // {
              //   Console.WriteLine(e.Message);
              // }

              await turnContext.SendActivityAsync(MessageFactory.Text(WelcomeMessage), cancellationToken);
            }
          }
        }

      protected override async Task OnInstallationUpdateActivityAsync(ITurnContext<IInstallationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
          var activity = turnContext.Activity;
          ConversationReference botConRef = turnContext.Activity.GetConversationReference();
          var currentMember = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);

          if (activity.Action.Equals("add"))
            {
              if (currentMember.Id != turnContext.Activity.Recipient.Id)
              {
                var userId = currentMember.Id;

                await turnContext.SendActivityAsync(MessageFactory.Text(WelcomeMessage), cancellationToken);

              }

              await _conversationReferenceHelper.AddorUpdateConversationRefrenceAsync(botConRef, currentMember);
            }
            else if (activity.Action.Equals("remove"))
            {

              await _conversationReferenceHelper.DeleteConversationRefrenceAsync(botConRef, currentMember);
            }
        }

      protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // Echo back what the user said
            await turnContext.SendActivityAsync(MessageFactory.Text($"You sent '{turnContext.Activity.Text}'"), cancellationToken);
        }
}
  }
