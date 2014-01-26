using System;

namespace AssistantFallDetector.Services
{
    public interface IApplicationSettingsService
    {
        void LoadApplicationSettings();

        bool AddOrUpdateValue(string Key, Object value);

        valueType GetValueOrDefault<valueType>(string Key, valueType defaultValue);

        void Save();
    }
}
