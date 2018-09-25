using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PushnotificationsDemo.Models
{
    /// <summary>
    /// Database representation of a push subscription
    /// </summary>
    public class PushSubscription
    {
        /// <inheritdoc />
        public PushSubscription() { }

        /// <inheritdoc />
        public PushSubscription(string userId, WebPush.PushSubscription subscription)
        {
            UserId = userId;
            Endpoint = subscription.Endpoint;
            ExpirationTime = null;
            P256Dh = subscription.P256DH;
            Auth = subscription.Auth;
        }

        /// <summary>
        /// User id associated with the push subscription.
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        /// <summary>
        /// The endpoint associated with the push subscription.
        /// </summary>
        [Required]
        public string Endpoint { get; set; }

        /// <summary>
        /// The subscription expiration time associated with the push subscription, if there is one, or null otherwise.
        /// </summary>
        public double? ExpirationTime { get; set; }

        /// <summary>
        /// An
        /// <see href="https://en.wikipedia.org/wiki/Elliptic_curve_Diffie%E2%80%93Hellman">Elliptic curve Diffie–Hellman</see>
        /// public key on the P-256 curve (that is, the NIST secp256r1 elliptic curve).
        /// The resulting key is an uncompressed point in ANSI X9.62 format.
        /// </summary>
        [Required]
        [Key]
        public string P256Dh { get; set; }

        /// <summary>
        /// An authentication secret, as described in
        /// <see href="https://tools.ietf.org/html/draft-ietf-webpush-encryption-08">Message Encryption for Web Push</see>.
        /// </summary>
        [Required]
        public string Auth { get; set; }

        /// <summary>
        /// Converts the push subscription to the format of the library WebPush
        /// </summary>
        /// <returns>WebPush subscription</returns>
        public WebPush.PushSubscription ToWebPushSubscription()
        {
            return new WebPush.PushSubscription(Endpoint, P256Dh, Auth);
        }
    }
}
