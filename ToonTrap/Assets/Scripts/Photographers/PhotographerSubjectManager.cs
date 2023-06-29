using System.Collections.Generic;
using System.Linq;
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
		public IPhotographerSubject FindByPriority()
		{
			return photographerSubjects.OrderByDescending(x => x.priority).FirstOrDefault();
		}
		public void Delete(IPhotographerSubject photographerSubject)
		{
			photographerSubjects.Remove(photographerSubject);
		}
	}
}
