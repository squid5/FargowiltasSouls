using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace FargowiltasSouls.Content.Bosses.CursedCoffin
{
    public partial class CursedCoffinInactive : ModNPC
    {
        public override bool IsLoadingEnabled(Mod mod) => CursedCoffin.Enabled;

        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;

			NPCID.Sets.MPAllowedEnemies[Type] = true;

            this.ExcludeFromBestiary();
            NPCID.Sets.ImmuneToAllBuffs[Type] = true;
		}
		public const int BaseHP = 3333;
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = BaseHP;
            NPC.defense = 10;
            NPC.damage = 35;
            NPC.knockBackResist = 0f;
            NPC.width = 90;
            NPC.height = 150;
            NPC.boss = false;
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit4; 
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.dontTakeDamage = true;
            NPC.immortal = true;

            //Music = MusicID.OtherworldlyBoss1;
            //SceneEffectPriority = SceneEffectPriority.BossLow;
            NPC.value = 0;
			//NPC.value = Item.buyPrice(0, 2);

        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * balance);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.IsABestiaryIconDummy)
				return true;
			Texture2D bodytexture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawPos = NPC.Center - screenPos;
			SpriteEffects spriteEffects = NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Vector2 origin = new Vector2(bodytexture.Width / 2, bodytexture.Height / 2 / Main.npcFrameCount[NPC.type]);

            spriteBatch.Draw(bodytexture, drawPos, NPC.frame, drawColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);

            return false;
		}
	}
}