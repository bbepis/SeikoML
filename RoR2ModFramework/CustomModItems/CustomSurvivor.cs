using System;
using RoR2;
using UnityEngine;

namespace SeikoML
{
    public class CustomSurvivor
    {
        public SurvivorDef GetSurvivorDef()
        {
			SkillLocator skillLocator = Survivor.bodyPrefab.GetComponent<SkillLocator>();

            if (Primary != null) skillLocator.primary = Primary;
            if (Secondary!= null) skillLocator.secondary = Secondary;
            if (Utility!= null) skillLocator.utility= Utility;
            if (Special != null) skillLocator.special= Special;

            return Survivor;
        }

        public SurvivorDef Survivor { get; set; }

        public GenericSkill Primary { get; set; } = null;
        public GenericSkill Secondary { get; set; } = null;
        public GenericSkill Utility { get; set; } = null;
        public GenericSkill Special { get; set; } = null;

        public int ToReplace { get; set; } = -1;
	}
}
