﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaviorData
{
	interface ITracker
	{
		void RegisterTracker(SongData data);
		void EndOfSong(LevelCompletionResults results);
	}
}