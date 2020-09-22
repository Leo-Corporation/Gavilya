using System;
using System.Collections.Generic;
using System.IO;

namespace Gavilya.SDK.RAWG
{
    public class GamesResults
    {
        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public List<Game> results { get; set; }
        public bool user_platform { get; set; }
    }

    public class Game
    {
        public string slug { get; set; }
        public string name { get; set; }
        public int playtime { get; set; }
        public List<Platform> platforms { get; set; }
        public List<Store> stores { get; set; }
        public string released { get; set; }
        public bool tba { get; set; }
        public string background_image { get; set; }
        public float rating { get; set; }
        public int rating_top { get; set; }
        public List<Rating> ratings { get; set; }
        public int rating_count { get; set; }
        public int review_text_count { get; set; }
        public int added { get; set; }
        public Added_By_Status added_by_status { get; set; }
        public int metacritic { get; set; }
        public int suggestion_count { get; set; }
        public int id { get; set; }
        public object score { get; set; }
        public Clip clip { get; set; }
        //TODO: Tags
        public object user_count { get; set; }
        public int reviews_count { get; set; }
        public string saturated_color { get; set; }
        public string dominant_color { get; set; }
        //TODO: Short screenshots, Parent Platforms and Genres
    }

    public class Added_By_Status
    {
        public int yet { get; set; }
        public int owned { get; set; }
        public int beaten { get; set; }
        public int toplay { get; set; }
        public int dropped { get; set; }
        public int playing { get; set; }
    }

    public class Clip
    {
        public string clip { get; set; }
        public Clips clips { get; set; }
        public string video { get; set; }
        public string preview { get; set; }
    }

    public class Clips
    {
        public string _320 { get; set; }
        public string _640 { get; set; }
        public string full { get; set; }
    }

    public class Platform
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

    public class Rating
    {
        public int id { get; set; }
        public string title { get; set; }
        public int count { get; set; }
        public float percent { get; set; }
    }

    public class Store
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }
}
