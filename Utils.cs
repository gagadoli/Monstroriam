using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace Monstroriam
{
    public static class Utils
    {
        public static Vector2 RandomInsideUnitCircle()
        {
            double val = Main.rand.NextFloat() * Math.PI * 2f;
            return new Vector2((float)Math.Cos(val), (float)Math.Sin(val));
        }

        public static Vector2 FindSentrySpawnSpot(Player player, Vector2 worldStartPosition, int yOffset)
        {
            int tileX = (int)worldStartPosition.X / 16;
            int tileY = (int)worldStartPosition.Y / 16;

            if (player.gravDir == -1f)
            {
                tileY = (int)(Main.screenPosition.Y + Main.screenHeight - Main.mouseY) / 16;
            }

            for (; tileY < Main.maxTilesY - 10
                && Main.tile[tileX, tileY] != null
                && !WorldGen.SolidTile2(tileX, tileY)
                && Main.tile[tileX - 1, tileY] != null
                && !WorldGen.SolidTile2(tileX - 1, tileY)
                && Main.tile[tileX + 1, tileY] != null
                && !WorldGen.SolidTile2(tileX + 1, tileY); tileY++)
            {
            }
            tileY--;

            return new Vector2(worldStartPosition.X, tileY * 16 - yOffset);
        }

        public static bool SentryFindTarget(Projectile projectile, out Vector2 target, out float targetDistance)
        {
            bool hasTarget = false;
            float targetX = projectile.Center.X;
            float targetY = projectile.Center.Y;
            float distanceToTarget = 1000f;
            NPC ownerMinionAttackTargetNPC11 = projectile.OwnerMinionAttackTargetNPC;

            if (ownerMinionAttackTargetNPC11 != null && ownerMinionAttackTargetNPC11.CanBeChasedBy(projectile))
            {
                float targetCenterX = ownerMinionAttackTargetNPC11.position.X + ownerMinionAttackTargetNPC11.width / 2;
                float targetCenterY = ownerMinionAttackTargetNPC11.position.Y + ownerMinionAttackTargetNPC11.height / 2;
                float distance = Math.Abs(projectile.position.X + projectile.width / 2 - targetCenterX) + Math.Abs(projectile.position.Y + projectile.height / 2 - targetCenterY);
                if (distance < distanceToTarget && Collision.CanHit(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC11.position, ownerMinionAttackTargetNPC11.width, ownerMinionAttackTargetNPC11.height))
                {
                    distanceToTarget = distance;
                    targetX = targetCenterX;
                    targetY = targetCenterY;
                    hasTarget = true;
                }
            }

            if (!hasTarget)
            {
                for (int num1114 = 0; num1114 < 200; num1114++)
                {
                    if (Main.npc[num1114].CanBeChasedBy(projectile))
                    {
                        float targetCenterX = Main.npc[num1114].position.X + Main.npc[num1114].width / 2;
                        float targetCenterY = Main.npc[num1114].position.Y + Main.npc[num1114].height / 2;
                        float distance = Math.Abs(projectile.position.X + projectile.width / 2 - targetCenterX) + Math.Abs(projectile.position.Y + projectile.height / 2 - targetCenterY);
                        if (distance < distanceToTarget && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num1114].position, Main.npc[num1114].width, Main.npc[num1114].height))
                        {
                            distanceToTarget = distance;
                            targetX = targetCenterX;
                            targetY = targetCenterY;
                            hasTarget = true;
                        }
                    }
                }
            }

            if (hasTarget)
            {
                target = new Vector2(targetX, targetY);
                targetDistance = distanceToTarget;

                return true;
            }

            target = Vector2.Zero;
            targetDistance = 1000f;

            return false;
        }

        public static List<int> ApplyBuffInArea(Vector2 position, float distance, int buffType, int buffDuration)
        {
            List<int> playersAffected = new List<int>();

            for (int i = 0; i < 255; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead)
                {
                    float dist = Vector2.Distance(position, player.Center);
                    if (dist <= distance)
                    {
                        if (player.HasBuff(buffType) && player.buffTime[player.FindBuffIndex(buffType)] > buffDuration)
                        {
                            continue;
                        }

                        player.AddBuff(buffType, buffDuration);

                        playersAffected.Add(i);
                    }
                }
            }

            return playersAffected;
        }

        public static void ProjectilesSpawnEvenSpread(float projectileCount, float coneAngle, Vector2 position, Vector2 speed, int projType, int damage, float knockBack, int owner)
        {
            float rotation = MathHelper.ToRadians(coneAngle);

            for (int i = 0; i < projectileCount; i++)
            {
                Vector2 perturbedSpeed = speed.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (projectileCount - 1)));
                Projectile.NewProjectile(position, perturbedSpeed, projType, damage, knockBack, owner);
            }
        }

        public static void ProjectilesSpawnRandomSpread(float projectileCount, float coneAngle, Vector2 position, Vector2 speed, int projType, int damage, float knockBack, int owner)
        {
            float rotation = MathHelper.ToRadians(coneAngle);

            for (int i = 0; i < projectileCount; i++)
            {
                Vector2 perturbedSpeed = speed.RotatedByRandom(rotation);
                Projectile.NewProjectile(position, perturbedSpeed, projType, damage, knockBack, owner);
            }
        }

    }
}