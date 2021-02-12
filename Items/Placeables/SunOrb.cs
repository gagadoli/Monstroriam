using Monstroriam.Tiles;
using Monstroriam.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam.Items.Placeables
{
	public class SunOrb : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases damage[c/FFD800: at the cost of defense]");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Blue;
			item.createTile = TileType<TSunOrb>();
			item.placeStyle = 0;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sunglasses);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Buffs
{
	public class SunPower : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Sun Power");
			Description.SetDefault("+10% damage and +5 armor penetration but -10 defense");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			canBeCleared = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.allDamage += 0.10f;
			player.armorPenetration += 5;
			player.statDefense -= 10;

			if (player.dead)
			{
				player.DelBuff(buffIndex);
			}
		}
	}
}

namespace Monstroriam.Tiles
{
	public class TSunOrb : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLighted[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			dustType = 10;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Sun Orb");
			AddMapEntry(new Color(255, 169, 21), name);
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 1f;
			g = 0.5f;
			b = 0.01f;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if (frameX == 0)
			{
				Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("SunOrb"));
			}
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				if (!Main.player[Main.myPlayer].dead)
				{
					Main.player[Main.myPlayer].AddBuff(mod.BuffType("SunPower"), 600);
				}
			}
		}
	}
}