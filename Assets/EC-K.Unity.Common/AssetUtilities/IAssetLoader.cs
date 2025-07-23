using UnityEngine;
using Cysharp.Threading.Tasks;

namespace EC2K.Unity.Common.Asset
{
    /// <summary>
    /// Defines an interface for asynchronous asset loading and releasing.
    /// </summary>
    public interface IAssetLoader
    {
        /// <summary>
        /// Asynchronously loads an asset from the specified path.
        /// </summary>
        /// <typeparam name="T">The type of the asset to load.</typeparam>
        /// <param name="path">The path (address) of the asset.</param>
        /// <returns>A UniTask that resolves to the loaded asset.</returns>
        UniTask<T> LoadAssetAsync<T>(string path) where T : Object;

        /// <summary>
        /// Releases a previously loaded asset.
        /// </summary>
        /// <typeparam name="T">The type of the asset to release.</typeparam>
        /// <param name="asset">The asset instance to release.</param>
        void ReleaseAsset<T>(T asset) where T : Object;
    }
}
