using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Common.Graphics.Particles
{
	public class FargoParticleManager : ModSystem
	{
		public override void Load()
		{
			On_Main.DrawProjectiles += DrawParticles_Projectiles;
			On_Main.DrawDust += DrawParticles_Dust;
			On_Main.DrawInfernoRings += DrawParticles_AfterEverything;
			FargoParticle.ActiveParticles = new();

		}

		public override void Unload()
		{
			On_Main.DrawProjectiles -= DrawParticles_Projectiles;
			On_Main.DrawDust -= DrawParticles_Dust;
			On_Main.DrawInfernoRings -= DrawParticles_AfterEverything;
			FargoParticle.ActiveParticles.Clear();
			FargoParticle.ActiveParticles = null;
		}

		private void DrawParticles_Projectiles(On_Main.orig_DrawProjectiles orig, Main self)
		{
			FargoParticle.DrawParticles(Main.spriteBatch, FargoParticleLayers.BeforeProjectiles);
			orig(self);
			FargoParticle.DrawParticles(Main.spriteBatch, FargoParticleLayers.AfterProjectiles);
		}

		private void DrawParticles_Dust(On_Main.orig_DrawDust orig, Main self)
		{
			orig(self);
			FargoParticle.DrawParticles(Main.spriteBatch, FargoParticleLayers.Dust);
		}

		private void DrawParticles_AfterEverything(On_Main.orig_DrawInfernoRings orig, Main self)
		{
			orig(self);
			FargoParticle.DrawParticles(Main.spriteBatch, FargoParticleLayers.AfterEverything);
		}

		public override void PostUpdateDusts() => FargoParticle.UpdateParticles();

		public override void ClearWorld() => FargoParticle.ActiveParticles?.Clear();
	}
	
}
