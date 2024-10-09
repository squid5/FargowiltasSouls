using FargowiltasSouls.Content.Buffs.Masomode;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.NPCMatching;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.NPCs.EternityModeNPCs.VanillaEnemies.BloodMoon
{
    public class EvilCritters : EModeNPCBehaviour
    {
        public override NPCMatcher CreateMatcher() => new NPCMatcher().MatchTypeRange(
            NPCID.CorruptBunny,
            NPCID.CrimsonBunny,
            NPCID.CorruptGoldfish,
            NPCID.CrimsonGoldfish,
            NPCID.CorruptPenguin,
            NPCID.CrimsonPenguin
        );

        public override void AI(NPC npc)
        {
            base.AI(npc);

            if (npc.type == NPCID.CorruptPenguin || npc.type == NPCID.CrimsonPenguin)
            {
                //npc.waterMovementSpeed = 2.5f;

                if (npc.wet)
                {
                    npc.AddBuff(BuffID.Wet, 180);
                }

                if (npc.wet || npc.HasBuff(BuffID.Wet))
                {
                    float speedToAdd = 1.5f;
                    Vector2 newPos = npc.position + npc.velocity * speedToAdd;

                    if (!Collision.SolidCollision(newPos, npc.width, npc.height))
                        npc.position = newPos;

                    
                }
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            base.OnHitPlayer(npc, target, hurtInfo);

            target.AddBuff(ModContent.BuffType<SqueakyToyBuff>(), 120);
        }
    }
}
