using BepInEx;
using RoR2;
using SeikoML;
using UnityEngine;

public class BanditMod : BaseUnityPlugin
{
	public BanditMod()
	{
		CustomSurvivor survivorModInfo = new CustomSurvivor
		{
			Survivor = new SurvivorDef
			{
				bodyPrefab = BodyCatalog.FindBodyPrefab("BanditBody"),
				descriptionToken = "BANDIT_DESCRIPTION",
				displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/BanditDisplay"),
				primaryColor = new Color(0.8039216f, 0.482352942f, 0.843137264f),
				unlockableName = ""
			},
			ToReplace = -1
		};

		var skillLocator = survivorModInfo.Survivor.bodyPrefab.GetComponent<SkillLocator>();

		skillLocator.primary.name = "Blast";
		skillLocator.primary.skillDescriptionToken = "Fire a powerful slug for <style=cIsDamage>150% damage</style>.";
		skillLocator.secondary.name = "Lights Out";
		skillLocator.secondary.skillDescriptionToken = "Take aim for a headshot, dealing <style=cIsDamage>600% damage</style>. If the ability <style=cIsDamage>kills an enemy</style>, the Bandit's <style=cIsUtility>Cooldowns are all reset to 0</style>.";
		skillLocator.utility.name = "Smokebomb";
		skillLocator.utility.skillDescriptionToken = "Turn invisible for <style=cIsDamage>3 seconds</style>, gaining <style=cIsUtility>increased movement speed</style>.";
		skillLocator.special.name = "Thermite Toss";
		skillLocator.special.skillDescriptionToken = "Fire off a burning Thermite grenade, dealing <style=cIsDamage>damage over time</style>.";

		Api.RegisterSurvivor(survivorModInfo);
	}
}