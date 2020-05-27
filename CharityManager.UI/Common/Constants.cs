﻿namespace CharityManager.UI
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
        public static string Dialog => nameof(Dialog);
        public static string Slider => nameof(Slider);
    }
    public static class AppModules
    {
        public static string Splash => nameof(Splash);
        public static string Licence => nameof(Licence);
        public static string Login => nameof(Login);
        public static string Main => nameof(Main);
        public static string Person => nameof(Person);

        public static string PersonInput => nameof(PersonInput);
        public static string AddressInput => nameof(AddressInput);
        public static string FamilyInput => nameof(FamilyInput);
        public static string JobInput => nameof(JobInput);
        public static string AssetInput => nameof(AssetInput);
        public static string DocumentInput => nameof(DocumentInput);

        public static string PersonGlance => nameof(PersonGlance);
    }

    public static class IDToTitleEntity
    {
        public static string Entity => nameof(Entity);
        public static string EducationStatus => nameof(EducationStatus);
        public static string EmploymentStatus => nameof(EmploymentStatus);
    }
}