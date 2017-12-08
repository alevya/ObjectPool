using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ObjectPool
{
    public abstract class ObjectPool<T>
    {
        private readonly int _expirationTime;
        private IDictionary<T, int> _locked;
        private readonly IDictionary<T, int> _unlocked;

        protected ObjectPool()
        {
            _expirationTime = 10 * 1000;
            _locked = new ConcurrentDictionary<T, int>();
            _unlocked = new ConcurrentDictionary<T, int>();
        }

        protected abstract T _create();

        public abstract bool Validate(T obj);
        public abstract void Expire(T obj);

        public T CheckOut()
        {
            T t = default(T);
            int now = DateTime.Now.Millisecond;
            if (_unlocked.Any())
            {
                foreach (var unlockedKey in _unlocked.Keys)
                {
                    t = unlockedKey;
                    int value;
                    if(_unlocked.TryGetValue(unlockedKey, out value)) continue;

                    if ((now - value) > _expirationTime)
                    {
                        _unlocked.Remove(unlockedKey);
                        Expire(t);
                    }
                    else
                    {
                        if (Validate(t))
                        {
                            _unlocked.Remove(t);
                            _locked.Add(t, now);
                            return t;
                        }
                        _unlocked.Remove(t);
                        Expire(t);
                        t = default(T);
                    }
                }
            }

            t = _create();
            if(t != null)
                _locked.Add(t, now);

            return t;
        }

        public void CheckIn(T t)
        {
            if(t == null) return;
            _locked.Remove(t);
            _unlocked.Add(t, DateTime.Now.Millisecond);
        }

    }
}
