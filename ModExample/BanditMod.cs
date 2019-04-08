using System;
using RoR2;
using EntityStates;
using SeikoML;
using UnityEngine;

public class BanditMod : RoR2Mod
{
	public void Awake()
    {
        CustomSurvivor survivorModInfo = new CustomSurvivor {
            Survivor = new SurvivorDef() {
                bodyPrefab = BodyCatalog.FindBodyPrefab("BanditBody"),
                descriptionToken = "BANDIT_DESCRIPTION",
                displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/BanditDisplay"),
                primaryColor = new Color(0.8039216f, 0.482352942f, 0.843137264f),
                unlockableName = "",
            },
            ToReplace = -1,

        };
        var SkillLocator = survivorModInfo.Survivor.bodyPrefab.GetComponent<SkillLocator>();

        SkillLocator.primary.name = "Blast";
        SkillLocator.primary.skillDescriptionToken = "Fire a powerful slug for <style=cIsDamage>150% damage</style>.";
        SkillLocator.secondary.name = "Lights Out";
        SkillLocator.secondary.skillDescriptionToken = "Take aim for a headshot, dealing <style=cIsDamage>600% damage</style>. If the ability <style=cIsDamage>kills an enemy</style>, the Bandit's <style=cIsUtility>Cooldowns are all reset to 0</style>.";
        SkillLocator.utility.name = "Smokebomb";
        SkillLocator.utility.skillDescriptionToken = "Turn invisible for <style=cIsDamage>3 seconds</style>, gaining <style=cIsUtility>increased movement speed</style>.";
        SkillLocator.special.name = "Thermite Toss";
        SkillLocator.special.skillDescriptionToken = "Fire off a burning Thermite grenade, dealing <style=cIsDamage>damage over time</style>.",
        
        ModLoader.RegisterSurvivor(survivorModInfo);
	}
}
