using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Hqub.Lastfm;
using Hqub.Lastfm.Entities;
using RhythmScrobbler.Configs;
using RhythmScrobbler.Helpers;
using Splat;

namespace RhythmScrobbler.Services;

public class LastFmService
{
    private LastfmClient Client { get; set; }
    private DbService DbService { get; set; }
    
    public string Username { get; set; } 
    public string Password { get; set; }

    public  LastFmService()
    {
        var lastFmConfig = Locator.Current.GetService<LastFmConfig>();
        Client = new LastfmClient(lastFmConfig!.ApiKey, lastFmConfig.SharedSecret);
        DbService = Locator.Current.GetService<DbService>()!;
    }


    public async Task<bool> Authenticate(string username, string password)
    {
        try
        {
            await Client.AuthenticateAsync(username, password);
            if (Client.Session.Authenticated)
            {
                Username = username;
                Password = password;
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Auth Error: {e.Message}");
            return false;
        }

        return false;
    }

    public void Logout()
    {
        if (Client.Session.Authenticated)
        {
            var lastFmConfig = Locator.Current.GetService<LastFmConfig>();
            Client = new LastfmClient(lastFmConfig!.ApiKey, lastFmConfig.SharedSecret);
        }

        Username = string.Empty;
        Password = string.Empty;
    }

    public async Task LoveTrack(string track, string artist)
    {
        if (Client.Session.Authenticated)
        {
            var response = await Client.Track.LoveAsync(track, artist);
            Debug.WriteLine($"Loved track: {response}");
        }
    }

    public async Task<Track> FetchTrackInfoAsync(string artist, string track)
    {
        var trackInfo = await Client.Track.GetInfoAsync(track, artist);
        return trackInfo;
    }
    
    public async Task<bool> ScrobbleTrack(string track, string artist, string album)
    {
        if (!Client.Session.Authenticated)
        {
            return false;
        }

        Debug.WriteLine($"Scrobblei a : {track}");

        var scrobble = new Scrobble()
        {
            Artist = artist,
            Track = track,
            Album = album,
            Date = DateTime.Now
        };

        var response = await Client.Track.ScrobbleAsync(scrobble);

        var rhythmScrobble = new RhythmScrobble(scrobble);

        if (response.Accepted > 0)
        {
            rhythmScrobble.Accepted = true;
        }
        
        DbService.InsertScrobble(rhythmScrobble);        

        return response.Accepted  > 0;
    }
}