using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Api.Controllers
{
    [Route("api/[controller]")]
    public class CommonsController : ControllerBase
    {
        private readonly IFirebaseMessagingService _firebaseMessagingService;

        public CommonsController(IFirebaseMessagingService firebaseMessagingService)
        {
            _firebaseMessagingService = firebaseMessagingService;
        }

        [HttpGet]
        public async Task<IActionResult> SendMessageAsync()
        {
            Notification notification = new Notification()
            {
                Title = "Hello",
                Body = "Hello World"
            };

            Dictionary<string, string> payload = new Dictionary<string, string>();
            payload.Add("text", "text1");
            await _firebaseMessagingService.SendAsync("fuBseKbt8Ljr0XASLmwvTl:APA91bEzn3xE29Xj3AoU_cx2PFqxND7bXz1fHz0YL_EtnlYKGYPbt8agoT7w8MMm5uOFaRm7nHOfHCyP_mqXilk5lcE93-hjohR1r9ykcgRQSIIT28LDHpSs3_cf8ks9HvVZu8ZEj5XD", notification, payload);

            return NoContent();
        }

        [HttpGet("multiple")]
        public async Task<IActionResult> SendMultipleMessageAsync()
        {
            Notification notification = new Notification()
            {
                Title = "Multiple",
                Body = "Multiple Hello"
            };

            List<string> tokens = new List<string>(){
                "fuBseKbt8Ljr0XASLmwvTl:APA91bEzn3xE29Xj3AoU_cx2PFqxND7bXz1fHz0YL_EtnlYKGYPbt8agoT7w8MMm5uOFaRm7nHOfHCyP_mqXilk5lcE93-hjohR1r9ykcgRQSIIT28LDHpSs3_cf8ks9HvVZu8ZEj5XD"
            };

            Dictionary<string, string> payload = new Dictionary<string, string>();

            payload.Add("text", "text1");

            await _firebaseMessagingService.SendMultipleAsync(tokens, notification, payload);

            return NoContent();
        }
    }
}