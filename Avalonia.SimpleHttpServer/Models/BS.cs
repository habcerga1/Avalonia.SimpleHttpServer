using System;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.SimpleHttpServer.ViewModels;
using ReactiveUI;

namespace Avalonia.SimpleHttpServer.Models;

public class BS<T> : ViewModelBase
{
    private T _value;
    public ObservableAsPropertyHelper<T> valueBasedOnState;
    
    /// <summary>
    /// Get value of instance wich you created
    /// </summary>
    public T Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }
    public T ValueBasedOnState => valueBasedOnState.Value;
    public ReactiveCommand<T,Unit> OnClickCommand { get; }
    public BS(Func<T> action,Action<T> onCommandParam)
    {
        valueBasedOnState = this.WhenAnyValue(vm => vm.Value)
            .Select(func => action.Invoke())
            .ToProperty(this, vm => vm.ValueBasedOnState);
            
        OnClickCommand = ReactiveCommand.Create<T>(onCommandParam);
    }
    
    public BS(Func<T> action)
    {
        valueBasedOnState = this.WhenAnyValue(vm => vm.Value)
            .Select(func => action.Invoke())
            .ToProperty(this, vm => vm.ValueBasedOnState);
    }
    
    public BS(Action<T> onCommandParam)
    {
        OnClickCommand = ReactiveCommand.Create<T>(onCommandParam);
    }
    
    public BS( )
    {
        
    }
}