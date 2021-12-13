using MyNet6Demo.Domain.Interfaces;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Microsoft.Extensions.Logging;

namespace MyNet6Demo.Core.Services
{
    public class FirebaseMessagingService : IFirebaseMessagingService
    {
        private readonly FirebaseMessaging _firebaseMessaging;
        private readonly ILogger<FirebaseMessagingService> _logger;

        public FirebaseMessagingService(ILogger<FirebaseMessagingService> logger)
        {
            _firebaseMessaging = FirebaseMessaging.GetMessaging(FirebaseApp.DefaultInstance);
            _logger = logger;
        }

        public async Task SendAsync(string deviceToken, Notification notification, Dictionary<string, string> payload)
        {
            ArgumentNullException.ThrowIfNull(deviceToken);

            ArgumentNullException.ThrowIfNull(notification);

            ArgumentNullException.ThrowIfNull(payload);

            Message message = new Message()
            {
                Token = deviceToken,
                Notification = notification,
                Data = payload,
                Webpush = new WebpushConfig()
                {
                    FcmOptions = new WebpushFcmOptions()
                    {
                        Link = "https://localhost:3000"
                    },
                },
                Apns = new ApnsConfig()
                {
                    Aps = new Aps()
                    {
                        Alert = new ApsAlert()
                        {
                            Title = notification.Title,
                            Body = notification.Body
                        },
                        // Sound = "default",
                        // Badge = 1,
                        CustomData = payload.ToDictionary(pair => pair.Key, pair => (object)pair.Value),
                        ContentAvailable = true
                    },
                    Headers = new Dictionary<string, string>
                    {
                        { "apns-push-type", "alert" },
                        { "apns-priority", "5" }
                    }
                }
            };

            try
            {
                await _firebaseMessaging.SendAsync(message);
            }
            catch (FirebaseMessagingException ex)
            {
                _logger.LogInformation($"--> Send Notification Failed: {ex.Message}");
                _logger.LogError(new EventId(), ex, ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogInformation($"--> Send Notification Failed: {ex.Message}");
            }
        }

        public async Task SendMultipleAsync(List<string> tokens, Notification notification, Dictionary<string, string> payload)
        {
            // FCM only accept at most 500 device tokens per message;
            for (int x = 0; x < tokens.Count / 500; x++)
            {
                MulticastMessage message = new MulticastMessage()
                {
                    Tokens = tokens.Skip(x * 500).Take(500).ToList(),
                    Notification = notification,
                    Data = payload,
                    Webpush = new WebpushConfig()
                    {
                        FcmOptions = new WebpushFcmOptions()
                        {
                            Link = "https://localhost:3000"
                        },
                    },
                    Apns = new ApnsConfig()
                    {
                        Aps = new Aps()
                        {
                            Alert = new ApsAlert()
                            {
                                Title = notification.Title,
                                Body = notification.Body
                            },
                            // Sound = "default",
                            // Badge = 1,
                            CustomData = payload.ToDictionary(pair => pair.Key, pair => (object)pair.Value),
                            ContentAvailable = true
                        },
                        Headers = new Dictionary<string, string>
                    {
                        { "apns-push-type", "alert" },
                        { "apns-priority", "5" }
                    }
                    }
                };

                try
                {
                    await _firebaseMessaging.SendMulticastAsync(message);
                }
                catch (FirebaseMessagingException ex)
                {
                    _logger.LogInformation($"--> Send Notification Failed: {ex.Message}");
                    _logger.LogError(new EventId(), ex, ex.Message);
                }
                catch (ArgumentException ex)
                {
                    _logger.LogInformation($"--> Send Notification Failed: {ex.Message}");
                }
            }
        }
    }
}