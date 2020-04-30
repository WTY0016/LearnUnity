using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Script.AlogLearn
{
    public class Sort
    {
        public void QuickSort()
        {
            var arrays = new[] {3, 4, 653, 23, 45, 1, 532, 75, 29};
            QuickSortImplement(arrays, 0, arrays.Length-1);
            foreach (var i in arrays)
            {
                Debug.Log(i);
            }
        }

        private void QuickSortImplement(IList<int> array, int left, int right)
        {
            if (left >= right) return;
            int i = left, j = right, x = array[left];
            while (i < j)
            {
                while (i < j && array[j] > x) j--;
                if (i < j) array[i++] = array[j];
                while (i < j && array[i] < x) i++;
                if (i < j) array[j--] = array[i];
            }
            array[i] = x;
            QuickSortImplement(array, left, i - 1);
            QuickSortImplement(array, i + 1, right);
        }
        
        public void TwoSplitSort()
        {
            Debug.Log("two split sort");
        }
    }
}