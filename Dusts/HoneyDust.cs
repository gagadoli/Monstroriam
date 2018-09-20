using Terraria;
using Terraria.ModLoader;

namespace Monstroriam.Dusts
{
	public class HoneyDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0.5f;
			dust.velocity.Y -= 0.5f;
			dust.noGravity = true;
			dust.noLight = true;
			dust.scale *= 1.5f;
			dust.fadeIn = 2.1f;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.1f;
			dust.scale -= 0.09f;
			if (dust.scale < 0.5f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}
