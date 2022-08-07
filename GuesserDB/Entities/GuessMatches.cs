using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuesserDB.Entities
{
	public class GuessMatches
	{
		//public long Id { get; set; }
		public short GuessId { get; set; }
		public ushort FullGuessId { get; set; }
	}
}
