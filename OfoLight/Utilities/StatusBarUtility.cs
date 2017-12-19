using System;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace OfoLight.Utilities
{
    /// <summary>
    /// 状态栏工具
    /// </summary>
    public static class StatusBarUtility
    {
        #region 属性

        /// <summary>
        /// 是否有状态栏
        /// </summary>
        public static bool IsAnyStatusBar { get; private set; }

        /// <summary>
        /// 状态栏高度
        /// </summary>
        public static double StatusBarHeight { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 状态栏工具
        /// </summary>
        static StatusBarUtility()
        {
            // 判断是否存在 StatusBar
            IsAnyStatusBar = ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar");
            if (IsAnyStatusBar)
            {
                StatusBarHeight = StatusBar.GetForCurrentView().OccludedRect.Height;
            }
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 获取状态栏
        /// </summary>
        /// <returns></returns>
        public static StatusBar GetStatusBar()
        {
            if (IsAnyStatusBar)
            {
                return StatusBar.GetForCurrentView();
            }
            return null;
        }

        /// <summary>
        /// 隐藏状态栏
        /// </summary>
        /// <returns></returns>
        public static async Task HideAsync()
        {
            if (IsAnyStatusBar)
            {
                var statusBar = StatusBar.GetForCurrentView();
                await statusBar.HideAsync();
            }
        }

        /// <summary>
        /// 设置状态栏前景色
        /// </summary>
        /// <param name="foreground"></param>
        public static void SetForeground(Color foreground)
        {
            if (IsAnyStatusBar)
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.ForegroundColor = foreground;
            }
        }

        /// <summary>
        /// 设置状态栏透明度
        /// </summary>
        /// <param name="opacity"></param>
        public static void SetOpacity(double opacity)
        {
            if (IsAnyStatusBar)
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundOpacity = opacity;
            }
        }

        /// <summary>
        /// 显示状态栏
        /// </summary>
        /// <returns></returns>
        public static async Task ShowAsync()
        {
            if (IsAnyStatusBar)
            {
                var statusBar = StatusBar.GetForCurrentView();
                await statusBar.ShowAsync();
            }
        }

        #endregion 方法
    }
}