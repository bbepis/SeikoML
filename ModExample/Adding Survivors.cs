using System;
using SeikoML;
using UnityEngine;
using RoR2;

public class ExampleMod : RoR2Mod //Make sure you implement RoR2Mod 
{
    /* This method gets called as soon as the game is loaded, you can call other methods you have created
     * as well as any method that exist in the RoR2 library.
     * However you don't have access to variables outside of your class yet.
     * Make sure you have SeikoML and UnityEngine.CoreModule 
     * (found in your \Risk of Rain 2\Risk of Rain 2_Data\Managed folder)
     * added to your dependencies.*/
    public void Awake() 
    {
        //Define a new survivor mod info, which contains the information for a modded survivor.
        CustomSurvivor survivorModInfo = new CustomSurvivor
        {
            Survivor = new SurvivorDef()
            {
                bodyPrefab = BodyCatalog.FindBodyPrefab("LemurianBody"), //The prefab that will be used for the survivor
                descriptionToken= "LEMURIAN_DESCRIPTION", //This text is pulled from the game's language files, you might have to make your own
                displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/LemurianDisplay"), //also pulled from the prefab, might have to make your own as well.
                primaryColor = new Color(0.42352942f, 0.81960785f, 0.91764706f),
                unlockableName = "" //if you want to Tie your character to an achievement, add the name of the achivement here, leaving it empty means it's unlocked by default.
            }
        };
        //You can define multiple at once!
        CustomSurvivor survivorModInfo2 = new CustomSurvivor
        {
            Survivor = new SurvivorDef()
            {
                bodyPrefab = BodyCatalog.FindBodyPrefab("LemurianBruiserBody"),
                descriptionToken = "LEMURIANBRUISER_DESCRIPTION",
                displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/LemurianBruiserDisplay"),
                primaryColor = new Color(0.42352942f, 0.81960785f, 0.91764706f)
            }
        };
        //Make sure to add your modded survivors to the SurvivorMods list! This will ensure they get added!
        ModLoader.RegisterSurvivor(survivorModInfo);
        ModLoader.RegisterSurvivor(survivorModInfo2);
    }
    /*Character IDs:
     * 0 = Comando
     * 1 = Huntress
     * 2 = MUL-T
     * 3 = Engineer
     * 4 = Artificer
     * 5 = Mercenary
     */
  
}
