using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Xaml.Interactions.Core.AddOn
{
    /// <summary>
    /// 用于Behavior的动画Action
    /// （由于使用.net Native发布，默认的Microsoft.Xaml.Interactions.Core.CallMethodAction基于反射实现，工作可能不正常，所以写了一个不依赖反射实现控制动画的Action）
    /// </summary>
    public class StoryBoardAction : DependencyObject, IAction
    {
        #region 字段

        /// <summary>
        /// 执行方法名称依赖属性
        /// </summary>
        public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
            "MethodName",
            typeof(string),
            typeof(StoryBoardAction),
            new PropertyMetadata(null, new PropertyChangedCallback(StoryBoardAction.OnMethodNameChanged)));

        /// <summary>
        /// 目标动画依赖属性
        /// </summary>
        public static readonly DependencyProperty TargetStoryBoardProperty = DependencyProperty.Register(
            "TargetStoryBoard",
            typeof(Storyboard),
            typeof(StoryBoardAction),
            new PropertyMetadata(null, new PropertyChangedCallback(StoryBoardAction.OnTargetStoryBoardChanged)));

        #endregion 字段

        #region 属性

        /// <summary>
        /// 执行方法名
        /// 目前只支持
        /// Begin    Stop
        /// </summary>
        public string MethodName
        {
            get
            {
                return (string)this.GetValue(StoryBoardAction.MethodNameProperty);
            }

            set
            {
                this.SetValue(StoryBoardAction.MethodNameProperty, value);
            }
        }

        /// <summary>
        /// 目标动画
        /// </summary>
        public Storyboard TargetStoryBoard
        {
            get
            {
                return this.GetValue(StoryBoardAction.TargetStoryBoardProperty) as Storyboard;
            }

            set
            {
                this.SetValue(StoryBoardAction.TargetStoryBoardProperty, value);
            }
        }

        #endregion 属性

        #region 方法

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public object Execute(object sender, object parameter)
        {
            Storyboard target;
            if (this.ReadLocalValue(StoryBoardAction.TargetStoryBoardProperty) != DependencyProperty.UnsetValue)
            {
                target = this.TargetStoryBoard;
            }
            else
            {
                target = sender as Storyboard;
            }

            if (target == null || string.IsNullOrEmpty(this.MethodName))
            {
                return false;
            }

            switch (MethodName)
            {
                case "Begin":
                    target.Begin();
                    break;

                case "Stop":
                    target.Stop();
                    break;
            }

            return false;
        }

        /// <summary>
        /// 执行方法名称变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void OnMethodNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            StoryBoardAction StoryBoardAction = (StoryBoardAction)sender;
            if (args.NewValue is string newMethodName && !newMethodName.Equals(StoryBoardAction.MethodName))
            {
                StoryBoardAction.MethodName = newMethodName;
            }
        }

        /// <summary>
        /// 目标动画变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void OnTargetStoryBoardChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            StoryBoardAction StoryBoardAction = (StoryBoardAction)sender;

            if (args.NewValue is Storyboard newStoryboard && !newStoryboard.Equals(StoryBoardAction.TargetStoryBoard))
            {
                StoryBoardAction.TargetStoryBoard = newStoryboard;
            }
        }

        #endregion 方法
    }
}