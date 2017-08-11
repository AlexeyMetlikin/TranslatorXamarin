using Plugin.Settings;
using System;
using Translator.Model;
using Xamarin.Forms;

namespace Translator
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            object key = CrossSettings.Current.GetValueOrDefault("API_Key", "");
            if (String.IsNullOrEmpty(key.ToString()))
            {
                CrossSettings.Current.AddOrUpdateValue("API_Key", Translator.Properties.Resources.API_Key);
            }

            MainPage = new MainPage(new TranslatorApi("https://translate.yandex.net", CrossSettings.Current.GetValueOrDefault("API_Key", "")));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
