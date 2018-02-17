using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RpRotation.Annotations;

namespace RpRotation
{
  public class RpRotationViewModel : INotifyPropertyChanged
  {
    private string _entry = string.Empty;
    private int _selectedEntry = -1;

    public string Entry
    {
      get => _entry;
      set
      {
        if (value != _entry)
        {
          _entry = value;
          OnPropertyChanged();
          AddEntryCommand.InvokeCanExecuteChanged();
        }
      }
    }

    public int SelectedEntryIndex
    {
      get => _selectedEntry;
      set
      {
        if (value != _selectedEntry)
        {
          _selectedEntry = value;
          OnPropertyChanged();
          MoveEntryUpCommand.InvokeCanExecuteChanged();
          MoveEntryDownCommand.InvokeCanExecuteChanged();
          ChooseEntryCommand.InvokeCanExecuteChanged();
          DeleteEntryCommand.InvokeCanExecuteChanged();
        }
      }
    }

    private bool IndexValid => SelectedEntryIndex != -1;

    public ObservableCollection<string> Entries { get; } = new ObservableCollection<string>();

    public DelegateCommand AddEntryCommand { get; }
    public DelegateCommand MoveEntryUpCommand { get; }
    public DelegateCommand MoveEntryDownCommand { get; }
    public DelegateCommand ChooseEntryCommand { get; }
    public DelegateCommand DeleteEntryCommand { get; }

    public RpRotationViewModel()
    {
      AddEntryCommand = new DelegateCommand(() =>
      {
        Entries.Insert(0, Entry);
        Entry = string.Empty;
      }, () => !string.IsNullOrWhiteSpace(Entry));

      MoveEntryUpCommand = new DelegateCommand(() =>
      {
        Entries.Move(SelectedEntryIndex, SelectedEntryIndex - 1);
      }, () => IndexValid && SelectedEntryIndex > 0);

      MoveEntryDownCommand = new DelegateCommand(() =>
      {
        Entries.Move(SelectedEntryIndex, SelectedEntryIndex + 1);
      }, () => IndexValid && SelectedEntryIndex < Entries.Count - 1);

      DeleteEntryCommand = new DelegateCommand(() =>
      {
        var previousIndex = SelectedEntryIndex;
        Entries.RemoveAt(SelectedEntryIndex);
        SelectedEntryIndex = previousIndex == Entries.Count ? Entries.Count - 1 : previousIndex;
      }, () => IndexValid);

      ChooseEntryCommand =new DelegateCommand(() =>
      {
        var previousIndex = SelectedEntryIndex;
        Entries.Move(SelectedEntryIndex, Entries.Count - 1);
        SelectedEntryIndex = previousIndex;
      }, () => IndexValid);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
