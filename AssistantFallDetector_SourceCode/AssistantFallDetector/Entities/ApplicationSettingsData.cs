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
        // Isolated storage key names
        private string initialLaunchSettingKeyName = "InitialLaunchSetting";
        private string lastUpdatedTimeSettingKeyName = "LastUpdatedTime";
        private string accelerationAlarmSettingKeyName = "AccelerationAlarmSetting";
        private string idleTimeAccelerationAlarmSettingKeyName = "IdleTimeAccelerationAlarmSetting";
        private string phoneNumberFavoriteContactSettingKeyName = "PhoneNumberFavoriteContactSetting";      

        // Default values of our settings
        private bool initialLaunchSettingDefault = true;
        private string lastUpdatedTimeSettingDefault = null;
        private double accelerationAlarmSettingDefault = 1.5;
        private uint idleTimeAccelerationAlarmSettingDefault = 3000;
        private string phoneNumberFavoriteContactSettingDefault = null;

        // Isolated storage key names
        private bool initialLaunchSetting;
        private string lastUpdatedTimeSetting;
        private double accelerationAlarmSetting;
        private uint idleTimeAccelerationAlarmSetting;
        private string phoneNumberFavoriteContactSetting;  


        public ApplicationSettingsData()
        {
        }

        public string InitialLaunchSettingKeyName
        { 
            get { return initialLaunchSettingKeyName; }
            set { initialLaunchSettingKeyName=value; }
        }

        public string LastUpdatedTimeSettingKeyName
        {
            get { return lastUpdatedTimeSettingKeyName; }
            set { lastUpdatedTimeSettingKeyName = value; }
        }

        public string AccelerationAlarmSettingKeyName
        {
            get { return accelerationAlarmSettingKeyName; }
            set { accelerationAlarmSettingKeyName = value; }
        }

        public string IdleTimeAccelerationAlarmSettingKeyName
        {
            get { return idleTimeAccelerationAlarmSettingKeyName; }
            set { idleTimeAccelerationAlarmSettingKeyName = value; }
        }

        public string PhoneNumberFavoriteContactSettingKeyName
        {
            get { return phoneNumberFavoriteContactSettingKeyName; }
            set { phoneNumberFavoriteContactSettingKeyName = value; }
        }


        public bool InitialLaunchSettingDefault
        {
            get { return initialLaunchSettingDefault; }
            set { initialLaunchSettingDefault = value; }
        }

        public string LastUpdatedTimeSettingDefault
        {
            get { return lastUpdatedTimeSettingDefault; }
            set { lastUpdatedTimeSettingDefault = value; }
        }

        public double AccelerationAlarmSettingDefault
        {
            get { return accelerationAlarmSettingDefault; }
            set { accelerationAlarmSettingDefault = value; }
        }

        public uint IdleTimeAccelerationAlarmSettingDefault
        {
            get { return idleTimeAccelerationAlarmSettingDefault; }
            set { idleTimeAccelerationAlarmSettingDefault = value; }
        }

        public string PhoneNumberFavoriteContactSettingDefault
        {
            get { return phoneNumberFavoriteContactSettingDefault; }
            set { phoneNumberFavoriteContactSettingDefault = value; }
        }


        public bool InitialLaunchSetting
        {
            get { return initialLaunchSetting; }
            set { initialLaunchSetting = value; }
        }

        public string LastUpdatedTimeSetting
        {
            get { return lastUpdatedTimeSetting; }
            set { lastUpdatedTimeSetting = value; }
        }

        public double AccelerationAlarmSetting
        {
            get { return accelerationAlarmSetting; }
            set { accelerationAlarmSetting = value; }
        }

        public uint IdleTimeAccelerationAlarmSetting
        {
            get { return idleTimeAccelerationAlarmSetting; }
            set { idleTimeAccelerationAlarmSetting = value; }
        }

        public string PhoneNumberFavoriteContactSetting
        {
            get { return phoneNumberFavoriteContactSetting; }
            set { phoneNumberFavoriteContactSetting = value; }
        }
    }
}
