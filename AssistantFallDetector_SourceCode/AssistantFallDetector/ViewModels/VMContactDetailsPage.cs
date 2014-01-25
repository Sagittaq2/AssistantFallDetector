using AssistantFallDetector.Services;
using AssistantFallDetector.ViewModels.Base;
using System;
using System.Windows.Input;
using Microsoft.Phone.UserData;
using Microsoft.Phone.Controls;

namespace AssistantFallDetector.ViewModels
{
    public class VMContactDetailsPage : VMBase
    {
        private INavigationService navService;

        private Contact contact;

        private Lazy<DelegateCommand<object>> navigateToMainPageCommand;

        public VMContactDetailsPage(INavigationService navService)
        {
            this.navService = navService;

            navigateToMainPageCommand = new Lazy<DelegateCommand<object>>(
                () =>
                    new DelegateCommand<object>(NavigateToMainPageCommandExecute, NavigateToMainPageCommandCanExecute)
            );
        }

        public Contact Contact
        {
            get
            {
                return contact;
            }
            set
            {
                contact = value;
                RaisePropertyChanged();
            }
        }

        public ICommand NavigateToMainPageCommand
        {
            get { return navigateToMainPageCommand.Value; }
        }

        public void NavigateToMainPageCommandExecute(object telefono)
        {
            ContactPhoneNumber telef = (ContactPhoneNumber)telefono;

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = Resources.AppResources.ContactDetailsPageDialogCaption,
                Message = Resources.AppResources.ContactDetailsPageDialogMessage1 + " " + telef.PhoneNumber + " "
                + Resources.AppResources.ContactDetailsPageDialogMessage2 + " \"" + Contact.DisplayName + "\" "
                + Resources.AppResources.ContactDetailsPageDialogMessage3,
                LeftButtonContent = Resources.AppResources.ContactDetailsPageDialogLeftButtonText

            };

            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        // Do something.
                        this.navService.NavigateToMainPage(telefono);
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

        public bool NavigateToMainPageCommandCanExecute(object telefono)
        {
            return true;
        }

    }
}
