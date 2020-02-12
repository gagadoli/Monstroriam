using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Placeables
{
	public class SunOrb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun Orb");
			Tooltip.SetDefault("It's just like the sun,but cooler");
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
			item.value = 0;
			item.createTile = mod.TileType("SunOrb");
			item.placeStyle = 0;
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Sunglasses);         
            recipe.AddIngredient(ItemID.FallenStar);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
