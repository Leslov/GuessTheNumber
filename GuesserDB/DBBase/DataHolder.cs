using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuesserDB.DBBase.EntityRange;
using GuesserDB.Entities;

namespace GuesserDB.DBBase
{
	public class DataHolder
	{
		private static Lazy<DataHolder> _dataHolder = new(() => new DataHolder());
		public static DataHolder Instance => _dataHolder.Value;
		private DataHolder()
		{

		}
		public IEntityRange<Guess> Guess { get; } = EntityRangeFactory.CreateSome<Guess>();
		public IEntityRange<GuessResult> GuessResult { get; } = EntityRangeFactory.CreateSome<GuessResult>();
		public IEntityRange<FullGuess> FullGuess { get; } = EntityRangeFactory.CreateSome<FullGuess>();

		/// <summary>
		/// [FullGuessId, GuessId]
		/// </summary>
		public bool[,] GuessMatches { get; set; }
		
		public void ClearAll()
		{
			FullGuess.Clear();
			Guess.Clear();
			GuessResult.Clear();
			//GuessMatches.Clear();
		}

		public void Save()
		{
			FullGuess.Save();
			Guess.Save();
			GuessResult.Save();
			//GuessMatches.Save();
		}
	}
}
