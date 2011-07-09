using System;
using System.Collections.Generic;
using System.Linq;
using Path = System.IO.Path;
using SQLite;

namespace Monospace11
{
	public class FavoriteSession
	{
		[PrimaryKey]
		public string Code {get;set;}
	}
	/// <summary>
	/// Use to store 'favorite sessions'
	/// </summary>
	public class UserDatabase : SQLiteConnection
	{
		public UserDatabase (string path) : base (path)
		{
			CreateTable<FavoriteSession>();
		}
		public IEnumerable<FavoriteSession> GetFavorites ()
		{
			return Query<FavoriteSession> ("SELECT Code FROM FavoriteSession ORDER BY Code");
		}
		public List<string> GetFavoriteCodes ()
		{
			IEnumerable<FavoriteSession> x = Query<FavoriteSession> ("SELECT Code FROM FavoriteSession ORDER BY Code");
			List<string> l = new List<string>();
			foreach (var s in x)
				l.Add(s.Code);
			return l;
		}
		public bool IsFavorite (string sessionCode)
		{
			IEnumerable<FavoriteSession> x = Query<FavoriteSession> ("SELECT Code FROM FavoriteSession WHERE Code = ?", sessionCode);
			return x.ToList().Count > 0;
		}
		public void AddFavoriteSession (string sessionCode) {
			Insert(new FavoriteSession() {
				Code = sessionCode
			});
		}
		public void RemoveFavoriteSession (string sessionCode) {
			Delete(new FavoriteSession() {
				Code = sessionCode
			});
		}
	}
}