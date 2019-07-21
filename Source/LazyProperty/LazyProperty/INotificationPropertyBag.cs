using System.ComponentModel;

namespace Studiotaiha.LazyProperty
{
	public interface INotificationPropertyBag : IPropertyBag, INotifyPropertyChanging, INotifyPropertyChanged
	{
	}
}
