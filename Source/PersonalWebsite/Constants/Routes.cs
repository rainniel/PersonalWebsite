namespace PersonalWebsite.Constants
{
    public static class Routes
    {
        public static class Public
        {
            public const string Home = "/Index";
            public const string Portfolio = "/Portfolio";
            public const string Blog = "/Blog";
            public const string Contact = "/Contact";
            public const string Maintenance = "/Maintenance";
            public const string Error = "/Error";
        }

        public static class Admin
        {
            public const string Root = "/Admin";
            public const string Dashboard = "/Admin/Index";
            public const string Home = "/Admin/Home";
            public const string Portfolio = "/Admin/Portfolio";
            public const string Blog = "/Admin/Blog";
            public const string Contact = "/Admin/Contact";
            public const string Profile = "/Admin/Profile";
            public const string Settings = "/Admin/Settings";
            public const string Login = "/Admin/Login";
            public const string Logout = "/Admin/Logout";
        }
    }
}
