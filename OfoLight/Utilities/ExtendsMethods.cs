using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 拓展方法
    /// </summary>
    public static class ExtendsMethods
    {
        #region 方法

        /// <summary>
        /// 此任务可以以异步执行，不需要编译器进行警告
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NoWarning(this Task task)
        { }

        #endregion 方法
    }
}