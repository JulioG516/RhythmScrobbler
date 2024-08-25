using System;
using System.Reactive;
using ReactiveUI;

namespace RhythmScrobbler.Helpers;

public static class Interactions
{
    public static readonly Interaction<Exception, Unit>
        Errors = new Interaction<Exception, Unit>();

    public static readonly Interaction<Unit, string?> GetFolderDialog =
        new Interaction<Unit, string?>();

    public static readonly Interaction<Unit, bool>
        LoginDialog = new Interaction<Unit, bool>();
}