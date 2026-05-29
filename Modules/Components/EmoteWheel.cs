using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EmotesMod.Patches;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace EmotesMod.Modules.Components
{
    public class EmoteWheel(IntPtr ptr) : MonoBehaviour(ptr)
    {
        public List<GameObject> pages = new List<GameObject>();
        public Il2CppReferenceField<GameObject> pagesParent;
        public Il2CppReferenceField<TextMeshProUGUI> titleText;
        public int currentPageIndex = 0;
        public void OnClickUp()
        {
            SwitchPage(currentPageIndex + 1);
        }
        public void OnClickDown()
        {
            SwitchPage(currentPageIndex - 1);
        }

        public void SwitchPage(int index)
        {
            try
            {
                var page = pages[index];
                currentPageIndex = index;
                foreach (var p in pages)
                {
                    p.SetActive(p == page);
                }
                titleText.Value.text = $"Select An Emote: \n<size=14>Page {index + 1}/{pages.Count}</size>";
            }
            catch (System.IndexOutOfRangeException)
            {
                Debug.LogError("Invalid page index.");
            }

        }
        void OnEnable()
        {
            if (pages.Count == 0)
            {
                for (int i = 0; i != pagesParent.Value.transform.childCount; i++)
                {
                    pages.Add(pagesParent.Value.transform.GetChild(i).gameObject);
                }
            }
            SwitchPage(0);
            HudManager.Instance.gameObject.SetActive(false);
        }
        void OnDisable()
        {
            HudManager.Instance.gameObject.SetActive(true);
        }
        /*private void Update()
        {
            var player = ReInput.players.GetPlayer(0);
            if (player.GetButtonDown(InputPatches.SwitchPageUpBind.id)) OnClickUp();
            else if (player.GetButtonDown(InputPatches.SwitchPageDownBind.id)) OnClickDown();
        }*/
    }
}