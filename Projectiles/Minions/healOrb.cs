using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Projectiles.Minions
{
    public class healOrb : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.minion = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 1200;
            projectile.aiStyle = 1;
            projectile.tileCollide = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.type != NPCID.TargetDummy)
            { 
                Player owner = Main.player[projectile.owner];
                {
                    owner.statLife += 2;
                    owner.HealEffect(2, true);
                }
            }
            target.immune[projectile.owner] = 25;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            double deg = (double)projectile.ai[1]; 
            double rad = deg * (Math.PI / 180); 
            double dist = 64;
            projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width / 2;
            projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height / 2;
            projectile.ai[1] += 3f;

            Vector2 dustPosition = projectile.Center + new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
            Dust dust = Dust.NewDustPerfect(dustPosition, 75, null, 100, Color.Lime, 1.8f);
            dust.velocity *= 0.3f;
            dust.noGravity = true;
        }
    }
}