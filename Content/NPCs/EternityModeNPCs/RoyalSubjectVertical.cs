using FargowiltasSouls.Content.Bosses.VanillaEternity;
using FargowiltasSouls.Content.Buffs.Masomode;
using FargowiltasSouls.Core;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.NPCs.EternityModeNPCs
{
    public class RoyalSubjectVertical : RoyalSubject
    {
        public override string Texture => "FargowiltasSouls/Content/NPCs/EternityModeNPCs/RoyalSubject";


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            this.ExcludeFromBestiary();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = 0;
            NPC.aiStyle = -1;
            NPC.timeLeft = 60 * 5;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.7 * System.Math.Max(1.0, balance / 2));
            NPC.damage = (int)(NPC.damage * 0.9);
        }

        public override void AI()
        {
            if (NPC.localAI[2] == 0)
            {
                SoundEngine.PlaySound(SoundID.Zombie125, NPC.Center);
                NPC.localAI[2] = 1;
            }
            if (!FargoSoulsUtil.BossIsAlive(ref EModeGlobalNPC.beeBoss, NPCID.QueenBee)
                && !NPC.AnyNPCs(NPCID.QueenBee))
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.checkDead();
                return;
            }
            if (++NPC.ai[0] > 60 * 5)
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.checkDead();
                return;
            }
            NPC.rotation = -NPC.velocity.ToRotation();
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 3)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y >= 3 * frameHeight)
                NPC.frame.Y = 0;
        }
    }
}