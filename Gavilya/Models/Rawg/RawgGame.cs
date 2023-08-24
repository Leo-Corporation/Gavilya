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
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Gavilya.Models.Rawg
{
    public class RawgGame
    {
		[JsonPropertyName("slug")]
		public string Slug { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("description_raw")]
		public string DescriptionRaw { get; set; }

		[JsonPropertyName("playtime")]
		public int Playtime { get; set; }

		[JsonPropertyName("platforms")]
		public List<ReturnedPlatforms> Platforms { get; set; }

		[JsonPropertyName("stores")]
		public List<ParentStore> Stores { get; set; }

		[JsonPropertyName("released")]
		public string Released { get; set; }

		[JsonPropertyName("tba")]
		public bool TBA { get; set; }

		[JsonPropertyName("background_image")]
		public string BackgroundImage { get; set; }

		[JsonPropertyName("background_image_additional")]
		public string BackgroundImageAdditional { get; set; }

		[JsonPropertyName("rating")]
		public float Rating { get; set; }

		[JsonPropertyName("rating_top")]
		public int RatingTop { get; set; }

		[JsonPropertyName("ratings")]
		public List<Rating> Ratings { get; set; }

		[JsonPropertyName("rating_count")]
		public int RatingCount { get; set; }

		[JsonPropertyName("review_text_count")]
		public int ReviewTextCount { get; set; }

		[JsonPropertyName("added")]
		public int Added { get; set; }

		[JsonPropertyName("added_by_status")]
		public AddedByStatus AddedByStatus { get; set; }

		[JsonPropertyName("metacritic")]
		public int? Metacritic { get; set; }

		[JsonPropertyName("suggestion_count")]
		public int SuggestionCount { get; set; }

		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("score")]
		public object Score { get; set; }

		[JsonPropertyName("clip")]
		public Clip Clip { get; set; }

		[JsonPropertyName("tags")]
		public List<Tag> Tags { get; set; }

		[JsonPropertyName("user_count")]
		public object UserCount { get; set; }

		[JsonPropertyName("reviews_count")]
		public int ReviewsCount { get; set; }

		[JsonPropertyName("saturated_color")]
		public string SaturatedColor { get; set; }

		[JsonPropertyName("dominant_color")]
		public string DominantColor { get; set; }

		[JsonPropertyName("short_screenshots")]
		public List<ShortScreenshot> ShortScreenshots { get; set; }

		[JsonPropertyName("parent_platforms")]
		public List<ParentPlatform> ParentPlatforms { get; set; }

		[JsonPropertyName("genres")]
		public Genre[] Genres { get; set; }
	}
}
