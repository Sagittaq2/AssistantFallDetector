using AssistantFallDetector.Models;
using Microsoft.Phone.Tasks;
using System;

namespace AssistantFallDetector.Services
{
    public class SmsService : ISmsService
    {
        /// <summary>
        /// Prepares and shows the SMS application
        /// </summary>
        /// <param name="sms"></param>
        /// <returns></returns>
        public bool SendSMS(SmsData sms)
        {
            try
            {
                SmsComposeTask smsComposeTask = new SmsComposeTask();

                smsComposeTask.To = sms.Number;
                smsComposeTask.Body = sms.Text;
                smsComposeTask.Show();

                return true;
            } catch (Exception) {
                return false;
            }
        }

    }
}
