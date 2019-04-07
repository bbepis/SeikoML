namespace SeikoML
{
	public interface ISeikoMod
	{
		// Token: 0x0600000A RID: 10
		void OnStart();
	}

	// Temporary interface for compatibility with mods that don't implement new method.
	public interface IKookehsMod : ISeikoMod
	{
		void OnUpdate();
	}
}
