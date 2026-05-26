namespace AGInterfaces;

public interface IAsyncProgressPopup
{
	int Maximum { get; set; }

	int Value { get; set; }

	string Text { get; set; }

	string Caption { get; set; }

	string ProgressTooltip { get; set; }

	bool ShowTitle { get; set; }

	bool ButtonEnabled { get; set; }

	bool ButtonVisible { get; set; }

	bool CancelRequested { get; }

	event AsyncProgressPopupCancelClickDelegate OnCancelClick;

	bool Show(string _Caption, string _Body, string _ButtonCaption);

	void Hide();
}
