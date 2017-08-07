using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using XTabs.DataModels;
using Plugin.Geolocator;
using System.Diagnostics;

namespace XTabs
{
    public class AzureManager
    {

        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<emmaptable> emotiontable;
        private AzureManager()
        {
            this.client = new MobileServiceClient("http://emmap.azurewebsites.net");
            this.emotiontable = this.client.GetTable<emmaptable>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }
        public async Task<List<emmaptable>> GetEmotionInformation()
        {
           
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 1000;

            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            string plat = Convert.ToString(position.Latitude);
            string plong = Convert.ToString(position.Longitude);

        }

        public async Task PostEmotionInformation(emmaptable emotionmodel)
        {
            await this.emotiontable.InsertAsync(emotionmodel);
        }
    }
}