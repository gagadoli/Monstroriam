using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Accessories
{	
	[AutoloadEquip(EquipType.Shield)]
    public class CactusShield : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cactus Shield");
			Tooltip.SetDefault("Grands 1 Defense"
				+ "\nAttackers receive damage back");
		}

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.value = 560;
            item.rare = 0;
            item.accessory = true;
			item.defense = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cactus, 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        { 
            if (player.thorns < 1f)
			{
				player.thorns = 0.01f;
			}
        }
    }
}