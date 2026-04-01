using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace QRCodeApp;

/// <summary>
/// ViewModel for the QR Code Generator page.
/// Implements INotifyPropertyChanged so the UI updates automatically when data changes.
/// </summary>
public class GeneratorViewModel : INotifyPropertyChanged
{
    // ── Backing fields ──────────────────────────────────────────────────────
    private string _inputText = string.Empty;
    private string _qrValue = string.Empty;
    private bool _showQr = false;

    // ── Properties ──────────────────────────────────────────────────────────

    /// <summary>Text the user types into the input field.</summary>
    public string InputText
    {
        get => _inputText;
        set
        {
            _inputText = value;
            OnPropertyChanged();
        }
    }

    /// <summary>The value fed into the ZXing BarcodeGeneratorView.</summary>
    public string QrValue
    {
        get => _qrValue;
        set
        {
            _qrValue = value;
            OnPropertyChanged();
        }
    }

    /// <summary>Controls whether the QR image is visible.</summary>
    public bool ShowQr
    {
        get => _showQr;
        set
        {
            _showQr = value;
            OnPropertyChanged();
        }
    }

    //Command

    /// <summary>Called when the user taps the Generate button.</summary>
    public ICommand GenerateCommand { get; }

    /// <summary>Called when the user taps Clear.</summary>
    public ICommand ClearCommand { get; }

    // Constructor
    public GeneratorViewModel()
    {
        GenerateCommand = new Command(OnGenerate, CanGenerate);
        ClearCommand = new Command(OnClear);
    }

    //Private helpers 

    private bool CanGenerate() => !string.IsNullOrWhiteSpace(InputText);

    private void OnGenerate()
    {
        if (string.IsNullOrWhiteSpace(InputText)) return;

        QrValue = InputText.Trim();
        ShowQr = true;
    }

    private void OnClear()
    {
        InputText = string.Empty;
        QrValue = string.Empty;
        ShowQr = false;
    }

    //INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}