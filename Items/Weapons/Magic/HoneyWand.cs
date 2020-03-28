using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Weapons.Magic
{
	public class HoneyWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Honey Wand");
			Tooltip.SetDefault("Shoots honey spores");
			Item.staff[item.type] = true; 
		}

		public override void SetDefaults()
		{
			item.damage = 14;
			item.magic = true;
            item.noMelee = true;
            item.useStyle = 5;
            item.knockBack = 4;
            item.mana = 12;
			item.width = 44;
			item.height = 44;
			item.useTime = 35;
			item.useAnimation = 35;
            item.value = Item.buyPrice(0, 0, 50, 0);
			item.rare = 3;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("HoneyPro");
			item.shootSpeed = 14f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HoneyBucket);
			recipe.AddIngredient(ItemID.HiveWand);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}