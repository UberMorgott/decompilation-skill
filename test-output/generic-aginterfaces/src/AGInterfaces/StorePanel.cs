using System;
using System.Reflection;

namespace AGInterfaces;

[Serializable]
[Obfuscation(Exclude = true)]
public sealed class StorePanel : StoreMultiCaption
{
	public bool ShowInFullScreen { get; set; }
}
