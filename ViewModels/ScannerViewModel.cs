using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace QRCodeApp;




public class ScannerViewModel : INotifyPropertyChanged
{
    //backing fields
    private string _scanResult = string.Empty;
    private bool _hasResult = false;
    private bool _isScannerOn = true;
    private bool _isUrl = false;

    //Properties

    /// <summary>The decoded text from the last successful scan.</summary>
    public string ScanResult
    {
        get => _scanResult;
        set
        {
            _scanResult = value;
            OnPropertyChanged();
        }
    }


    public bool HasResult
    {
        get => _hasResult;
        set
        {
            _hasResult = value;
            OnPropertyChanged();
        }
    }

    public bool IsScannerOn
    {
        get => _isScannerOn;
        set
        {
            _isScannerOn = value;
            OnPropertyChanged();
        }
    }

    public bool IsUrl
    {
        get => _isUrl;
        set
        {
            _isUrl = value;
            OnPropertyChanged();
        }
    }

    //Commands 

    /// <summary>Copy result text to clipboard.</summary>
    public ICommand CopyCommand { get; }

    /// <summary>Reset and scan again.</summary>
    public ICommand ScanAgainCommand { get; }

    /// <summary>Open the scanned URL in the device's default browser.</summary>
    public ICommand OpenInBrowserCommand { get; }

    // ── Constructor
    public ScannerViewModel()
    {
        CopyCommand = new Command(OnCopy, () => HasResult);
        ScanAgainCommand = new Command(OnScanAgain, () => HasResult);
        OpenInBrowserCommand = new Command(OnOpenInBrowser, () => IsUrl);
    }

    //Public methods

    /// <summary>Called by the code-behind when ZXing fires a detection event.</summary>
    public void OnBarcodeDetected(string rawValue)
    {
        ScanResult = rawValue;
        HasResult = true;
        IsScannerOn = false; // pause camera after first hit
        IsUrl = IsValidUrl(rawValue);
    }

    //Private helpers

    private static bool IsValidUrl(string value)
    {
        return Uri.TryCreate(value, UriKind.Absolute, out var uri)
               && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    private async void OnCopy()
    {
        if (string.IsNullOrEmpty(ScanResult)) return;
        await Clipboard.SetTextAsync(ScanResult);
    }

    private void OnScanAgain()
    {
        ScanResult = string.Empty;
        HasResult = false;
        IsUrl = false;
        IsScannerOn = true;
    }

    private async void OnOpenInBrowser()
    {
        if (!IsUrl || string.IsNullOrEmpty(ScanResult)) return;

        try
        {

            await Launcher.OpenAsync(new Uri(ScanResult));
        }
        catch (Exception ex)
        {
            // Launcher can fail if no app is registered for the scheme
            System.Diagnostics.Debug.WriteLine($"Browser launch failed: {ex.Message}");
        }
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}