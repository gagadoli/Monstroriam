using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Monstroriam;
using Monstroriam.Projectiles;
using Monstroriam.Buffs;

namespace Monstroriam.Items.Accessories
{
	public class TreeGuardianAmulet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tree Guardian Amulet");
			Tooltip.SetDefault("Summons a [c/11DD33:Tree Guardian] that gives defense/damage");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 26;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.defense = 5;
			item.shoot = ProjectileType<Projectiles.TreeGuardian>();
			item.buffType = BuffType<Buffs.TreeGuardian>();
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
	public class TreeGuardian : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Tree Guardian");
			Description.SetDefault("+5 defense and +10% summon damage");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 5;
			player.minionDamage += 0.10f;
			player.buffTime[buffIndex] = 1;
			player.GetModPlayer<MyPlayer>().treeGuardian = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ProjectileType<Projectiles.TreeGuardian>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ProjectileType<Projectiles.TreeGuardian>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class TreeGuardian : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tree Guardian");
			Main.projFrames[projectile.type] = 1;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.DD2PetGato);
			aiType = ProjectileID.DD2PetGato;
			projectile.width = 42;
			projectile.height = 50;
			projectile.scale = 0.5f;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.zephyrfish = false;
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if (player.dead)
			{
				modPlayer.treeGuardian = false;
			}
			if (modPlayer.treeGuardian)
			{
				projectile.timeLeft = 2;
			}
		}
	}
}