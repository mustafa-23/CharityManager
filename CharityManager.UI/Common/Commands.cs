using CharityManager.UI.CharityService;
using CharityManager.UI.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CharityManager.UI
{
    public static class Commands
    {
        public static ICommand Shutdown => new DelegateCommand(() => AppUIManager.Application.Shutdown());
        public static ICommand CloseDialog => new DelegateCommand(DialogHelper.Close);
        public static ICommand CloseSlider => new DelegateCommand(SliderHelper.Close);
        public static ICommand Logout => new DelegateCommand(() =>
        {
            AppUIManager.Manager.Clear(AppRegions.Shell);
            AppUIManager.Manager.Inject(AppRegions.Shell, AppModules.Splash);
            AppUIManager.Application.MainWindow.WindowState = System.Windows.WindowState.Normal;
        });
        public static ICommand ShowSidebar => new DelegateCommand<string>(module =>
        {
            if (AppUIManager.Default.SideBarState == AppUIManager.PROFILE_SHOW)
            {
                if (AppUIManager.Manager.IsInjected(AppRegions.SideBar, module))
                    return;
                else
                    AppUIManager.Manager.Clear(AppRegions.SideBar);
            }
            AppUIManager.Manager.Inject(AppRegions.SideBar, module);
            AppUIManager.Default.SideBarState = AppUIManager.PROFILE_SHOW;
        });
        public static ICommand CloseSidebar => new DelegateCommand(() =>
        {
            AppUIManager.Manager.Clear(AppRegions.SideBar);
            AppUIManager.Default.SideBarState = AppUIManager.PROFILE_HIDE;
        });
        public static ICommand EditPerson => new DelegateCommand<object>(obj =>
        {
            AppUIManager.Manager.InjectOrNavigate(AppRegions.Person, AppModules.PersonInput, obj);
        });
        public static ICommand AddNote => new DelegateCommand(() => AppUIManager.Notes.Add(new NoteModel(null)));
        public static ICommand DeleteNote => new DelegateCommand<NoteModel>(note => AppUIManager.Notes.Remove(note));
        public static ICommand PersonSetImage => new DelegateCommand<PersonModel>(person =>
        {
            DialogHelper.OpenFileDialog.Filter = DialogHelper.FILTER_PIC;
            if (DialogHelper.OpenFileDialog.ShowDialog())
            {
                var size = DialogHelper.OpenFileDialog.File.Length;
                if (size > 256000)
                {
                    Helper.NotifyWarning("سایز عکس نباید از 250 کیلوبایت بیشتر باشد");
                    return;
                }
                person.Image = new BitmapImage(new Uri(DialogHelper.OpenFileDialog.File.GetFullName(), UriKind.Absolute));
            }
        });

        // Address
        public static ICommand AddAddress => new DelegateCommand<int>(patronId => SliderHelper.Open(AppModules.AddressInput, patronId));
        public static ICommand EditAddress => new DelegateCommand<AddressModel>(model => SliderHelper.Open(AppModules.AddressInput, model));
        public static ICommand DeleteAddress => new DelegateCommand<AddressModel>(model =>
        {
            var request = new AddressRequest { DTO = new AddressDTO { ID = model.ID }, UserID = GlobalVar.UserID };
            var response = Helper.Call(s => s.AddressDelete(request));
            ServiceResponseHelper.CheckServiceResponse(response, "AddressDelete", request);
            Messenger.Default.Send(Messages.Address.Refresh);
        });
        // Asset
        public static ICommand AddAsset => new DelegateCommand<int>(patronId => SliderHelper.Open(AppModules.AssetInput, patronId));
        public static ICommand EditAsset => new DelegateCommand<AssetModel>(model => SliderHelper.Open(AppModules.AssetInput, model));
        public static ICommand DeleteAsset => new DelegateCommand<AssetModel>(model =>
        {
            var request = new AssetRequest { DTO = new AssetDTO { ID = model.ID }, UserID = GlobalVar.UserID };
            var response = Helper.Call(s => s.AssetDelete(request));
            ServiceResponseHelper.CheckServiceResponse(response, "AssetDelete", request);
            Messenger.Default.Send(Messages.Asset.Refresh);
        });
        // Family
        public static ICommand AddFamily => new DelegateCommand<int>(patronId => SliderHelper.Open(AppModules.FamilyInput, patronId));
        public static ICommand EditFamily => new DelegateCommand<FamilyModel>(model => SliderHelper.Open(AppModules.FamilyInput, model));
        public static ICommand DeleteFamily => new DelegateCommand<FamilyModel>(model =>
        {
            var request = new FamilyRequest { DTO = new FamilyDTO { ID = model.ID }, UserID = GlobalVar.UserID };
            var response = Helper.Call(s => s.FamilyDelete(request));
            ServiceResponseHelper.CheckServiceResponse(response, "FamilyDelete", request);
            Messenger.Default.Send(Messages.Family.Refresh);
        });
        // Job
        public static ICommand AddJob => new DelegateCommand<int>(patronId => SliderHelper.Open(AppModules.JobInput, patronId));
        public static ICommand EditJob => new DelegateCommand<JobModel>(model => SliderHelper.Open(AppModules.JobInput, model));
        public static ICommand DeleteJob => new DelegateCommand<JobModel>(model =>
        {
            var request = new JobRequest { DTO = new JobDTO { ID = model.ID }, UserID = GlobalVar.UserID };
            var response = Helper.Call(s => s.JobDelete(request));
            ServiceResponseHelper.CheckServiceResponse(response, "JobDelete", request);
            Messenger.Default.Send(Messages.Job.Refresh);
        });
        public static ICommand SelectPatron => new DelegateCommand<object>(parameter =>
        {
            SliderHelper.Open(AppModules.SelectPatron,parameter);
        });

        public static ICommand AddResearch => new DelegateCommand<int>(requestId => SliderHelper.Open(AppModules.ResearchInput, requestId));
        public static ICommand AddViewPoint => new DelegateCommand<int>(patronId => SliderHelper.Open(AppModules.ManagerViewPointInput, patronId));
    }
}
