using AssistantFallDetector.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace AssistantFallDetector.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        // Isolated storage settings
        private IsolatedStorageSettings _isolatedStore;

        /// <summary>
        /// Constructor that gets the application settings.
        /// </summary>
        public void LoadApplicationSettings()
        {
            try
            {
                // Get previous application settings.
                _isolatedStore = IsolatedStorageSettings.ApplicationSettings;               
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e.ToString());
            }
        }

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (_isolatedStore.Contains(Key))
            {
                // If the value has changed
                if (_isolatedStore[Key] != value)
                {
                    // Store the new value
                    _isolatedStore[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                _isolatedStore.Add(Key, value);
                valueChanged = true;
            }

            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public valueType GetValueOrDefault<valueType>(string Key, valueType defaultValue)
        {
            valueType value;

            // If the key exists, retrieve the value.
            if (_isolatedStore.Contains(Key))
            {
                value = (valueType)_isolatedStore[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            _isolatedStore.Save();
        }

    }
}
