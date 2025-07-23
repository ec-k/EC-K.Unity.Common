using UnityEngine;
using Cysharp.Threading.Tasks;

namespace EC2K.Unity.Common.Asset
{
    /// <summary>
    /// An implementation of <see cref="IAssetLoader"/> that loads assets from Unity's Resources folder.
    /// Note: Resources.LoadAsync does not perform true asynchronous I/O.
    /// </summary>
    public class ResourcesAssetLoader : IAssetLoader
    {
        /// <summary>
        /// Asynchronously loads an asset from the Resources folder.
        /// Note: Resources.LoadAsync performs its work on the main thread and does not utilize asynchronous I/O.
        /// </summary>
        /// <typeparam name="T">The type of the asset to load.</typeparam>
        /// <param name="path">The path of the asset within a Resources folder.</param>
        /// <returns>A UniTask that resolves to the loaded asset.</returns>
        public async UniTask<T> LoadAssetAsync<T>(string path) where T : Object
        {
            // LoadAsync does not actually perform asynchronous I/O, but provides an asynchronous API in UniTask
            var request = Resources.LoadAsync<T>(path);
            await request;
            return request.asset as T;
        }

        /// <summary>
        /// Releases an asset loaded from the Resources folder.
        /// Note: Resources.UnloadUnusedAssets() is the primary way to unload Resources assets globally.
        /// Individual asset release is not directly managed by ResourcesLoader.
        /// </summary>
        /// <typeparam name="T">The type of the asset to release.</typeparam>
        /// <param name="asset">The asset instance to release.</param>
        public void ReleaseAsset<T>(T asset) where T : Object
        {
            // Resources.UnloadUnusedAssets() affects the entire system,
            // so releasing individual assets is not the responsibility of ResourcesLoader.
            Debug.LogWarning($"Individual asset release is not directly supported by ResourcesAssetLoader " +
                             $"for asset: {asset?.name}. Use Resources.UnloadUnusedAssets() for global cleanup.");
        }
    }
}
