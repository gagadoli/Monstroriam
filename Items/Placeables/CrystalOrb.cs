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
	public class CrystalOrb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Orb");
			Tooltip.SetDefault("Increases [c/7561BF:max minions/sentries]");
		}

		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 38;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Blue;
			item.createTile = TileType<TCrystalOrb>();
			item.placeStyle = 0;
		}
	}
}

namespace Monstroriam.Buffs
{
	public class CrystalPower : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Crystal Power");
			Description.SetDefault("+1 Minion and +1 Sentry");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			canBeCleared = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxMinions += 1;
			player.maxTurrets += 1;

			if (player.dead)
			{
				player.DelBuff(buffIndex);
			}
		}
	}
}

namespace Monstroriam.Tiles
{
	public class TCrystalOrb : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			dustType = 68;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crystal Orb");
			AddMapEntry(new Color(114, 91, 197), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if (frameX == 0)
			{
				Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("CrystalOrb"));
			}
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				if (!Main.player[Main.myPlayer].dead)
				{
					Main.player[Main.myPlayer].AddBuff(mod.BuffType("CrystalPower"), 800);
				}
			}
		}
	}
}