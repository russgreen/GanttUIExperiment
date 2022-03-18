using System.ComponentModel;

namespace CoreLibrary.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

    }
}