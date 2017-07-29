using System;
using System.Collections.Generic;

namespace XTabs.Model
{
    public class Face
    {
        public FaceRectangle FaceRectangle { get; set; }
        public Scores Scores { get; set; }
        public Tuple<double,string> getTop()
        {
            var score_list = new List<Tuple<double, string>>
            {
                Tuple.Create( this.Scores.Neutral, "neutral" ),
                Tuple.Create( this.Scores.Anger, "anger" ),
                Tuple.Create( this.Scores.Contempt, "contempt" ),
                Tuple.Create( this.Scores.Disgust, "disgust" ),
                Tuple.Create( this.Scores.Fear, "fear" ),
                Tuple.Create( this.Scores.Happiness, "happiness" ),
                Tuple.Create( this.Scores.Sadness, "sadness" ),
                Tuple.Create( this.Scores.Surprise, "surprise" ),
            };

            Tuple<double, string> tmax = score_list[0];
            foreach (Tuple<double,string> t in score_list) { 
                if (tmax.Item1 < t.Item1){
                    tmax = t;
                }
                else
                {

                }
            };

            return tmax;
            
        }
    }

    public class FaceRectangle
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Scores
    {
        public double Anger { get; set; }
        public double Contempt { get; set; }
        public double Disgust { get; set; }
        public double Fear { get; set; }
        public double Happiness { get; set; }
        public double Neutral { get; set; }
        public double Sadness { get; set; }
        public double Surprise { get; set; }

    }

    

}