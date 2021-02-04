using System;
using System.Collections;
using System.Collections.Generic;

namespace MyList
{
    public class MyList<T> : IList<T>
    {
        private List<T> list;
        public int Count { get; private set; }
        public bool IsReadOnly => false;

        public MyList()
        {
            list = new List<T>();
        }

        /// <summary>
        /// Свойство для получения и установки объекта в лист по заданному индексу
        /// </summary>
        /// <param name="index">Индекс для установки или получения объекта</param>
        /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, когда индекс меньше нуля или
        /// больше количества объектов в списке</exception>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return list[index];
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                list[index] = value;
            }
        }

        /// <summary>
        /// Метод для добавления объекта в лист
        /// </summary>
        /// <param name="item">Объект, добавляемый в лист</param>
        public void Add(T item)
        {
            list.Add(item);
            Count++;
        }

        /// <summary>
        /// Метод для очищения листа.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < Count; ++i)
                list[i] = default;
            Count = 0;
        }

        /// <summary>
        /// Метод для проверки наличия входного объекта в листе
        /// </summary>
        /// <param name="item">Объект для поиска в листе</param>
        /// <returns>true если объект найден, false если объект не найден</returns>
        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Метод для копирования объектов листа во входной массив, начиная с указанного индекса
        /// </summary>
        /// <param name="array">Массив, в который происходит копирование</param>
        /// <param name="arrayIndex">Индекс, с которого объекты помещаются в новый массив</param>
        /// <exception cref="ArgumentNullException">Выкидывается, когда входной массив null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Выкидывается, когда указанный индекс меньше нуля</exception>
        /// <exception cref="ArgumentException">Выкидывается, когда входной массив недостаточной длины
        /// для копирования всех объектов</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (Count > array.Length - arrayIndex)
                throw new ArgumentException(nameof(array));

            for (int i = 0; i < Count; ++i)
            {
                array[i + arrayIndex] = list[i];
            }
        }

        /// <summary>
        /// Метод для нахождения индекса первого равного объекта, находящегося в листе
        /// </summary>
        /// <param name="item">Объект для поиска в листе</param>
        /// <returns>Индекс объекта, если он найден, -1 если объект не найден</returns>
        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (item == null && list[i] == null || item != null && item.Equals(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Метод для вставки входного объекта в лист по указанному индексу
        /// </summary>
        /// <param name="index">Индекс, по которому нужно вставить входной объект</param>
        /// <param name="item">Объект для вставки в лист</param>
        /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, когда указанный индекс меньше нуля или больше длины листа</exception>
        public void Insert(int index, T item)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            list.Insert(index, item);
            Count++;
        }

        /// <summary>
        /// Метод для удаления объекта, найденного в листе
        /// </summary>
        /// <param name="item">Объект для удаления из листа</param>
        /// <returns>true если объект найден и удален, false если объект не найден</returns>
        public bool Remove(T item)
        {
            int itemIndex = IndexOf(item);
            if (itemIndex == -1)
                return false;
            RemoveAt(itemIndex);
            return true;
        }

        /// <summary>
        /// Метод для удаления элемента, находящегося на заданном индексе
        /// </summary>
        /// <param name="index">Индекс, на котором необходимо удалить элемент</param>
        /// <exception cref="ArgumentOutOfRangeException">Выкидывается, когда заданный индекс меньше нуля или больше длины массива</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            list[index] = default;
            for (int i = index; i < Count - 1; i++)
            {
                list[i] = list[i + 1];
                list[i + 1] = default;
            }

            Count--;
        }

        public MyList(IEnumerable<T> collection) : this()
        {
            foreach (T item in collection)
            {
                Add(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return list[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
