namespace AGInterfaces;

public interface IContextMenuContainer
{
	event CustomActionPopupDelegate OnPopup;

	void Add(params CustomActionItem[] items);

	void Remove(params CustomActionItem[] items);
}
