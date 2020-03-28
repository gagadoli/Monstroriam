using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Projectiles.Minions
{
	public class SentryStomp : ModProjectile
	{

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DD2OgreSmash);
            aiType = ProjectileID.DD2OgreSmash;
			projectile.hostile = false;
			projectile.friendly = true;
            projectile.minion = true;
            projectile.magic = false;
            projectile.melee = false;
            projectile.ranged = false;
        }

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.DD2OgreSmash; }
		}
    }
}