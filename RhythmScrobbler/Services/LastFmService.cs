using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Hqub.Lastfm;
using Hqub.Lastfm.Entities;
using Microsoft.Extensions.Configuration;
using Splat;

namespace RhythmScrobbler.Services;

public class LastFmService
{
    private LastfmClient _client { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }

    public LastFmService()
    {
    }

    public LastFmService(IConfigurationSection lastFmConfig)
    {
        _client = new LastfmClient(lastFmConfig["ApiKey"], lastFmConfig["SharedSecret"]);
    }

    public async Task<bool> Authenticate(string username, string password)
    {
        try
        {
            await _client.AuthenticateAsync(username, password);
            if (_client.Session.Authenticated)
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
        if (_client.Session.Authenticated)
        {
            var lastFmConfig = Locator.Current.GetService<IConfigurationSection>();
            _client = new LastfmClient(lastFmConfig!["ApiKey"], lastFmConfig["SharedSecret"]);
            ;
        }

        Username = string.Empty;
        Password = string.Empty;
    }

    public async Task LoveTrack(string track, string artist)
    {
        if (_client.Session.Authenticated)
        {
            var response = await _client.Track.LoveAsync(track, artist);
            Debug.WriteLine($"Loved track: {response}");
        }
    }

    public async Task<bool> ScrobbleTrack(string track, string artist, string album)
    {
        if (_client.Session.Authenticated)
        {
            Debug.WriteLine($"Scrobblei a : {track}");
            var response = await _client.Track.ScrobbleAsync(new Scrobble()
            {
                Artist = artist,
                Track = track,
                Album = album,
                Date = DateTime.Now
            });

            if (response.Accepted > 0)
            {
                Debug.WriteLine($"Scrobble aceito.");
                return true;
            }

            return false;
        }   


        // new Scrobble()
        // {
        //     Artist = artist,
        //     Track = track,
        //     Album = album,
        //     Date = DateTime.Now
        // };

        return false;
    }
}