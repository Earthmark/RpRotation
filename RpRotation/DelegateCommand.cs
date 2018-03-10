﻿using System;
using System.Windows.Input;

namespace RpRotation
{
  public class DelegateCommand : ICommand
  {
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    public event EventHandler CanExecuteChanged;

    public DelegateCommand(Action execute, Func<bool> canExecute = null)
    {
      _execute = execute;
      _canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

    public void Execute(object parameter) => _execute();

    public void InvokeCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
  }
}
