using System.IO.IsolatedStorage;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

internal class AvaloniaIsolatedStorageFile(IsolatedStorageFile isolatedStorageFile) : IIsolatedStorageFile
{
    public Task CreateDirectoryAsync(string path)
    {
        isolatedStorageFile.CreateDirectory(path);
        return Task.CompletedTask;
    }

    public Task<bool> GetDirectoryExistsAsync(string path) => Task.FromResult(isolatedStorageFile.DirectoryExists(path));

    public Task<bool> GetFileExistsAsync(string path) => Task.FromResult(isolatedStorageFile.FileExists(path));

    public Task<DateTimeOffset> GetLastWriteTimeAsync(string path) => Task.FromResult(isolatedStorageFile.GetLastWriteTime(path));

    public Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access) => Task.FromResult((Stream)isolatedStorageFile.OpenFile(path, mode, access));

    public Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access, FileShare share) => Task.FromResult((Stream)isolatedStorageFile.OpenFile(path, mode, access, share));
}