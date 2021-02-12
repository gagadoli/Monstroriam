using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam.Items.Weapons.Magic
{
	public class GraniteTrident : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoots [c/322E68:Granite Shards] that reduce defense by 5");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults() 
		{
			item.damage = 12;
			item.magic = true;
			item.knockBack = 9f;
			item.useStyle = 5;
			item.mana = 10;
			item.useAnimation = 60;
			item.useTime = 60;
			item.width = 48;
			item.height = 46;
			item.rare = ItemRarityID.Blue;
			item.noMelee = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item1;
			item.value = Item.sellPrice(0, 1, 7, 0);
			item.shoot = mod.ProjectileType("GraniteShard");
			item.shootSpeed = 12f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Granite, 25);
			recipe.AddIngredient(ItemID.Trident);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Buffs
{
	public class GraniteDebuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Granite Shard");
			Description.SetDefault("Lowered Defense");
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.defense > 6) 
			{
				npc.defense = npc.defense - 5;
			}
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class GraniteShard : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Shard");
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.penetrate = 3;
			projectile.hide = true;
			projectile.scale = 0.7f;
		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			// If attached to an NPC, draw behind tiles (and the npc) if that NPC is behind tiles, otherwise just behind the NPC.
			if (projectile.ai[0] == 1f) // or if(isStickingToTarget) since we made that helper method.
			{
				int npcIndex = (int)projectile.ai[1];
				if (npcIndex >= 0 && npcIndex < 200 && Main.npc[npcIndex].active)
				{
					if (Main.npc[npcIndex].behindTiles)
					{
						drawCacheProjsBehindNPCsAndTiles.Add(index);
					}
					else
					{
						drawCacheProjsBehindNPCs.Add(index);
					}

					return;
				}
			}
			// Since we aren't attached, add to this list
			drawCacheProjsBehindProjectiles.Add(index);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			// For going through platforms and such, javlins use a tad smaller size
			width = height = 10; // notice we set the width to the height, the height to 10. so both are 10
			return true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// Inflate some target hitboxes if they are beyond 8,8 size
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			// Return if the hitboxes intersects, which means the javlin collides or not
			return projHitbox.Intersects(targetHitbox);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y); // Play a death sound
			Vector2 usePos = projectile.position; // Position to use for dusts

			// Please note the usage of MathHelper, please use this!
			// We subtract 90 degrees as radians to the rotation vector to offset the sprite as its default rotation in the sprite isn't aligned properly.
			Vector2 rotVector = (projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2(); // rotation vector to use for dust velocity
			usePos += rotVector * 16f;
		}

		// 
		/*
		 * The following showcases recommended practice to work with the ai field
		 * You make a property that uses the ai as backing field
		 * This allows you to contextualize ai better in the code
		 */

		// Are we sticking to a target?
		public bool IsStickingToTarget
		{
			get => projectile.ai[0] == 1f;
			set => projectile.ai[0] = value ? 1f : 0f;
		}

		// Index of the current target
		public int TargetWhoAmI
		{
			get => (int)projectile.ai[1];
			set => projectile.ai[1] = value;
		}

		private const int MAX_STICKY_JAVELINS = 6; // This is the max. amount of javlins being able to attach
		private readonly Point[] _stickingJavelins = new Point[MAX_STICKY_JAVELINS]; // The point array holding for sticking javlins

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			IsStickingToTarget = true; // we are sticking to a target
			TargetWhoAmI = target.whoAmI; // Set the target whoAmI
			projectile.velocity =
				(target.Center - projectile.Center) *
				0.75f; // Change velocity based on delta center of targets (difference between entity centers)
			projectile.netUpdate = true; // netUpdate this javlin
			target.AddBuff(BuffType<Buffs.GraniteDebuff>(), 900);

			projectile.damage = 0; // Makes sure the sticking javlins do not deal damage anymore

			// It is recommended to split your code into separate methods to keep code clean and clear
			UpdateStickyGranite(target);
		}

		/*
		 * The following code handles the javlin sticking to the enemy hit.
		 */
		private void UpdateStickyGranite(NPC target)
		{
			int currentJavelinIndex = 0; // The javlin index

			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (i != projectile.whoAmI // Make sure the looped projectile is not the current javlin
					&& currentProjectile.active // Make sure the projectile is active
					&& currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
					&& currentProjectile.type == projectile.type // Make sure the projectile is of the same type as this javlin
					&& currentProjectile.modProjectile is GraniteShard GraniteProjectile // Use a pattern match cast so we can access the projectile like an ExampleJavlinProjectile
					&& GraniteProjectile.IsStickingToTarget // the previous pattern match allows us to use our properties
					&& GraniteProjectile.TargetWhoAmI == target.whoAmI)
				{
					_stickingJavelins[currentJavelinIndex++] = new Point(i, currentProjectile.timeLeft); // Add the current projectile's index and timeleft to the point array
					if (currentJavelinIndex >= _stickingJavelins.Length)  // If the javlin's index is bigger than or equal to the point array's length, break
						break;
				}
			}

			// Remove the oldest sticky javlin if we exceeded the maximum
			if (currentJavelinIndex >= MAX_STICKY_JAVELINS)
			{
				int oldJavelinIndex = 0;
				// Loop our point array
				for (int i = 1; i < MAX_STICKY_JAVELINS; i++)
				{
					// Remove the already existing javlin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javlin's timeLeft
					if (_stickingJavelins[i].Y < _stickingJavelins[oldJavelinIndex].Y)
					{
						oldJavelinIndex = i; // Remember the index of the removed javlin
					}
				}
				// Remember that the X value in our point array was equal to the index of that javlin, so it's used here to kill it.
				Main.projectile[_stickingJavelins[oldJavelinIndex].X].Kill();
			}
		}

		// Added these 2 constant to showcase how you could make AI code cleaner by doing this
		// Change this number if you want to alter how long the javlin can travel at a constant speed
		private const int MAX_TICKS = 45;

		// Change this number if you want to alter how the alpha changes
		private const int ALPHA_REDUCTION = 25;

		public override void AI()
		{

			UpdateAlpha();
			// Run either the Sticky AI or Normal AI
			// Separating into different methods helps keeps your AI clean
			if (IsStickingToTarget) StickyAI();
			else NormalAI();
		}

		private void UpdateAlpha()
		{
			// Slowly remove alpha as it is present
			if (projectile.alpha > 0)
			{
				projectile.alpha -= ALPHA_REDUCTION;
			}

			// If alpha gets lower than 0, set it to 0
			if (projectile.alpha < 0)
			{
				projectile.alpha = 0;
			}
		}

		private void NormalAI()
		{
			TargetWhoAmI++;

			// For a little while, the javlin will travel with the same speed, but after this, the javlin drops velocity very quickly.
			if (TargetWhoAmI >= MAX_TICKS)
			{
				// Change these multiplication factors to alter the javlin's movement change after reaching maxTicks
				const float velXmult = 0.98f; // x velocity factor, every AI update the x velocity will be 98% of the original speed
				const float velYmult = 0.35f; // y velocity factor, every AI update the y velocity will be be 0.35f bigger of the original speed, causing the javlin to drop to the ground
				TargetWhoAmI = MAX_TICKS; // set ai1 to maxTicks continuously
				projectile.velocity.X *= velXmult;
				projectile.velocity.Y += velYmult;
			}

			// Make sure to set the rotation accordingly to the velocity, and add some to work around the sprite's rotation
			// Please notice the MathHelper usage, offset the rotation by 90 degrees (to radians because rotation uses radians) because the sprite's rotation is not aligned!
			projectile.rotation =
				projectile.velocity.ToRotation() +
				MathHelper.ToRadians(90f);

			// Spawn some random dusts as the javlin travels
			if (Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.height, projectile.width, 14,
					projectile.velocity.X * .2f, projectile.velocity.Y * .2f, 200, Scale: 1.2f);
				dust.velocity += projectile.velocity * 0.3f;
				dust.velocity *= 0.2f;
				dust.noGravity = true;
			}
			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.height, projectile.width, 14,
					0, 0, 254, Scale: 0.3f);
				dust.velocity += projectile.velocity * 0.5f;
				dust.velocity *= 0.5f;
				dust.noGravity = true;
			}
		}

		private void StickyAI()
		{
			// These 2 could probably be moved to the ModifyNPCHit hook, but in vanilla they are present in the AI
			projectile.ignoreWater = true; // Make sure the projectile ignores water
			projectile.tileCollide = false; // Make sure the projectile doesn't collide with tiles anymore
			const int aiFactor = 15; // Change this factor to change the 'lifetime' of this sticking javlin
			projectile.localAI[0] += 1f;

			// Every 30 ticks, the javlin will perform a hit effect
			bool hitEffect = projectile.localAI[0] % 30f == 0f;
			int projTargetIndex = (int)TargetWhoAmI;
			if (projectile.localAI[0] >= 60 * aiFactor || projTargetIndex < 0 || projTargetIndex >= 200)
			{ // If the index is past its limits, kill it
				projectile.Kill();
			}
			else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
			{ // If the target is active and can take damage
			  // Set the projectile's position relative to the target's center
				projectile.Center = Main.npc[projTargetIndex].Center - projectile.velocity * 2f;
				projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
				if (hitEffect)
				{ // Perform a hit effect here
					Main.npc[projTargetIndex].HitEffect(0, 1.0);
				}
			}
			else
			{ // Otherwise, kill the projectile
				projectile.Kill();
			}
		}
	}
}