using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Monstroriam.Items.Accessories
{
	[AutoloadEquip(EquipType.Neck)]
	class ShadowflameScarf : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame Scarf");
			Tooltip.SetDefault("+10% summon damage"
			+ "\nSummon attacks will cause [c/A41AE2:Shadowflame debuff]");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 30;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.defense = 5;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<MyPlayer>().shadowflameSummon = true;
			player.minionDamage += 0.10f;
		}

		private static readonly int[] unwantedPrefixes = new int[] { PrefixID.Precise, PrefixID.Lucky, PrefixID.Wild, PrefixID.Rash, PrefixID.Intrepid, PrefixID.Violent};

		public override bool AllowPrefix(int pre)
		{
			if (Array.IndexOf(unwantedPrefixes, pre) > -1)
			{
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShadowScale, 20);
			recipe.AddIngredient(ItemID.ApprenticeScarf);
			recipe.AddTile(TileID.Loom);
			recipe.SetResult(this);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ItemID.TissueSample, 20);
			recipe1.AddIngredient(ItemID.ApprenticeScarf);
			recipe1.AddTile(TileID.Loom);
			recipe1.SetResult(this);
			recipe1.AddRecipe();
		}
	}
}