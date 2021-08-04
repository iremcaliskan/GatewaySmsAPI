using RestSharp;
using System;

namespace GatewaySmsAPI
{
    public class SmsSenderHelper
    {
        public static UserPartialResponseDto SendSmsMessage(UserGetDto user)
        {
            var client = new RestSharp.RestClient("https://gatewayapi.com/rest/");
            var apiToken = "_Your_API_Token_Here_";
            client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(apiToken, "");
            var request = new RestSharp.RestRequest("mtsms", RestSharp.Method.POST);
            request.AddJsonBody(new
            {
                sender = string.Format($"{user.Sender}"),
                message = string.Format($"Hello {user.FirstName} {user.LastName}, Thank you for joining us! Your activation one time password is {user.OtpArea}"),
                validity_period = 180,
                recipients = new[] { new { msisdn = user.PhoneNumber } }
            });
            var response = client.Execute(request);

            // On 200 OK
            if ((int)response.StatusCode == 200)
            {
                var res = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
                // Sample response content:
                // {"ids":[1775475914],"usage":{"countries":{"TR":1},"currency":"EUR","total_cost":0.0061}}
                var req = Newtonsoft.Json.Linq.JObject.Parse(response.Request.Body.Value.ToString());
                // Sample response.Request.Body.Value:
                // {"sender":"Demo Project","message":"Hello İrem Çalışkan, Thank you for joining us! Your activation one time password is 649018","recipients":[{"msisdn":901234567891}]}
                foreach (var i in res["ids"])
                {
                    user.MessageId = (int)i;
                }
                user.Message = (string)req["message"];
                user.SentTime = DateTime.Now;
                user.Status = GetMessageStatus(user);

                var userDetail = new UserPartialResponseDto()
                {
                    MessageId = user.MessageId,
                    Message = user.Message,
                    SentTime = user.SentTime,
                    Status = user.Status
                };
                return userDetail;
            }
            else if (response.ResponseStatus == RestSharp.ResponseStatus.Completed)
            {
                Console.WriteLine(response.Content);
                throw new SystemException("Any kind of server error(Network down, failed Dns, etc.) but except 404!");
            }
            else
            {
                Console.WriteLine(response.ErrorMessage);
                throw new SystemException("Any kind of server error(network down, failed Dns, etc.) but except 404!");
            }
        }

        public static string GetMessageStatus(UserGetDto user)
        {
            var client = new RestSharp.RestClient("https://gatewayapi.com/rest/");
            var apiToken = "_Your_API_Token_Here_";
            client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(apiToken, "");
            var request = new RestSharp.RestRequest(string.Format($"mtsms/{user.MessageId}"), Method.GET);
            var response = client.Execute(request);

            // Sample response.Content :
            // {"anonymized": null, "callback_url": null, "class": "standard", "destaddr": "MOBILE", "encoding": "UTF8", "id": 1776420005,
            // "label": null, "message": "Hello \u0130rem \u00c7al\u0131\u015fkan, Thank you for joining us! Your activation one time password is 130985",
            // "payload": null, "priority": "NORMAL",
            // "recipients": [{"country": "TR", "csms": 1, "dsnerror": null, "dsnerrorcode": null, "dsnstatus": "DELIVERED", "dsntime": 1627975839.0,
            // "mcc": null, "mnc": null, "msisdn": 901234567891, "senttime": 1627975833.0, "tagvalues": null}], "sender": "Demo Project",
            // "sendtime": null, "tags": null, "udh": null, "userref": null, "validity_period": 180}

            // On 200 OK
            if ((int)response.StatusCode == 200)
            {
                var res = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
                foreach (var item in res["recipients"])
                {
                    user.Status = (string)item["dsnstatus"];
                }

                var status = user.Status;
                return status;
            }
            else if (response.ResponseStatus == RestSharp.ResponseStatus.Completed)
            {
                Console.WriteLine(response.Content);
                throw new SystemException("Any kind of server error(network down, failed Dns, etc.) but except 404!");
            }
            else
            {
                Console.WriteLine(response.ErrorMessage);
                throw new SystemException("Any kind of server error(network down, failed Dns, etc.) but except 404!");
            }
        }

        public static string GenerateOTP()
        { // One Time Password
            string num = "0123456789";
            int len = num.Length;
            int otpDigit = 6;
            string otp = "";
            string finalDigit;
            int getIndex;

            for (int i = 0; i < otpDigit; i++)
            {
                do
                {
                    getIndex = new Random().Next(0, len);
                    finalDigit = num.ToCharArray()[getIndex].ToString();
                } while (otp.IndexOf(finalDigit) != -1);
                otp += finalDigit;
            }
            return otp;
        }
    }
}