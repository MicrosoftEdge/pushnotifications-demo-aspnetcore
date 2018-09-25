using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PushnotificationsDemo.Models;
using PushnotificationsDemo.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PushnotificationsDemo.Controllers
{
    /// <summary>
    /// VAPID Push Notification API
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PushController : ControllerBase
    {
        private readonly IHostingEnvironment _env;
        private readonly IPushService _pushService;

        /// <inheritdoc />
        public PushController(IHostingEnvironment hostingEnvironment, IPushService pushService)
        {
            _env = hostingEnvironment;
            _pushService = pushService;
        }

        // GET: api/push/vapidpublickey
        /// <summary>
        /// Get VAPID Public Key
        /// </summary>
        /// <returns>VAPID Public Key</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet, Route("vapidpublickey")]
        public ActionResult<string> GetVapidPublicKey()
        {
            return Ok(_pushService.GetVapidPublicKey());
        }

        // POST: api/push/subscribe
        /// <summary>
        /// Subscribe for push notifications
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">NoContent</response>
        /// <response code="400">BadRequest if subscription is null or invalid.</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("subscribe")]
        public async Task<ActionResult<PushSubscription>> Subscribe([FromBody] PushSubscriptionViewModel model)
        {
            var subscription = new PushSubscription
            {
                UserId = Guid.NewGuid().ToString(), // You'd use your existing user id here
                Endpoint = model.Subscription.Endpoint,
                ExpirationTime = model.Subscription.ExpirationTime,
                Auth = model.Subscription.Keys.Auth,
                P256Dh = model.Subscription.Keys.P256Dh
            };

            return await _pushService.Subscribe(subscription);
        }

        // POST: api/push/unsubscribe
        /// <summary>
        /// Unsubscribe for push notifications
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">NoContent</response>
        /// <response code="400">BadRequest if subscription is null or invalid.</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("unsubscribe")]
        public async Task<ActionResult<PushSubscription>> Unsubscribe([FromBody] PushSubscriptionViewModel model)
        {
            var subscription = new PushSubscription
            {
                Endpoint = model.Subscription.Endpoint,
                ExpirationTime = model.Subscription.ExpirationTime,
                Auth = model.Subscription.Keys.Auth,
                P256Dh = model.Subscription.Keys.P256Dh
            };

            await _pushService.Unsubscribe(subscription);

            return subscription;
        }

        // POST: api/push/send
        /// <summary>
        /// Send a push notifications to a specific user's every device (for development only!)
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="202">Accepted</response>
        /// <response code="400">BadRequest if subscription is null or invalid.</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("send/{userId}")]
        public async Task<ActionResult<AcceptedResult>> Send([FromRoute] string userId, [FromBody] Notification notification, [FromQuery] int? delay)
        {
            if (!_env.IsDevelopment()) return Forbid();

            if (delay != null) Thread.Sleep((int)delay);

            await _pushService.Send(userId, notification);

            return Accepted();
        }
    }

    /// <summary>
    /// Request body model for Push registration
    /// </summary>
    public class PushSubscriptionViewModel
    {
        /// <inheritdoc cref="Subscription"/>
        public Subscription Subscription { get; set; }

        /// <summary>
        /// Other attributes, like device id for example.
        /// </summary>
        public string DeviceId { get; set; }
    }

    /// <summary>
    /// Representation of the Web Standard Push API's <see href="https://developer.mozilla.org/en-US/docs/Web/API/PushSubscription">PushSubscription</see>
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// The endpoint associated with the push subscription.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// The subscription expiration time associated with the push subscription, if there is one, or null otherwise.
        /// </summary>
        public double? ExpirationTime { get; set; }

        /// <inheritdoc cref="Keys"/>
        public Keys Keys { get; set; }

        /// <summary>
        /// Converts the push subscription to the format of the library WebPush
        /// </summary>
        /// <returns>WebPush subscription</returns>
        public WebPush.PushSubscription ToWebPushSubscription() => new WebPush.PushSubscription(Endpoint, Keys.P256Dh, Keys.Auth);
    }

    /// <summary>
    /// Contains the client's public key and authentication secret to be used in encrypting push message data.
    /// </summary>
    public class Keys
    {
        /// <summary>
        /// An <see href="https://en.wikipedia.org/wiki/Elliptic_curve_Diffie%E2%80%93Hellman">Elliptic curve Diffie–Hellman</see> public key on the P-256 curve (that is, the NIST secp256r1 elliptic curve).
        /// The resulting key is an uncompressed point in ANSI X9.62 format.
        /// </summary>
        public string P256Dh { get; set; }

        /// <summary>
        /// An authentication secret, as described in <see href="https://tools.ietf.org/html/draft-ietf-webpush-encryption-08">Message Encryption for Web Push</see>.
        /// </summary>
        public string Auth { get; set; }
    }
}