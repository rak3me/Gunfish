﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class GunGeneratorWindow : EditorWindow {

    static GunGeneratorWindow window;

    static string gunName;
    static Texture2D texture;
    static Sprite sprite;
    static float force;
    static AudioClip fireSound;
    static float shotCooldown;

    static float minCD = 0.1f;
    static float maxCD = 3f;


    [MenuItem("Gunfish/Create new Gun %g")]
    private static void GunInit () {
        window = GetWindow<GunGeneratorWindow>("Gun Generator");
        window.minSize = new Vector2(220f, 140f);
        window.maxSize = new Vector2(2000f, 2000f);

        gunName = "Gun";
        force = 300f;
        shotCooldown = 1f;
    }

    private void OnGUI () {
        gunName = EditorGUILayout.TextField("Name", gunName);
        texture = (Texture2D)EditorGUILayout.ObjectField("Texture", texture, typeof(Texture2D), false);
        force = EditorGUILayout.FloatField("Force", force);
        fireSound = (AudioClip)EditorGUILayout.ObjectField("Fire Sound", fireSound, typeof(AudioClip), false);
        shotCooldown = EditorGUILayout.Slider("Shot Cooldown", shotCooldown, 0.1f, 3f);

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Generate")) {
            FormatTexture();
            Generate();
            Debug.Log("Successfully created Gun! You may see it in Resources/Prefabs/Guns/");

            if (window) {
                window.Close();
            }
        }
        GUILayout.Space(10f);
    }

    void FormatTexture () {
        string path = AssetDatabase.GetAssetPath(texture);
        sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

        //TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
        //TextureImporterSettings settings = new TextureImporterSettings();
        //importer.ReadTextureSettings(settings);
        //settings.readable = true;
        //settings.textureType = TextureImporterType.Sprite;
        //settings.spriteMode = (int)SpriteImportMode.Single;
        ////settings.spritePixelsPerUnit = 128f;
        //importer.SetTextureSettings(settings);
        //texture.Apply();
    }

    void Generate () {
        string path = "Assets/Resources/Prefabs/Guns/" + gunName + ".prefab";

        GameObject gunGO = new GameObject();
        gunGO.name = gunName;

        GameObject gunPivot = new GameObject();
        gunPivot.transform.SetParent(gunGO.transform);
        gunPivot.name = "Sprite";

        gunPivot.transform.localPosition = Vector2.right * texture.width / sprite.pixelsPerUnit * 0.5f;

        SpriteRenderer sr = gunPivot.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
            
        Gun gun = gunGO.AddComponent<Gun>();
        gun.force = force;
        gun.fireSound = fireSound;
        gun.maxShootCD = shotCooldown;

        PrefabUtility.CreatePrefab(path, gunGO);
        DestroyImmediate(gunGO);
    }
}