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
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			item.damage = 5; //projectile damage
			item.magic = true; //does magic damage
			item.mana = 2; //mana cost
			item.width = 46; //sprite size
			item.height = 46; //sprite size
			item.useTime = 45;
			item.useAnimation = 45;
			item.useStyle = 5; //staff style
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 2; //almost no knockback
			item.value = 300; //3 silver coins
			item.rare = 0; //very bad
			item.UseSound = SoundID.Item8;
			item.autoReuse = false; //auto attack
			item.shoot = mod.ProjectileType("SunPetal"); //custom projectile
			item.shootSpeed = 12f;
		}

		public override void AddRecipes() //item's recipe
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CopperShortsword); //materials
			recipe.AddIngredient(ItemID.Sunflower);
			recipe.SetResult(this); //craft result = this item
			recipe.AddTile(TileID.WorkBenches); //need to be near this in game
			recipe.AddRecipe();
		}
	}
}