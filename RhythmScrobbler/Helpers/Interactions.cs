using System;
using System.Reactive;
using ReactiveUI;

namespace RhythmScrobbler.Models;

public static class Interactions
{
    //TODO: Trocar de Errros para ShowMessage, com Titulo, Mensagem etc.
    public static readonly Interaction<Exception, Unit>
        Errors = new Interaction<Exception, Unit>();

    public static readonly Interaction<Unit, string?> GetFolderDialog =
        new Interaction<Unit, string?>();

    public static readonly Interaction<Unit, bool>
        LoginDialog = new Interaction<Unit, bool>();

    public static readonly Interaction<string, bool>
        IsGameFolderValid = new Interaction<string, bool>();
}