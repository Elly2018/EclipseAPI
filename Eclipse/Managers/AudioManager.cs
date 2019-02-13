using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Base.Struct;
using Eclipse.Components;
using Eclipse.Backend;

namespace Eclipse.Managers
{
    [AddComponentMenu("Eclipse/Managers/Audio")]
    public class AudioManager : ManagerBase
    {
        [SerializeField] private Transform AudioParent;
        [SerializeField] private float Default_MusicVolume = 1.0f;
        [SerializeField] private float Default_SFXVolume = 1.0f;
        [SerializeField] private float MusicVolume = 1.0f;
        [SerializeField] private float SFXVolume = 1.0f;
        [SerializeField] private List<AudioAsset> Music = new List<AudioAsset>();
        [SerializeField] private List<AudioAsset> SFX = new List<AudioAsset>();

        private void Start()
        {
            CommandBackend.InputCommand = "audio music " + Default_MusicVolume.ToString();
            CommandBackend.CommandRecordEnter(CommandBackend.CommandEnter());
            CommandBackend.InputCommand = "audio sfx " + Default_SFXVolume.ToString();
            CommandBackend.CommandRecordEnter(CommandBackend.CommandEnter());
            //MusicVolume = Default_MusicVolume;
            //SFXVolume = Default_SFXVolume;
        }

        /* Music functionality */
        /* The worker respones for the audio music control */
        public class MusicControl
        {
            public static List<AudioAsset> GetListMusicAudio()
            {
                AudioManager AM = LinkerHelper.ToManager.GetManagerByType<AudioManager>();
                return AM.GetMusic();
            
}

            public static bool PlayMusic(string musicID, bool deactive = true)
            {
                AudioManager AM = LinkerHelper.ToManager.GetManagerByType<AudioManager>();
                if (deactive) StopAllMusic();
                if (!AM.CheckParent()) return false;
                List<AudioAsset> mMusic = LinkerHelper.ToManager.GetManagerByType<AudioManager>().Music;
                for(int i = 0; i < mMusic.Count; i++)
                {
                    if(mMusic[i].AssetID == musicID && mMusic[i].Asset != null)
                    {
                        AM.SpawnAudio(mMusic[i].Asset, Vector3.zero, true,
                            -1.0f, false, true, AudioRegister.AudioType.Music);
                        return true;
                    }
                }
                EclipseDebug.Log(2, EclipseDebug.DebugState.Warning, "無法播放此音樂ID: " + musicID);
                return false;
            }

            /* Kill all music */
            public static void StopAllMusic()
            {
                AudioManager AM = LinkerHelper.ToManager.GetManagerByType<AudioManager>();
                if (!AM.CheckParent()) return;
                Transform tT = LinkerHelper.ToManager.GetManagerByType<AudioManager>().GetAudioParent();
                for (int i = 0; i < tT.childCount; i++)
                {
                    AudioRegister AR = tT.GetChild(i).GetComponent<AudioRegister>();
                    if (AR && AR.GetAudioType() == AudioRegister.AudioType.Music)
                    {
                        Destroy(tT.GetChild(i).gameObject);
                    }
                }
            }

            public static bool AdjustVolume(float v)
            {
                if (v > 1.0f || v < 0.0f)
                    return false;
                AudioManager AM = LinkerHelper.ToManager.GetManagerByType<AudioManager>();
                AM.SetMusicVolume(v);
                return true;
            }
        }

        /* SFX functionality */
        /* The worker respones for the audio effect control */
        public class SFXControl
        {
            public static List<AudioAsset> GetListSFXAudio()
            {
                AudioManager AM = LinkerHelper.ToManager.GetManagerByType<AudioManager>();
                return AM.GetSFX();
            }

