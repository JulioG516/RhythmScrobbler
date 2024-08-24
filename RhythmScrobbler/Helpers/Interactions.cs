using System.Reactive;
using ReactiveUI;

namespace RhythmScrobbler.Helpers;

public static class Interactions
{
    public static readonly Interaction<Unit, string?> GetFolderDialog =
        new Interaction<Unit, string?>();
}