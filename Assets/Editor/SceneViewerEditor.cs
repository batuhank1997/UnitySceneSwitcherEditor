using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), id: ID_SCENE_VIEWER_OVERLAY, displayName:"Scene Viewer")]
[Icon("Assets/Sprites/Icons/unity_scene.png")]
public class SceneViewerEditor : Overlay
{
    private const string ID_SCENE_VIEWER_OVERLAY = "sceneViewerOverlay";
 
    public override VisualElement CreatePanelContent()
    {
        var root = new VisualElement
        {
            style =
            {
                width = new StyleLength(new Length(120, LengthUnit.Pixel)),
                fontSize = 10
            }
        };

        for (int i = 0; i < EditorSceneManager.sceneCountInBuildSettings; i++)
        {
            int tempIndex = i;

            var sceneButton = new Button(() =>
            {
                if (EditorSceneManager.GetActiveScene().isDirty)
                {
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                }

                EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(tempIndex));
            });
            
            string fileName = Path.GetFileName(SceneUtility.GetScenePathByBuildIndex(tempIndex));

            sceneButton.text = fileName.Substring(0, fileName.Length - 6);

            root.Add(sceneButton);
        }

        return root;
    }
}
