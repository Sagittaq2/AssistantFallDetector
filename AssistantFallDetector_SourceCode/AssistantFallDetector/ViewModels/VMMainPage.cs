using AssistantFallDetector.Models;
using AssistantFallDetector.Services;
using AssistantFallDetector.ViewModels.Base;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using System.Collections.ObjectModel;
using Microsoft.Devices;
using Microsoft.Phone.UserData;
using Microsoft.Phone.Tasks;
using System.Windows.Controls;

namespace AssistantFallDetector.ViewModels
{
    public class VMMainPage : VMBase
    {
        private IDispatcherService dispatcherService;

        private INavigationService navService;

        private IApplicationSettingsService applicationSettingsService;
        private ApplicationSettingsData applicationSettingsData;

        private IAccelerometerService accelerometerService;
        private AccelerometerData accelerometerData;
        private AccelerometerMaxData accelerometerMaxData;
        private AccelerometerGraphData accelerometerGraphData;
        
        private ObservableCollection<string> coordinates = new ObservableCollection<string>();
        private IGpsService gpsService;
        private GpsData gpsData;

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

        private bool popup = false;

        private DelegateCommand ackAlarmCommand;
        private DelegateCommand sendNotificationAlarmCommand;
        private DelegateCommand showLocationMapCommand;
        private Lazy<DelegateCommand<string>> searchContactsCommand;
        private Lazy<DelegateCommand<object>> navigateToContactDetailsPageCommand;
        private DelegateCommand aboutCommand;

        private ContactPhoneNumber telefono;

        private Uri alarmAudio;

