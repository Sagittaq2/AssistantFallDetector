using AssistantFallDetector.Models;

namespace AssistantFallDetector.Services
{
    public interface ISmsService
    {
        bool SendSMS(SmsData sms);
    }
}
