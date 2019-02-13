using Eclipse.Managers;

namespace Eclipse.Base.Struct
{
    public class EngineGUIString
    {
        public string ChineseString;
        public string EnglishString;

        public EngineGUIString(string chineseString, string englishString)
        {
            ChineseString = chineseString;
            EnglishString = englishString;
        }

        public override string ToString()
        {
            if (EngineManager.stringSelection == EngineManager.EngineGUIStringSelection.CH)
                return ChineseString;
            else if (EngineManager.stringSelection == EngineManager.EngineGUIStringSelection.EN)
                return EnglishString;
            return EnglishString;
        }
    }
}
