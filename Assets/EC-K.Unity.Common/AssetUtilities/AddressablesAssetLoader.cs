using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace EC2K.Unity.Common.Asset
{
    /// <summary>
    /// An implementation of <see cref="IAssetLoader"/> that utilizes Unity Addressables
    /// for asynchronous asset loading and management.
    /// </summary>
    public class AddressablesAssetLoader : IAssetLoader
    {
        /// <summary>
        /// Asynchronously loads an asset from the specified Addressables path.
        /// </summary>
        /// <typeparam name="T">The type of the asset to load.</typeparam>
        /// <param name="path">The Addressables path (address) of the asset.</param>
        /// <returns>A UniTask that resolves to the loaded asset, or null if loading fails.</returns>
        public async UniTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Addressable asset path cannot be null or empty for asynchronous loading.");
                return null;
            }

            Debug.Log($"[AddressablesAssetLoader] Starting async load for: '{path}'...");

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);

            try
            {
                await handle.ToUniTask();

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    T loadedAsset = handle.Result;
                    Debug.Log($"[AddressablesAssetLoader] Successfully loaded asset: '{loadedAsset.name}' from path: '{path}'.");
                    return loadedAsset;
                }
                else
                {
                    Debug.LogError($"[AddressablesAssetLoader] Failed to load asset at path: '{path}'. Error: {handle.OperationException?.Message}");
                    if (handle.OperationException != null)
                    {
                        Debug.LogError($"[AddressablesAssetLoader] Stack Trace: {handle.OperationException.StackTrace}");
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AddressablesAssetLoader] An unexpected exception occurred during async loading for path: '{path}'. Error: {ex.Message}");
                Debug.LogError($"[AddressablesAssetLoader] Stack Trace: {ex.StackTrace}");
                return null;
            }
            finally
            {
                // The `finally` block here remains valid for general error handling,
                // even if `Addressables.Release(handle)` is not called here directly.
            }
        }

        /// <summary>
        /// Releases a previously loaded asset, decrementing its reference count within Addressables.
        /// When the reference count reaches zero, the asset may be unloaded from memory.
        /// </summary>
        /// <typeparam name="T">The type of the asset to release.</typeparam>
        /// <param name="asset">The asset instance to release.</param>
        public void ReleaseAsset<T>(T asset) where T : UnityEngine.Object
        {
            if (asset != null)
            {
                Debug.Log($"[AddressablesAssetLoader] Releasing asset: '{asset.name}'.");
                Addressables.Release(asset);
            }
        }
    }
}