        public VMMainPage(INavigationService navService, IApplicationSettingsService applicationSettingsService, IAccelerometerService accelerometerService, IGpsService gpsService, ISmsService smsService, IDispatcherService dispatcherService)
        {
            // Enlazamos a los servicios
            this.navService = navService;
            this.applicationSettingsService = applicationSettingsService;
            this.accelerometerService = accelerometerService;
            this.gpsService = gpsService;
            this.smsService = smsService;
            this.dispatcherService = dispatcherService;

            // Registramos los eventos del acelerómetro y del GPS
            this.accelerometerService.AccelerometerReadingChanged += accelerometerService_AccelerometerReadingChanged;
            this.gpsService.GpsPositionChanged += gpsService_GpsPositionChanged;
           
            //Popup = false;

            // Creamos los comandos que se van a usar
            ackAlarmCommand = new DelegateCommand(AckAlarmCommandExecute, AckAlarmCommandCanExecute);
            sendNotificationAlarmCommand = new DelegateCommand(SendNotificationAlarmCommandExecute, SendNotificationAlarmCommandCanExecute);
            showLocationMapCommand = new DelegateCommand(ShowLocationMapCommandExecute, ShowLocationMapCommandCanExecute);
            searchContactsCommand = new Lazy<DelegateCommand<string>>(
                () =>
                    new DelegateCommand<string>(SearchContactsCommandExecute, SearchContactsCommandCanExecute)
            );
            navigateToContactDetailsPageCommand = new Lazy<DelegateCommand<object>>(
                () =>
                    new DelegateCommand<object>(NavigateToContactDetailsPageCommandExecute, NavigateToContactDetailsPageCommandCanExecute)
            );
            aboutCommand = new DelegateCommand(AboutCommandExecute, AboutCommandCanExecute);

            iVibrateController = VibrateController.Default;

            //Se cargan todos los parámetro de configuración
            this.applicationSettingsService.LoadApplicationSettings();
            this.applicationSettingsData = new ApplicationSettingsData();
            this.applicationSettingsData.AccelerationAlarmSetting = applicationSettingsService.GetValueOrDefault(this.applicationSettingsData.AccelerationAlarmSettingKeyName, this.applicationSettingsData.AccelerationAlarmSettingDefault);
            this.applicationSettingsData.IdleTimeAccelerationAlarmSetting = applicationSettingsService.GetValueOrDefault(this.applicationSettingsData.IdleTimeAccelerationAlarmSettingKeyName, this.applicationSettingsData.IdleTimeAccelerationAlarmSettingDefault);
            this.applicationSettingsData.InitialLaunchSetting = applicationSettingsService.GetValueOrDefault(this.applicationSettingsData.InitialLaunchSettingKeyName, this.applicationSettingsData.InitialLaunchSettingDefault);
            this.applicationSettingsData.LastUpdatedTimeSetting = applicationSettingsService.GetValueOrDefault(this.applicationSettingsData.LastUpdatedTimeSettingKeyName, this.applicationSettingsData.LastUpdatedTimeSettingDefault);
            this.applicationSettingsData.PhoneNumberFavoriteContactSetting = applicationSettingsService.GetValueOrDefault(this.applicationSettingsData.PhoneNumberFavoriteContactSettingKeyName, this.applicationSettingsData.PhoneNumberFavoriteContactSettingDefault);
            this.applicationSettingsData.OrientationPortraitSetting = applicationSettingsService.GetValueOrDefault(this.applicationSettingsData.OrientationPortraitSettingKeyName, this.applicationSettingsData.OrientationPortraitSettingDefault);

            //Acelerómetro
            try
            {
                //Se inicializa el acelerómetro y los modelos asociados
                this.accelerometerService.InitializeAccelerometer();
                this.accelerometerMaxData = new AccelerometerMaxData();
                this.accelerometerGraphData = new AccelerometerGraphData();             
            }
            catch (NotSupportedException exception)
            {
                this.accelerometerService.AccelerometerReadingChanged -= accelerometerService_AccelerometerReadingChanged;
                MessageBox.Show(exception.Message);
            }

            //GPS
            try
            {
                // Se inicializa el GPS
                this.gpsData = new GpsData();

                //Recogemos la posición actual del dispositivo
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

                //ALGORITMO PARA DETECTAR CAÍDAS
                //==============================
                //Se evalúa si la aceleración actual sobrepasa la configurada en la aplicación y también si ya hay en proceso una identificación de caída
                if ((AData.Acceleration > applicationSettingsData.AccelerationAlarmSetting) && (!StartedAlarmAck))
                {
                    //Si se detecta una posible caída se almacena el tiempo en que empieza la identificación 
                    //y se marca como inicio de identificación de caída.
                    StartedTimeStamp = DateTime.Now;
                    StartedAlarmAck = true;
                    StateAlarm = Resources.AppResources.MainPageAccelerometerStateAlarmText2;
                }
                else
                {
                    //Se evalúa si ya hay iniciado un proceso de posible caída aunque no se haya sobrepasado el umbral en esta lectura
                    if (StartedAlarmAck)
                    {
                        //Si ya hay iniciado un proceso revisamos si la aceleración actual está dentro de umbrales LA y HA 
                        //de esta manera podemos detectar que hay aceleraciones del dispositivo pero que son despreciables y está parado
                        if ((LA < AData.Acceleration) && (AData.Acceleration < HA))
                        {
                            IdleTime = (DateTime.Now - StartedTimeStamp);
                            //Se evalúa si desde que se sobrepasó el umbral si ha pasado el tiempo configurado para poder reconocer la alarma por parte el usuari
                            if (IdleTime.TotalMilliseconds > applicationSettingsData.IdleTimeAccelerationAlarmSetting)
                            {
                                //Si se ha sobrepasado el tiempo y el dispositivo no ha sufrido aceleraciones perceptibles significa que está parado.
                                //SE DETECTA UNA CAÍDA
                                StateAlarm = Resources.AppResources.MainPageAccelerometerStateAlarmText3;
                                //Se muestra una pantalla al usuario para que pueda reconocer la alarma y cancelar el envío de notificación al contacto favorito
                                Popup = true;
                                iVibrateController.Start(TimeSpan.FromSeconds(1));

                                // HACK for MediaElement: to force it to play a new source, set source to null then put the real source URI. 
                                AlarmAudio = null;
                                AlarmAudio = new Uri("Assets/Bocina.mp3", UriKind.Relative);

                                StartedAlarmAck = false;
                            }
                        }
                        else
                        {
                            //Miramos si han pasado 500ms desde que se detectó la caída, para evitar falsos positivos por aceleraciones extras.                            
                            if ((DateTime.Now - StartedTimeStamp).TotalMilliseconds > 500)
                            {
                                //Si han pasado más de 500ms y el dispositivo ha tenido una aceleración apreciable fuera de los umbrales LA y HA 
                                //significa que el dispositivo no está parado por lo que cancelamos el proceso de detección de la caída.
                                //SE CANCELA LA CAÍDA
                                StartedAlarmAck = false;
                                StateAlarm = Resources.AppResources.MainPageAccelerometerStateAlarmText1;
                            }
                        }
                    }
                }


            });
        }

        //Propiedades relacionadas con el acelerómetro enlazadas a la vista
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

