using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Weapons.Summon
{
	public class DemonBook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demon Book");
			Tooltip.SetDefault("Summons a [c/9E4A4A:Demon]");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 20;
            item.summon = true;
            item.noMelee = true;
            item.knockBack = 2;
            item.useStyle = 5;
            item.mana = 15;
			item.width = 28;
			item.height = 32;
			item.useTime = 42;
			item.useAnimation = 42;
            item.value = Item.buyPrice(0, 0, 32, 0);
			item.rare = 4;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("DemoPro");
			item.shootSpeed = 15f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			speedX = new Vector2(speedX, speedY).Length() * (speedX > 0 ? 1 : -1);
			speedY = 0;
			
			Vector2 speed = new Vector2(speedX, speedY);
			speed = speed.RotatedByRandom(MathHelper.ToRadians(5));
			
			speedX = speed.X;
			speedY = speed.Y;
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Book);
			recipe.AddIngredient(ItemID.Fireblossom, 5);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Bookcases);
			recipe.AddRecipe();
		}
	}
}