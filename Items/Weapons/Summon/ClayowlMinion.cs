using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Monstroriam.Projectiles;
using Monstroriam.Buffs;

namespace Monstroriam.Items.Weapons.Summon
{
	public class ClayBook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clay Owl Book");
			Tooltip.SetDefault("Summons a [c/A55F55:Clay Owl] to fight for you"
				+ "\nIt [c/A55F55:Slows] enemies in contact"
				+ "\nYou can have only one");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 2;
			item.summon = true;
			item.noMelee = true;
			item.useStyle = 5;
			item.knockBack = 0;
			item.mana = 12;
			item.width = 28;
			item.height = 30;
			item.useTime = 36;
			item.useAnimation = 36;
			item.value = Item.sellPrice(0, 0, 3, 2);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item44;
			item.buffType = BuffType<ClayowlBuff>();
			item.shoot = ProjectileType<Clayowl>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.AddBuff(item.buffType, 2);
			position = Main.MouseWorld;
			return true;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ProjectileType<Clayowl>()] <= 0;

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ClayBlock, 25);
			recipe.AddIngredient(ItemID.Book);
			recipe.AddTile(TileID.Bookcases);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Buffs
{
	public class ClayowlBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Clay owl Minion");
			Description.SetDefault("The Clay Owl will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ProjectileType<Clayowl>()] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}

	public class SlowDebuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Slow");
			Description.SetDefault("You are slower");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.boss)
			{
				npc.velocity.X = npc.velocity.X / 2;
				npc.velocity.Y = npc.velocity.Y / 2;
			}
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class Clayowl : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clay Owl Minion");
			Main.projFrames[projectile.type] = 5;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 40;
			projectile.tileCollide = true;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
			projectile.scale = 0.7f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("SlowDebuff"), 60);

		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			#region Active check
			if (player.dead || !player.active)
			{
				player.ClearBuff(BuffType<ClayowlBuff>());
			}
			if (player.HasBuff(BuffType<ClayowlBuff>()))
			{
				projectile.timeLeft = 2;
			}
			#endregion

			#region General behavior
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 48f;
			float minionPositionOffsetX = (10 + projectile.minionPos * 40) * -player.direction;
			idlePosition.X += minionPositionOffsetX;
			Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
			{
				projectile.position = idlePosition;
				projectile.velocity *= 0.1f;
				projectile.netUpdate = true;
			}

			float overlapVelocity = 0.04f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile other = Main.projectile[i];
				if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width)
				{
					if (projectile.position.X < other.position.X) projectile.velocity.X -= overlapVelocity;
					else projectile.velocity.X += overlapVelocity;

					if (projectile.position.Y < other.position.Y) projectile.velocity.Y -= overlapVelocity;
					else projectile.velocity.Y += overlapVelocity;
				}
			}
			#endregion

			#region Find target
			float distanceFromTarget = 700f;
			Vector2 targetCenter = projectile.position;
			bool foundTarget = false;

			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, projectile.Center);
				if (between < 2000f)
				{
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}
			if (!foundTarget)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy())
					{
						float between = Vector2.Distance(npc.Center, projectile.Center);
						bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
						bool closeThroughWall = between < 100f;
						if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}
			projectile.friendly = foundTarget;
			#endregion

			#region Movement
			float speed = 8f;
			float inertia = 20f;
			if (foundTarget)
			{
				if (distanceFromTarget > 40f)
				{
					Vector2 direction = targetCenter - projectile.Center;
					direction.Normalize();
					direction *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
				}
			}
			else
			{
				if (distanceToIdlePosition > 600f)
				{
					speed = 10f;
					inertia = 60f;
				}
				else
				{
					speed = 4f;
					inertia = 80f;
				}
				if (distanceToIdlePosition > 20f)
				{
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
				else if (projectile.velocity == Vector2.Zero)
				{
					projectile.velocity.X = -0.15f;
					projectile.velocity.Y = -0.05f;
				}
			}
			#endregion

			#region Animation and visuals
			projectile.rotation = projectile.velocity.X * 0.05f;
			int frameSpeed = 5;
			projectile.frameCounter++;
			if (projectile.frameCounter >= frameSpeed)
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
				{
					projectile.frame = 0;
				}
			}

			if (projectile.velocity.X >= 0f)
			{
				projectile.spriteDirection = 1;
			}
			else
			{
				projectile.spriteDirection = -1;
			}
			#endregion
		}
	}

}