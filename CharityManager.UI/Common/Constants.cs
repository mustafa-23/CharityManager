namespace CharityManager.UI
{
    public static class ApplicationInfo
    {
        public static string AppName => "Charitit";
    }
    public static class AppRegions
    {
        public static string Shell => nameof(Shell);
        public static string Main => nameof(Main);
        public static string PersonMaster => nameof(PersonMaster);
        public static string Person => nameof(Person);
        public static string Introducer => nameof(Introducer);
        public static string Request => nameof(Request);
        public static string Dialog => nameof(Dialog);
        public static string Slider => nameof(Slider);
        public static string SideBar => nameof(SideBar);
    }
    public static class AppModules
    {
        public static string Splash => nameof(Splash);
        public static string Licence => nameof(Licence);
        public static string Login => nameof(Login);
        public static string Main => nameof(Main);

        #region Tools
        public static string UserProfile => nameof(UserProfile); 
        public static string NotificationCenter => nameof(NotificationCenter); 
        public static string Notes => nameof(Notes); 
        #endregion

        public static string Person => nameof(Person);

        #region Introducer
        public static string Introducer => nameof(Introducer);
        public static string IntroducerInput => nameof(IntroducerInput); 
        #endregion

        public static string PersonInput => nameof(PersonInput);
        public static string AddressInput => nameof(AddressInput);
        public static string FamilyInput => nameof(FamilyInput);
        public static string JobInput => nameof(JobInput);
        public static string AssetInput => nameof(AssetInput);
        public static string DocumentInput => nameof(DocumentInput);
        public static string SelectPatron => nameof(SelectPatron);

        public static string PersonGlance => nameof(PersonGlance);
        public static string QuickAddUser => nameof(QuickAddUser);

        #region Request
        public static string Request => nameof(Request);
        public static string RequestInput => nameof(RequestInput); 
        public static string ResearchInput => nameof(ResearchInput); 
        public static string ManagerViewPointInput => nameof(ManagerViewPointInput); 
        #endregion
    }
    public static class Messages
    {
        public enum Address
        {
            Refresh
        }
        public enum Asset
        {
            Refresh
        }
        public enum Family
        {
            Refresh
        }
        public enum Job
        {
            Refresh
        }
        public enum Research
        {
            Refresh
        }
    }
    public static class IDToTitleEntity
    {
        public static string Entity => nameof(Entity);
        public static string Introducer => nameof(Introducer);
        public static string EducationStatus => nameof(EducationStatus);
        public static string EmploymentStatus => nameof(EmploymentStatus);
    }
}
