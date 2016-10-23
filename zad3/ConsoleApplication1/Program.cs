using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string sizein = Console.ReadLine();
            int size = Convert.ToInt32(sizein);
            IGenericList <string> stringList = new GenericList <string> (size);
            new Program().ListExample(stringList);
        }
        public void ListExample(IGenericList <string> list)
        {


            list.Add("Hello");
            list.Add("World");
            list.Add("!");
        
            foreach (string value in list)
            {
                Console.WriteLine(value);
            }

           //IEnumerator<string> enumerator = list.GetEnumerator();
           // while(enumerator.MoveNext())
           // {
           //     string value = (string)enumerator.Current;
           //     Console.WriteLine(value);
           // }
    
        }

    }


    public interface IGenericList <X> : IEnumerable <X>
    {
      
            void Add(X item);
            bool Remove(X item);
            bool RemoveAt(int index);
            X GetElement(int index);
            int IndexOf(X item);
            int Count { get; }
            void Clear();
            bool Contains(X item);
            IEnumerator<X> GetEnumerator();

    }
    public class GenericList <X> : IGenericList <X>
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

        public IEnumerator <X> GetEnumerator()
        {
            return new GenericListEnumerator<X> (this);
        }

        IEnumerator  IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class GenericListEnumerator<X> : IEnumerator <X>
        {
            private X[] _inStorage;
            private int _current=-1;

            public GenericListEnumerator (GenericList <X> genlist)
                {
                _inStorage = genlist._internalStorage;
                }
            public X Current
            {
                get
                {
                    return _inStorage[_current];
                }
            }

            public bool MoveNext()
            {
                _current++;
                return (_current < _inStorage.Count());

            }
            public void Reset()
            {
                _current = -1;
            }

            void IDisposable.Dispose() { }
            private object Current1
            {
                get { return this.Current; }
            }
            object IEnumerator.Current
            {
                get { return Current1; }
            }


        }

        public void Add(X item)
        {

            if (len >= _internalStorage.Length)
            {
                Array.Resize(ref _internalStorage, _internalStorage.Length * 2);
            }


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
   


    }


