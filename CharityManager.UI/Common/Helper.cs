using CharityManager.UI.CharityService;
using CharityManager.UI.Models;
using CharityManager.UI.ViewModels;
using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
using DevExpress.Mvvm.POCO;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CharityManager.UI
{
    public static class Helper
    {
        public static T Call<T>(Func<CharityClient, T> method)
        {
            CharityClient client = null;
            try
            {
                client = new CharityClient();
                client.Open();
                return method.Invoke(client);
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
        public static void Show(string caption, string message, NotificationType type)
        {
            var service = (AppUIManager.Application.MainWindow.DataContext as ShellViewModel).NotificationService;
            if (service == null) throw new NullReferenceException("customeService sent to ShowCustomeNotification Method is null");

            CustomNotificationViewModel vm = ViewModelSource.Create(() => new CustomNotificationViewModel());
            vm.Content = message;
            vm.Type = type;
            vm.Caption = caption;
            AppUIManager.Default.AddNotification(caption, message, type);
            INotification notification = service.CreateCustomNotification(vm);
            notification.ShowAsync();
        }
        public static void Notify(string message, string caption = "پیام") => Show(caption, message, NotificationType.Normal);
        public static void NotifyCaution(string message, string caption = "پیام") => Show(caption, message, NotificationType.Caution);
        public static void NotifySuccess(string message, string caption = "اعلان") => Show(caption, message, NotificationType.Success);
        public static void NotifyWarning(string message, string caption = "هشدار") => Show(caption, message, NotificationType.Warning);
        public static void NotifyError(string message, string caption = "خطا") => Show(caption, message, NotificationType.Error);
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

        public static void InvokeMainThread(Action callback) => Application.Current.Dispatcher.Invoke(callback);
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

    public static class ServiceResponseHelper
    {
        public static void CheckServiceResponse(ResponseBase response, string serviceName, object request = null)
        {
            if (response == null)
                throw new CallServiceException(serviceName, $"response is null", "پاسخص از سرویس دریافت نشد", request);
            if (response.Success == false)
                throw new CallServiceException(serviceName, response.Message, response.UserMessage, request);
        }
    }

    public static class QuickServiceCall
    {
        public static PersonDTO PersonByNationalNo(string nationalNo)
        {
            if (nationalNo?.Length == 0)
                return null;
            var request = new PersonRequest { Filter = new PersonFilter { NationalNo = nationalNo } };
            var response = Helper.Call(s => s.PersonGet(request));
            ServiceResponseHelper.CheckServiceResponse(response, "PersonGet", request);
            return response.Result;
        }

        public static BitmapImage LoadPersonImage(int id)
        {
            var request = new PersonRequest { Filter = new PersonFilter { ID = id } };
            var response = Helper.Call(s => s.PersonPictureGet(request));
            if (response?.Success == true)
                return response.ResultList.FirstOrDefault()?.Data.ToBitmapImage();
            return null;
        }
    }
}
