﻿using System;
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
        private IMobileServiceTable<emmaptable> emmaptable;
        private AzureManager()
        {
            this.client = new MobileServiceClient("http://emmap.azurewebsites.net");
            this.emmaptable = this.client.GetTable<emmaptable>();
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
            locator.DesiredAccuracy = 10;

            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            Debug.WriteLine(position.Latitude);
            Debug.WriteLine(position.Longitude);
            return await emmaptable.Where(EmotionInformation => EmotionInformation.Latitude == position.Latitude).Where(EmotionInformation => EmotionInformation.Longitude == position.Longitude).ToListAsync();
        }
    }
}