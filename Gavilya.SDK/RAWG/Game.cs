/*
MIT License

Copyright (c) Léo Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
*/
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
		public string description { get; set; }
		public string description_raw { get; set; }
		public int playtime { get; set; }
		public List<ReturnedPlatforms> platforms { get; set; }
		public List<ParentStore> stores { get; set; }
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
		public int? metacritic { get; set; }
		public int suggestion_count { get; set; }
		public int id { get; set; }
		public object score { get; set; }
		public Clip clip { get; set; }
		public List<Tag> tags { get; set; }
		public object user_count { get; set; }
		public int reviews_count { get; set; }
		public string saturated_color { get; set; }
		public string dominant_color { get; set; }
		public List<Short_Screenshot> short_screenshots { get; set; }
		public List<Parent_Platform> parent_platforms { get; set; }
		public Genre[] genres { get; set; }
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

	public class Genre
	{
		public int id { get; set; }
		public string name { get; set; }
		public string slug { get; set; }
	}

	public class Parent_Platform
	{
		public Platform platform { get; set; }
	}

	public class ReturnedPlatforms
	{
		public Platform platform { get; set; }
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

	public class Short_Screenshot
	{
		public int id { get; set; }
		public string image { get; set; }
	}

	public class ParentStore
	{
		public Store store { get; set; }
	}

	public class Store
	{
		public int id { get; set; }
		public string name { get; set; }
		public string slug { get; set; }
	}

	public class Tag
	{
		public int id { get; set; }
		public string name { get; set; }
		public string slug { get; set; }
		public string language { get; set; }
		public int games_count { get; set; }
		public string image_background { get; set; }
	}
}
