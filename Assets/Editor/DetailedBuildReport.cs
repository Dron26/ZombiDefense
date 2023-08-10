using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DetailedBuildReport : MonoBehaviour
{
    [MenuItem("Build/DetailedBuildReport")]
    public static void MyBuild()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Initial.unity","Assets/Scenes/Menu.unity", "Assets/Scenes/Level.unity" };
        buildPlayerOptions.locationPathName = "DetailedReportBuild/ZombiDefence.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
 
        buildPlayerOptions.options = BuildOptions.DetailedBuildReport;
 
        var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
