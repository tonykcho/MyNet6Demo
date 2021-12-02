using FirebaseAdmin.Messaging;

namespace MyNet6Demo.Domain.Interfaces
{
    public interface IFirebaseMessagingService
    {
        Task SendAsync(string deviceToken, Notification notification, Dictionary<string, string> payload);

        Task SendMultipleAsync(List<string> tokens, Notification notification, Dictionary<string, string> payload);
    }
}