using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GunfishGenerator))]
public class GunfishGeneratorEditor : Editor {

    GunfishGenerator generator;

    public override void OnInspectorGUI () {
        base.OnInspectorGUI ();

        if (GUILayout.Button("Generate")) {
            generator = (GunfishGenerator)target;

            generator.Generate();
        }
    }
}
