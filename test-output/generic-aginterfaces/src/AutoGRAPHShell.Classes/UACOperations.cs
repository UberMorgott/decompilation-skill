using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;

namespace AutoGRAPHShell.Classes;

public class UACOperations
{
	public static string AG_EXE_FILE;

	private const string UAC_COMMAND = "-uac";

	public static string[] GetUACCommandLineArguments(string[] commandLineArguments)
	{
		if (commandLineArguments == null || commandLineArguments.Length != 4 || string.Compare(commandLineArguments[1], "-uac", ignoreCase: true) != 0)
		{
			return null;
		}
		List<string> list = new List<string>();
		list.Add(commandLineArguments[0]);
		list.Add(commandLineArguments[1]);
		list.Add(commandLineArguments[2]);
		try
		{
			string[] collection = File.ReadAllLines(commandLineArguments[3]);
			list.AddRange(collection);
		}
		catch
		{
		}
		return list.ToArray();
	}

	public static UACResult DoOperations(string[] commandLineArguments)
	{
		if (commandLineArguments == null || commandLineArguments.Length < 3)
		{
			return UACResult.NoArguments;
		}
		UACOperationType uACOperationType = (UACOperationType)int.Parse(commandLineArguments[2]);
		int num = 3;
		switch (uACOperationType)
		{
		case UACOperationType.FileCopy:
			if (commandLineArguments.Length < 5)
			{
				return UACResult.InvalidArgumentsCount;
			}
			try
			{
				int num3 = num;
				for (int j = num3 + 1; j < commandLineArguments.Length; j += 2)
				{
					string sourceFileName = commandLineArguments[num3];
					string text3 = commandLineArguments[j];
					string directoryName = Path.GetDirectoryName(text3);
					if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
					{
						Directory.CreateDirectory(directoryName);
					}
					File.Copy(sourceFileName, text3, overwrite: true);
					num3 += 2;
				}
			}
			catch (Exception)
			{
				return UACResult.CantProcess;
			}
			break;
		case UACOperationType.FileMove:
			if (commandLineArguments.Length < 5)
			{
				return UACResult.InvalidArgumentsCount;
			}
			try
			{
				int num5 = num;
				for (int n = num5 + 1; n < commandLineArguments.Length; n += 2)
				{
					string sourceFileName2 = commandLineArguments[num5];
					string text5 = commandLineArguments[n];
					string directoryName2 = Path.GetDirectoryName(text5);
					if (!string.IsNullOrEmpty(directoryName2) && !Directory.Exists(directoryName2))
					{
						Directory.CreateDirectory(directoryName2);
					}
					if (File.Exists(text5))
					{
						File.Delete(text5);
					}
					File.Move(sourceFileName2, text5);
					num5 += 2;
				}
			}
			catch (Exception)
			{
				return UACResult.CantProcess;
			}
			break;
		case UACOperationType.FileDelete:
			if (commandLineArguments.Length < 4)
			{
				return UACResult.InvalidArgumentsCount;
			}
			try
			{
				for (int k = num; k < commandLineArguments.Length; k++)
				{
					if (File.Exists(commandLineArguments[k]))
					{
						File.Delete(commandLineArguments[k]);
					}
				}
			}
			catch (Exception)
			{
				return UACResult.CantProcess;
			}
			break;
		case UACOperationType.DirectoryDelete:
			if (commandLineArguments.Length < 4)
			{
				return UACResult.InvalidArgumentsCount;
			}
			try
			{
				for (int l = num; l < commandLineArguments.Length; l++)
				{
					if (Directory.Exists(commandLineArguments[l]))
					{
						Directory.Delete(commandLineArguments[l], recursive: true);
					}
				}
			}
			catch (Exception)
			{
				return UACResult.CantProcess;
			}
			break;
		case UACOperationType.ZIPExtract:
		case UACOperationType.ZIPExtractAndDelete:
		{
			if (commandLineArguments.Length < 5)
			{
				return UACResult.InvalidArgumentsCount;
			}
			int num4 = num;
			for (int m = num4 + 1; m < commandLineArguments.Length; m += 2)
			{
				string text4 = commandLineArguments[num4];
				string path2 = commandLineArguments[m];
				try
				{
					using (ZipFile zipFile2 = new ZipFile(text4, Encoding.GetEncoding(866)))
					{
						zipFile2.ExtractAll(path2, ExtractExistingFileAction.OverwriteSilently);
					}
					if (uACOperationType == UACOperationType.ZIPExtractAndDelete)
					{
						File.Delete(text4);
					}
				}
				catch (Exception)
				{
					return UACResult.CantProcess;
				}
				num4 += 2;
			}
			break;
		}
		case UACOperationType.ZIPExtractSetTimeAndDelete:
		{
			if (commandLineArguments.Length < 5)
			{
				return UACResult.InvalidArgumentsCount;
			}
			int num2 = num;
			for (int i = num2 + 1; i < commandLineArguments.Length; i += 2)
			{
				string text = commandLineArguments[num2];
				string text2 = commandLineArguments[i];
				try
				{
					DateTime creationTime = File.GetCreationTime(text);
					using (ZipFile zipFile = new ZipFile(text, Encoding.GetEncoding(866)))
					{
						foreach (ZipEntry item in zipFile)
						{
							string path = Path.Combine(text2, item.FileName);
							item.Extract(text2, ExtractExistingFileAction.OverwriteSilently);
							File.SetCreationTime(path, creationTime);
						}
					}
					File.Delete(text);
				}
				catch (Exception)
				{
					return UACResult.CantProcess;
				}
				num2 += 2;
			}
			break;
		}
		default:
			return UACResult.InvalidCommand;
		}
		return UACResult.OK;
	}

