using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Monstroriam.Items.Accessories
{	
	[AutoloadEquip(EquipType.Shield)]
    public class CactusShield : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cactus Shield");
			Tooltip.SetDefault("Attackers [c/528711:receive damage back]");
		}

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.value = Item.sellPrice(0, 0, 6, 5);
            item.rare = ItemRarityID.White;
            item.defense = 1;
            item.accessory = true;
        }      

        public override void UpdateAccessory(Player player, bool hideVisual)
        { 
            if (player.thorns < 1f)
			{
				player.thorns = 0.01f;
			}
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            return rand.Next(new int[] { PrefixID.Hard, PrefixID.Guarding, PrefixID.Armored, PrefixID.Warding });
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cactus, 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}