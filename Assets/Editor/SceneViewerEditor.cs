using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), id: ID_SCENE_VIEWER_OVERLAY, displayName: "Scene Viewer")]
[Icon("Assets/Sprites/Icons/unity_scene.png")]
public class SceneViewerEditor : Overlay
{
    private const string ID_SCENE_VIEWER_OVERLAY = "sceneViewerOverlay";
    private VisualElement root;

    public override VisualElement CreatePanelContent()
    {
        root = new VisualElement
        {
            style =
            {
                width = new StyleLength(new Length(120, LengthUnit.Pixel)),
                backgroundColor = new StyleColor(Color.black),
                opacity = new StyleFloat(0.85f),
                fontSize = 14
            }
        };

        CreateSceneButtons();

        return root;
    }

    public override void OnCreated()
    {
        EditorBuildSettings.sceneListChanged += CreateSceneButtons;
    }

    public override void OnWillBeDestroyed()
    {
        base.OnWillBeDestroyed();
        EditorBuildSettings.sceneListChanged -= CreateSceneButtons;
    }

    private void CreateSceneButtons()
    {
        root.Clear();

        if (EditorSceneManager.sceneCountInBuildSettings == 0)
        {
            var warningText = new TextElement();
            warningText.text = "No Scenes in Build Settings";
            warningText.style.fontSize = 12;
            warningText.style.color = new StyleColor(Color.red);

            root.Add(warningText);
            return;
        }

        for (int i = 0; i < EditorSceneManager.sceneCountInBuildSettings; i++)
        {
            int tempIndex = i;

            var sceneButton = new Button(() => ButtonCallback(tempIndex));

            string fileName = Path.GetFileName(SceneUtility.GetScenePathByBuildIndex(tempIndex));

            //Removes the extension part of the file name (e.g: "MainScene.unity" -> "MainScene")
            sceneButton.text = fileName.Substring(0, fileName.Length - 6);

            root.Add(sceneButton);
        }
    }

    private void ButtonCallback(int index)
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            int dialogResult = EditorUtility.DisplayDialogComplex(
                "Scene has been modified",
                "Do you want to save the changes you made in the current scene?",
                "Save", "Don't Save", "Cancel");

            switch (dialogResult)
            {
                case 0: //Save and open the new scene
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                    EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(index));
                    break;
                case 1: //Open the new scene without saving current.
                    EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(index));
                    break;
                case 2: //Cancel process (Basically do nothing for now.)
                    break;
                default:
                    Debug.LogWarning("Something went wrong when switching scenes.");
                    break;
            }
        }
        else
        {
            EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(index));
        }
    }
}