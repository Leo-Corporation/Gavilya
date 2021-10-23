﻿/*
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

namespace Gavilya.Enums
{
	/// <summary>
	/// The pages that are used in the <see cref="Windows.FirstRun"/> window.
	/// </summary>
	public enum FirstRunPages
	{
		/// <summary>
		/// The <see cref="Pages.FirstRunPages.Welcome"/> page.
		/// </summary>
		Welcome,

		/// <summary>
		/// The <see cref="Pages.FirstRunPages.AddGamesPage"/> page.
		/// </summary>
		AddGames,

		/// <summary>
		/// The <see cref="Pages.FirstRunPages.SearchRAWGPage"/> page.
		/// </summary>
		SearchRawgGames,

		/// <summary>
		/// The <see cref="Pages.FirstRunPages.ImportGamesPage"/> page.
		/// </summary>
		ImportGames,

		/// <summary>
		/// The <see cref="Pages.FirstRunPages.FinishPage"/> page.
		/// </summary>
		Finish,

		/// <summary>
		/// The <see cref="Pages.FirstRunPages.SelectImportedGamesPage"/> page.
		/// </summary>
		SelectImportedGames
	}
}
