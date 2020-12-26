using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Monstroriam.Items.Weapons.Magic
{
	public class SunflowerStaff : ModItem
	{
		public override void SetStaticDefaults() 
		{		
			Tooltip.SetDefault("Shoots piercing [c/E5C511:Sunflower Petals]");
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
			item.value = Item.sellPrice(0, 0, 3, 0);
			item.rare = ItemRarityID.White;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("SunPetal");
			item.shootSpeed = 12f;
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			if (player.HasBuff(BuffID.Sunflower))
			{
				mult = 1.2f;
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 35f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
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

namespace Monstroriam.Projectiles
{
	public class SunPetal : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 600;
		}

		public override void AI()
		{
			projectile.rotation += 3;
		}
	}
}