using System;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using AssistantFallDetector.Entities;
using System.Windows.Controls;
using System.Windows;
using AssistantFallDetector.Resources;

namespace AssistantFallDetector
{
    public partial class ContactDetailsPage : PhoneApplicationPage
    {
        public ContactDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        }
    }
}
