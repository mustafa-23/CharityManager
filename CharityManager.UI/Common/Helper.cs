using CharityManager.UI.ViewModels;
using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
using DevExpress.Mvvm.POCO;
using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media.Imaging;

namespace CharityManager.UI
{
    public static class Helper
    {
        public static T Call<T>(Func<CharityService.CharityClient, T> method)
        {
            CharityService.CharityClient client = null;
            try
            {
                client = new CharityService.CharityClient();
                client.Open();
                var result = method.Invoke(client);
                return result;
            }
            catch (Exception ex)
            {
                Araneo.Common.LogHelper.Log(ex);
                return default;
            }
            finally
            {
                if (client != null)
                    client.Abort();
            }
        }

        public static string HashPassword(string pass)
        {
            using (var cryptor = new SHA1CryptoServiceProvider())
                return Convert.ToBase64String(cryptor.ComputeHash(Encoding.ASCII.GetBytes(pass)));
        }

        #region Notification
        public static void Show(string caption, string message, CustomNotificationViewModel.NotificationType type)
        {
            var service = (AppUIManager.Application.MainWindow.DataContext as ShellViewModel).NotificationService;
            if (service == null) throw new NullReferenceException("customeService sent to ShowCustomeNotification Method is null");

            CustomNotificationViewModel vm = ViewModelSource.Create(() => new CustomNotificationViewModel());
            vm.Content = message;
            vm.Type = type;
            vm.Caption = caption;
            INotification notification = service.CreateCustomNotification(vm);
            notification.ShowAsync();
        }
        public static void Notify(string message, string caption = "پیام") => Show(caption, message, CustomNotificationViewModel.NotificationType.Normal);
        public static void NotifyCaution(string message, string caption = "پیام") => Show(caption, message, CustomNotificationViewModel.NotificationType.Caution);
        public static void NotifySuccess(string message, string caption = "اعلان") => Show(caption, message, CustomNotificationViewModel.NotificationType.Success);
        public static void NotifyWarning(string message, string caption = "هشدار") => Show(caption, message, CustomNotificationViewModel.NotificationType.Warning);
        public static void NotifyError(string message, string caption = "خطا") => Show(caption, message, CustomNotificationViewModel.NotificationType.Error);
        #endregion

        #region Image
        public static byte[] ToByteArray(this BitmapImage image)
        {
            byte[] buffer = null;
            if (image != null)
            {
                var stream = image.StreamSource;
                if (stream?.Length > 0)
                    using (BinaryReader reader = new BinaryReader(stream))
                        buffer = reader.ReadBytes((int)stream.Length);
            }
            return buffer;
        }
        public static byte[] ImageUriToByteArray(string path)
        {
            if (File.Exists(path))
                using (var sr = new StreamReader(path))
                using (var br = new BinaryReader(sr.BaseStream))
                    return br.ReadBytes((int)sr.BaseStream.Length);
            return null;
        }
        public static BitmapImage ToBitmapImage(this byte[] array)
        {
            BitmapImage image = new BitmapImage();
            using (var ms = new MemoryStream(array))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            BitmapImage image = new BitmapImage();
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }
        #endregion
    }

    public static class DialogHelper
    {
        public const string FILTER_PIC = "All Pic|*.jpeg;*.jpg;*.png;*.bmp;|JPEG|*.jpeg;*.jpg|PNG|*.png;|Bitmap|*.bmp;";
        private static Dialog Dialog { get; set; }
        public static IOpenFileDialogService OpenFileDialog => (AppUIManager.Application.MainWindow.DataContext as ShellViewModel).OpenFileService;
        public static void Show(string moduleKey, object parameter = null)
        {
            AppUIManager.Manager.InjectOrNavigate(AppRegions.Dialog, moduleKey, parameter);
            Dialog = new Dialog();
            Dialog.ShowDialog();
        }
        public static void Close()
        {
            AppUIManager.Manager.Clear(AppRegions.Dialog);
            if (Dialog != null)
                Dialog.Close();
            Dialog = null;
            GC.Collect();
        }
    }

    public static class SliderHelper
    {
        public static void Open(string moduleKey, object parameter = null)
        {
            AppUIManager.Manager.InjectOrNavigate(AppRegions.Slider, moduleKey, parameter);
            AppUIManager.Default.SliderState = AppUIManager.SLIDER_OPEN;
        }
        public static void Close()
        {
            AppUIManager.Manager.Clear(AppRegions.Slider);
            AppUIManager.Default.SliderState = AppUIManager.SLIDER_CLOSE;
        }
    }
}
