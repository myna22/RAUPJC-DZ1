using System;
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
            IIntegerList list = new IntegerList(size);
            new Program().ListExample(list);
        }
        public void ListExample(IIntegerList list)
        {


            list.Add(2);
            list.Add(3);
            Console.WriteLine(list);
            list.Add(4);
            list.Add(5);

            list.RemoveAt(0);
            Console.WriteLine(list);
            Console.WriteLine(list.Count);
            Console.ReadLine();
        }

    }

    public interface IIntegerList
    {
        void Add(int item);
        bool Remove(int item);
        bool RemoveAt(int index);
        int GetElement(int index);
        int IndexOf(int item);
        int Count { get; }
        void Clear();
        bool Contains(int item);

    }


    public class IntegerList : IIntegerList
        {
            private int[] _internalStorage;
            private int len=0;

            public IntegerList()
            {

                int[] _internalStorage = new int[4];
            }

            public IntegerList(int initialSize)
            {
                if (initialSize > 0)
                {
                    Array.Resize(ref _internalStorage, initialSize);

                }
                else
                    throw new ArgumentException("Size must be greater than zero");
            }

            public void Add(int item)
            {
                
                if (len>=_internalStorage.Length)
                {
                    Array.Resize(ref _internalStorage,_internalStorage.Length * 2);
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
                    if(len==_internalStorage.Length)
                    Array.Resize(ref _internalStorage, len);
                    return true;
                }

            }
            public bool Remove(int item)
            {
                int poz;
                poz = IndexOf(item);
                if (poz == -1)
                    return false;
                else return RemoveAt(poz);
            }
            public int GetElement(int index)
            {
                if (index < 0)
                    throw new IndexOutOfRangeException("Index cannot have negative value");
                else
                    return _internalStorage[index];
            }
            public int IndexOf(int item)
            {
                int  i;
                for (i = 0; i < _internalStorage.Length; i++)
                    if (_internalStorage[i] == item)
                    {

                        return i;
                           
                    }

                return -1;


            }
            public void Clear()
            {
                _internalStorage = null;
            }
            public bool Contains(int item)
            {
                int i;
                for (i = 0; i < _internalStorage.Length; i++)
                    if (_internalStorage[i] == item)
                    {

                        return true;
                    }

                return false;
            }
        }

           
    }



