using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eclipse.Managers;
using Eclipse.Base;
using Eclipse.Base.Struct;

namespace Eclipse.Components.Language
{
    [AddComponentMenu("Eclipse/Components/Language/Register")]
    public class LanguageRegister : ComponentBase
    {
        [SerializeField] private string Category;
        [SerializeField] private string Label;
        [SerializeField] private List<StringFontBase> stringFonts = new List<StringFontBase>();
        private Text target;

        private void Start()
        {
            target = GetComponent<Text>();
        }

        private void OnEnable()
        {
            target = GetComponent<Text>();
        }

        public void StringUpdate()
        {
            if (!target) return;
            if (!target) FontUpdate();
            string result = StringManager.StringControl.GetString
                (Category, Label, LinkerHelper.ToManager.GetManagerByType<StringManager>().GetLanguageTag());
            if (result != null)
                target.text = result;
            else
                EclipseDebug.Log(2, EclipseDebug.DebugState.Warning, "在國際語言列表中查無此項，Category: "
                    + Category + ", Label: " + Label);

        }

        private void FontUpdate()
        {
            string tag = LinkerHelper.ToManager.GetManagerByType<StringManager>().GetLanguageTag();
            for(int i = 0; i < stringFonts.Count; i++)
            {
                if (tag == stringFonts[i].tag && stringFonts[i].font) target.font = stringFonts[i].font;
            }
        }
    }
}
