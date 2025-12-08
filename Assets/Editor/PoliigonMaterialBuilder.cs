using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class PoliigonMaterialBuilder
{
    private enum MapKind
    {
        Unknown,
        BaseColor,
        Normal,
        Metallic,
        Roughness,
        Occlusion,
        Height
    }

    private sealed class Maps
    {
        public Texture2D BaseColor;
        public Texture2D Normal;
        public Texture2D Metallic;
        public Texture2D Roughness;
        public Texture2D Occlusion;
        public Texture2D Height;
        public string FirstPath;
    }

    [MenuItem("Assets/Create/Poliigon Materials From Selection", priority = 2100)]
    private static void BuildFromSelection()
    {
        var selected = Selection.objects;
        if (selected == null || selected.Length == 0)
        {
            EditorUtility.DisplayDialog("Poliigon Builder", "Select textures or folders containing Poliigon textures.", "OK");
            return;
        }

        var texPaths = new List<string>();
        foreach (var obj in selected)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path))
            {
                continue;
            }

            if (AssetDatabase.IsValidFolder(path))
            {
                texPaths.AddRange(AssetDatabase.FindAssets("t:Texture2D", new[] { path }).Select(AssetDatabase.GUIDToAssetPath));
            }
            else if (obj is Texture2D)
            {
                texPaths.Add(path);
            }
        }

        if (texPaths.Count == 0)
        {
            EditorUtility.DisplayDialog("Poliigon Builder", "No textures found in the selection.", "OK");
            return;
        }

        var groups = new Dictionary<string, Maps>();
        for (int i = 0; i < texPaths.Count; i++)
        {
            var texPath = texPaths[i];
            EditorUtility.DisplayProgressBar("Poliigon Builder", texPath, (float)i / texPaths.Count);
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
            if (tex == null)
            {
                continue;
            }

            var kind = DetectKind(texPath);
            if (kind == MapKind.Unknown)
            {
                continue;
            }

            var key = GetGroupKey(texPath);
            if (!groups.TryGetValue(key, out var maps))
            {
                maps = new Maps { FirstPath = texPath };
                groups[key] = maps;
            }

            switch (kind)
            {
                case MapKind.BaseColor:
                    maps.BaseColor = tex;
                    break;
                case MapKind.Normal:
                    maps.Normal = tex;
                    break;
                case MapKind.Metallic:
                    maps.Metallic = tex;
                    break;
                case MapKind.Roughness:
                    maps.Roughness = tex;
                    break;
                case MapKind.Occlusion:
                    maps.Occlusion = tex;
                    break;
                case MapKind.Height:
                    maps.Height = tex;
                    break;
            }
        }

        EditorUtility.ClearProgressBar();

        if (groups.Count == 0)
        {
            EditorUtility.DisplayDialog("Poliigon Builder", "No Poliigon-style suffixes detected.", "OK");
            return;
        }

        foreach (var pair in groups)
        {
            CreateMaterial(pair.Key, pair.Value);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Poliigon Builder", $"Created/updated {groups.Count} material(s).", "OK");
    }

    private static void CreateMaterial(string key, Maps maps)
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            Debug.LogError("URP Lit shader not found.");
            return;
        }

        var material = new Material(shader);

        if (maps.BaseColor != null)
        {
            material.SetTexture("_BaseMap", maps.BaseColor);
        }

        if (maps.Normal != null)
        {
            EnsureNormalImport(maps.Normal);
            material.SetTexture("_BumpMap", maps.Normal);
            material.EnableKeyword("_NORMALMAP");
        }

        if (maps.Metallic != null)
        {
            EnsureLinearImport(maps.Metallic);
            material.SetTexture("_MetallicGlossMap", maps.Metallic);
            material.SetFloat("_Metallic", 1f);
            material.EnableKeyword("_METALLICSPECGLOSSMAP");
        }

        if (maps.Roughness != null)
        {
            EnsureLinearImport(maps.Roughness);
            material.SetFloat("_Smoothness", 0.5f);
        }

        if (maps.Occlusion != null)
        {
            EnsureLinearImport(maps.Occlusion);
            material.SetTexture("_OcclusionMap", maps.Occlusion);
            material.SetFloat("_OcclusionStrength", 1f);
        }

        if (maps.Height != null)
        {
            EnsureLinearImport(maps.Height);
            material.SetTexture("_ParallaxMap", maps.Height);
            material.SetFloat("_Parallax", 0.005f);
            material.EnableKeyword("_PARALLAXMAP");
        }

        var dir = Path.GetDirectoryName(maps.FirstPath) ?? "Assets";
        var matPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(dir, key + ".mat"));
        AssetDatabase.CreateAsset(material, matPath);
        EditorUtility.SetDirty(material);
    }

    private static MapKind DetectKind(string path)
    {
        var name = Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
        if (HasSuffix(name, new[] { "basecolor", "albedo", "color", "diffuse" })) return MapKind.BaseColor;
        if (HasSuffix(name, new[] { "normalgl", "normal", "nor" })) return MapKind.Normal;
        if (HasSuffix(name, new[] { "metallic", "metalness", "metal", "met" })) return MapKind.Metallic;
        if (HasSuffix(name, new[] { "roughness", "rough", "rgh" })) return MapKind.Roughness;
        if (HasSuffix(name, new[] { "ao", "ambientocclusion", "occ" })) return MapKind.Occlusion;
        if (HasSuffix(name, new[] { "height", "displacement", "disp", "bump" })) return MapKind.Height;
        return MapKind.Unknown;
    }

    private static bool HasSuffix(string name, IEnumerable<string> suffixes)
    {
        foreach (var s in suffixes)
        {
            if (name.EndsWith("_" + s) || name.EndsWith("-" + s) || name.EndsWith(s))
            {
                return true;
            }
        }
        return false;
    }

    private static string GetGroupKey(string path)
    {
        var name = Path.GetFileNameWithoutExtension(path);
        name = StripSuffix(name);
        name = StripResolution(name);
        name = name.TrimEnd('-', '_', ' ');
        return name;
    }

    private static string StripSuffix(string name)
    {
        var suffixes = new[] { "basecolor", "albedo", "color", "diffuse", "normalgl", "normal", "nor", "metallic", "metalness", "metal", "met", "roughness", "rough", "rgh", "ao", "ambientocclusion", "occ", "height", "displacement", "disp", "bump" };
        foreach (var s in suffixes)
        {
            if (name.ToLowerInvariant().EndsWith("_" + s)) return name[..^(s.Length + 1)];
            if (name.ToLowerInvariant().EndsWith("-" + s)) return name[..^(s.Length + 1)];
            if (name.ToLowerInvariant().EndsWith(s)) return name[..^s.Length];
        }
        return name;
    }

    private static string StripResolution(string name)
    {
        var lowered = name.ToLowerInvariant();
        var tokens = new[] { "1k", "2k", "4k", "8k", "16k" };
        foreach (var token in tokens)
        {
            if (lowered.EndsWith("_" + token)) return name[..^(token.Length + 1)];
            if (lowered.EndsWith("-" + token)) return name[..^(token.Length + 1)];
            if (lowered.EndsWith(token)) return name[..^token.Length];
        }
        return name;
    }

    private static void EnsureNormalImport(Texture2D tex)
    {
        var path = AssetDatabase.GetAssetPath(tex);
        if (string.IsNullOrEmpty(path)) return;
        if (AssetImporter.GetAtPath(path) is TextureImporter importer)
        {
            if (importer.textureType != TextureImporterType.NormalMap || importer.sRGBTexture)
            {
                importer.textureType = TextureImporterType.NormalMap;
                importer.sRGBTexture = false;
                importer.SaveAndReimport();
            }
        }
    }

    private static void EnsureLinearImport(Texture2D tex)
    {
        var path = AssetDatabase.GetAssetPath(tex);
        if (string.IsNullOrEmpty(path)) return;
        if (AssetImporter.GetAtPath(path) is TextureImporter importer)
        {
            if (importer.sRGBTexture)
            {
                importer.sRGBTexture = false;
                importer.SaveAndReimport();
            }
        }
    }
}
