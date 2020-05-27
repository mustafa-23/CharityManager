using Araneo.Common;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using QRCoder;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class LicenceViewModel : ISupportParameter
    {
        public virtual string SerialNo { get; set; }
        public virtual string Licence { get; set; }
        public virtual string FormState { get; set; } = "Normal";
        public virtual string Message { get; set; }
        public virtual BitmapImage SMSQRCode { get; set; }
        public virtual BitmapImage WhatsAppQRCode { get; set; }

        public LicenceViewModel()
        {
            try
            {
                CheckSerial();
                var hard = Araneo.Common.Security.LicenceHelper.GetHDDSerialNumber().FirstOrDefault(h=>h.Type == "IDE");

                if (hard != null)
                {
                    SerialNo = $"{hard.Model}-{hard.SerialNo}".Replace(" ","");
                    GenerateSMSQRCode();
                    GenerateWhatsAppQRCode();
                }
                else
                    SerialNo = "خطا در دریافت اطلاعات. لطفا با پشتیبانی تماس بگیرید";

            }
            catch (System.Exception ex)
            {
                LogHelper.Log(ex);
            }
        }

        private void GenerateSMSQRCode()
        {
            var generator = new PayloadGenerator.SMS("+989391648703", SerialNo);
            string payload = generator.ToString();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            SMSQRCode = qrCode.GetGraphic(20, System.Drawing.Color.White, System.Drawing.Color.Transparent, false).ToBitmapImage();
        }
        private void GenerateWhatsAppQRCode()
        {
            var generator = new PayloadGenerator.WhatsAppMessage("+989391648703", SerialNo);
            string payload = generator.ToString();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            WhatsAppQRCode = qrCode.GetGraphic(20, System.Drawing.Color.White, System.Drawing.Color.Transparent, false).ToBitmapImage();
        }
        public void Input() => FormState = "Activation";
        public void Activate(KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            if (Araneo.Common.Security.LicenceHelper.Activate(ApplicationInfo.AppName, Licence))
            {
                Helper.NotifySuccess("نرم افزار با موفقیت فعال شد");
                Listener?.OnActivation();
            }
            else
                Helper.NotifyWarning("فعال سازی با مشکل مواجه شد");
        }
        public void CopySerialNo()
        {
            Clipboard.SetDataObject(SerialNo);
            Helper.Notify("سریال کپی شد");
        }

        private void CheckSerial()
        {
            var result = Araneo.Common.Security.LicenceHelper.CheckLicence(ApplicationInfo.AppName);
            switch (result)
            {
                case LicenceStatus.Expired:
                    Message = "زمان استفاده شما از نرم افزار به پایان رسیده است";
                    break;
                case LicenceStatus.NotMatch:
                    Message = "گواهی صادر شده با دستگاه شما همخوانی ندارد";
                    break;
                case LicenceStatus.ReadError:
                    Message = "روند بررسی اطلاعات گواهی با مشکل مواجه شده است";
                    break;
                case LicenceStatus.InvalidValue:
                    Message = "گواهی صادر شده نامعتبر است";
                    break;
                case LicenceStatus.HardDrive:
                    Message = "روند خواندن اطلاعات سیستم با مشکل مواجه شده است";
                    break;
                default:
                    break;
            }
        }

        public virtual object Parameter { get; set; }
        public ILicenceActivationListener Listener => Parameter as ILicenceActivationListener;
        public interface ILicenceActivationListener
        {
            void OnActivation();
        }
    }
}