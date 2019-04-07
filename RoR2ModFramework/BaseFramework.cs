using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using RoR2;
using UnityEngine;

namespace SeikoML
{
    public static class BaseFramework
    {
        public static readonly string Version = "v0.0.2";
        public static int SurvivorCount { get; set; }
        public static int VanillaCount { get; set; }

        public static void Begin()
        {
            Debug.LogFormat("[RoR2ML] Mod Loader active, {0}", new object[] { Version });
            //Thanks Wildbook!
            var gamePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var modsPath = System.IO.Path.Combine(gamePath, "Mods");
            foreach (string archiveFileName in Directory.EnumerateFiles(modsPath, "*.zip"))
            {
                Debug.LogFormat("[RoR2ML] Found mod archive {0}.", new object[] { archiveFileName });
                using (var zip = ZipFile.OpenRead(archiveFileName))
                {
                    var modZipEntry = zip.GetEntry("mod.dll");
                    if (modZipEntry == null)
                        continue;
                    Debug.LogFormat("[RoR2ML] Found mod.dll file.", new object[] { archiveFileName });
                    using (var file = modZipEntry.Open())
                    using (var fileMemoryStream = new MemoryStream())
                    {
                        file.CopyTo(fileMemoryStream);
                        var modAssembly = Assembly.Load(fileMemoryStream.ToArray());
                        var modAssemblyTypes = modAssembly.GetTypes();
                        var modClasses = modAssemblyTypes.Where(x => x.GetInterfaces().Contains(typeof(ISeikoMod)));
                        string name;
                        var manifest = zip.GetEntry("manifest.json");
                        if (manifest == null) name = modAssembly.GetName().ToString();
                        else
                        {
                            using (var manifestStream = manifest.Open())
                            using (var manifestStringReader = new StreamReader(manifestStream))
                            {
                                name = JsonUtility.FromJson<ThunderstoreManifest>(manifestStringReader.ReadToEnd()).name;
                            }
                        }
                        Debug.LogFormat("[RoR2ML] [{0}] Loading mod...", new object[] { name });
                        foreach (var modClass in modClasses)
                        {
                            var modClassInstance = Activator.CreateInstance(modClass);
                            ((ISeikoMod)modClassInstance).Start();
                        }
                    }
                }
            }
        }

        public static void Update()
        {
            Debug.LogFormat("[RoR2ML] Mod update event");
            //Thanks Wildbook!
            var gamePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var modsPath = System.IO.Path.Combine(gamePath, "Mods");
            foreach (string archiveFileName in Directory.EnumerateFiles(modsPath, "*.zip"))
            {
                Debug.LogFormat("[RoR2ML] Found mod archive {0}.", new object[] { archiveFileName });
                using (var zip = ZipFile.OpenRead(archiveFileName))
                {
                    var modZipEntry = zip.GetEntry("Mod.dll");
                    if (modZipEntry == null)
                        continue;
                    Debug.LogFormat("[RoR2ML] Found Mod.dll file.", new object[] { archiveFileName });
                    using (var file = modZipEntry.Open())
                    using (var fileMemoryStream = new MemoryStream())
                    {
                        file.CopyTo(fileMemoryStream);
                        var modAssembly = Assembly.Load(fileMemoryStream.ToArray());
                        var modAssemblyTypes = modAssembly.GetTypes();
                        var modClasses = modAssemblyTypes.Where(x => x.GetInterfaces().Contains(typeof(IKookehsMod)));
                        string name;
                        var manifest = zip.GetEntry("manifest.json");
                        if (manifest == null) name = modAssembly.GetName().ToString();
                        else
                        {
                            using (var manifestStream = manifest.Open())
                            using (var manifestStringReader = new StreamReader(manifestStream))
                            {
                                name = JsonUtility.FromJson<ThunderstoreManifest>(manifestStringReader.ReadToEnd()).name;
                            }
                        }
                        Debug.LogFormat("[RoR2ML] [{0}] calling update", new object[] { name });
                        foreach (var modClass in modClasses)
                        {
                            var modClassInstance = Activator.CreateInstance(modClass);
                            ((IKookehsMod)modClassInstance).Update();
                        }
                    }
                }
            }
        }

        public static int GetMaxPlayers()
        {
            return 32;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002184 File Offset: 0x00000384
        public static void AddSurvivors(ref SurvivorDef[] catalog)
        {
            
            if (BaseFramework.SurvivorMods.Count == 0) return;
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
                if (survivorModInfo.toReplace > -1 && survivorModInfo.toReplace < VanillaCount)
                {
                    catalog[survivorModInfo.toReplace] = survivorModInfo.RegisterModSurvivor();
                }
                else catalog[VanillaCount + index]=survivorModInfo.RegisterModSurvivor();
            }
        }
        public static SurvivorIndex[] BuildIdealOrder(SurvivorIndex[] og_order)
        {
            VanillaCount = og_order.Length;
            SurvivorIndex[] Order = new SurvivorIndex[og_order.Length + SurvivorMods.Count];
            for (int index = 0; index < Order.Length;index++)
            {
                Order[index] = (SurvivorIndex)index;
            }
            SurvivorCount = Order.Length;
            return Order.Length>= 24 ? Order.Take(24).ToArray() : Order.ToArray();
        }
        
        
        public static void AddItems()
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

        [Serializable]
        public class ThunderstoreManifest
        {
            public string name;
            public string version_number;
            public string website_url;
            public string description;
        }
        // Token: 0x04000001 RID: 1
        public static List<SurvivorModInfo> SurvivorMods = new List<SurvivorModInfo>();

        // Token: 0x04000002 RID: 2
        public static List<ItemModInfo> ItemMods = new List<ItemModInfo>();
    }
}
