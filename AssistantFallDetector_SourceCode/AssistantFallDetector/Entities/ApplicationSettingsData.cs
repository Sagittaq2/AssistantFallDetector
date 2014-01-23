using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AssistantFallDetector.ViewModels.Base;

namespace AssistantFallDetector.Entities
{
    public class ApplicationSettingsData
    {
        // Isolated storage settings
        private static IsolatedStorageSettings _isolatedStore;

        // Isolated storage key names
        private const string InitialLaunchSettingKeyName = "InitialLaunchSetting";
        private const string LastUpdatedTimeKeyName = "LastUpdatedTime";

        private const string AccelerationAlarmSettingKeyName = "AccelerationAlarmSetting";
        private const string IdleTimeAccelerationAlarmSettingKeyName = "IdleTimeAccelerationAlarmSetting";
        private const string PhoneNumberFavoriteContactSettingKeyName = "PhoneNumberFavoriteContactSetting";      

        // Default values of our settings
        private const bool InitialLaunchSettingDefault = true;
        private const string LastUpdatedDefault = null;

        private const double AccelerationAlarmSettingDefault = 1.5;
        private const uint IdleTimeAccelerationAlarmSettingDefault = 3000;
        private const string PhoneNumberFavoriteContactSettingDefault = null;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor that gets the application settings.
        /// </summary>
        public ApplicationSettingsData()
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
        private static bool AddOrUpdateValue(string Key, Object value)
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
        private static valueType GetValueOrDefault<valueType>(string Key, valueType defaultValue)
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
        private static void Save()
        {
            _isolatedStore.Save();
        }


        /// <summary>
        /// Setting that determines whether application 
        /// </summary>
        public static bool InitialLaunchSetting
        {
            get
            {
                return GetValueOrDefault<bool>(InitialLaunchSettingKeyName, InitialLaunchSettingDefault);
            }
            set
            {
                AddOrUpdateValue(InitialLaunchSettingKeyName, value);
                Save();
            }
        }

        /// <summary>
        /// Setting storing the Last Updated Time
        /// </summary>
        public static string LastUpdatedTime
        {
            get
            {
                return GetValueOrDefault<string>(LastUpdatedTimeKeyName, LastUpdatedDefault);
            }
            set
            {
                AddOrUpdateValue(LastUpdatedTimeKeyName, value);
                Save();
            }
        }

        /// <summary>
        /// Setting storing the Acceleration Alarm Setting
        /// </summary>
        public static double AccelerationAlarmSetting
        {
            get
            {
                return GetValueOrDefault<double>(AccelerationAlarmSettingKeyName, AccelerationAlarmSettingDefault);
            }
            set
            {
                AddOrUpdateValue(AccelerationAlarmSettingKeyName, value);
                Save();
            }
        }

        /// <summary>
        /// Setting storing the Idle Time of Acceleration Setting
        /// </summary>
        public static uint IdleTimeAccelerationAlarmSetting
        {
            get
            {
                return GetValueOrDefault<uint>(IdleTimeAccelerationAlarmSettingKeyName, IdleTimeAccelerationAlarmSettingDefault);
            }
            set
            {
                AddOrUpdateValue(IdleTimeAccelerationAlarmSettingKeyName, value);
                Save();
            }
        }

        /// <summary>
        /// Setting storing the Phone Number of Favorite Contact Setting
        /// </summary>
        public static string PhoneNumberFavoriteContactSetting
        {
            get
            {
                return GetValueOrDefault<string>(PhoneNumberFavoriteContactSettingKeyName, PhoneNumberFavoriteContactSettingDefault);
            }
            set
            {
                AddOrUpdateValue(PhoneNumberFavoriteContactSettingKeyName, value);
                Save();
                VMLocator prueba = new VMLocator();
                prueba.MainViewModel.PhoneNumberFavoriteContactSetting2 = value;               
            }
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                });

            }
        }
    }
}
