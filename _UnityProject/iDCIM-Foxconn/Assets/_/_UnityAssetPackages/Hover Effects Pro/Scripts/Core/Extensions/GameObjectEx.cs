using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

namespace HoverEffectsPro
{
    public static class GameObjectEx
    {
        #if UNITY_EDITOR
        public static bool IsSceneObject(this GameObject gameObject)
        {
            var prefabAssetType = PrefabUtility.GetPrefabAssetType(gameObject);
            return prefabAssetType == PrefabAssetType.NotAPrefab;
        }
        #endif

        public static void DestroyComponents<T>(this GameObject gameObject) where T : Component
        {
            var components = gameObject.GetComponents<T>();
            foreach (var comp in components)
                Component.Destroy(comp);
        }

        public static GameObjectType GetGameObjectType(this GameObject gameObject)
        {
            Mesh mesh = gameObject.GetMesh();
            if (mesh != null) return GameObjectType.Mesh;

            Sprite sprite = gameObject.GetSprite();
            if (sprite != null) return GameObjectType.Sprite;

            return GameObjectType.Empty;
        }

        public static List<GameObject> GetAllChildren(this GameObject gameObject)
        {
            Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
            var allChildren = new List<GameObject>(childTransforms.Length);

            foreach (var child in childTransforms)
            {
                if (child.gameObject != gameObject) allChildren.Add(child.gameObject);
            }

            return allChildren;
        }

        public static List<GameObject> GetAllChildrenAndSelf(this GameObject gameObject)
        {
            Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
            var allChildren = new List<GameObject>(childTransforms.Length);

            foreach (var child in childTransforms)
            {
                allChildren.Add(child.gameObject);
            }

            return allChildren;
        }

        public static Mesh GetMesh(this GameObject gameObject)
        {
            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null) return meshFilter.sharedMesh;

            SkinnedMeshRenderer skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null) return skinnedMeshRenderer.sharedMesh;

            return null;
        }

        public static Renderer GetMeshRenderer(this GameObject gameObject)
        {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null) return meshRenderer;

            SkinnedMeshRenderer skinnedRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
            return skinnedRenderer;
        }

        public static Sprite GetSprite(this GameObject gameObject)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return null;

            return spriteRenderer.sprite;
        }
    }
}
