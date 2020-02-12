using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam
{
	public class MGlobalNPC : GlobalNPC
	{
		public override void NPCLoot(NPC npc)
		{
			if(npc.type == NPCID.DD2DarkMageT1) //boss drop
			{
				if (Main.rand.NextFloat() < .15f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType("PaperTome"));
				}
			}


			if(npc.type == NPCID.WitchDoctor) //town npc drop
			{
				if (Main.rand.NextFloat() < .55f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType("ToxicStaff"));
				}
			}


			if(npc.type == NPCID.Dryad) //town npc drop
			{
				if (Main.rand.NextFloat() < .55f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType("LeafOrb"));
				}
			}


			if (!npc.boss && npc.lifeMax > 1 && npc.damage > 0 && !npc.friendly) //only non-bosses agressive npcs drop
			{
				if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneOldOneArmy) //biome drop
				{
					if (Main.rand.NextFloat() < .0002f)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType("DCrystal"));
					}
				}
			}


			if(npc.type == NPCID.CursedSkull) //npc drop
			{
				if (Main.rand.NextFloat() < .0015f)
				{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType("CursedRod"));
				}
			}

			if(npc.type == NPCID.Tumbleweed && Main.hardMode) //npc drop
			{
				if (Main.rand.NextFloat() < .0095f)
				{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType("ForbiddenWand"));
				}
			}


			if (!npc.boss && npc.lifeMax > 1 && npc.damage > 0 && !npc.friendly) //only non-bosses agressive npcs drop
			{
				if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUnderworldHeight) //biome drop
				{
					if (Main.rand.NextFloat() < .0008f)
					{
						Item.NewItem(npc.getRect(), ItemType("VulcanoWand"));
					}
				}
			}

			if (npc.type == NPCID.TombCrawlerHead)
			{
					if (Main.rand.NextFloat() < .001f)
					{
						Item.NewItem(npc.getRect(), ItemType("TombCrawlerStaff"));
					}
			}

			if (Main.hardMode && !npc.boss && npc.lifeMax > 1 && npc.damage > 0 && !npc.friendly)
			{
				if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneHoly)
				{
					if (Main.rand.NextFloat() < .0002f)
					{
						Item.NewItem(npc.getRect(), ItemType("CrystalWand"));
					}
				}
			}


			if (Main.hardMode && !npc.boss && npc.lifeMax > 1 && npc.damage > 0 && !npc.friendly)
			{
				if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCorrupt)
				{
					if (Main.rand.NextFloat() < .0009f)
					{
						Item.NewItem(npc.getRect(), ItemType("ShadowflameScarf"));
					}
				}
			}
		}
	}
}