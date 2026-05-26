using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AGInterfaces;

public class PhotoInfo
{
	public string FullFileName;

	public DateTime DT;

	public Coordinates Crds;

	private static readonly Regex regEx = new Regex("AGDS_(\\d+)_(\\d+)_(\\d{6}_\\d{6})_(\\d+)", RegexOptions.Compiled);

	private const string pattern = "yyMMdd_HHmmss";

	public int CameraNum { get; protected set; }

	public int PhotoNum { get; protected set; }

	public int SerialNo { get; protected set; }

	public long Length { get; protected set; }

	protected PhotoInfo()
	{
	}

	public PhotoInfo(string fileName, int cameraNum, DateTime dt, int photoNum, int serialNo, long length = 0L)
	{
		FullFileName = fileName;
		CameraNum = cameraNum;
		DT = dt;
		PhotoNum = photoNum;
		SerialNo = serialNo;
		Length = length;
	}

	public static PhotoInfo ParsePhotoFileName(string fileName, string fullName, int serialNo, long length = 0L)
	{
		Match match = regEx.Match(fileName);
		if (!match.Success)
		{
			return null;
		}
		int cameraNum = int.Parse(match.Groups[2].Value);
		if (!DateTime.TryParseExact(match.Groups[3].Value, "yyMMdd_HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
		{
			return null;
		}
		int photoNum = int.Parse(match.Groups[4].Value);
		return new PhotoInfo(fullName, cameraNum, result, photoNum, serialNo, length);
	}

	public static PhotoInfo ParsePhotoFileName(string fileName)
	{
		return ParsePhotoFileName(fileName, string.Empty, -1, 0L);
	}
}
