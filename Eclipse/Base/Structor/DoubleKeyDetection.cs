using System.Collections.Generic;
using UnityEngine;

namespace Eclipse.Base.Struct
{
    [System.Serializable]
    public class DoubleKeyDetection
    {
        [SerializeField] private List<DoubleKey> doubleKeys = new List<DoubleKey>();

        [System.Serializable]
        public class DoubleKey
        {
            [SerializeField] private float TimeSet;
            [SerializeField] private KeyCode Key;

            public DoubleKey(float time, KeyCode key)
            {
                TimeSet = time;
                Key = key;
            }

            public void Update(float deltaTime) { TimeSet -= deltaTime; }
            public float GetCurrentTime() { return TimeSet; }
            public KeyCode GetRegisterKey() { return Key; }
        }

        public void UpdateAll(float deltaTime)
        {
            for(int i = 0; i < doubleKeys.Count; i++)
            {
                doubleKeys[i].Update(deltaTime);
            }
            for (int i = 0; i < doubleKeys.Count; i++)
            {
                if (doubleKeys[i].GetCurrentTime() < 0.0f) doubleKeys.RemoveAt(i);
            }
        }

        public void RegisterKey(KeyCode key, float time)
        {
            DoubleKey doubleKey = new DoubleKey(time, key);
            doubleKeys.Add(doubleKey);
        }

        public bool CheckKeyExist(KeyCode key)
        {
            for(int i = 0; i < doubleKeys.Count; i++)
            {
                if (doubleKeys[i].GetRegisterKey() == key) return true;
            }
            return false;
        }

        public void DeleteKey(KeyCode key)
        {
            for (int i = 0; i < doubleKeys.Count; i++)
            {
                if (doubleKeys[i].GetRegisterKey() == key) doubleKeys.RemoveAt(i);
            }
        }
    }
}
