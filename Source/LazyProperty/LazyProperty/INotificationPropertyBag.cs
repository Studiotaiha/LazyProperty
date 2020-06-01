using System.ComponentModel;

namespace LazyProperty
{
	public interface INotificationPropertyBag : IPropertyBag, INotifyPropertyChanging, INotifyPropertyChanged
	{
	}
}
