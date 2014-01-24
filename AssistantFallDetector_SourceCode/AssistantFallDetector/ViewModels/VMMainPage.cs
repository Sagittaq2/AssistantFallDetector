using AssistantFallDetector.Entities;
using AssistantFallDetector.Services;
using AssistantFallDetector.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using System.ServiceModel;
using Windows.UI.Core;
using System.Collections.ObjectModel;
using Microsoft.Devices;
using System.IO.IsolatedStorage;
using Microsoft.Phone.UserData;

namespace AssistantFallDetector.ViewModels
{
    public class VMMainPage : VMBase
    {
        private INavigationService navService;

        private IApplicationSettingsService applicationSettingsService;
        private ApplicationSettingsData applicationSettingsData;

        private IAccelerometerService accelerometerService;
        private IDispatcherService dispatcherService;
        private AccelerometerData accelerometerData;
        private AccelerometerMaxData accelerometerMaxData;
        private AccelerometerGraphData accelerometerGraphData;
        
        private ObservableCollection<string> coordinates = new ObservableCollection<string>();
        private IGpsService gpsService;

        private ISmsService smsService;
        private SmsData smsData;

        private ObservableCollection<Contact> contactsData = new ObservableCollection<Contact>();
        private string stateContactsSearch = Resources.AppResources.MainPageContactsResultsLabelText;

        private VibrateController iVibrateController;

        private double AXAxisMax = 0;
        private double AYAxisMax = 0;
        private double AZAxisMax = 0;
        private double AAccelerationMax = 0;

        private string stateAlarm = Resources.AppResources.MainPageAccelerometerStateAlarmText1;

        private double LA = -0.5;
        private double HA = 0.5;

        private TimeSpan IdleTime;
        private DateTime StartedTimeStamp;
        private bool StartedAlarmAck = false;

        private bool popup;

        private DelegateCommand ackAlarmCommand;
        private DelegateCommand sendNotificationAlarmCommand;
        private Lazy<DelegateCommand<string>> searchContactsCommand;
        private Lazy<DelegateCommand<object>> navigateToContactDetailsPageCommand;

        private ContactPhoneNumber telefono;

        public VMMainPage(INavigationService navService, IApplicationSettingsService applicationSettingsService, IAccelerometerService accelerometerService, IGpsService gpsService, ISmsService smsService, IDispatcherService dispatcherService)
        {
            this.navService = navService;
            this.applicationSettingsService = applicationSettingsService;
            this.accelerometerService = accelerometerService;
            this.gpsService = gpsService;
            this.smsService = smsService;
            this.dispatcherService = dispatcherService;

            this.accelerometerService.AccelerometerReadingChanged += accelerometerService_AccelerometerReadingChanged;
            this.gpsService.GpsPositionChanged += gpsService_GpsPositionChanged;
           
            Popup = false;

            ackAlarmCommand = new DelegateCommand(AckAlarmCommandExecute, AckAlarmCommandCanExecute);
            sendNotificationAlarmCommand = new DelegateCommand(SendNotificationAlarmCommandExecute, SendNotificationAlarmCommandCanExecute);
            searchContactsCommand = new Lazy<DelegateCommand<string>>(
                () =>
                    new DelegateCommand<string>(SearchContactsCommandExecute, SearchContactsCommandCanExecute)
            );

            navigateToContactDetailsPageCommand = new Lazy<DelegateCommand<object>>(
                () =>
                    new DelegateCommand<object>(NavigateToContactDetailsPageCommandExecute, NavigateToContactDetailsPageCommandCanExecute)
            );

            iVibrateController = VibrateController.Default;
            
            try
            {
                this.applicationSettingsService.LoadApplicationSettings();
                this.applicationSettingsData = new ApplicationSettingsData();
                this.applicationSettingsData.AccelerationAlarmSetting = applicationSettingsService.GetValueOrDefault(this.applicationSettingsData.AccelerationAlarmSettingKeyName, this.applicationSettingsData.AccelerationAlarmSettingDefault);
                this.applicationSettingsData.IdleTimeAccelerationAlarmSetting = applicationSettingsService.GetValueOrDefault(this.applicationSettingsData.IdleTimeAccelerationAlarmSettingKeyName, this.applicationSettingsData.IdleTimeAccelerationAlarmSettingDefault);
                this.applicationSettingsData.InitialLaunchSetting = applicationSettingsService.GetValueOrDefault(this.applicationSettingsData.InitialLaunchSettingKeyName, this.applicationSettingsData.InitialLaunchSettingDefault);
                this.applicationSettingsData.LastUpdatedTimeSetting = applicationSettingsService.GetValueOrDefault(this.applicationSettingsData.LastUpdatedTimeSettingKeyName, this.applicationSettingsData.LastUpdatedTimeSettingDefault);
                this.applicationSettingsData.PhoneNumberFavoriteContactSetting = applicationSettingsService.GetValueOrDefault(this.applicationSettingsData.PhoneNumberFavoriteContactSettingKeyName, this.applicationSettingsData.PhoneNumberFavoriteContactSettingDefault);

                this.accelerometerService.InitializeAccelerometer();

                this.accelerometerMaxData = new AccelerometerMaxData();
                this.accelerometerGraphData = new AccelerometerGraphData();
                
            }
            catch (NotSupportedException exception)
            {
                this.accelerometerService.AccelerometerReadingChanged -= accelerometerService_AccelerometerReadingChanged;
                MessageBox.Show(exception.Message);
            }

            try
            {
                this.GetPositionGPS();
            }
            catch (NotSupportedException exception)
            {
                this.gpsService.GpsPositionChanged -= gpsService_GpsPositionChanged;
                MessageBox.Show(exception.Message);
            }

        }

