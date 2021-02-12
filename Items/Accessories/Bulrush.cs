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
	public class Bulrush : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons 3 [c/1E9652:Dragonflies] to attack foes"
				+ "\nLower enemies jump height on contact");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 16;
			item.height = 40;
			item.value = Item.sellPrice(0, 0, 24, 0);
			item.shoot = ProjectileType<Projectiles.DragonflyProj2>();
			item.buffType = BuffType<Buffs.DragonflyBuff>();
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) 
			{
				player.AddBuff(item.buffType, 60, true);
			}
		}
	}
}

namespace Monstroriam.Buffs
{
	public class DragonflyBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Dragonflies");
			Description.SetDefault("Attack enemies and lower their jump height");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 1;
			player.GetModPlayer<MyPlayer>().dragonFly = true;

			int projId1 = Main.rand.Next(new int[] { ProjectileType<Projectiles.DragonflyProj2>(), ProjectileType<Projectiles.DragonflyProj4>(), ProjectileType<Projectiles.DragonflyProj6>() });

			bool petProjectileNotSpawned1 = player.ownedProjectileCounts[projId1] <= 0;

			if (petProjectileNotSpawned1 && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, projId1, 5, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}

	public class SandDebuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Sand Debuff");
			Description.SetDefault("You cant jump");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.boss)
			{
				if (npc.velocity.Y < 0f)
				{
					npc.velocity.Y = npc.velocity.Y / 2f;
				}
			}
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class DragonflyProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragonfly 2");
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 12;
			projectile.tileCollide = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
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
				player.ClearBuff(BuffType<DragonflyBuff>());
			}
			if (player.HasBuff(BuffType<DragonflyBuff>()))
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
			float distanceFromTarget = 400f;
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
			float speed = 10f;
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

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("SandDebuff"), 60);
		}
	}

	public class DragonflyProj4 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragonfly 4");
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 12;
			projectile.tileCollide = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.scale = 1f;
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
				player.ClearBuff(BuffType<DragonflyBuff>());
			}
			if (player.HasBuff(BuffType<DragonflyBuff>()))
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
			float speed = 10f;
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

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("SandDebuff"), 60);
		}
	}

	public class DragonflyProj6 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragonfly 6");
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 12;
			projectile.tileCollide = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.scale = 1f;
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
				player.ClearBuff(BuffType<DragonflyBuff>());
			}
			if (player.HasBuff(BuffType<DragonflyBuff>()))
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
			float speed = 10f;
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

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("SandDebuff"), 60);
		}
	}
}