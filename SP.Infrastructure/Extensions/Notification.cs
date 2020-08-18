using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Infrastructure.Extensions
{
    public class Notification
    {

        public async void Send()
        {
            // This registration token comes from the client FCM SDKs.
            var registrationToken = "AAAASmnxeAQ:APA91bHAU32cLqU-bepAhgXwe5uSgASrtQN2jz2Bvz0CA6ME_7_2lnCglkD5TuApAnrVuMl6CJ6E2z22B96tl17sq7YFjvsweC6mv241Qchl3rxcu8pgrdpr7Kp1MVoPIPFatWxRwDJl";

            // See documentation on defining a message payload.
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                    {
                        { "score", "850" },
                        { "time", "2:45" },
                    },
                Token = registrationToken,
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);
        }


    }
}