        void accelerometerService_AccelerometerReadingChanged(AccelerometerData accelerometerData)
        {
            this.dispatcherService.CallDispatcher(() =>
            {
                AData = accelerometerData;
                AMaxData = accelerometerMaxData;

                if (Math.Abs(AData.XAxis) > Math.Abs(AXAxisMax))
                {
                    AMaxData.XAxisMax = AData.XAxis;
                    AXAxisMax = AData.XAxis;
                }

                if (Math.Abs(AData.YAxis) > Math.Abs(AYAxisMax))
                {
                    AMaxData.YAxisMax = AData.YAxis;
                    AYAxisMax = AData.YAxis;
                }

                if (Math.Abs(AData.ZAxis) > Math.Abs(AZAxisMax))
                {
                    AMaxData.ZAxisMax = AData.ZAxis;
                    AZAxisMax = AData.ZAxis;
                }

                if (AData.Acceleration > AAccelerationMax)
                {
                    AMaxData.AccelerationMax = AData.Acceleration;
                    AAccelerationMax = AData.Acceleration;
                }               

                accelerometerGraphData.XLineX2 = accelerometerGraphData.XLineX1 + accelerometerData.XAxis * 100;
                accelerometerGraphData.YLineY2 = accelerometerGraphData.YLineY1 - accelerometerData.YAxis * 100;
                accelerometerGraphData.ZLineX2 = accelerometerGraphData.ZLineX1 - accelerometerData.ZAxis * 50;
                accelerometerGraphData.ZLineY2 = accelerometerGraphData.ZLineY1 + accelerometerData.ZAxis * 50;
                AGraphData = accelerometerGraphData;

                if ((AData.Acceleration > applicationSettingsData.AccelerationAlarmSetting) && (!StartedAlarmAck))
                {
                    //ToDo: Tiempo que debe permanecer en reposo antes de dar la alarma.
                    StartedTimeStamp = DateTime.Now;
                    StartedAlarmAck = true;
                    StateAlarm = Resources.AppResources.MainPageAccelerometerStateAlarmText2;
                }
                else
                {
                    if (StartedAlarmAck)
                    {
                        if ((LA < AData.Acceleration) && (AData.Acceleration < HA))
                        {
                            IdleTime = (DateTime.Now - StartedTimeStamp);
                            if (IdleTime.TotalMilliseconds > applicationSettingsData.IdleTimeAccelerationAlarmSetting)
                            {
                                StateAlarm = Resources.AppResources.MainPageAccelerometerStateAlarmText3;
                                Popup = true;
                                iVibrateController.Start(TimeSpan.FromSeconds(1));
                                StartedAlarmAck = false;
                            }
                        }
                        else
                        {
                            if ((DateTime.Now - StartedTimeStamp).TotalMilliseconds > 500)
                            {
                                StartedAlarmAck = false;
                                StateAlarm = Resources.AppResources.MainPageAccelerometerStateAlarmText1;
                            }
                        }
                    }
                }


            });
        }

        public AccelerometerData AData
        {
            get { return this.accelerometerData; }
            set
            {
                this.accelerometerData = value;
                RaisePropertyChanged();
            }
        }

        public AccelerometerMaxData AMaxData
        {
            get { return this.accelerometerMaxData; }
            set
            {
                this.accelerometerMaxData = value;
                RaisePropertyChanged();
            }
        }

        public AccelerometerGraphData AGraphData
        {
            get { return this.accelerometerGraphData; }
            set
            {
                this.accelerometerGraphData = value;
                RaisePropertyChanged();
            }
        }

        public string StateAlarm
        {
            get { return this.stateAlarm; }
            set
            {
                this.stateAlarm = value;
                RaisePropertyChanged();
            }
        }

        public bool Popup
        {
            get { return this.popup; }
            set
            {
                this.popup = value;
                RaisePropertyChanged();
            }
        }

        public double AccelerationAlarmSetting
        {
            get { return applicationSettingsData.AccelerationAlarmSetting; }
            set
            {
                applicationSettingsData.AccelerationAlarmSetting = value;
                RaisePropertyChanged();
                applicationSettingsService.AddOrUpdateValue(applicationSettingsData.AccelerationAlarmSettingKeyName, value);
            }
        }

