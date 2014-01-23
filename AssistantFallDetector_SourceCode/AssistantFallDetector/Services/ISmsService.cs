using AssistantFallDetector.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssistantFallDetector.Services
{
    public interface ISmsService
    {
        bool SendSMS(SmsData sms);
    }
}