            public static bool PlaySFX(string sfxID, Vector3 position)
            {
                AudioManager AM = LinkerHelper.ToManager.GetManagerByType<AudioManager>();
                if (!AM.CheckParent()) return false;
                List<AudioAsset> mSFX = LinkerHelper.ToManager.GetManagerByType<AudioManager>().SFX;
                for (int i = 0; i < mSFX.Count; i++)
                {
                    if (mSFX[i].AssetID == sfxID && mSFX[i].Asset != null)
                    {
                        AM.SpawnAudio(mSFX[i].Asset, position, false,
                            mSFX[i].Asset.length, true, false, AudioRegister.AudioType.SFX);
                        return true;
                    }
                }
                EclipseDebug.Log(2, EclipseDebug.DebugState.Warning, "無法播放此音效ID: " + sfxID);
                return false;
            }

            public static bool PlaySFX(string sfxID)
            {
                AudioManager AM = LinkerHelper.ToManager.GetManagerByType<AudioManager>();
                if (!AM.CheckParent()) return false;
                List<AudioAsset> mSFX = LinkerHelper.ToManager.GetManagerByType<AudioManager>().SFX;
                for (int i = 0; i < mSFX.Count; i++)
                {
                    if (mSFX[i].AssetID == sfxID && mSFX[i].Asset != null)
                    {
                        AM.SpawnAudio(mSFX[i].Asset, new Vector3(0, 0, 0), true,
                            mSFX[i].Asset.length, true, false, AudioRegister.AudioType.SFX);
                        return true;
                    }
                }
                EclipseDebug.Log(2, EclipseDebug.DebugState.Warning, "無法播放此音效ID: " + sfxID);
                return false;
            }

            public static bool AdjustVolume(float v)
            {
                if (v > 1.0f || v < 0.0f)
                    return false;
                AudioManager AM = LinkerHelper.ToManager.GetManagerByType<AudioManager>();
                AM.SetSFXVolume(v);
                return true;
            }
        }

        /* Simple spawn audio source by the */
        /* clip / duration / autodestory flag / loop flag */
        #region For Control Function
        public void SpawnAudio(AudioClip _ac, Vector3 pos, bool Is2D
            , float _du, bool _au, bool loop, AudioRegister.AudioType _type)
        {
            if (!CheckParent()) return;
            Transform tT = LinkerHelper.ToManager.GetManagerByType<AudioManager>().GetAudioParent();
            GameObject G = new GameObject("Eclipse Audio Source: " + _ac.name);
            G.transform.SetParent(tT);
            G.transform.position = pos;
            AudioSource AC = G.AddComponent<AudioSource>();
            AC.loop = loop;
            AC.spatialBlend = Is2D ? 0 : 1;
            AC.clip = _ac;
            AC.Play();
            AudioRegister AR = G.AddComponent<AudioRegister>();
            AR.SetAudioType(_type);
            AR.InitliazeAudioRegister(_du, _au);
            EclipseDebug.Log(2, EclipseDebug.DebugState.Log, "音源生成成功，音源檔案: " + _ac.name);
        }

        /* Check if the parent exsits */
        public bool CheckParent()
        {
            if (LinkerHelper.ToManager.GetManagerByType<AudioManager>().GetAudioParent() == null)
                EclipseDebug.Log(2, EclipseDebug.DebugState.Warning, "音源管理腳本尚未註冊音源放置區");
            return LinkerHelper.ToManager.GetManagerByType<AudioManager>().GetAudioParent();
        }
        #endregion

        /* The getter and setter */
        #region Audio Clips Getter And Setter
        public List<AudioAsset> GetMusic() { return Music; }
        public List<AudioAsset> GetSFX() { return SFX; }
        public float GetMusicVolume() { return MusicVolume; }
        public float GetSFXVolume() { return SFXVolume; }
        public Transform GetAudioParent() { return AudioParent; }

        public void SetMusic(List<AudioAsset> buffer) { Music = buffer; }
        public void SetSFX(List<AudioAsset> buffer) { SFX = buffer; }
        public void SetMusicVolume(float v) { MusicVolume = v; }
        public void SetSFXVolume(float v) { SFXVolume = v; }
        #endregion
    }

    [CustomEditor(typeof(AudioManager))]
    [CanEditMultipleObjects]
    public class AudioManagerEditor : Editor
    {
        public bool bMusic;
        public bool bSFX;