        public uint IdleTimeAccelerationAlarmSetting
        {
            get { return applicationSettingsData.IdleTimeAccelerationAlarmSetting; }
            set
            {
                applicationSettingsData.IdleTimeAccelerationAlarmSetting = value;
                RaisePropertyChanged();
                applicationSettingsService.AddOrUpdateValue(applicationSettingsData.IdleTimeAccelerationAlarmSettingKeyName, value);
            }
        }

        public string PhoneNumberFavoriteContactSetting
        {
            get { return applicationSettingsData.PhoneNumberFavoriteContactSetting; }
            set
            {
                applicationSettingsData.PhoneNumberFavoriteContactSetting = value;
                RaisePropertyChanged();
                applicationSettingsService.AddOrUpdateValue(applicationSettingsData.PhoneNumberFavoriteContactSettingKeyName, value);
            }
        }

        public ICommand AckAlarmCommand
        {
            get { return ackAlarmCommand; }
        }

        public void AckAlarmCommandExecute()
        {
            Popup = false;
            StateAlarm = Resources.AppResources.MainPageAccelerometerStateAlarmText1;
        }

        public bool AckAlarmCommandCanExecute()
        {
            return true;
        }

        public ICommand SendNotificationAlarmCommand
        {
            get { return sendNotificationAlarmCommand; }
        }

        public void SendNotificationAlarmCommandExecute()
        {
            Popup = false;
            StateAlarm = Resources.AppResources.MainPageAccelerometerStateAlarmText1;

            smsData = new SmsData();
            smsData.Number = applicationSettingsData.PhoneNumberFavoriteContactSetting;
            smsData.Text = Resources.AppResources.NotificationAlarmText;
            var result = this.smsService.SendSMS(smsData);

        }

        public bool SendNotificationAlarmCommandCanExecute()
        {
            return true;
        }

        public ObservableCollection<string> Coordinates
        {
            get { return coordinates; }
            set
            {
                coordinates = value;
                RaisePropertyChanged();
            }
        }

        private async void GetPositionGPS()
        {
            var result = await this.gpsService.GetGpsCoordinates();

            if (result != null)
            {
                string coordinates = string.Format("{0}:\n    {1} , {2}",
                                                        result.Timestamp.ToUniversalTime(),
                                                        result.Latitude,
                                                        result.Longitude);
                Coordinates.Add(coordinates);
            }
        }

        private void gpsService_GpsPositionChanged(Geocoordinate coord)
        {
            if (coord != null)
            {
                this.dispatcherService.CallDispatcher(() =>
                {
                    string coordinates = string.Format("{0}:\n    {1} , {2}",
                                                        coord.Timestamp.ToUniversalTime(),
                                                        coord.Latitude,
                                                        coord.Longitude);

                    Coordinates.Add(coordinates);
                });
            }
        }

        public ICommand SearchContactsCommand
        {
            get { return searchContactsCommand.Value; }
        }

        public void SearchContactsCommandExecute(string param)
        {
            StateContactsSearch = Resources.AppResources.MainPageContactsLoadingLabelText1;

            FilterKind contactFilterKind = FilterKind.None;
            
            contactFilterKind = FilterKind.DisplayName;
            
            Contacts cons = new Contacts();

            cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);

            cons.SearchAsync(param, contactFilterKind, "Contacts Test #1");
        }

        void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            if (e.Results.Count<Contact>() > 0)
            {
                ContactsData = new ObservableCollection<Contact>(e.Results);

                StateContactsSearch = Resources.AppResources.MainPageContactsLoadingLabelText2;
            }
            else
            {
                StateContactsSearch = Resources.AppResources.MainPageContactsLoadingLabelText3;
            }

        }

        public bool SearchContactsCommandCanExecute(string param)
        {
            return true;
        }

        public ObservableCollection<Contact> ContactsData
        {
            get
            {
                return this.contactsData;
            }
            set
            {
                contactsData = value;
                RaisePropertyChanged();
            }
        }

        public string StateContactsSearch
        {
            get { return this.stateContactsSearch; }
            set
            {
                this.stateContactsSearch = value;
                RaisePropertyChanged();
            }
        }

        public ICommand NavigateToContactDetailsPageCommand
        {
            get { return navigateToContactDetailsPageCommand.Value; }
        }

        public void NavigateToContactDetailsPageCommandExecute(object contacto)
        {
            this.navService.NavigateToContactDetailsPage(contacto);
        }

        public bool NavigateToContactDetailsPageCommandCanExecute(object contacto)
        {
            return true;
        }

        public ContactPhoneNumber Telefono
        {
            get
            {
                return telefono;
            }
            set
            {
                telefono = value;
                PhoneNumberFavoriteContactSetting = telefono.PhoneNumber;
                //RaisePropertyChanged();
            }
        }

    }
}
