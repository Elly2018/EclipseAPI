using System.Collections.Generic;
using UnityEngine;

namespace Eclipse.Base.Struct
{
    [System.Serializable]
    public class StringCategoryBase
    {
        public string _category_unlocalizeName;
        public List<StringLabelBase> labelBases;

        public StringCategoryBase()
        {
            labelBases = new List<StringLabelBase>();
        }
    }
}
