using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Hqub.Lastfm;
using RhythmScrobbler.Helpers;
using Scrobble = Hqub.Lastfm.Entities.Scrobble;

namespace RhythmScrobbler.Services;

public class LastFmService 
{
    private readonly LastfmClient _client;

    public string Username { get; set; }
    public string Password { get; set; }

    public LastFmService()
    {
    }

    public LastFmService(LastfmClient client)
    {
        _client = client;
    }

    public async Task Authenticate(string username, string password)
    {
        try
        {
            await _client.AuthenticateAsync(username, password);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Auth Error: {e.Message}");
        }
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
        if (!_client.Session.Authenticated)
        {
            Debug.WriteLine($"Scrobblei a : {track}");
            return true;
            // if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            // {
            //     await Authenticate(Username, Password);
            // }
            // else
            // {
            //     var exception = new ArgumentException("Cannot authenticate, Username or Password is blank.");
            //
            //     await Interactions.Errors.Handle(exception);
            //     return false;
            // }
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