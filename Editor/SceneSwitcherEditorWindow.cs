using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Path = System.IO.Path;

namespace MyEditor
{
    public class SceneSwitcherEditorWindow : EditorWindow
    {
        [MenuItem("Tools/Scene Switcher")]
        private static void OpenWindow() => GetWindow<SceneSwitcherEditorWindow>("Scene Switcher");

        private void OnGUI()
        {
            DrawToolbar();
            DrawSceneList();
        }

        private Vector2 _scrollPosition;

        private void DrawSceneList()
        {
            using (var scrollView = new EditorGUILayout.ScrollViewScope(_scrollPosition))
            {
                _scrollPosition = scrollView.scrollPosition;

                for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                {
                    EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
                    DrawSceneListItem(i, scene);
                }
            }
        }

        private void DrawSceneListItem(int i, EditorBuildSettingsScene scene)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scene.path);

            EditorGUI.BeginDisabledGroup(SceneManager.GetActiveScene().path == scene.path);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(i.ToString(), GUILayout.Width(20));
                GUILayout.Label(new GUIContent(sceneName, scene.path));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Load"))
                    if (EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new[] { SceneManager.GetActiveScene() }))
                        EditorSceneManager.OpenScene(scene.path);
                if (GUILayout.Button("Load Additively"))
                    if (EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new[] { SceneManager.GetActiveScene() }))
                        EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Additive);

                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("..."))
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Locate"), false, () =>
                    {
                        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
                        EditorGUIUtility.PingObject(sceneAsset);
                    });
                    menu.ShowAsContext();
                }
            }
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                EditorGUILayout.LabelField("Scenes In Build");
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Build Settings", EditorStyles.toolbarButton)) OpenBuildSettings();
            }
        }

        private void OpenBuildSettings() => GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
    }
}