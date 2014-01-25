using AssistantFallDetector.Models;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssistantFallDetector.Services
{
    public class SmsService : ISmsService
    {

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
