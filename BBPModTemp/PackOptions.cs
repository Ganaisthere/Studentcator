using MTM101BaldAPI.OptionsAPI;
using MTM101BaldAPI.UI;
using TMPro;
using UnityEngine;

namespace Studentcator
{
    public class PackOptions : CustomOptionsCategory
    {
        public TextMeshProUGUI PackText;
        public StandardMenuButton[] SwitchButton = new StandardMenuButton[2];

        public override void Build()
        {
            if (BasePlugin.packs.Count <= 0)
            {
                return;
            }

            PackText = CreateText(
                "PackText",
                BasePlugin.packNames[BasePlugin.Instance.ConfigPackIndex.Value],
                new Vector3(0f, 40f, 0f),
                BaldiFonts.ComicSans24,
                TextAlignmentOptions.Center,
                new Vector2(420f, 360f),
                Color.black);

            SwitchButton[0] = CreateButton(
                SwitchButtonLeftAction,
                base.menuArrowLeft,
                base.menuArrowLeftHighlight,
                "SwitchButtonLeft",
                new Vector3(-120f, 40f, 0f));

            SwitchButton[1] = CreateButton(
                SwitchButtonRightAction,
                base.menuArrowRight,
                base.menuArrowRightHighlight,
                "SwitchButtonRight",
                new Vector3(120f, 40f, 0f));

            UpdatePage();
            BasePlugin.optionsMenuBuilt = true;
        }

        private void SwitchButtonLeftAction()
        {
            int index = BasePlugin.Instance.ConfigPackIndex.Value;
            index--;
            if (index < 0)
            {
                index = BasePlugin.packNames.Count - 1;
            }
            BasePlugin.Instance.ConfigPackIndex.Value = index;
            UpdatePage();
        }

        private void SwitchButtonRightAction()
        {
            int index = BasePlugin.Instance.ConfigPackIndex.Value;
            index++;
            if (index > BasePlugin.packNames.Count - 1)
            {
                index = 0;
            }
            BasePlugin.Instance.ConfigPackIndex.Value = index;
            UpdatePage();
        }

        private void UpdatePage()
        {
            int index = BasePlugin.Instance.ConfigPackIndex.Value;
            if (index < 0)
            {
                index = BasePlugin.packNames.Count - 1;
            }
            if (index > BasePlugin.packNames.Count - 1)
            {
                index = 0;
            }
            BasePlugin.Instance.ConfigPackIndex.Value = index;

            PackText.text = BasePlugin.packNames[BasePlugin.Instance.ConfigPackIndex.Value];
        }
    }
}
