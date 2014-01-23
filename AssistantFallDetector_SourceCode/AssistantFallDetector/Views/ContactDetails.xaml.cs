using System;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using AssistantFallDetector.Entities;
using System.Windows.Controls;
using System.Windows;
using AssistantFallDetector.Resources;

namespace AssistantFallDetector
{
    public partial class ContactDetails : PhoneApplicationPage
    {
        public ContactDetails()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //Set the data context for this page to the selected contact
            this.DataContext = App.con;

            try
            {
                //Try to get a picture of the contact
                BitmapImage img = new BitmapImage();
                img.SetSource(App.con.GetPicture());
                Picture.Source = img;
            }
            catch (Exception)
            {
                //can't get a picture of the contact
            }
        }

        private void PhoneNumberContact_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock PhoneNumberFavoriteContact = sender as TextBlock;
            ApplicationSettingsData.PhoneNumberFavoriteContactSetting = PhoneNumberFavoriteContact.Text;            

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = AppResources.ContactDetailsPageDialogCaption,
                Message = AppResources.ContactDetailsPageDialogMessage1 + " " + PhoneNumberFavoriteContact.Text + " "
                + AppResources.ContactDetailsPageDialogMessage2 + " \"" + App.con.DisplayName + "\" "
                + AppResources.ContactDetailsPageDialogMessage3,
                LeftButtonContent = AppResources.ContactDetailsPageDialogLeftButtonText
                
            };

            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        // Do something.
                        break;
                    case CustomMessageBoxResult.RightButton:
                        // Do something.
                        break;
                    case CustomMessageBoxResult.None:
                        // Do something.
                        break;
                    default:
                        break;
                }
            };

            messageBox.Show();
        }
    }
}
