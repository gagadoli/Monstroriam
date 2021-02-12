using Monstroriam.Items.Placeables;
using Monstroriam.Items.Accessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;
using Monstroriam;

namespace Monstroriam
{
	public class MyWorld : ModWorld
	{
		public override void PostWorldGen()
		{
			int[] itemsToPlaceInSkyChests = {ItemType<SunOrb>()};
			int itemsToPlaceInSkyChestsChoice = 0;
			for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
			{
				Chest chest = Main.chest[chestIndex];
				if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 13 * 36)
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						if (Main.rand.NextBool(10))
						{
							if (chest.item[inventoryIndex].type == ItemID.None)
							{
								chest.item[inventoryIndex].SetDefaults(itemsToPlaceInSkyChests[itemsToPlaceInSkyChestsChoice]);
								itemsToPlaceInSkyChestsChoice = (itemsToPlaceInSkyChestsChoice + 1) % itemsToPlaceInSkyChests.Length;
								break;
							}
						}
					}
				}
			}

			int[] itemsToPlaceInCaveChests = {ItemType<LeafOrb>()};
			int itemsToPlaceInCaveChestsChoice = 0;
			for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
			{
				Chest chest = Main.chest[chestIndex];
				if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 10 * 36)
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						if (Main.rand.NextBool(20)) 
						{
							if (chest.item[inventoryIndex].type == ItemID.None)
							{
								chest.item[inventoryIndex].SetDefaults(itemsToPlaceInCaveChests[itemsToPlaceInCaveChestsChoice]);
								itemsToPlaceInCaveChestsChoice = (itemsToPlaceInCaveChestsChoice + 1) % itemsToPlaceInCaveChests.Length;
								break;
							}
						}
					}
				}
			}
		}
	}
}
