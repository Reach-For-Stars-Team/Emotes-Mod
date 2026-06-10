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
        public PointerFollowerBehaviour pointerFollowerBehaviour;
        public void PlayEmote()
        {
            if (!currentEmote) return;
            if (currentEmote.Animation.Value == null && currentEmote.PointerSprite.Value != null) Coroutines.Start(CoHandleCursorOnlyEmote());
            else if (currentEmote.CanMove.Value) Coroutines.Start(CoHandleContinuousEmote());
            else Coroutines.Start(CoHandleIdleEmote(currentEmote.PlayLooped.Value));
            
            if (currentEmote.PointerSprite.Value) pointerFollowerBehaviour.SetActive(true, currentEmote);
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
                    pc.MyPhysics.Animations.Animator.Play(currentEmote.Animation.Value);
                    yield return new WaitForSeconds(currentEmote.Animation.Value.length);
                }
            }
            else yield return new WaitForAnimationFinish(pc.MyPhysics.Animations.Animator, currentEmote.Animation, true, -1);

            pc.cosmetics.gameObject.SetActive(true);
            pointerFollowerBehaviour.SetActive(false);
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
                pc.MyPhysics.Animations.Animator.Play(currentEmote.Animation.Value);
                yield return new WaitForSeconds(currentEmote.Animation.Value.length);
            }

            pc.cosmetics.gameObject.SetActive(true);
            pointerFollowerBehaviour.SetActive(false);
            currentEmote = null!;
            pc.MyPhysics.Animations.PlayIdleAnimation();
            HudManagerPatches.EmoteCanvas.transform.GetChild(1).gameObject.SetActive(false);
            yield break;
        }
        
        public IEnumerator CoHandleCursorOnlyEmote()
        {
            if (pc.AmOwner) HudManagerPatches.EmoteCanvas.transform.GetChild(1).gameObject.SetActive(true);
            while (currentEmote)
            {
                yield return null;
            }
            pointerFollowerBehaviour.SetActive(false);
            currentEmote = null!;
            HudManagerPatches.EmoteCanvas.transform.GetChild(1).gameObject.SetActive(false);
            yield break;
        }


        public void StopEmote()
        {
            pointerFollowerBehaviour.SetActive(false);
            currentEmote = null!;
            pc.MyPhysics.Animations.PlayIdleAnimation();
            pc.cosmetics.gameObject.SetActive(true);
        }
    }
}