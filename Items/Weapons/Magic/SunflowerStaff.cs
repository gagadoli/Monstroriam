using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Weapons.Magic
{
	public class SunflowerStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunflower Staff");
			Tooltip.SetDefault("Shoots Sunflower Petals");
			Item.staff[item.type] = true; 
		}

		public override void SetDefaults()
		{
			item.damage = 5; 
			item.magic = true;
            item.noMelee = true;
            item.useStyle = 5;
            item.knockBack = 2;
            item.mana = 2; 
			item.width = 46; 
			item.height = 46; 
			item.useTime = 45;
			item.useAnimation = 45;								
            item.value = Item.buyPrice(0, 0, 3, 0);
			item.rare = 0; 
			item.UseSound = SoundID.Item8;
			item.autoReuse = false; 
			item.shoot = mod.ProjectileType("SunPetal"); 
			item.shootSpeed = 12f;
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CopperShortsword); 
			recipe.AddIngredient(ItemID.Sunflower);
			recipe.SetResult(this); 
			recipe.AddTile(TileID.WorkBenches); 
			recipe.AddRecipe();
		}
	}
}