using System;
using RoR2;
using UnityEngine;

namespace SeikoML
{
    // Token: 0x02000004 RID: 4
    public class SurvivorModInfo
    {
        // Token: 0x06000009 RID: 9 RVA: 0x00002310 File Offset: 0x00000510
        public SurvivorDef RegisterModSurvivor()
        {
            characterObject = UnityEngine.Object.Instantiate(BodyCatalog.FindBodyPrefab(bodyPrefabString));
			UnityEngine.Object.DontDestroyOnLoad(characterObject);
			characterObject.SetActive(false);
			SkillLocator skillLocator = characterObject.GetComponent<SkillLocator>();
			if (primarySkillNameToken != null) skillLocator.primary.skillNameToken = primarySkillNameToken;
			if (secondarySkillNameToken != null) skillLocator.secondary.skillNameToken = secondarySkillNameToken;
			if (utilitySkillNameToken != null) skillLocator.utility.skillNameToken = utilitySkillNameToken;
			if (specialSkillNameToken != null) skillLocator.special.skillNameToken = specialSkillNameToken;
			if (primarySkillDescriptionToken != null) skillLocator.primary.skillDescriptionToken = primarySkillDescriptionToken;
			if (secondarySkillDescriptionToken != null) skillLocator.secondary.skillDescriptionToken = secondarySkillDescriptionToken;
			if (utilitySkillDescriptionToken != null) skillLocator.utility.skillDescriptionToken = utilitySkillDescriptionToken;
			if (specialSkillDescriptionToken != null) skillLocator.special.skillDescriptionToken = specialSkillDescriptionToken;
			return new SurvivorDef{
                bodyPrefab = characterObject,
                displayPrefab = Resources.Load<GameObject>(portraitPrefabString),
                descriptionToken = descriptionTokenString,
                primaryColor = new Color(usedColor.r, usedColor.g, usedColor.b),
                unlockableName = unlockableNameString
            };
        }

        public string bodyPrefabString;
        public string portraitPrefabString;
        public string descriptionTokenString;
        public Color usedColor;
        public int toReplace = -1;
        public string unlockableNameString = "";
        public GameObject characterObject;

		public string primarySkillNameToken;
		public string secondarySkillNameToken;
		public string utilitySkillNameToken;
		public string specialSkillNameToken;
		public string primarySkillDescriptionToken;
		public string secondarySkillDescriptionToken;
		public string utilitySkillDescriptionToken;
		public string specialSkillDescriptionToken;
	}
}
