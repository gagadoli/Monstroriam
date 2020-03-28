using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Weapons.Magic
{
	public class CursedRod : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Rod");
			Tooltip.SetDefault("Shoots cursed skulls");
			Item.staff[item.type] = true; 
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.magic = true;
            item.noMelee = true;
            item.useStyle = 5;
            item.knockBack = 2;
            item.mana = 15;
			item.width = 40;
			item.height = 40;
			item.useTime = 25;
			item.useAnimation = 25;			
            item.value = Item.buyPrice(0, 16, 20, 0);
			item.rare = 7;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ProjectileID.ClothiersCurse;
			item.shootSpeed = 15f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) 
		{
			int numberProjectiles = 1 + Main.rand.Next(3); 
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15)); 
				
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale; 
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}