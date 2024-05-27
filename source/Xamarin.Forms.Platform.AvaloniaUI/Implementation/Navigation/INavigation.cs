namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Navigation;

public interface INavigation
{
    int StackDepth { get; }

    void InsertPageBefore(object page, object before);

    void Pop();

    void Pop(bool animated);

    void PopModal();

    void PopModal(bool animated);

    void PopToRoot();

    void PopToRoot(bool animated);

    void Push(object page);

    void Push(object page, bool animated);

    void PushModal(object page);

    void PushModal(object page, bool animated);

    void RemovePage(object page);
}