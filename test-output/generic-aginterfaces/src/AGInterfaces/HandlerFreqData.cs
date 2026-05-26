namespace AGInterfaces;

public sealed class HandlerFreqData
{
	public int[] pos;

	public double[] freq;

	public bool counterAsVoltage;

	public HandlerFreqData(int len, bool counterAsVoltage)
	{
		pos = new int[len];
		freq = new double[len];
		this.counterAsVoltage = counterAsVoltage;
	}

	public override string ToString()
	{
		if (pos.Length >= 2 && freq.Length >= 2)
		{
			return $"pos={pos[0]}: {freq[0]}, pos={pos[1]}: {freq[1]}";
		}
		if (pos.Length >= 1 && freq.Length >= 1)
		{
			return $"pos={pos[0]}: {freq[0]}";
		}
		return base.ToString();
	}
}
