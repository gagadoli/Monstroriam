using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Weapons.Magic
{
	public class ForbiddenWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forbidden Wand");
			Tooltip.SetDefault("Shoots Black bolts");
			Item.staff[item.type] = true; 
		}

		public override void SetDefaults()
		{
			item.damage = 80;
			item.magic = true;
            item.noMelee = true;
            item.useStyle = 5;
            item.knockBack = 8;
            item.mana = 45;
			item.width = 40;
			item.height = 40;
			item.useTime = 35;
			item.useAnimation = 35;
            item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = 7;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ProjectileID.BlackBolt;
			item.shootSpeed = 15f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) 
		{
			int numberProjectiles = 3 + Main.rand.Next(6); 
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