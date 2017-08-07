using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using XTabs.Model;
using Xamarin.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Collections.Generic;
using Plugin.Geolocator;
using XTabs.DataModels;

namespace XTabs
{
    public class Person
    {
        public int Anger { get; set; }
        public int Contempt { get; set; }
        public int Disgust { get; set; }
        public int Fear { get; set; }
        public int Happiness { get; set; }
        public int Neutral { get; set; }
        public int Sadness { get; set; }
        public int Surprise { get; set; }
    }

    public partial class CustomVision : ContentPage
    {
        public CustomVision()
        {
            InitializeComponent();
        }

        private async void loadCamera(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });


            await MakeRequest(file);


        }

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }


        async Task MakeRequest(MediaFile file)
        {
            
            var client = new HttpClient();

            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "93800861ed354f2ab03e5e9d37600150");

            // NOTE: You must use the same region in your REST call as you used to obtain your subscription keys.
            //   For example, if you obtained your subscription keys from westcentralus, replace "westus" in the 
            //   URI below with "westcentralus".
            string uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?";
            HttpResponseMessage response;
            string responseContent;

            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
               

                if (response.IsSuccessStatusCode)
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                    responseContent = response.Content.ReadAsStringAsync().Result;
                    List<Face> Face = JsonConvert.DeserializeObject<List<Face>>(responseContent);
                    try {
                        TagLabel.Text = "Emotion shown is: " + (Face[0].getTop().Item2);
                        
                        var locator = CrossGeolocator.Current;
                        locator.DesiredAccuracy = 1000;

                        var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                        string plat = Convert.ToString(position.Latitude);
                        string plong = Convert.ToString(position.Longitude);
                        
                        emmaptable emmapmodel = new emmaptable();
                        emmapmodel.Latitude = plat;
                        emmapmodel.Longitude = plong;
                        emmapmodel.Emotion = Face[0].getTop().Item2;

                        Debug.WriteLine((float)position.Latitude);
                        Debug.WriteLine((float)position.Longitude);
                        
                        await AzureManager.AzureManagerInstance.PostEmotionInformation(emmapmodel);
                        
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error in face processing (detection/deserialisation/post)");
                        TagLabel.Text = "Cannot detect face. Please try again.";
                    }
                   
                }
                file.Dispose();

            }
        }
    }
}