using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Monstroriam.Items.Weapons.Magic
{
	public class EtherianStaff : ModItem
	{
		public override void SetStaticDefaults() 
		{		
			Tooltip.SetDefault("Shoots [c/2A9B39:Etherian Shards] at the ground");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults() 
		{
			item.damage = 8;
			item.magic = true;
			item.noMelee = true;
			item.useStyle = 5;
			item.knockBack = 1;
			item.mana = 4;
			item.width = 40;
			item.height = 40;
			item.useTime = 25;
			item.useAnimation = 25;
			item.value = Item.sellPrice(0, 0, 15, 0);
			item.rare = ItemRarityID.White;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("EtherianShard");
			item.shootSpeed = 6f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);

			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DD2EnergyCrystal, 5);
			recipe.AddIngredient(ItemID.Wood, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class EtherianShard : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 1600;
			projectile.aiStyle = 14;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item7, projectile.position);
		}
	}
}