	private static UACResult startAndWaitForExit(UACOperationType operationType, params string[] commandLineArguments)
	{
		string tempFileName = Path.GetTempFileName();
		try
		{
			File.WriteAllLines(tempFileName, commandLineArguments);
			Process? process = Process.Start(new ProcessStartInfo(AG_EXE_FILE)
			{
				Verb = "runas",
				UseShellExecute = true,
				WindowStyle = ProcessWindowStyle.Normal,
				CreateNoWindow = false,
				Arguments = $"{"-uac"} {(int)operationType} {tempFileName}"
			});
			process.WaitForExit();
			return (UACResult)process.ExitCode;
		}
		catch (Win32Exception)
		{
			throw new UnauthorizedAccessException();
		}
		finally
		{
			try
			{
				File.Delete(tempFileName);
			}
			catch
			{
			}
		}
	}

	public static UACResult Write(string fileName, byte[] buff, Action<FileInfo> afterWriteHandler = null)
	{
		string tempFileName = Path.GetTempFileName();
		try
		{
			File.WriteAllBytes(tempFileName, buff);
			afterWriteHandler?.Invoke(new FileInfo(tempFileName));
			return startAndWaitForExit(UACOperationType.FileCopy, tempFileName, fileName);
		}
		finally
		{
			try
			{
				File.Delete(tempFileName);
			}
			catch
			{
			}
		}
	}

	public static UACResult Move(string fileFrom, string fileTo)
	{
		return startAndWaitForExit(UACOperationType.FileMove, fileFrom, fileTo);
	}

	public static UACResult Move(params UACItem[] items)
	{
		if (items == null || !items.Any())
		{
			return UACResult.InvalidArgumentsCount;
		}
		return startAndWaitForExit(UACOperationType.FileMove, items.Select((UACItem p) => new string[2] { p.FileName, p.Target }).SelectMany((string[] p) => p).ToArray());
	}

	public static UACResult Delete(params string[] fileNames)
	{
		if (fileNames == null || !fileNames.Any())
		{
			return UACResult.InvalidArgumentsCount;
		}
		return startAndWaitForExit(UACOperationType.FileDelete, fileNames);
	}

	public static UACResult DeleteDirectory(params string[] directories)
	{
		if (directories == null || !directories.Any())
		{
			return UACResult.InvalidArgumentsCount;
		}
		return startAndWaitForExit(UACOperationType.DirectoryDelete, directories);
	}

	public static UACResult Extract(UACOperationType type, string fileName, string toDirectory)
	{
		return startAndWaitForExit(type, fileName, toDirectory);
	}

	public static UACResult Extract(UACOperationType type, params UACItem[] items)
	{
		if (items == null || !items.Any())
		{
			return UACResult.InvalidArgumentsCount;
		}
		return startAndWaitForExit(type, items.Select((UACItem p) => new string[2] { p.FileName, p.Target }).SelectMany((string[] p) => p).ToArray());
	}
}
