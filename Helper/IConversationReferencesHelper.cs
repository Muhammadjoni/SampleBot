using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using SampleBot.Models;

namespace SampleBot.Helper
{
  public interface IConversationReferencesHelper
  {
    public Task AddorUpdateConversationRefrenceAsync(ConversationReference reference, TeamsChannelAccount member);
    //Task DeleteConversationRefrenceAsync(ConversationReference reference, TeamsChannelAccount member);
    public Task<ConvRef> GetConversationRefrenceAsync(string upn);

    public Task DeleteConversationRefrenceAsync(ConversationReference reference, TeamsChannelAccount member);
  }
}
