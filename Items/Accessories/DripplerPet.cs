using Monstroriam;
using Monstroriam.Projectiles;
using Monstroriam.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam.Items.Accessories
{
	public class DripplerPet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Drippler Rock");
			Tooltip.SetDefault("The Nurse will give a [c/BC3434:Regeneration buff] after healing");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 26;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 5, 0);
			item.shoot = ProjectileType<Projectiles.DripplerPetProj>();
			item.buffType = BuffType<Buffs.DripplerPetBuff>();
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) 
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
}

namespace Monstroriam.Buffs
{
	public class DripplerPetBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Drippler Pet");
			Description.SetDefault("Gives regeneration after nurse heal");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.enemySpawns = true;
			player.buffTime[buffIndex] = 1;
			player.GetModPlayer<MyPlayer>().crimsonPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ProjectileType<Projectiles.DripplerPetProj>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ProjectileType<Projectiles.DripplerPetProj>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class DripplerPetProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Drippler Pet");
			Main.projFrames[projectile.type] = 8;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.DD2PetGato);
			aiType = ProjectileID.DD2PetGato;
			projectile.aiStyle = 66;
			projectile.width = 34;
			projectile.height = 44;
		}

		public override bool? CanHitNPC(NPC npc)
		{
			return false;
		}

		public override void AI()
		{
			if (++projectile.frameCounter >= 12)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 8)
				{
					projectile.frame = 0;
				}
			}

			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if (player.dead)
			{
				modPlayer.crimsonPet = false;
			}
			if (modPlayer.crimsonPet)
			{
				projectile.timeLeft = 2;
			}
		}
	}
}