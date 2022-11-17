using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), id: "S", displayName:"SceneViewer")]
[Icon("Assets/Sprites/Icons/unity_scene.png")]
public class SceneViewerEditor : Overlay
{

    private const string ID_SCENE_VIEWER_OVERLAY = "sceneViewerOverlay";
    public override VisualElement CreatePanelContent()
    {
        var root = new VisualElement();

        root.style.width = new StyleLength(new Length(120, LengthUnit.Pixel));
        
        for (int i = 0; i < EditorSceneManager.sceneCountInBuildSettings; i++)
        {
            int index = i;
            
            var sceneButton = new Button(() =>
            {
                EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(index));
            })
            {
                text = Path.GetFileName(SceneUtility.GetScenePathByBuildIndex(index))
            };

            root.Add(sceneButton);
        }

        return root;
    }
}
