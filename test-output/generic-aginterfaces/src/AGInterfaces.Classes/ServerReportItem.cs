using System;

namespace AGInterfaces.Classes;

public record ServerReportItem(string Name, int Length, DateTime Updated) : ServerMapItem(Name, Length, Updated);
