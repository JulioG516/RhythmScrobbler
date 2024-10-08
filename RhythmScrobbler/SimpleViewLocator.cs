﻿using System;
using ReactiveUI;
using RhythmScrobbler.ViewModels;
using RhythmScrobbler.Views;
using Splat;

namespace RhythmScrobbler;

public class SimpleViewLocator : IViewLocator
{
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
    {
        if (viewModel is HomeViewModel firstViewModel)
        {
            return new HomeView { DataContext = firstViewModel };
        }
        else if (viewModel is LogViewModel secondViewModel)
        {
            // Instantiate SecondView and return it
            // Set the DataContext to secondViewModel
            // Handle other view models similarly
            return new LogView { DataContext = secondViewModel };
        }
        // Add more cases for other view models

        throw new ArgumentOutOfRangeException(nameof(viewModel));
    }
 
    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}

