using System;
using System.Collections.Generic;
using System.Linq;

namespace HRSystem.Repositories
{
    /// <summary>
    /// Базовое хранилище для работы с сущностями по ID.
    /// </summary>
    public abstract class BaseRepository<T> where T : class
    {
        protected List<T> _items = new List<T>();
        protected int _nextId = 1;

        protected abstract int GetId(T item);
        protected abstract void SetId(T item, int id);

        public virtual List<T> GetAll() => _items.ToList();

        public virtual T GetById(int id) => _items.FirstOrDefault(item => GetId(item) == id);

        public virtual void Add(T item)
        {
            SetId(item, _nextId++);
            _items.Add(item);
        }

        public virtual void Update(T item)
        {
            var existing = GetById(GetId(item));
            if (existing != null)
            {
                var index = _items.IndexOf(existing);
                _items[index] = item;
            }
        }

        public virtual void Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
                _items.Remove(item);
        }

        public virtual int GetNextId() => _nextId;
    }
}
