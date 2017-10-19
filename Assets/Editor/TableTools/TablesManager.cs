using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ArkCrossEngine
{
    public static class EditorTableManager
    {
        const string EditModeMenuItem = "TableTools/EditMode";

        static bool EditModeState
        {
            get { return EditorPrefs.HasKey(EditModeMenuItem) && EditorPrefs.GetBool(EditModeMenuItem); }
            set { EditorPrefs.SetBool(EditModeMenuItem, value); }
        }

        static EditorTableManager()
        {
            
        }

        public static Data_SceneConfig GetCurrentOpendSceneConfig()
        {
            return CurrentSceneConfig;
        }

        static void ShowNotifyOrLog(string msg)
        {
            if (Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
                EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
            else
                UnityEngine.Debug.Log(msg);
        }

        [MenuItem(EditModeMenuItem, false, 150)]
        public static void BeginEdit()
        {
            EditModeState = !EditModeState;
            Menu.SetChecked(EditModeMenuItem, EditModeState);

            FileReaderProxy.MakeSureAllHandlerRegistered();

            // UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += OnSceneOpenedCallback;

            ShowNotifyOrLog(EditModeState ? "BeginEdit" : "EndEdit");
        }

        [MenuItem(EditModeMenuItem, true)]
        public static bool EndEdit()
        {
            Menu.SetChecked(EditModeMenuItem, EditModeState);
            return true;
        }

        public static GameObject AddEmptyObject()
        {
            // find root indicator
            GameObject go = GameObject.Find("TableTools/NPC/Indicator");
            if (go == null)
            {
                go = new GameObject("TableTools/NPC/Indicator");
                go.transform.position = UnityEngine.Vector3.zero;
                go.transform.rotation = UnityEngine.Quaternion.identity;
                go.transform.localScale = UnityEngine.Vector3.one;
            }

            // add new npc to root
            GameObject newObject = new GameObject();
            newObject.transform.SetParent(go.transform);
            if (Camera.current != null)
            {
                newObject.transform.position = Camera.current.transform.position;
            }
            return newObject;
        }

        [MenuItem("TableTools/NPC/AddNPC")]
        public static void AddNPC()
        {
            FileReaderProxy.MakeSureAllHandlerRegistered();

            GameObject newNpc = AddEmptyObject();
            newNpc.AddComponent<EditorIndicator_NPC>();
        }

        [MenuItem("TableTools/NPC/AddRevivePoint")]
        public static void AddRevivePoint()
        {
            FileReaderProxy.MakeSureAllHandlerRegistered();

            GameObject newRevivePoint = AddEmptyObject();
            newRevivePoint.AddComponent<EditorIndicator_RevivePoint>();
        }

        private static List<GameObject> CollectAllNPCInCurrentScene()
        {
            GameObject go = GameObject.Find("TableTools/NPC/Indicator");
            if (go != null)
            {
                Component[] components = go.GetComponentsInChildren<EditorIndicator_NPC>();
                List<GameObject> objects = new List<GameObject>();
                foreach( var c in components )
                {
                    objects.Add(c.gameObject);
                }
                return objects;
            }
            return null;
        }

        private static GameObject CollectRevivePointInCurrentScene()
        {
            GameObject go = GameObject.Find("TableTools/NPC/Indicator");
            if (go != null)
            {
                Component[] components = go.GetComponentsInChildren<EditorIndicator_RevivePoint>();
                if (components.Length > 1)
                {
                    UnityEngine.Debug.LogError("Scene revive point greater than 1.");
                    return components[0].gameObject;
                }
                return components[0].gameObject;
            }
            return null;
        }

        [MenuItem("TableTools/NPC/ExportNPC")]
        public static void ExportAllNPC()
        {
            FileReaderProxy.MakeSureAllHandlerRegistered();

            ReloadSceneConfig();

            string sceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;
            CurrentSceneConfig = FindSceneConfig(sceneName);
            if (CurrentSceneConfig == null)
            {
                UnityEngine.Debug.LogError("Missing scene config info. scene = " + sceneName);
                return;
            }

            string unitFilePath = Application.dataPath + "/StreamingAssets/" + CurrentSceneConfig.m_UnitFile;
            if (!File.Exists(unitFilePath))
            {
                UnityEngine.Debug.LogError("Missing unit file. file = " + unitFilePath);
                return;
            }

            List<GameObject> allNPCs = CollectAllNPCInCurrentScene();
            if (allNPCs == null || allNPCs.Count == 0)
            {
                return;
            }

            GameObject revivePoint = CollectRevivePointInCurrentScene();

            StringBuilder builder = new StringBuilder();

            // append revive point
            if (revivePoint != null)
            {
                UnityEngine.Vector3 pos = revivePoint.transform.position;
                float rot = revivePoint.transform.rotation.eulerAngles.y;
                builder.AppendFormat("{0}\t{1}\t{2}\t{3} {4} {5}\t\t{6}\t{7}\t\t{8}\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n", 20001, 0, 3, pos.x, pos.y, pos.z, rot, "FALSE", 0);
            }

            int baseId = 1001;
            foreach (var npc in allNPCs)
            {
                // Id
                int id = baseId++;
                // LinkId
                int linkId = GetLinkIdByObject(npc);
                // CampId
                int campId = GetCampIdByObject(npc);
                // Position
                UnityEngine.Vector3 pos = npc.transform.position;
                // RotAngle
                float rot = npc.transform.rotation.eulerAngles.y;
                // idle animation set
                string idleAnimSet = GetIdleAnimSetByObject(npc);
                // AI logic
                int aiLogic = GetAILogicByObject(npc);

                builder.AppendFormat("{0}\t{1}\t{2}\t{3} {4} {5}\t\t{6}\t{7}\t{8}\t{9}\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n", id, linkId, campId, pos.x, pos.y, pos.z, rot, "FALSE", idleAnimSet, aiLogic);
            }

            StreamReader rf = new StreamReader(unitFilePath);
            if (rf == null)
            {
                UnityEngine.Debug.LogError("unit file not found.");
                return;
            }

            // header
            string finalUnitFile = rf.ReadLine();
            finalUnitFile += "\n";

            if (revivePoint == null)
            {
                // initial vive point
                finalUnitFile += rf.ReadLine();
                // revive point
                finalUnitFile += rf.ReadLine();
            }
            
            rf.Close();

            StreamWriter wf = new StreamWriter(unitFilePath, false, Encoding.UTF8);
            wf.Write(finalUnitFile);
            wf.Write(builder.ToString());
            wf.Close();

            UnityEngine.Debug.Log("NPC table flushed success.");
        }

        private static int GetLinkIdByObject(GameObject obj)
        {
            EditorIndicator_NPC npc = obj.GetComponentInChildren<EditorIndicator_NPC>();
            if (npc != null)
            {
                return npc.LinkId;
            }

            return 0;
        }

        private static int GetCampIdByObject(GameObject obj)
        {
            EditorIndicator_NPC npc = obj.GetComponentInChildren<EditorIndicator_NPC>();
            if (npc != null)
            {
                return npc.CampId;
            }

            return 0;
        }

        private static string GetIdleAnimSetByObject(GameObject obj)
        {
            EditorIndicator_NPC npc = obj.GetComponentInChildren<EditorIndicator_NPC>();
            if (npc != null)
            {
                return npc.IdleAnimSet;
            }

            return "";
        }

        private static int GetAILogicByObject(GameObject obj)
        {
            EditorIndicator_NPC npc = obj.GetComponentInChildren<EditorIndicator_NPC>();
            if (npc != null)
            {
                return npc.AILogic;
            }

            return 0;
        }

        private static void OnSceneOpenedCallback(UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
        {
            if (mode != UnityEditor.SceneManagement.OpenSceneMode.Single)
            {
                return;
            }

            if (string.IsNullOrEmpty(scene.name))
            {
                return;
            }

            CurrentOpenedSceneName = scene.name;
        }

        private static void ReloadSceneConfig()
        {
            string sceneConfigPath = Application.dataPath + "/StreamingAssets/Public/Scenes/SceneConfig.txt";
            SceneConfigProvider.Instance.Clear();
            SceneConfigProvider.Instance.Load(sceneConfigPath, "");
        }

        private static Data_SceneConfig FindSceneConfig(string name)
        {
            var allScenes = SceneConfigProvider.Instance.GetAllSceneConfig();
            foreach( var scene in allScenes )
            {
                Data_SceneConfig sceneConfig = scene.Value as Data_SceneConfig;
                if (sceneConfig.m_ClientSceneFile.ToLower() == name.ToLower())
                {
                    return sceneConfig;
                }
            }
            return null;
        }

        private static string CurrentOpenedSceneName = "";
        private static Data_SceneConfig CurrentSceneConfig;
    }
}