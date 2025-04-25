namespace 教务管理系统
{
    public static class GlobalVariables
    {
        // 存储当前登录的用户信息
        public static string CurrentUser { get; set; }

        public static string CurrentRole { get; set; }

        public static string AdminStatusTeacherNumber { get; set; }

        public static void Clear()
        {
            CurrentUser = string.Empty;
            CurrentRole = string.Empty;
            AdminStatusTeacherNumber = string.Empty;
        }
    }
}
