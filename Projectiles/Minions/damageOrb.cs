using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Monstroriam.Buffs;

namespace Monstroriam.Projectiles.Minions
{
    public class damageOrb : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.minion = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 3;
            projectile.timeLeft = 500;
            projectile.aiStyle = 1;
            projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.ai[0] += 0.1f;
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.velocity.Y = -oldVelocity.Y;
            }
            Main.PlaySound(SoundID.Item10, projectile.position);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.type != NPCID.TargetDummy)
            {
                Player player = Main.player[projectile.owner];
                Main.player[Main.myPlayer].AddBuff(mod.BuffType("ProtectiveOrb"), 660);
            }
            target.AddBuff(BuffID.DryadsWardDebuff, 120);
        }

        public override void AI()
        {          
            Vector2 dustPosition = projectile.Center + new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
            Dust dust = Dust.NewDustPerfect(dustPosition, 75, null, 100, Color.Lime, 1.8f);
            dust.velocity *= 0.3f;
            dust.noGravity = true;
        }
    }
}