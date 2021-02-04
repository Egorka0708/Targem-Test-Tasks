using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyList;
using System;
using System.Collections.Generic;

namespace MyListTests
{
    [TestClass]
    public class MyListTests
    {
        [TestMethod]
        public void Count_ShouldBeZero_WhenListEmpty()
        {
            MyList<int> myList = new MyList<int>();
            Assert.AreEqual(0, myList.Count);
        }

        [TestMethod]
        public void Count_ShouldIncrease_WhenItemAdded()
        {
            MyList<int> myList = new MyList<int> { 1 };
            Assert.AreEqual(1, myList.Count);
        }
        
        [TestMethod]
        public void List_ShouldReturnItem_WhenAccessedByIndex()
        {
            MyList<int> myList = new MyList<int> { 0 };
            Assert.AreEqual(0, myList[0]);
        }

        [TestMethod]
        public void List_ShouldSetItem_WhenAccessedByIndex()
        {
            MyList<int> myList = new MyList<int> { 0 };
            myList[0] = 1;
            Assert.AreEqual(1, myList[0]);
        }

        [TestMethod]
        public void List_ShouldBeIterable_ByForEach()
        {
            MyList<int> myList = new MyList<int>();
            for (int i = 0; i < 5; ++i)
            {
                myList.Add(i);
            }

            int counter = 0;
            foreach (int i in myList)
            {
                Assert.AreEqual(counter++, i);
            }

            Assert.AreEqual(5, counter);
        }

        [TestMethod]
        public void List_ShouldBeCleared_WhenClearCalled()
        {
            MyList<int> myList = new MyList<int> { 0 };
            myList.Clear();
            Assert.AreEqual(0, myList.Count);
        }

        [TestMethod]
        public void IndexOf_ShouldReturnIndex_WhenSameInstance()
        {
            Stack<int> stack = new Stack<int>();
            MyList<Stack<int>> myList = new MyList<Stack<int>> { stack };
            Assert.AreEqual(0, myList.IndexOf(stack));
        }

        [TestMethod]
        public void IndexOf_ShouldNotFindIndex_WhenOtherInstance()
        {
            Stack<int> stackA = new Stack<int>();
            Stack<int> stackB = new Stack<int>();
            MyList<Stack<int>> myList = new MyList<Stack<int>> { stackA };
            Assert.AreEqual(-1, myList.IndexOf(stackB));
        }

        [TestMethod]
        public void CopyTo_ShouldThrowException_WhenArrayNull()
        {
            MyList<int> myList = new MyList<int>();
            Assert.ThrowsException<ArgumentNullException>(() => myList.CopyTo(null, 0));
        }

        [TestMethod]
        public void CopyTo_ShouldThrowException_WhenArgumentOutOfRange()
        {
            MyList<int> myList = new MyList<int>();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => myList.CopyTo(new int[10], -1));
        }

        [TestMethod]
        public void CopyTo_ShouldCopyItems()
        {
            MyList<int> myList = new MyList<int> { 1, 2, 3 };
            int[] array = new int[4];
            myList.CopyTo(array, 1);
            for (int i = 0; i < array.Length; ++i)
            {
                Assert.AreEqual(i, array[i]);
            }
        }

        [TestMethod]
        public void CopyTo_ShouldCopyByReference_WhenListContainsClasses()
        {
            Stack<int> stack = new Stack<int>();
            MyList<Stack<int>> myList = new MyList<Stack<int>> { stack };
            Stack<int>[] array = new Stack<int>[1];
            myList.CopyTo(array, 0);
            Assert.AreSame(stack, array[0]);
        }

        [TestMethod]
        public void RemoveAt_ShouldRemoveOneElement()
        {
            MyList<int> myList = new MyList<int> { 0 };
            myList.RemoveAt(0);
            Assert.AreEqual(0, myList.Count);
        }

        [TestMethod]
        public void RemoveAt_ShouldRemoveLastElement_WhenManyElements()
        {
            MyList<int> myList = new MyList<int> { 0, 1, 2, 3 };
            myList.RemoveAt(myList.Count - 1);
            Assert.AreEqual(3, myList.Count);
            for (int i = 0; i < myList.Count; ++i)
            {
                Assert.AreEqual(i, myList[i]);
            }
        }

        [TestMethod]
        public void RemoveAt_ShouldRemoveElementInMiddle()
        {
            MyList<int> myList = new MyList<int> { 0, 1, 4, 2 };
            myList.RemoveAt(2);
            Assert.AreEqual(3, myList.Count);
            for (int i = 0; i < myList.Count; ++i)
            {
                Assert.AreEqual(i, myList[i]);
            }
        }

        [TestMethod]
        public void Remove_ShouldReturnFalse_WhenListNotContainsItem()
        {
            MyList<int> myList = new MyList<int>();
            Assert.AreEqual(false, myList.Remove(0));
        }

        [TestMethod]
        public void Remove_ShouldReturnTrueAndRemove_WhenItemFound()
        {
            MyList<int> myList = new MyList<int> { 0 };
            Assert.AreEqual(true, myList.Remove(0));
            Assert.AreEqual(0, myList.Count);
        }

        [TestMethod]
        public void Insert_ShouldInsertItem_WhenListEmpty()
        {
            MyList<int> myList = new MyList<int>();
            myList.Insert(0, 0);
            Assert.AreEqual(1, myList.Count);
            Assert.AreEqual(0, myList[0]);
        }

        [TestMethod]
        public void Insert_ShouldInsertItemAtEnd_WhenListNotEmpty()
        {
            MyList<int> myList = new MyList<int> { 0, 1, 2 };
            myList.Insert(3, 3);
            Assert.AreEqual(4, myList.Count);
            Assert.AreEqual(3, myList[3]);
        }

        [TestMethod]
        public void Insert_ShouldOffsetItems_WhenInsertNotAtEnd()
        {
            MyList<int> myList = new MyList<int> { 1, 2, 3 };
            myList.Insert(0, 0);
            Assert.AreEqual(4, myList.Count);
            for (int i = 0; i < myList.Count; ++i)
            {
                Assert.AreEqual(i, myList[i]);
            }
        }

        [TestMethod]
        public void IEnumerableConstructor_ShouldCopyInputCollection()
        {
            List<int> list = new List<int> { 0, 1, 2 };
            MyList<int> myList = new MyList<int>(list);
            Assert.AreEqual(list.Count, myList.Count);
            for (int i = 0; i < myList.Count; ++i)
            {
                Assert.AreEqual(i, myList[i]);
            }
        }

        [TestMethod]
        public void Contains_ShouldReturnTrue_WhenItemFound()
        {
            List<int> list = new List<int>() { 0, 1, 2 };
            Assert.AreEqual(true, list.Contains(1));
        }

        [TestMethod]
        public void Contains_ShouldReturnFalse_WhenItemNotFound()
        {
            List<int> list = new List<int>() { 0, 1, 2 };
            Assert.AreEqual(false, list.Contains(3));
        }
    }
}
