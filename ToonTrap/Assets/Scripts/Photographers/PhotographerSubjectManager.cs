using System.Collections.Generic;
using UniRx;
using System;

namespace Ryocatusn.Photographers
{
	public class PhotographerSubjectManager
	{
		private List<IPhotographerSubject> photographerSubjects = new List<IPhotographerSubject>();

		private Subject<IPhotographerSubject> saveSubject = new Subject<IPhotographerSubject>();

		public IObservable<IPhotographerSubject> SaveSubject => saveSubject;
		
		public void Save(IPhotographerSubject photographerSubject)
		{
			photographerSubjects.Add(photographerSubject);
			saveSubject.OnNext(photographerSubject);
		}
		public bool TryFindRandom(out IPhotographerSubject photographerSubject)
		{
			photographerSubject = null;
			if (photographerSubjects.Count == 0) return false;

			photographerSubject = photographerSubjects[UnityEngine.Random.Range(0, photographerSubjects.Count - 1)];
			return true;
		}
		public void Delete(IPhotographerSubject photographerSubject)
		{
			photographerSubjects.Remove(photographerSubject);
		}
	}
}
