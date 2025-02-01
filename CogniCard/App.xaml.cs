using CogniCard.Firebase;
using CogniCard.Model;
using CogniCard.Properties;
using CogniCard.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CogniCard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            FirebaseSetup.InitializeFirebase();

            if (!Settings.Default.IsUploadDisplayNameInitialized)
            {
                Settings.Default.IsUploadDisplayNameInitialized = true;
                Settings.Default.UploadDisplayName = Environment.UserName;
                Settings.Default.Save();
            }
        }
    }

}
