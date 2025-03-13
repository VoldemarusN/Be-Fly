using System;
using System.Globalization;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace MyEditor
{
    public class BuildVersionProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        private const string _initialVersion = "0.0";


        public void OnPreprocessBuild(BuildReport report)
        {
            UpdateVersion(FindCurrentVersion());
        }


        private string FindCurrentVersion()
        {
            string[] currentVersion = PlayerSettings.bundleVersion.Split('[', ']');
            return currentVersion.Length <= 1 ? _initialVersion : currentVersion[1];
        }

        private void UpdateVersion(string version)
        {
            if (float.TryParse(version, NumberStyles.Float,
                    CultureInfo.InvariantCulture, out var versionNumber))
            {
                float newVersion = versionNumber + 0.01f;
                string date = DateTime.Now.ToString("d");
                PlayerSettings.bundleVersion =
                    $"Version [{newVersion.ToString("F", CultureInfo.InvariantCulture)}] - {date}";
                Debug.Log(PlayerSettings.bundleVersion);
            }
        }
    }
}