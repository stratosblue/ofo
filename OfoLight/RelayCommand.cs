using System;
using System.Windows.Input;

namespace OfoLight
{
    /// <summary>
    /// 命令
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region 事件

        /// <summary>
        /// 可执行状态更改事件
        /// </summary>
        public event EventHandler CanExecuteChanged;

        #endregion 事件

        #region 字段

        /// <summary>
        /// 检查命令是否可执行委托
        /// </summary>
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// 命令执行委托
        /// </summary>
        private readonly Action<object> _execute;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 创建一个命令
        /// </summary>
        /// <param name="execute">命令的执行委托</param>
        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// 创建一个命令
        /// </summary>
        /// <param name="execute">命令的执行委托</param>
        /// <param name="canExecute">检查命令是否可执行的执行委托</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = _execute ?? execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 检查是否可执行
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion 方法
    }
}