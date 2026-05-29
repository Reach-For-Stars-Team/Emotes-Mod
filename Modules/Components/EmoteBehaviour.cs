using System;
using System.Collections;
using EmotesMod.Patches;
using PowerTools;
using Reactor.Utilities;
using UnityEngine;
using UnityEngine.ProBuilder;
using Object = UnityEngine.Object;

namespace EmotesMod.Modules.Components
{
    public class EmoteBehaviour(IntPtr ptr) : MonoBehaviour(ptr)
    {
        public Emote currentEmote;
        public PlayerControl pc;

        public void PlayEmote()
        {
            if (!currentEmote) return;

            if (currentEmote.canMove) Coroutines.Start(CoHandleContinuousEmote());
            else Coroutines.Start(CoHandleIdleEmote(currentEmote.playLooped));
        }

        public IEnumerator CoHandleIdleEmote(bool loop)
        {
            if (pc.AmOwner) HudManagerPatches.EmoteCanvas.transform.GetChild(1).gameObject.SetActive(true);
            pc.cosmetics.gameObject.SetActive(false);
            if (loop)
            {
                Vector2 originalPos = pc.GetTruePosition();
                while (currentEmote && originalPos == pc.GetTruePosition())
                {
                    pc.MyPhysics.Animations.Animator.Play(currentEmote.anim.Value);
                    yield return new WaitForSeconds(currentEmote.anim.Value.length);
                }
            }
            else yield return new WaitForAnimationFinish(pc.MyPhysics.Animations.Animator, currentEmote.anim, true, -1);

            pc.cosmetics.gameObject.SetActive(true);
            currentEmote = null!;
            pc.MyPhysics.Animations.PlayIdleAnimation();
            if (pc.AmOwner) HudManagerPatches.EmoteCanvas.transform.GetChild(1).gameObject.SetActive(false);
            yield break;
        }

        public IEnumerator CoHandleContinuousEmote()
        {
            if (pc.AmOwner) HudManagerPatches.EmoteCanvas.transform.GetChild(1).gameObject.SetActive(true);
            pc.cosmetics.gameObject.SetActive(false);
            while (currentEmote)
            {
                pc.MyPhysics.Animations.Animator.Play(currentEmote.anim.Value);
                yield return new WaitForSeconds(currentEmote.anim.Value.length);
            }

            pc.cosmetics.gameObject.SetActive(true);
            currentEmote = null!;
            pc.MyPhysics.Animations.PlayIdleAnimation();
            HudManagerPatches.EmoteCanvas.transform.GetChild(1).gameObject.SetActive(false);
            yield break;
        }

        public void StopEmote()
        {
            currentEmote = null!;
            pc.MyPhysics.Animations.PlayIdleAnimation();
            pc.cosmetics.gameObject.SetActive(true);
        }
    }
}