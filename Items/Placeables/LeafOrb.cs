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
	public class LeafOrb : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases [c/007F0F:max health/mana]");
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
			item.createTile = TileType<TLeafOrb>();
			item.placeStyle = 0;
		}
	}
}

namespace Monstroriam.Buffs
{
	public class LeafPower : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Leaf Power");
			Description.SetDefault("+25 max health and mana");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			canBeCleared = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statLifeMax2 += 25;
			player.statManaMax2 += 25;

			if (player.dead)
			{
				player.DelBuff(buffIndex);
			}
		}
	}
}

namespace Monstroriam.Tiles
{
	public class TLeafOrb : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			dustType = 2;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Leaf Orb");
			AddMapEntry(new Color(0, 127, 15), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if (frameX == 0)
			{
				Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("LeafOrb"));
			}
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				if (!Main.player[Main.myPlayer].dead)
				{
					Main.player[Main.myPlayer].AddBuff(mod.BuffType("LeafPower"), 600);
				}
			}
		}
	}
}