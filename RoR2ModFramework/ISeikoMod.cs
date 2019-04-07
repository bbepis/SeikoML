using System;

// Token: 0x02000005 RID: 5
namespace SeikoML
{
	public interface ISeikoMod
	{
		// Token: 0x0600000A RID: 10
		void Start();
	}

    // Temporary interface for compatibility with mods that don't implement new method.
    public interface IKookehsMod : ISeikoMod
    {
        void Update();
    }
}
