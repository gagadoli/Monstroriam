using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Projectiles.Minions
{
	public class VulcanoSentry : ModProjectile
    {
 
        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 74;  
            projectile.hostile = false;  
            projectile.friendly = false;   
            projectile.ignoreWater = false; 
            projectile.timeLeft = 6800;  
            projectile.penetrate = -1; 
            projectile.tileCollide = true; 
            projectile.sentry = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

		public override void SetStaticDefaults()
		{
		   Main.projFrames[projectile.type] = 4;
        }

        public override void AI()
        {	
			if (++projectile.frameCounter >= 6)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 4)
				{
					projectile.frame = 0;
				}
			}
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
                        int damage = 19; 
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, 15, damage, 0, Main.myPlayer, 0f, 0f);
                        projectile.ai[0] = 0f;
                    }
                }
            }
            projectile.ai[0] += 1f;
        }
    }
}
