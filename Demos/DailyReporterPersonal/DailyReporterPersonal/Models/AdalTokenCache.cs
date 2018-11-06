using Microsoft.Identity.Core.Cache;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.IO;
using System.Security.Cryptography;

namespace DailyReporterPersonal.Models {

  class ADALTokenCache : TokenCache {

    public string CacheFilePath { get; }
    public string CacheFilePathV3 { get; }
    private static readonly object FileLock = new object();

    // Initializes the cache against a local file.
    // If the file is already present, it loads its content in the ADAL cache
    public ADALTokenCache() {
      this.AfterAccess = AfterAccessNotification;
      this.CacheFilePath = System.Web.HttpContext.Current.Server.MapPath("~/adalTokenCache.json"); ;
      this.CacheFilePathV3 = System.Web.HttpContext.Current.Server.MapPath("~/adalTokenCacheV3.data"); ;
      this.BeforeAccess = BeforeAccessNotification;
      lock (FileLock) {
        CacheData cacheData = new CacheData();
        cacheData.UnifiedState = ReadFromFileIfExists(CacheFilePath);
        cacheData.AdalV3State = ReadFromFileIfExists(CacheFilePathV3);
        this.DeserializeAdalAndUnifiedCache(cacheData);
      }
    }

    // Empties the persistent store.
    public override void Clear() {
      base.Clear();
      File.Delete(CacheFilePath);
      File.Delete(CacheFilePathV3);
    }


    // Triggered right before ADAL needs to access the cache.
    // Reload the cache from the persistent store in case it changed since the last access.
    void BeforeAccessNotification(TokenCacheNotificationArgs args) {
      lock (FileLock) {
        CacheData cacheData = new CacheData();
        cacheData.UnifiedState = ReadFromFileIfExists(CacheFilePath);
        cacheData.AdalV3State = ReadFromFileIfExists(CacheFilePathV3);
        this.DeserializeAdalAndUnifiedCache(cacheData);
      }
    }

    // Triggered right after ADAL accessed the cache.
    void AfterAccessNotification(TokenCacheNotificationArgs args) {
      // if the access operation resulted in a cache update
      if (this.HasStateChanged) {
        lock (FileLock) {
          // reflect changes in the persistent store
          CacheData cacheData = this.SerializeAdalAndUnifiedCache();
          WriteToFileIfNotNull(CacheFilePath, cacheData.UnifiedState);
          WriteToFileIfNotNull(CacheFilePathV3, cacheData.AdalV3State);
          // once the write operation took place, restore the HasStateChanged bit to false
          this.HasStateChanged = false;
        }
      }
    }

    private byte[] ReadFromFileIfExists(string path) {
      byte[] bytes = (!string.IsNullOrEmpty(path) && File.Exists(path)) ? File.ReadAllBytes(path) : null;
      return bytes;
    }

    private static void WriteToFileIfNotNull(string path, byte[] blob) {
      if (blob != null) {
        File.WriteAllBytes(path, blob);
      }
      else {
        File.Delete(path);
      }
    }
  }
}