using OfoLight.Entity;
using OfoLight.Utilities;
using OfoLight.View;
using Windows.UI.Xaml;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 没有登录的页面vm
    /// </summary>
    public class NoLoginViewModel : BaseViewModel
    {
        public NoLoginViewModel()
        {
            CanExitApplication = true;
        }

        protected override async void NavigationActionAsync(object state)
        {
            await StatusBarUtility.ShowAsync();

            ContentPageArgs args = new ContentPageArgs()
            {
                Name = "登录",
                HeaderVisibility = Visibility.Collapsed,
            };
            args.ContentElement = new LoginFirstStepContentView();

            TryReplaceNavigate(typeof(ContentPageView), args);
        }
    }
}
