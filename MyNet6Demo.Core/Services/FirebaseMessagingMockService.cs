using FirebaseAdmin.Messaging;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Core.Services
{
    public class FirebaseMessagingMockService : IFirebaseMessagingService
    {
        public Task SendAsync(string deviceToken, Notification notification, Dictionary<string, string> payload)
        {
            return Task.CompletedTask;
        }

        public Task SendMultipleAsync(List<string> tokens, Notification notification, Dictionary<string, string> payload)
        {
            return Task.CompletedTask;
        }
    }
}