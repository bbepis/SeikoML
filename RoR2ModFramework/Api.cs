using System.Collections.Generic;
using System.Linq;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace SeikoML
{
	public static class Api
	{
		// Ideal if we can get onInventoryChanged event to update this.
		public static ItemIcon[] GetItemIcons()
		{
			return GameObject.FindObjectsOfType<ItemIcon>();
		}

		public static EquipmentIcon[] GetEquipmentIcons()
		{
			return GameObject.FindObjectsOfType<EquipmentIcon>();
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
			if (SurvivorMods.Count == 0)
				return catalog;
			Debug.LogFormat("[ROR2ML] Attempting to load {0} mod survivors.", SurvivorMods.Count);

			foreach (CustomSurvivor survivor in SurvivorMods)
			{
				int index = SurvivorMods.IndexOf(survivor);
				if (survivor.ToReplace > -1)
				{
					Debug.LogFormat("[RoR2ML] Replacing Survivor on Index {1} with survivor {0}...", survivor.ToReplace, survivor.Survivor.displayNameToken);
					catalog[survivor.ToReplace] = survivor.GetSurvivorDef();
				}
				else
				{
					Debug.LogFormat("[ROR2ML] Adding Survivor {0} with Index Order {1})", survivor.Survivor.displayNameToken, VanillaCount + index);
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
			Debug.LogFormat("[ROR2ML] Attempting to load {0} mod items.", ItemMods.Count);

			uint num = 1u;
			foreach (CustomItem itemModInfo in ItemMods)
			{
				Debug.LogFormat("[ROR2ML] Adding mod item... (Name: {0})", itemModInfo.nameTokenString);
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

		public static void RegisterSurvivor(CustomSurvivor survivor)
		{
			SurvivorMods.Add(survivor);
		}

		// Token: 0x04000002 RID: 2
		public static void RegisterItem(CustomItem item)
		{
			ItemMods.Add(item);
		}

		public static void SetLobbySize(int value)
		{
			LobbySize = value;
		}

		private static List<CustomSurvivor> SurvivorMods = new List<CustomSurvivor>();
		private static List<CustomItem> ItemMods = new List<CustomItem>();
		private static int LobbySize { get; set; }
		private static readonly string Version = "v0.0.1";
		private static int SurvivorCount { get; set; }
		private static int VanillaCount { get; set; }
	}
}