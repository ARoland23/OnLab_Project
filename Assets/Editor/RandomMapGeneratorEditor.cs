using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractMapGenerator),true)]
public class RandomMapGeneratorEditor : Editor
{
    AbstractMapGenerator generator;

    private void Awake()
    {
        generator = (AbstractMapGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate Map"))
        {
            generator.GenerateMap();
        }
    }
}
