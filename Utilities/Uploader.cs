using uplink.NET.Services;
using uplink.NET.Models;
using anime_climax_api.Models;
using anime_climax_api.Database;
using anime_climax_api.Binding;

namespace anime_climax_api.Utilities;
using System;
using System.IO;
public class Uploader {
    private readonly int duration = 10; //minutes
    private readonly DataContext _db;

    public Uploader(DataContext db) {
        _db = db;
    }

    private long getUnixTimestamp() {
        DateTimeOffset currentTime = DateTimeOffset.UtcNow;
       return currentTime.ToUnixTimeSeconds();
    }

    private DateTime getExpiratonDate() {
        DateTime now = DateTime.Now;
        return now.AddMinutes(duration);
    }

    public async Task<Buckets> PickBucket(float fileSize) {
        try {
            Buckets availableBucket = _db.Buckets.FirstOrDefault(bucket => (fileSize) + bucket.Usage <= bucket.Capacity);
            return availableBucket;
        } catch {
            throw new Exception("No buckets are available");
        }
    }

    public async Task<(String, UploadInfo, MultipartUploadService)> Upload(Buckets bucket, NewClip clip) {
        try {
            Access access = new Access(bucket.Token);
            BucketService bucketService = new BucketService(access);
            await bucketService.EnsureBucketAsync(bucket.BucketName);
            
            MultipartUploadService uploadService = new MultipartUploadService(access);
            long timeStamp = getUnixTimestamp();
            UploadOptions options = new();
            options.Expires = getExpiratonDate();
            
            String objectKey = String.Format("{0}-{1}", timeStamp, clip.File.FileName);
            UploadInfo uploadInfo = uploadService.BeginUploadAsync(bucket.BucketName, objectKey , options).Result;

            return (objectKey, uploadInfo, uploadService);
        } catch {
            throw new Exception("");
        }
    }
}