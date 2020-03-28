using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Weapons.Magic
{
	public class SnowballWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snowball Wand");
			Tooltip.SetDefault("Shoots Enchanted Snowballs");
			Item.staff[item.type] = true; 
		}

		public override void SetDefaults()
		{
			item.damage = 6;
			item.magic = true;
            item.noMelee = true;
            item.useStyle = 5;
            item.knockBack = 3;
            item.mana = 3;
			item.width = 48;
			item.height = 48;
			item.useTime = 26;
			item.useAnimation = 78;
            item.value = Item.buyPrice(0, 0, 0, 40);
            item.value = 40;
			item.rare = 0;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("EnchantedBall");
			item.shootSpeed = 12f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Snowball, 99);
			recipe.AddIngredient(ItemID.FallenStar, 3);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}