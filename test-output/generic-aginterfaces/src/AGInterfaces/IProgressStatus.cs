namespace AGInterfaces;

public interface IProgressStatus
{
	string Text { set; }

	string OLEText { set; }

	int Max { set; }

	int Value { set; }

	string OnlineText { set; }

	string OnlineHint { set; }

	bool OnlineVisible { set; }

	int OnlinePosition { set; }

	int CacheMax { set; }

	int CacheValue { set; }

	string CacheText { set; }

	string CacheHint { set; }

	bool CacheVisible { set; }

	int CachePosition { set; }
}
