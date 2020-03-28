using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Weapons.Summon
{
	public class SpiderBook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spider Book");
			Tooltip.SetDefault("Summons a [c/DAAA9E:Spider]");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 12;
            item.summon = true;
            item.noMelee = true;
            item.knockBack = 2;
            item.useStyle = 5;
			item.mana = 9;
			item.width = 28;
			item.height = 32;
			item.useTime = 38;
			item.useAnimation = 38;
            item.value = Item.buyPrice(0, 0, 12, 0);
			item.rare = 2;
			item.UseSound = SoundID.Item8;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("SpiderPro");
			item.shootSpeed = 10f;
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Book);
			recipe.AddIngredient(ItemID.Vertebrae, 5);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Bookcases);
			recipe.AddRecipe();
		}
	}
}