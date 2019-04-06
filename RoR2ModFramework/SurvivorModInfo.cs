using System;
using RoR2;
using UnityEngine;

namespace SeikoML
{
	// Token: 0x02000004 RID: 4
	public class SurvivorModInfo
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002310 File Offset: 0x00000510
		public SurvivorDef RegisterModSurvivor(){
			characterObject = UnityEngine.Object.Instantiate<GameObject>(BodyCatalog.FindBodyPrefab(bodyPrefabString));
			UnityEngine.Object.DontDestroyOnLoad(characterObject);
			characterObject.SetActive(false);
			if (primarySkillNameToken != null) characterObject.GetComponent<SkillLocator>().primary.skillNameToken = this.primarySkillNameToken;
			if (secondarySkillNameToken != null) characterObject.GetComponent<SkillLocator>().secondary.skillNameToken = secondarySkillNameToken;
			if (utilitySkillNameToken != null) characterObject.GetComponent<SkillLocator>().utility.skillNameToken = utilitySkillNameToken;
			if (specialSkillNameToken != null) characterObject.GetComponent<SkillLocator>().special.skillNameToken = specialSkillNameToken;
			if (primarySkillDescriptionToken != null) characterObject.GetComponent<SkillLocator>().primary.skillDescriptionToken = primarySkillDescriptionToken;
			if (secondarySkillDescriptionToken != null) characterObject.GetComponent<SkillLocator>().secondary.skillDescriptionToken = secondarySkillDescriptionToken;
			if (utilitySkillDescriptionToken != null) characterObject.GetComponent<SkillLocator>().utility.skillDescriptionToken = utilitySkillDescriptionToken;
			if (specialSkillDescriptionToken != null) characterObject.GetComponent<SkillLocator>().special.skillDescriptionToken = specialSkillDescriptionToken;
			return new SurvivorDef{
				bodyPrefab = this.characterObject,
				displayPrefab = Resources.Load<GameObject>(this.portraitPrefabString),
				descriptionToken = this.descriptionTokenString,
				primaryColor = new Color(this.usedColor.r, this.usedColor.g, this.usedColor.b),
				unlockableName = this.unlockableNameString
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
