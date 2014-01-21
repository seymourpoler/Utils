using System;
using System.Collections.Generic;
using CodeFirstSample;
using CodeFirstSample.Entities;

namespace CodeFirstSample.Repositories
{
    public class TaskRepository : BaseRepository
    {
        public TaskRepository(DataBaseContext context)
            : base(context)
        { }

        public Guid Insert(Task task)
        {
            var guid = Guid.NewGuid();
            task.IdTask = guid;

            _db.Tasks.Add(task);
            _db.SaveChanges();

            return guid;
        }

        public void Update(Task task)
        {
            var currentTask = _db.Tasks.Find(task.IdTask);
            var currentState = _db.Status.Find(task.Status.IdStatus);
            currentTask.Status = currentState;
            currentTask.Minutes = task.Minutes;
            currentTask.Name = task.Name;
            currentTask.Priority = task.Priority;
            _db.Entry<Status>(currentState).State = System.Data.EntityState.Unchanged;
            _db.Entry<Task>(currentTask).State = System.Data.EntityState.Modified;
            _db.SaveChanges();
        }

        public Task GetById(Guid idTask)
        {
            return _db.Tasks.Find(idTask);
        }

        public void Delete(Guid idTask)
        {
            var task = _db.Tasks.Find(idTask);
            _db.Tasks.Remove(task);
            _db.SaveChanges();
        }
    }
}