using System;

namespace AGInterfaces.Classes;

public record ServerSchemaItem(int ID, string Name, Guid UID, bool State, int Cars, int GFs, int Length, bool IsCommon, DateTime Updated) : ServerMapItem(Name, Length, Updated);
