using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Projectiles
{
	public class SunPetal : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SunPetal");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.FlowerPowPetal);
			aiType = ProjectileID.FlowerPowPetal;
		}
	}
}