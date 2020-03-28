using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Projectiles.Minions
{
	public class SmasherSentry : ModProjectile
    {
 
        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 68;  
            projectile.hostile = false;  
            projectile.friendly = false;   
            projectile.ignoreWater = false; 
            projectile.timeLeft = 6800;  
            projectile.penetrate = -1; 
            projectile.tileCollide = true; 
            projectile.sentry = true;
            projectile.minion = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity.X = 0;
            projectile.velocity.Y = 0;
			return false;
		}
        public override void AI()
        {
			projectile.velocity.Y = 10;

            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
 
                float shootToX = target.position.X + target.width * 0.5f - projectile.Center.X;
                float shootToY = target.position.Y + target.height * 0.5f - projectile.Center.Y;
                float distance = (float)Math.Sqrt(shootToX * shootToX + shootToY * shootToY);

                if (distance < 420f && !target.friendly && target.active)
                {
                    if (projectile.ai[0] > 180f) 
                    {
                        distance = 1.6f / distance;
                        shootToX *= distance * 3;
                        shootToY *= distance * 3;
                        int damage = 10; 
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("SentryStomp"), damage, 0, Main.myPlayer, 0f, 0f);
                        projectile.ai[0] = 0f;
                    }
                }
            }
            projectile.ai[0] += 1f;
        }
    }
}
