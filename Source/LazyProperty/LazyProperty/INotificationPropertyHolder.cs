using System.ComponentModel;

namespace LazyProperty
{
	public interface INotificationPropertyHolder : IPropertyHolder, INotifyPropertyChanging, INotifyPropertyChanged
	{
	}
}