        //Propiedades relacionadas con los ajustes enlazadas a la vista
        public double AccelerationAlarmSetting
        {
            get { return applicationSettingsData.AccelerationAlarmSetting; }
            set
            {
                applicationSettingsData.AccelerationAlarmSetting = value;
                
                if (applicationSettingsService.AddOrUpdateValue(applicationSettingsData.AccelerationAlarmSettingKeyName, value))
                {
                    RaisePropertyChanged();
                    applicationSettingsService.Save();
                }
            }
        }

        public uint IdleTimeAccelerationAlarmSetting
        {
            get { return applicationSettingsData.IdleTimeAccelerationAlarmSetting; }
            set
            {
                applicationSettingsData.IdleTimeAccelerationAlarmSetting = value;

                if (applicationSettingsService.AddOrUpdateValue(applicationSettingsData.IdleTimeAccelerationAlarmSettingKeyName, value))
                {
                    RaisePropertyChanged();
                    applicationSettingsService.Save();
                }
            }
        }

        public string PhoneNumberFavoriteContactSetting
        {
            get { return applicationSettingsData.PhoneNumberFavoriteContactSetting; }
            set
            {
                applicationSettingsData.PhoneNumberFavoriteContactSetting = value;
                
                if (applicationSettingsService.AddOrUpdateValue(applicationSettingsData.PhoneNumberFavoriteContactSettingKeyName, value))
                {
                    RaisePropertyChanged();
                    applicationSettingsService.Save();
                }
            }
        }

        public bool OrientationPortraitSetting
        {
            get { return applicationSettingsData.OrientationPortraitSetting; }
            set
            {
                applicationSettingsData.OrientationPortraitSetting = value;

                if (applicationSettingsService.AddOrUpdateValue(applicationSettingsData.OrientationPortraitSettingKeyName, value))
                {
                    RaisePropertyChanged();
                    applicationSettingsService.Save();
                }
            }
        }

        //Comando para reconocer una alarma
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

        //Comando para enviar la notificación al contacto favorito
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
            smsData.Text = Resources.AppResources.NotificationAlarmText + "\n" + gpsData.ToString();
            var result = this.smsService.SendSMS(smsData);

        }

        public bool SendNotificationAlarmCommandCanExecute()
        {
            return true;
        }

        //Propiedades relacionadas con el GPS enlazadas a la vista
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

                gpsData.SetGpsData(result.Timestamp.ToUniversalTime(), result.Latitude, result.Longitude);
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

                    gpsData.SetGpsData(coord.Timestamp.ToUniversalTime(), coord.Latitude, coord.Longitude);
                });
            }
        }

        //Comando para visualizar las coordenadas en la aplicación de mapas de WP8
        public ICommand ShowLocationMapCommand
        {
            get { return showLocationMapCommand; }
        }

        public void ShowLocationMapCommandExecute()
        {
            MapsTask map = new MapsTask();
            map.ZoomLevel = 15;            
            map.Center = gpsData.GetGpsData();
            map.Show();           
        }

        public bool ShowLocationMapCommandCanExecute()
        {
                return true;
        }

        //Comando para buscar contactos en la agenda del dispositivo
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

        //Propiedades relacionadas con la búsqueda de contactos enlazadas a la vista
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

        //Comando para navegar a la página de detalles del contacto con el contacto seleccionado
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

        //Propiedad con el teléfono del contacto favorito que se rellena desde VMContactDetails al navegar hacia la página principal
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
            }
        }

        //Comando para mostrar la versión de la aplicación
        public ICommand AboutCommand
        {
            get { return aboutCommand; }
        }

        public void AboutCommandExecute()
        {
            //String appVersion = System.Reflection.Assembly.GetExecutingAssembly()
            //        .FullName.Split('=')[1].Split(',')[0];
            System.Version version = new System.Reflection.AssemblyName(System.Reflection.Assembly.GetExecutingAssembly().FullName).Version;

            string appName = Resources.AppResources.ApplicationTitle;
            string author = "David Eiroa Menasalvas";

            MessageBox.Show(appName + "\n\n" 
                + "version: " + version.ToString() + "\n\n" 
                + author);

        }

        public bool AboutCommandCanExecute()
        {
            return true;
        }

        //Property to play audio file
        public Uri AlarmAudio
        {
            get { return this.alarmAudio; }
            private set
            {
                if (this.alarmAudio != value)
                {
                    this.alarmAudio = value;
                    this.RaisePropertyChanged();
                }
            }
        }

    }
}
