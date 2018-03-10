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
          EraseEntryCommand.InvokeCanExecuteChanged();
          FloorEntryCommand.InvokeCanExecuteChanged();
          RaiseEntryCommand.InvokeCanExecuteChanged();
          LowerEntryCommand.InvokeCanExecuteChanged();
        }
      }
    }

    public ObservableCollection<string> Entries { get; } = new ObservableCollection<string>();

    public DelegateCommand AddEntryCommand { get; }

    public DelegateCommand EraseEntryCommand { get; }

    public DelegateCommand FloorEntryCommand { get; }

    public DelegateCommand RaiseEntryCommand { get; }

    public DelegateCommand LowerEntryCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    public RpRotationViewModel()
    {
      AddEntryCommand = new DelegateCommand(AddEntry, () => !string.IsNullOrWhiteSpace(Entry));
      EraseEntryCommand = new DelegateCommand(EraseEntry, () => SelectedEntryIndex != -1);
      FloorEntryCommand = new DelegateCommand(FloorEntry, () => SelectedEntryIndex != -1);
      RaiseEntryCommand = new DelegateCommand(RaiseEntry, () => SelectedEntryIndex != -1 && SelectedEntryIndex > 0);
      LowerEntryCommand = new DelegateCommand(LowerEntry, () => SelectedEntryIndex != -1 && SelectedEntryIndex < Entries.Count - 1);
    }

    private void AddEntry()
    {
      Entries.Insert(0, Entry);
      Entry = string.Empty;
    }


    private void EraseEntry()
    {
      var previousIndex = SelectedEntryIndex;
      Entries.RemoveAt(SelectedEntryIndex);
      SelectedEntryIndex = previousIndex == Entries.Count ? Entries.Count - 1 : previousIndex;
    }

    private void FloorEntry()
    {
      var previousIndex = SelectedEntryIndex;
      Entries.Move(SelectedEntryIndex, Entries.Count - 1);
      SelectedEntryIndex = previousIndex;
    }

    private void RaiseEntry()
    {
      Entries.Move(SelectedEntryIndex, SelectedEntryIndex - 1);
    }

    private void LowerEntry()
    {
      Entries.Move(SelectedEntryIndex, SelectedEntryIndex + 1);
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
