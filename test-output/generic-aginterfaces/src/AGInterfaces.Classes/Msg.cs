using System;
using System.IO;
using System.Text;
using Polenter.Serialization;
using Polenter.Serialization.Core;

namespace AGInterfaces.Classes;

public class Msg
{
	private const string DateTimeFormat = "ddMMyyyyHHmm";

	private static readonly SharpSerializerXmlSettings SerializerXmlSettings = new SharpSerializerXmlSettings
	{
		Encoding = Encoding.UTF8,
		IncludeAssemblyVersionInTypeName = false,
		IncludeCultureInTypeName = false,
		IncludePublicKeyTokenInTypeName = false,
		AdvancedSettings = new AdvancedSharpSerializerXmlSettings
		{
			RootName = "Messages"
		}
	};

	private static readonly SharpSerializer Serializer = new SharpSerializer(SerializerXmlSettings);

	public string FileName { get; set; }

	public Guid Id { get; set; }

	public bool Select { get; set; }

	public DateTime DateFromUtc { get; set; }

	public DateTime DateToUtc { get; set; }

	public string User { get; set; }

	public bool Important { get; set; }

	public bool Modal { get; set; }

	public TimeSpan Duration { get; set; }

	public string Text { get; set; }

	public bool IsRead { get; set; }

	[ExcludeFromSerialization]
	public DateTime DateFrom
	{
		get
		{
			return DateFromUtc.ToLocalTime();
		}
		set
		{
			DateFromUtc = value.ToUniversalTime();
		}
	}

	[ExcludeFromSerialization]
	public DateTime DateTo
	{
		get
		{
			return DateToUtc.ToLocalTime();
		}
		set
		{
			DateToUtc = value.ToUniversalTime();
		}
	}

	public Msg()
	{
	}

	public Msg(bool important, bool modal, TimeSpan duration, string text)
	{
		Important = important;
		Modal = modal;
		Duration = duration;
		Text = text;
	}

	public Msg(DateTime dateFrom, DateTime dateTo, bool important, bool modal, TimeSpan duration, string text, string user)
	{
		DateFrom = dateFrom;
		DateTo = dateTo;
		Important = important;
		Modal = modal;
		Duration = duration;
		Text = text;
		User = user;
		FileName = "Messages\\" + DateFromUtc.ToString("ddMMyyyyHHmm") + "-" + DateToUtc.ToString("ddMMyyyyHHmm") + ".xml";
		Id = Guid.NewGuid();
	}

	public void ChangeDateFrom(DateTime dateFrom)
	{
		string oldValue = DateFromUtc.ToString("ddMMyyyyHHmm");
		DateFrom = dateFrom;
		FileName = FileName.Replace(oldValue, DateFromUtc.ToString("ddMMyyyyHHmm"));
	}

	public void ChangeDateTo(DateTime dateTo)
	{
		string oldValue = DateToUtc.ToString("ddMMyyyyHHmm");
		DateTo = dateTo;
		FileName = FileName.Replace(oldValue, DateToUtc.ToString("ddMMyyyyHHmm"));
	}

	public static Msg[] FromBytes(byte[] data)
	{
		try
		{
			using MemoryStream stream = new MemoryStream(data);
			return (Msg[])Serializer.Deserialize(stream);
		}
		catch
		{
			return new Msg[0];
		}
	}

	public static byte[] ToBytes(Msg[] msgs)
	{
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			Serializer.Serialize(msgs, memoryStream);
			return memoryStream.ToArray();
		}
		catch
		{
			return new byte[0];
		}
	}
}
