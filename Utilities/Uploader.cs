using uplink.NET.Services;
using uplink.NET.Models;
namespace anime_climax_api.Utilities;
public class Uploader {
    
    public UploadInfo Upload() {
        Access access = new Access("15M6fjomdWMwh4cdbZx5YmDQpQsc8EN73sYKcfLodh6yz6PXEbNJe1WKFvKrwMotebVhRWPiihQoPEuKkaEt1reW5WhPwipmRZnqcfnA6ATKsf6ApbkjvjtJaG1YRZMshrYoTi1Xp5CpFHMLHBaHHBaJNTZjXKfwLDaNFFyE9cGjWZpSYkFzu2Xhc3joQ3QyyakkjETrDR2fWkuxjAJNGGaBpRTMdXwJp3awCMV5jgMsvqA47qC3ocrSBM3vn7bBjt22BResHwrEvJCMC3bqt8oR1NkZc9ip5");
        BucketService bucket = new BucketService(access);
        const String bucketName = "jonhacutuk3";
        bucket.EnsureBucketAsync(bucketName);
        
        ListBucketsOptions listBucketsOptions = new ListBucketsOptions();
        Task<BucketList> bucketListTask = bucket.ListBucketsAsync(listBucketsOptions);
        BucketList bucketList = bucketListTask.Result;
        MultipartUploadService uploadService = new MultipartUploadService(access);
        UploadOptions options = new();
        DateTime now = DateTime.Now;
        options.Expires = now.AddMinutes(10);
        UploadInfo uploadInfo = uploadService.BeginUploadAsync(bucketName, "six-weeks.mp4", options).Result;
        Console.WriteLine("Expires:" + options.Expires);
        return uploadInfo;
    }
}