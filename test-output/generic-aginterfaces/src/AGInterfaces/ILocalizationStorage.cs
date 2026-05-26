using System.Collections.Generic;
using System.Xml;
using AGInterfaces.Localization;

namespace AGInterfaces;

public interface ILocalizationStorage
{
	CommonFields CommonFields { get; set; }

	List<MemberClassInfo> BaseMemberClassInfo { get; }

	List<MemberClassInfo> CrdIndMemberClassInfo { get; }

	List<MemberClassInfo> CrdDepMemberClassInfo { get; }

	List<MemberClassInfo> TskDepMemberClassInfo { get; }

	List<MemberClassInfo> TripMemberClassInfo { get; }

	List<MemberClassInfo> FinalMemberClassInfo { get; }

	List<MemberClassInfo> StructMemberClassInfo { get; }

	Dictionary<string, XmlElement> FormXmlElements { get; set; }

	Dictionary<string, XmlElement> UserControlXmlElements { get; set; }
}
