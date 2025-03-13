using UnityEditor;

namespace MyEditor
{
    public static class DefinesUtility
    {
        private const string BASIC_DEFINES = "TMP_PRESENT, UNITY_POST_PROCESSING_STACK_V2, USE_PHYSICS2D, DOTWEEN, ODIN_INSPECTOR";

        [MenuItem("Defines/STEAM")]
        private static void Defines_STEAM()
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, BASIC_DEFINES + ", STEAM_INTEGRATION");
        }


        [MenuItem("Defines/VKPLAY")]
        private static void Defines_VKPLAY()
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, BASIC_DEFINES + ", VKPLAY_INTEGRATION");
        }

        [MenuItem("Defines/REMOVE_DEPENDENCES")]
        private static void Defines_REMOVE()
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, BASIC_DEFINES);
        }
    }
}