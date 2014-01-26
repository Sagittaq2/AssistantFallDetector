using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AssistantFallDetector.ViewModels.Base
{
    public class VMBase : INotifyPropertyChanged
    {
        public VMBase()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Trigger PropertyChanged event with the property modified
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Refresh all view values bound with the ViewModel
        /// </summary>
        public void UpdateAll()
        {
            RaisePropertyChanged(String.Empty);
        }
    }
}
