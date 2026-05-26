namespace AGInterfaces;

public class AGBaseMemberExample : AGBaseMember
{
	public readonly string[] examples;

	public AGBaseMemberExample(AGBaseMember src, string[] examples)
		: base(src)
	{
		this.examples = examples;
	}
}
