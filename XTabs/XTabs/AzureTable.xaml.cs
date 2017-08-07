using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.WindowsAzure.MobileServices;
using XTabs.DataModels;

namespace XTabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AzureTable : ContentPage
    {
        MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;

        public AzureTable()
        {
            InitializeComponent();
        }
        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            List<emmaptable> emotionInformation = await AzureManager.AzureManagerInstance.GetEmotionInformation();
            EmotionsList.ClearValue(ListView.ItemsSourceProperty);
            EmotionsList.ItemsSource = emotionInformation;
        }
    }
}