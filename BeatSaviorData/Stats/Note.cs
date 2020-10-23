﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaviorData
{
	public enum BSD_NoteType
	{
		right,
		left
	}

	public enum CutType
	{
		cut,
		miss,
		badCut
	}

	public class Note
	{
		private static int actualId = 0;

		public BSD_NoteType noteType;
		public NoteCutDirection noteDirection;
		public int index, id;
		public float time;
		public CutType cutType;
		public int multiplier;
		public int[] score;
		public float timeDeviation, speed;
		public float[] cutPoint, saberDir;

		private NoteCutInfo info;

		private Note(NoteData data, CutType cut)
		{
			if (data.noteType == NoteType.NoteB)
				noteType = BSD_NoteType.right;
			else if (data.noteType == NoteType.NoteA)
				noteType = BSD_NoteType.left;


			score = new int[] { 0, 0, 0 };
			index = data.lineIndex + 4 * (int)data.noteLineLayer;
			time = data.time;
			id = actualId;
			actualId++;

			noteDirection = data.cutDirection;
			cutType = cut;
		}

		public Note(NoteData data, CutType cut, NoteCutInfo _info, int _multiplier) : this(data, cut)
		{
			multiplier = _multiplier;

			info = _info;

			if (info != null && info.swingRatingCounter != null)
			{    // If it's a miss info is null
				info.swingRatingCounter.didFinishEvent -= WaitForSwing;
				info.swingRatingCounter.didFinishEvent += WaitForSwing;
			}
		}

		public Note(NoteData data, CutType cut, int _multiplier) : this(data, cut)
		{
			multiplier = _multiplier;
		}

		private void WaitForSwing(SaberSwingRatingCounter s)
		{
			ScoreModel.RawScoreWithoutMultiplier(info, out int before, out int after, out int accuracy);

			score = new int[] { before, accuracy, after };
			timeDeviation = info.timeDeviation;
			speed = info.saberSpeed;
			cutPoint = Utils.FloatArrayFromVector(info.cutPoint);
			saberDir = Utils.FloatArrayFromVector(info.saberDir);

			info.swingRatingCounter.didFinishEvent -= WaitForSwing;
		}

		public static void ResetID()
		{
			actualId = 0;
		}

		public int GetTotalScore()
		{
			return (score[0] + score[1] + score[2]) * multiplier;
		}

		public bool IsAMiss() => GetTotalScore() == 0;
	}
}