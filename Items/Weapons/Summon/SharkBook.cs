using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Weapons.Summon
{
	public class SharkBook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shark Book");
			Tooltip.SetDefault("Summons a [c/0922B5:Shark]");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 16;
            item.summon = true;
            item.noMelee = true;
            item.knockBack = 2;
            item.useStyle = 5;
            item.mana = 12;
			item.width = 28;
			item.height = 32;
			item.useTime = 40;
			item.useAnimation = 40;		
            item.value = Item.buyPrice(0, 0, 22, 0);
			item.rare = 3;
			item.UseSound = SoundID.Item8;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("SharkPro");
			item.shootSpeed = 10f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Book);
			recipe.AddIngredient(ItemID.SharkFin, 5);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Bookcases);
			recipe.AddRecipe();
		}
	}
}