using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ApiNominas.Utils
{
    public class PropertyChanged
    {
        public event PropertyChangedEventHandler myPropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            myPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}