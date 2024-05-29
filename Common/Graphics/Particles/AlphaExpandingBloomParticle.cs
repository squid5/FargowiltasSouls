using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace FargowiltasSouls.Common.Graphics.Particles
{
	public class AlphaExpandingBloomParticle : BaseExpandingParticle
	{
        public override Texture2D MainTexture => ModContent.Request<Texture2D>(ParticleTexturePath + "AlphaBloom").Value;
        public override bool UseAdditiveBlend => false;
        public override bool UseNonPreMultipliedBlend => true;
        public AlphaExpandingBloomParticle(Vector2 position, Vector2 velocity, Color drawColor, Vector2 startScale, Vector2 endScale, int lifetime, bool useExtraBloom = false, Color? extraBloomColor = null)
			: base(position, velocity, drawColor, startScale, endScale, lifetime, useExtraBloom, extraBloomColor)
		{

		}
	}
}
