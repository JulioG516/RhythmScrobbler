using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ReactiveUI;
using RhythmScrobbler.Models;

namespace RhythmScrobbler.ViewModels;

public class RhythmScrobbleViewModel : ViewModelBase
{
    private readonly RhythmScrobble _rhythmScrobble;
    private readonly HttpClient _httpClient = new HttpClient();

    public RhythmScrobbleViewModel(RhythmScrobble rhythmScrobble)
    {
        _rhythmScrobble = rhythmScrobble;
        
    }

    public string Artist => _rhythmScrobble.Artist;
    public string Track => _rhythmScrobble.Track;
    public string Album => _rhythmScrobble.Album;
    public string Id => _rhythmScrobble.Guid;

    public bool Accepted => _rhythmScrobble.Accepted;

    private Bitmap? _cover;

    public Bitmap? Cover
    {
        get => _cover;
        private set => this.RaiseAndSetIfChanged(ref _cover, value);
    }

    private string _imageUrl;
    public string ImageUrl
    {
        get => _imageUrl;
        set
        {
            this.RaiseAndSetIfChanged(ref _imageUrl, value);
            DownloadImageAsync(_imageUrl);
        }
    }
    
    private async Task DownloadImageAsync(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            try
            {
                var bytes = await _httpClient.GetByteArrayAsync(url);
                DownloadComplete(bytes);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                Cover = null; // Could not download...
            }
        }
    }

    private void DownloadComplete(byte[] bytes)
    {
        try
        {
            Stream stream = new MemoryStream(bytes);
            var image = new Avalonia.Media.Imaging.Bitmap(stream);
            Cover = image;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            Cover = null; // Could not download...
        }
    }
}