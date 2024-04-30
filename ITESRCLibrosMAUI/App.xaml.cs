using ITESRCLibrosMAUI.Services;

namespace ITESRCLibrosMAUI
{
    public partial class App : Application
    {
        public static LibrosService LibrosService { get; set; } = new();

        public App()
        {
            InitializeComponent();

            Thread thread=new Thread(Sincronizador) { IsBackground=true };
            thread.Start();

            MainPage = new AppShell();
        }

        async void Sincronizador()
        {
            while (true)
            {
                await LibrosService.GetLibros();
                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
        }

    }
}
