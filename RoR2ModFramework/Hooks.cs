using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using BepInEx;
using MonoMod;
using MonoMod.Utils;
using Mono.Cecil;
using SeikoML;
using System.Linq;

namespace RoR2ModAPI
{
    [BepInPlugin("ModLoaderAPIHook","RoR2MLAPI","0.0.1")]
    public class Hooks : BaseUnityPlugin
    {
        public void Awake()
        {
            //Inject custom variables into the SurvivorCatalog's constructor
            IL.RoR2.SurvivorCatalog.cctor += (IL) =>
            {   
                MMILCursor c = IL.At(0);
                //If survivor count > Vanilla Max, change max survivor count 
                if ((int)c.Next.Operand < ModLoader.GetSurvivorCount()) c.EmitDelegate<Func<int>>(ModLoader.GetSurvivorCount);

                //move Cursor to the right place
                c.GotoNext(x=>x.MatchStsfld(typeof(SurvivorCatalog), "idealSurvivorOrder"));

                //Replace the IdealSurvivorOrder with the one from the Modloader
                c.EmitDelegate<Func<SurvivorIndex[], SurvivorIndex[]>>(r => ModLoader.BuildIdealOrder(r));
            };

            //Replace the GetSurvivorDef method entirely
            On.RoR2.SurvivorCatalog.GetSurvivorDef += (Orig, survivorIndex) => 
            {
                //orig is the original method and SurvivorIndex is the variable that is given to the original GetSirvivorDef
                if ((int)survivorIndex < 0 || (int)survivorIndex > SurvivorCatalog.survivorDefs.Length)
                {
                    return null;
                }
                
                return SurvivorCatalog.survivorDefs[(int)survivorIndex];
                //by never doing Orig(), the original method is never executed whenever it's called, effectively being replaced
            };

            //Inject customt variables into the SurvivorCatalog's Init method
            IL.RoR2.SurvivorCatalog.Init += (il) =>
            {
                MMILCursor c = il.At(0);

                c.Remove(); //Remove Variable 7 (Size of SurvivorArray)
                c.EmitDelegate<Func<int>>(ModLoader.GetSurvivorCount); //Set Survivor Array to custom amount

                //Inject the SurvivorDef[] that includes the modded survivors into the SurvivorCatalog right before
                //The Viewable node is created, ensuring that the modded survivors are added to the menu
                c.GotoNext(x => x.MatchStsfld(typeof(SurvivorCatalog), "_allSurvivorDefs"));
            };
        }
    }
}

