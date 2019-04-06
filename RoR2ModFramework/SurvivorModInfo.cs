using System;
using RoR2;
using UnityEngine;

namespace SeikoML
{
    public class SurvivorModInfo
    {
        public SurvivorDef RegisterModSurvivor()
        {
            this.characterObject = UnityEngine.Object.Instantiate<GameObject>(BodyCatalog.FindBodyPrefab(this.bodyPrefabString));
            this.characterObject.SetActive(false);
            UnityEngine.Object.DontDestroyOnLoad(this.characterObject);
            return new SurvivorDef
            {
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
    }
}
