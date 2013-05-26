namespace ClassLibraryHelper.C_
{
    /// <summary>
    /// 排序算法
    /// </summary>
    //***********************************
    /*　1：插入排序 a.直接插入排序  b.希尔排序

　    　2：交换排序 a.冒泡排序      b.快速排序

　    　3：选择排序 a.直接选择排序

　    　4：归并排序 a.归并排序

　　    5：分配排序 a.箱排序        b.基数排序*/

    public static class SortingAlgorithm
    {
        #region 冒泡排序
        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="list"></param>
        public static void BubbleSorting(int[] list)
        {
            int min;
            for (int i = 0; i < list.Length - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < list.Length; j++)
                {
                    if (list[j] < list[min])
                        min = j;
                }
                int t = list[min];
                list[min] = list[i];
                list[i] = t;
            }
        } 
        #endregion

        #region 选择排序
        /// <summary>
        /// 选择排序
        /// </summary>
        /// <param name="list"></param>
        public static void SelectionSort(int[] list)
        {
            int min;
            for (int i = 0; i < list.Length - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < list.Length; j++)
                {
                    if (list[j] < list[min])
                        min = j;
                }
                int t = list[min];
                list[min] = list[i];
                list[i] = t;
            }
        } 
        #endregion

        #region 插入排序
        /// <summary>
        /// 插入排序
        /// </summary>
        /// <param name="list"></param>
        public static void InsertSort(int[] list)
        {
            for (int i = 1; i < list.Length; i++)
            {
                int t = list[i];
                int j = i;
                while ((j > 0) && (list[j - 1] > t))
                {
                    list[j] = list[j - 1];
                    --j;
                }
                list[j] = t;
            }
        } 
        #endregion

        #region 希尔排序
        /// <summary>
        /// 希尔排序
        /// </summary>
        /// <param name="list"></param>
        public static void ShellSort(int[] list)
        {
            int inc;
            for (inc = 1; inc <= list.Length / 9; inc = 3 * inc + 1) ;
            for (; inc > 0; inc /= 3)
            {
                for (int i = inc + 1; i <= list.Length; i += inc)
                {
                    int t = list[i - 1];
                    int j = i;
                    while ((j > inc) && (list[j - inc - 1] > t))
                    {
                        list[j - 1] = list[j - inc - 1];
                        j -= inc;
                    }
                    list[j - 1] = t;
                }
            }
        }

        #endregion

        #region 快速排序
        /// <summary>
        /// 快速排序
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void QuickSort(int[] arr, int left, int right)
        {
            int i, j, s, temp;

            if (left < right)
            {
                i = left - 1;
                j = right + 1;
                s = arr[(i + j) / 2];
                while (true)
                {
                    while (arr[++i] < s) ;
                    while (arr[--j] > s) ;
                    if (i >= j)
                        break;
                    temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
                QuickSort(arr, left, i - 1);
                QuickSort(arr, j + 1, right);
            }
        } 
        #endregion
    }
}
