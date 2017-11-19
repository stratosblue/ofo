using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace OfoLight.Utilities
{
    /// <summary>
    /// 消息提示工具类
    /// </summary>
    public static class MessageDialogUtility
    {
        /// <summary>
        /// 显示确认对话消息
        /// 确认和取消
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <returns></returns>
        public static async Task<MessageDialogResult> ShowMessageAsync(string content, string title)
        {
            return await ShowMessageAsync(content, title, MessageDialogType.OKCancel);
        }

        /// <summary>
        /// 显示确认对话消息
        /// 确认和取消
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="type">对话类型</param>
        /// <returns></returns>
        public static async Task<MessageDialogResult> ShowMessageAsync(string content, string title, MessageDialogType type)
        {
            MessageDialogResult result = MessageDialogResult.Default;
            var dialog = new MessageDialog(content, title);

            List<UICommand> commandList = new List<UICommand>();

            switch (type)
            {
                case MessageDialogType.OK:
                    commandList.Add(new UICommand("确定", cmd => { result = MessageDialogResult.OK; }, commandId: 0));
                    break;
                case MessageDialogType.AbortRetryIgnore:
                    commandList.Add(new UICommand("中止", cmd => { result = MessageDialogResult.Abort; }, commandId: 0));
                    commandList.Add(new UICommand("重试", cmd => { result = MessageDialogResult.Retry; }, commandId: 1));
                    commandList.Add(new UICommand("忽略", cmd => { result = MessageDialogResult.Ignore; }, commandId: 2));
                    break;
                case MessageDialogType.YesNoCancel:
                    commandList.Add(new UICommand("是", cmd => { result = MessageDialogResult.Yes; }, commandId: 0));
                    commandList.Add(new UICommand("否", cmd => { result = MessageDialogResult.No; }, commandId: 1));
                    commandList.Add(new UICommand("取消", cmd => { result = MessageDialogResult.Cancel; }, commandId: 2));
                    break;
                case MessageDialogType.YesNo:
                    commandList.Add(new UICommand("是", cmd => { result = MessageDialogResult.Yes; }, commandId: 0));
                    commandList.Add(new UICommand("否", cmd => { result = MessageDialogResult.No; }, commandId: 1));
                    break;
                case MessageDialogType.RetryCancel:
                    commandList.Add(new UICommand("重试", cmd => { result = MessageDialogResult.Retry; }, commandId: 0));
                    commandList.Add(new UICommand("取消", cmd => { result = MessageDialogResult.Cancel; }, commandId: 1));
                    break;
                case MessageDialogType.OKCancel:
                default:
                    commandList.Add(new UICommand("确定", cmd => { result = MessageDialogResult.OK; }, commandId: 0));
                    commandList.Add(new UICommand("取消", cmd => { result = MessageDialogResult.Cancel; }, commandId: 1));
                    break;
            }

            foreach (var command in commandList)
            {
                dialog.Commands.Add(command);
            }

            //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = (uint)commandList.Count - 1;

            //获取返回值
            await dialog.ShowAsync();
            return result;
        }
    }

    /// <summary>
    /// MessageDialog类型
    /// </summary>
    public enum MessageDialogType
    {
        /// <summary>
        /// 消息框包含“确定”按钮。
        /// </summary>
        OK = 0,
        /// <summary>
        /// 消息框包含“确定”和“取消”按钮。
        /// </summary>
        OKCancel = 1,
        /// <summary>
        /// 消息框包含“中止”、“重试”和“忽略”按钮。
        /// </summary>
        AbortRetryIgnore = 2,
        /// <summary>
        /// 消息框包含“是”、“否”和“取消”按钮。
        /// </summary>
        YesNoCancel = 3,
        /// <summary>
        /// 消息框包含“是”和“否”按钮。
        /// </summary>
        YesNo = 4,
        /// <summary>
        /// 消息框包含“重试”和“取消”按钮。
        /// </summary>
        RetryCancel = 5
    }

    /// <summary>
    /// MessageDialog返回结果
    /// </summary>
    public enum MessageDialogResult
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 确定
        /// </summary>
        OK,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel,
        /// <summary>
        /// 中止
        /// </summary>
        Abort,
        /// <summary>
        /// 重试
        /// </summary>
        Retry,
        /// <summary>
        /// 忽略
        /// </summary>
        Ignore,
        /// <summary>
        /// 是
        /// </summary>
        Yes,
        /// <summary>
        /// 否
        /// </summary>
        No,
    }

}
