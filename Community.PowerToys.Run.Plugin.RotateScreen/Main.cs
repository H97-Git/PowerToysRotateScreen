namespace Community.PowerToys.Run.Plugin.RotateScreen;

using ManagedCommon;
using Wox.Plugin;
using Wox.Plugin.Logger;

/// <summary>
/// Main class of this plugin that implement all used interfaces.
/// </summary>
public class Main : IPlugin, IDisposable
{
    private PluginInitContext? Context { get; set; }
    private string? IconPath { get; set; }
    private bool Disposed { get; set; }

    /// <summary>
    /// ID of the plugin.
    /// </summary>
    public static string PluginID => "D5C2EBB4AA71471A9E9201CF3E52605F";

    /// <summary>
    /// Name of the plugin.
    /// </summary>
    public string Name => "RotateScreen";

    /// <summary>
    /// Description of the plugin.
    /// </summary>
    public string Description => "Rotate your screen directly from PowerToys.";

    /// <summary>
    /// Initialize the plugin with the given <see cref="PluginInitContext"/>.
    /// </summary>
    /// <param name="context">The <see cref="PluginInitContext"/> for this plugin.</param>
    public void Init(PluginInitContext context)
    {
        Log.Info("Init", GetType());

        Context = context ?? throw new ArgumentNullException(nameof(context));
        Context.API.ThemeChanged += OnThemeChanged;
        UpdateIconPath(Context.API.GetCurrentTheme());
    }

    /// <summary>
    /// Return a filtered list, based on the given query.
    /// </summary>
    /// <param name="query">The query to filter the list.</param>
    /// <returns>A filtered list, can be empty when nothing was found.</returns>
    public List<Result> Query(Query query)
    {
        Log.Info("Query: " + query.Search, GetType());

        uint screen = 1;
        var querySplit = query.Search.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        if (querySplit.Length > 1)
        {
            if (!uint.TryParse(querySplit[1], out screen)) ;
        }

        var results = new List<Result>()
        {
            new()
            {
                IcoPath = IconPath,
                Title = "0°",
                SubTitle = "Rotate the screen 0°",
                Action = _ =>
                {
                    Log.Debug("Action", GetType());
                    return Display.Rotate(screen, Display.Orientations.DegreesCw0);
                }
            },
            new()
            {
                IcoPath = IconPath,
                Title = "90°",
                SubTitle = "Rotate the screen 90°",
                Action = _ =>
                {
                    Log.Debug("Action", GetType());
                    return Display.Rotate(screen, Display.Orientations.DegreesCw90);
                }
            },
            new()
            {
                IcoPath = IconPath,
                Title = "180°",
                SubTitle = "Rotate the screen 180°",
                Action = _ =>
                {
                    Log.Debug("Action", GetType());
                    return Display.Rotate(screen, Display.Orientations.DegreesCw180);
                }
            },
            new()
            {
                IcoPath = IconPath,
                Title = "270°",
                SubTitle = "Rotate the screen 270°",
                Action = _ =>
                {
                    Log.Debug("Action", GetType());
                    return Display.Rotate(screen, Display.Orientations.DegreesCw270);
                }
            },
            new()
            {
                IcoPath = IconPath,
                Title = "Default",
                SubTitle = "Reset ALL screens rotations",
                Action = _ =>
                {
                    Log.Debug("Action", GetType());
                    Display.ResetAllRotations();
                    return true;
                }
            }
        };

        return results.OrderBy(x => x.SelectedCount).ToList();
    }

    private void UpdateIconPath(Theme theme) => IconPath = theme is Theme.Light or Theme.HighContrastWhite
        ? Context?.CurrentPluginMetadata.IcoPathLight
        : Context?.CurrentPluginMetadata.IcoPathDark;

    private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);

    /// <inheritdoc/>
    public void Dispose()
    {
        Log.Info("Dispose", GetType());
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Wrapper method for <see cref="Dispose()"/> that dispose additional objects and events form the plugin itself.
    /// </summary>
    /// <param name="disposing">Indicate that the plugin is disposed.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (Disposed || !disposing)
        {
            return;
        }

        if (Context?.API != null)
        {
            Context.API.ThemeChanged -= OnThemeChanged;
        }

        Disposed = true;
    }
}