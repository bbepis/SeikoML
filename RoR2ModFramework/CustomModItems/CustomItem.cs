using RoR2;

namespace SeikoML
{
	public class CustomItem
	{
		public ItemDef RegisterModItem()
		{
			return new ItemDef
			{
				tier = itemTier,
				pickupModelPath = modelPathString,
				pickupIconPath = iconPathString,
				nameToken = nameTokenString,
				pickupToken = pickupTokenString,
				descriptionToken = descriptionTokenString,
				addressToken = "",
				unlockableName = unlockableNameString
			};
		}

		public string descriptionTokenString;

		public int toReplace = -1;

		public string unlockableNameString = "";

		public string modelPathString;

		public string nameTokenString;

		public string pickupTokenString;

		public ItemTier itemTier;

		public string iconPathString;
	}
}