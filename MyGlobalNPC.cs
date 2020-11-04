using Monstroriam.Items.Placeables;
using Monstroriam.Items.Accessories;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace Monstroriam
{
	public class MyGlobalNPC : GlobalNPC
	{
		public override void NPCLoot(NPC npc)
		{
			if (!npc.boss && npc.lifeMax > 1 && npc.damage > 0 && !npc.friendly)
			{
				if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneOldOneArmy)
				{
					if (Main.rand.NextFloat() < .0002f)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DCrystal"));
					}
				}
			}

			/*if (!npc.boss && npc.lifeMax > 1 && npc.damage > 0 && !npc.friendly)
			{
				if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUnderworldHeight)
				{
					if (Main.rand.NextFloat() < .0008f)
					{
						Item.NewItem(npc.getRect(), mod.ItemType("VulcanoWand"));
					}
				}
			}

			if (npc.type == NPCID.TombCrawlerHead)
			{
					if (Main.rand.NextFloat() < .001f)
					{
						Item.NewItem(npc.getRect(), mod.ItemType("TombCrawlerStaff"));
					}
			}*/
		}

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
			if (type == NPCID.Dryad) 
			{
				if (Main.LocalPlayer.HasItem(ItemID.DefenderMedal))
				{
					shop.item[nextSlot].SetDefaults(ItemType<LeafOrb>());
					shop.item[nextSlot].shopCustomPrice = 10;
					shop.item[nextSlot].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					nextSlot++;
				}
			}

			if (type == NPCID.WitchDoctor)
			{
				if (Main.LocalPlayer.HasItem(ItemID.DefenderMedal))
				{
					shop.item[nextSlot].SetDefaults(ItemType<TreeGuardianAmulet>());
					shop.item[nextSlot].shopCustomPrice = 20;
					shop.item[nextSlot].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					nextSlot++;
				}

				shop.item[nextSlot].SetDefaults(ItemID.PygmyNecklace);
				nextSlot++;
			}
		}

		public override bool InstancePerEntity => true;

		public bool SlowBuff;

		public override void ResetEffects(NPC npc)
		{
			SlowBuff = false;
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (SlowBuff)
			{	
				if (!npc.boss)
                {
					npc.velocity.X = npc.velocity.X / 2;
					npc.velocity.Y = npc.velocity.Y / 2;
				}
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (SlowBuff)
			{
				if (!npc.boss)
				{
					if (Main.rand.Next(4) < 1)
					{
						int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 28, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 5, default(Color), 0.9f);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].noLight = true;
						Main.dust[dust].velocity *= 1f;
						Main.dust[dust].velocity.Y -= 0.5f;
						if (Main.rand.NextBool(4))
						{
							Main.dust[dust].noGravity = true;
							Main.dust[dust].noLight = true;
							Main.dust[dust].scale *= 0.5f;
						}
					}
				}
			}
		}
	}
}