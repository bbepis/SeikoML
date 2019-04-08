using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;
using RoR2;
using BepInEx;
using UnityEngine;

namespace SeikoML
{
    [BepInPlugin("RoR2ModAPI","Mod API", "0.0.1")]
	public static class ModLoader 
	{
		public static void Awake()
		{

            while(!RoR2Application.instance.loaded)
            {
                Task.Delay(1);
            }

			
		}

        internal static int GetSurvivorCount()
        {
            return SurvivorCount;
        }

		public static int GetMaxPlayers()
		{
			return LobbySize;
		}

		public static SurvivorDef[] LoadSurvivors(SurvivorDef[] catalog)
		{

            if (ModLoader.SurvivorMods.Count == 0) return catalog;
			Debug.LogFormat("[ROR2ML] Attempting to load {0} mod survivors.", new object[]
			{
				ModLoader.SurvivorMods.Count
			});

			foreach (CustomSurvivor survivor in ModLoader.SurvivorMods)
			{
				int index = SurvivorMods.IndexOf(survivor);
                if(survivor.ToReplace>-1)
                {
                    Debug.LogFormat("[RoR2ML] Replacing Survivor on Index {1} with survivor {0}...",new object[]
                        {
                            survivor.ToReplace,
                            survivor.Survivor.displayNameToken
                        });
                    catalog[survivor.ToReplace] = survivor.GetSurvivorDef();
                }
                else
                {
                    Debug.LogFormat("[ROR2ML] Adding Survivor {0} with Index Order {1})", new object[]
                    {
                        survivor.Survivor.displayNameToken,
                        VanillaCount+index
                    });
                    catalog[VanillaCount + index] = survivor.GetSurvivorDef();
                }
			}
            return catalog;
		}
		public static SurvivorIndex[] BuildIdealOrder(SurvivorIndex[] og_order)
		{
			VanillaCount = og_order.Length;
			SurvivorIndex[] Order = new SurvivorIndex[og_order.Length + SurvivorMods.Count];
			for (int index = 0; index < Order.Length; index++)
			{
				Order[index] = (SurvivorIndex)index;
			}
			SurvivorCount = Order.Length;
			return Order.Length >= 24 ? Order.Take(24).ToArray() : Order.ToArray();
		}


		private static void AddItems()
		{
			Debug.LogFormat("[ROR2ML] Attempting to load {0} mod items.", new object[]
			{
				ModLoader.ItemMods.Count
			});
			uint num = 1u;
			foreach (CustomItem itemModInfo in ModLoader.ItemMods)
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
		private class ThunderstoreManifest
		{
			public string name;
			public string version_number;
			public string website_url;
			public string description;
		}

        public static void RegisterSurvivor(CustomSurvivor survivor) 
        {
            SurvivorMods.Add(survivor);
        }

		// Token: 0x04000002 RID: 2
        public static void RegisterIten(CustomItem item)
        {
            ItemMods.Add(item);
        }
        public static void SetLobbySize(int value)
        {
            LobbySize = value;
        }

        public static bool Loaded;
		private static List<CustomSurvivor> SurvivorMods = new List<CustomSurvivor>();
		private static List<CustomItem> ItemMods = new List<CustomItem>();
        private static int LobbySize { get; set; }
        private static readonly string Version = "v0.0.1";
        private static int SurvivorCount { get; set; }
        private static int VanillaCount { get; set; }
    }
}
