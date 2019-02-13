using System.Collections.Generic;
using UnityEngine;

namespace Eclipse.Base.Struct
{
    [System.Serializable]
    public class StringLabelBase
    {
        public string _label_unlocalizeName;
        public List<LabelDataBase> _string;

        public StringLabelBase()
        {
            _string = new List<LabelDataBase>();
        }
    }
}
