using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using RoR2;
using UnityEngine;

namespace RoR2ModFramework
{
    // Token: 0x02000002 RID: 2
    public static class BaseFramework
    {
        public static readonly string Version = "v0.0.2";
        public static int SurvivorCount { get
            {
                return SurvivorCatalog.survivorDefs.Count()+SurvivorMods.Count;
            }
        }
        private static int VanillaSurvivorCount = SurvivorCatalog.survivorDefs.Count();

        public static void Begin()
        {
            Debug.LogFormat("[RoR2ML] Mod Loader active, {0}", new object[] { Version });
            //Thanks Wildbook!
            var gamePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var modsPath = System.IO.Path.Combine(gamePath, "Mods");
            foreach (string archiveFileName in Directory.EnumerateFiles(modsPath, "*.zip"))
            {
                using (var zip = ZipFile.OpenRead(archiveFileName))
                {
                    var modZipEntry = zip.GetEntry("mod.dll");
                    if (modZipEntry == null)
                        continue;

                    using (var file = modZipEntry.Open())
                    using (var fileMemoryStream = new MemoryStream())
                    {
                        file.CopyTo(fileMemoryStream);
                        var modAssembly = Assembly.Load(fileMemoryStream.ToArray());
                        var modAssemblyTypes = modAssembly.GetTypes();
                        var modClasses = modAssemblyTypes.Where(x => x.GetInterfaces().Contains(typeof(IModInterface)));
            
                        foreach (var modClass in modClasses)
                        {
                            var modClassInstance = Activator.CreateInstance(modClass);
                            ((IModInterface)modClassInstance).onStart();
                            
                        }
                    }
                }
            }
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002184 File Offset: 0x00000384
        public static void addSurvivors()
        {
            Debug.LogFormat("[ROR2ML] Attempting to load {0} mod survivors.", new object[]
            {
                BaseFramework.SurvivorMods.Count
            });
            
            foreach (SurvivorModInfo survivorModInfo in BaseFramework.SurvivorMods)
            {
                int index = SurvivorMods.IndexOf(survivorModInfo);
                Debug.LogFormat("[ROR2ML] Adding mod survivor... (Body: {0}, Index Order: {1})", new object[]
                {
                    survivorModInfo.bodyPrefabString,
                    index
                });
                SurvivorCatalog.RegisterSurvivor((SurvivorIndex)VanillaSurvivorCount+index, survivorModInfo.RegisterModSurvivor());
            }
        }
        public static SurvivorIndex[] BuildIdealOrder(SurvivorIndex[] og_order)
        {
            List<SurvivorIndex> Order = og_order.TakeWhile(x=>x.ToString() != "Count").ToList();
            foreach (SurvivorModInfo S in SurvivorMods)
            {
                Order.Add((SurvivorIndex)SurvivorMods.IndexOf(S)+og_order.Length-1);
            }
            return Order.Count >= 24 ? Order.Take(24).ToArray() : Order.ToArray();
        }
        
        
        public static void addItems()
        {
            Debug.LogFormat("[ROR2ML] Attempting to load {0} mod items.", new object[]
            {
                BaseFramework.ItemMods.Count
            });
            uint num = 1u;
            foreach (ItemModInfo itemModInfo in BaseFramework.ItemMods)
            {
                Debug.LogFormat("[ROR2ML] Adding mod item... (Name: {0})", new object[]
                {
                    itemModInfo.nameTokenString
                });
                if (itemModInfo.toReplace == -1)
                {
                    ItemCatalog.RegisterItem(ItemIndex.DrizzlePlayerHelper + (int)num, itemModInfo.RegisterModItem());
                    num += 1u;
                }
                else
                {
                    ItemCatalog.RegisterItem((ItemIndex)itemModInfo.toReplace, itemModInfo.RegisterModItem());
                }
            }
        }

        // Token: 0x04000001 RID: 1
        public static List<SurvivorModInfo> SurvivorMods = new List<SurvivorModInfo>();

        // Token: 0x04000002 RID: 2
        public static List<ItemModInfo> ItemMods = new List<ItemModInfo>();
    }
}