        public override void OnInspectorGUI()
        {
            AudioManager Target = (AudioManager)target;
            GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("音源管理腳本", "Audio Manager"), serializedObject);
            /* Return */
            EditorHelper.EditorOption.ReturnToEngineManager(new EngineGUIString("你能透過呼叫 ID 的方式去控制音樂", "You Can Calling ID To Control Audio."));
            /* Audio parent register */
            #region Audio Parent
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("音源放置區","Audio Root Object").ToString(), skinT);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AudioParent"), GUIContent.none);
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            #endregion
            /* Audio variable setting */
            #region Audio Setting
            EditorGUILayout.BeginVertical("GroupBox");
            serializedObject.FindProperty("MusicVolume").floatValue =
                EditorGUILayout.Slider(new EngineGUIString("音樂預設值", "Music Default").ToString(), serializedObject.FindProperty("Default_MusicVolume").floatValue, 0, 1.0f);
            serializedObject.FindProperty("SFXVolume").floatValue =
                EditorGUILayout.Slider(new EngineGUIString("音效預設值", "SFX Default").ToString(), serializedObject.FindProperty("Default_SFXVolume").floatValue, 0, 1.0f);
            EditorGUILayout.EndVertical();
            #endregion
            /* Music Register*/
            #region Music Register
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new EngineGUIString("音樂音源操作", "Music Register").ToString(), skinT);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new EngineGUIString("加入新的音樂", "Add New Slot").ToString(), GUILayout.Height(35)))
            {
                List<AudioAsset> buffer = Target.GetMusic();
                buffer.Add(new AudioAsset("Empty", null));
                Target.SetMusic(buffer);
            }
            if (GUILayout.Button(new EngineGUIString("刪除最後的音樂", "Delete Last Slot").ToString(), GUILayout.Height(35)))
            {
                List<AudioAsset> buffer = Target.GetMusic();
                if (buffer.Count > 0) buffer.RemoveAt(buffer.Count - 1);
                Target.SetMusic(buffer);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
            ListIterator("Music", new EngineGUIString("音樂", "Music").ToString(), ref bMusic);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            #endregion
            /* SFX Register*/
            #region SFX Register
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new EngineGUIString("音效音源操作", "SFX Register").ToString(), skinT);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new EngineGUIString("加入新的音效", "Add New Slot").ToString(), GUILayout.Height(35)))
            {
                List<AudioAsset> buffer = Target.GetSFX();
                buffer.Add(new AudioAsset("Empty", null));
                Target.SetSFX(buffer);
            }
            if (GUILayout.Button(new EngineGUIString("刪除最後的音效", "Delete Last Slot").ToString(), GUILayout.Height(35)))
            {
                List<AudioAsset> buffer = Target.GetSFX();
                if (buffer.Count > 0) buffer.RemoveAt(buffer.Count - 1);
                Target.SetSFX(buffer);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
            ListIterator("SFX", new EngineGUIString("音效", "SFX").ToString(), ref bSFX);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            #endregion
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }

        public void ListIterator(string listName, string tagName, ref bool fold)
        {
            //List object
            SerializedProperty listIterator = serializedObject.FindProperty(listName);
            bool showChildren = EditorGUILayout.Foldout(fold, new GUIContent(tagName));
            fold = showChildren;
            listIterator.Next(true);
            if (showChildren)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                EditorGUI.indentLevel++;
                for (int i = 0; i < listIterator.arraySize; i++)
                {
                    SerializedProperty elementProperty = listIterator.GetArrayElementAtIndex(i);
                    showChildren = EditorGUILayout.PropertyField(elementProperty);
                    if (showChildren)
                    {
                        EditorGUILayout.BeginVertical("GroupBox");
                        EditorGUI.indentLevel++;
                        elementProperty.Next(true);
                        EditorGUILayout.PropertyField(elementProperty);
                        elementProperty.Next(false);
                        EditorGUILayout.PropertyField(elementProperty);
                        EditorGUI.indentLevel--;
                        EditorGUILayout.EndVertical();
                    }
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
        }
    }
}
