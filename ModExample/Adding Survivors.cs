using System;
using SeikoML;
using UnityEngine;

public class ExampleMod : ISeikoMod //Make sure you implement IModInterface 
{
    /* This method gets called as soon as the mod is loaded, you can call other methods you have created
     * as well as any method that exist in the RoR2 library.
     * However you don't have access to variables outside of your class yet.
     * Make sure you have SeikoML and UnityEngine.CoreModule 
     * (found in your \Risk of Rain 2\Risk of Rain 2_Data\Managed folder)
     * added to your dependencies.*/
    public void OnStart() 
    {
        //Define a new survivor mod info, which contains the information for a modded survivor.
        SurvivorModInfo survivorModInfo = new SurvivorModInfo
        {
            bodyPrefabString = "LemurianBody", //The prefab that will be used for the survivor
            descriptionTokenString = "LEMURIAN_DESCRIPTION", //This text is pulled from the game's language files, you might have to make your own
            portraitPrefabString = "Prefabs/Characters/LemurianDisplay", //also pulled from the prefab, might have to make your own as well.
            usedColor = new Color(0.42352942f, 0.81960785f, 0.91764706f),
            toReplace = -1, // placed here as an example, if your character tries to replace one of the vanilla characters, put their id here
            unlockableNameString = "" //if you want to Tie your character to an achievement, add the name of the achivement here, leaving it empty means it's unlocked by default.
            
        };
        //You can define multiple at once!
        SurvivorModInfo survivorModInfo2 = new SurvivorModInfo
        {
            bodyPrefabString = "LemurianBruiserBody",
            descriptionTokenString = "LEMURIANBRUISER_DESCRIPTION",
            portraitPrefabString = "Prefabs/Characters/LemurianBruiserDisplay",
            usedColor = new Color(0.42352942f, 0.81960785f, 0.91764706f),
        };
        //Make sure to add your modded survivors to the SurvivorMods list! This will ensure they get added!
        ModLoader.SurvivorMods.Add(survivorModInfo);
        ModLoader.SurvivorMods.Add(survivorModInfo2);
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
