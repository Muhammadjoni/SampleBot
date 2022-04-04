using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using SampleBot.DB;
using SampleBot.Models;

namespace SampleBot.Helper
{
  public class ConversationReferencesHelper : IConversationReferencesHelper
  {
    private readonly ApplicationDbContext context;

    public ConversationReferencesHelper(ApplicationDbContext context)
    {
      this.context = context;
    }

    public async Task AddorUpdateConversationRefrenceAsync(ConversationReference reference, TeamsChannelAccount member)
    {

      var id = context.ConversationReference.Where(x => x.UPN.Equals(member.UserPrincipalName)).Select(x => x.UPN).FirstOrDefault();
      if (id != member.UserPrincipalName)
      {
        ConvRef conversationReference = new ConvRef();
        conversationReference.UserID = member.Id;
        conversationReference.UPN = member.UserPrincipalName;
        conversationReference.ConversationID = reference.Conversation.Id;
        conversationReference.ServiceUrl = reference.ServiceUrl;
        conversationReference.ActivityID = reference.ActivityId;
        context.ConversationReference.Add(conversationReference);
        await context.SaveChangesAsync();
      }
    }

    public async Task DeleteConversationRefrenceAsync(ConversationReference reference, TeamsChannelAccount member)
    {
      ConvRef cons = await GetConversationRefrenceAsync(member.UserPrincipalName);
      context.ConversationReference.Attach(cons);
      context.ConversationReference.Remove(cons);
      context.SaveChanges();
    }


    public async Task<ConvRef> GetConversationRefrenceAsync(string upn)
    {
      ConvRef conversationRef = new ConvRef();
      conversationRef.UPN = upn;
      conversationRef.Id = context.ConversationReference.Where(x => x.UPN.Equals(upn)).Select(x => x.Id).Single();
      conversationRef.ConversationID = context.ConversationReference.Where(x => x.UPN.Equals(upn)).Select(x => x.ConversationID).Single();

      return conversationRef;
    }

  }
}
