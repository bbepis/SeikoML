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
            On.RoR2.SurvivorCatalog.GetSurvivorDef += (Orig, survivorIndex) =>
            {
                if ((int)survivorIndex < 0 || (int)survivorIndex > SurvivorCatalog.survivorDefs.Length)
                {
                    return null;
                }
                return SurvivorCatalog.survivorDefs[(int)survivorIndex];
            };
            IL.RoR2.SurvivorCatalog.Init += (il) =>
            {
                MMILCursor c = il.At(0);

                c.Remove();
                c.EmitDelegate<Func<int>>(ModLoader.GetSurvivorCount);
            };
        }
    }
}

