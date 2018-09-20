using Terraria.ModLoader;

namespace Monstroriam
{
	class Monstroriam : Mod
	{
		public Monstroriam()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}
	}
}
