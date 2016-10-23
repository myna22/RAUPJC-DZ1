using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    


    public interface IGenericList<X>
    {

        void Add(X item);
        bool Remove(X item);
        bool RemoveAt(int index);
        X GetElement(int index);
        int IndexOf(X item);
        int Count { get; }
        void Clear();
        bool Contains(X item);

    }
    public class GenericList<X> : IGenericList<X>
    {
        private X[] _internalStorage;
        private int len = 0;

        public GenericList()
        {

            X[] _internalStorage = new X[4];
        }

        public GenericList(int initialSize)
        {
            if (initialSize > 0)
            {
                Array.Resize(ref _internalStorage, initialSize);

            }
            else
                throw new ArgumentException("Size must be greater than zero");
        }

        public void Add(X item)
        {

            if (len >= _internalStorage.Length)
            {
                Array.Resize(ref _internalStorage, _internalStorage.Length * 2);
            }
            else if(_internalStorage==null)
            Array.Resize(ref _internalStorage, 4);


        _internalStorage[len] = item;
            len++;

        }
        public int Count
        {
            get
            {
                return len;
            }
        }
        public bool RemoveAt(int index)
        {
            int i;
            if (index > len)
                return false;
            else
            {

                for (i = index; i < len - 1; i++)
                    _internalStorage[i] = _internalStorage[i + 1];
                //_internalStorage[_internalStorage.Count() - 1] = 0;
                len--;
                if (len == _internalStorage.Length)
                    Array.Resize(ref _internalStorage, len);
                return true;
            }

        }
        public bool Remove(X item)
        {
            int poz;
            poz = IndexOf(item);
            if (poz == -1)
                return false;
            else return RemoveAt(poz);
        }
        public X GetElement(int index)
        {
            if (index < 0)
                throw new IndexOutOfRangeException("Index cannot have negative value");
            else
                return _internalStorage[index];
        }
        public int IndexOf(X item)
        {
            int i;
            for (i = 0; i < _internalStorage.Length; i++)
                if (_internalStorage[i].Equals(item))
                {

                    return i;

                }

            return -1;


        }
        public void Clear()
        {
            _internalStorage = null;
        }
        public bool Contains(X item)
        {
            int i;
            for (i = 0; i < _internalStorage.Length; i++)
                if (_internalStorage[i].Equals(item))
                {

                    return true;
                }

            return false;
        }
    }


