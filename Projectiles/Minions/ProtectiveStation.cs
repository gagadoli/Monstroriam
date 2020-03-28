using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Projectiles.Minions
{
	public class ProtectiveStation : ModProjectile
    {
 
        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 64;  
            projectile.hostile = false;  
            projectile.friendly = false;   
            projectile.ignoreWater = false; 
            projectile.timeLeft = 36000;  
            projectile.penetrate = -1; 
            projectile.tileCollide = false; 
            projectile.sentry = true;
            projectile.minion = true;
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

            Lighting.AddLight(projectile.position, 0.58f, 0.976f, 0.651f);

            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
 
                float shootToX = target.position.X + target.width * 0.5f - projectile.Center.X;
                float shootToY = target.position.Y + target.height * 0.5f - projectile.Center.Y;
                float distance = (float)Math.Sqrt(shootToX * shootToX + shootToY * shootToY);

                if (distance < 500f && target.active)
                {
                    if (projectile.ai[0] > 600f) 
                    {
                        distance = 1.6f / distance;
                        shootToX *= distance * 4;
                        shootToY *= distance * 4;
                        int damage = 10; 
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("healOrb"), damage, 0, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("damageOrb"), damage, 0, Main.myPlayer, 0f, 0f);
                        projectile.ai[0] = 0f;
                    }
                }
            }
            projectile.ai[0] += 1f;
        }
    }
}
