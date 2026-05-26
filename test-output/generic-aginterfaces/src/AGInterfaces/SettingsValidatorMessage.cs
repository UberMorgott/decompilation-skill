namespace AGInterfaces;

public class SettingsValidatorMessage
{
	public SettingsValidatorLevel Level;

	public ValidatorMessageType Type;

	public string Message;

	public SettingsValidatorMessage(SettingsValidatorLevel level, ValidatorMessageType type, string message)
	{
		Level = level;
		Message = message;
		Type = type;
	}
}
