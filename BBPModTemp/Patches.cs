using HarmonyLib;
using MTM101BaldAPI.Reflection;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Studentcator
{
    [HarmonyPatch(typeof(HudManager))]
    public class HudManagerPatches
    {
        public static Mask StudentcatorMask;
        public static RawImage StudentcatorMaskImage;
        public static Image Studentcator;
        public static bool coming = false;
        public static float timer = 100f;

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void AwakePostfix(HudManager __instance)
        {
            timer = 100f;

            if (!BasePlugin.ready)
            {
                return;
            }

            Transform BaldiTransform = __instance.transform.Find("Baldi");
            if (BaldiTransform == null)
            {
                return;
            }

            GameObject StudentcatorMask_Obj = new GameObject("StudentcatorMask");
            StudentcatorMask_Obj.transform.SetParent(BaldiTransform);
            StudentcatorMaskImage = StudentcatorMask_Obj.AddComponent<RawImage>();
            StudentcatorMaskImage.rectTransform.sizeDelta = new Vector2(128f, 128f);
            StudentcatorMaskImage.rectTransform.anchoredPosition = new Vector2(-128f, 0f);

            StudentcatorMask = StudentcatorMask_Obj.AddComponent<Mask>();
            StudentcatorMask.showMaskGraphic = false;

            GameObject Studentcator_Obj = new GameObject("Studentcator");
            Studentcator_Obj.transform.SetParent(StudentcatorMaskImage.transform);
            Studentcator = Studentcator_Obj.AddComponent<Image>();
            Studentcator.rectTransform.sizeDelta = new Vector2(512f, 512f);
            Studentcator.rectTransform.anchoredPosition = new Vector2(-192f, 192f);
            Studentcator.maskable = true;

            SetSprite(1, 3);
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void PlayerInSightPostfix(HudManager __instance)
        {
            if (Studentcator.gameObject == null || __instance == null)
            {
                return;
            }
            if (timer < 0.065f * 8f)
            {
                int flame = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (math.round(timer * 100f / (0.065f * 8f * 10f)) == i)
                    {
                        flame = i;
                        break;
                    }
                }
                int a = flame % 4;
                int b = flame < 4 ? 1 : 0;

                SetSprite(a, 1 + b);
            }
            else if (timer < 0.065f * 8f + 0.475f)
            {
                SetSprite(3, 1);
            }
            else
            {
                if (coming)
                {
                    SetSprite(0, 3);
                }
                else
                {
                    SetSprite(0, 0);
                }
            }
            if (timer < 0.065f * 8f + 0.475f)
            {
                timer += Time.deltaTime;
            }
        }

        [HarmonyPatch("ActivateBaldicator")]
        [HarmonyPrefix]
        public static void ActivateBaldicatorPrefix(bool coming, HudManager __instance)
        {
            if (!BasePlugin.ready)
            {
                return;
            }
            bool hidden = (bool)__instance.ReflectionGetVariable("hidden");
            HudManagerPatches.coming = coming;
            if (!hidden)
            {
                timer = 0f;
            }
        }

        public static void SetSprite(float a = 0f, float b = 0f)
        {
            if (Studentcator.gameObject == null)
            {
                return;
            }
            if (BasePlugin.packs.Count <= 0)
            {
                Studentcator.color = Color.clear;
            }
            else
            {
                Studentcator.sprite = BasePlugin.packs[BasePlugin.Instance.ConfigPackIndex.Value];
                Studentcator.color = Color.white;
            }

            Studentcator.rectTransform.anchoredPosition = new Vector2(192f - 128f * a, 192f - 128f * b);
        }
    }
}
