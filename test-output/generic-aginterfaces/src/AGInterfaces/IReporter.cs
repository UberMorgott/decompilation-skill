namespace AGInterfaces;

public interface IReporter
{
	string GetUserTemplatePath();

	string GetBasicTemplatePath();

	void TemplateDirectoryReload();
}
