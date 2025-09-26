using MaterialDesignThemes.Wpf;
using System.Windows;

namespace AI_DDoS_Monitor
{
    public partial class App : Application
    {
        private bool _isDarkTheme = false;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Установим светлую тему по умолчанию
            ToggleTheme(_isDarkTheme);
            base.OnStartup(e);
        }

        public void ToggleTheme(bool isDark)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            if (isDark)
            {
                theme.SetBaseTheme(Theme.Dark);
            }
            else
            {
                theme.SetBaseTheme(Theme.Light);
            }

            paletteHelper.SetTheme(theme);
        }

        public void SwitchTheme()
        {
            _isDarkTheme = !_isDarkTheme;
            ToggleTheme(_isDarkTheme);
        }
    }
}