public static class URIs{
    public static string LOGIN_POST ="login";
    public static string LOGIN_DELETE ="login";
    public static string LOGIN_STUDIO_POST ="login/studio";
    public static string LOGIN_COOKIE_POST = "login/cookie";
    public static string LOGIN_COOKIE_CLI_POST ="login/cookieCli";
    public static string LOGIN_CLI_POST ="login/cli";
}

public static class URIs_Maestro
{
    public static string MAESTRO_VERSION = "maestro/version";
}

public static class URIs_Task
{
    public static string TASK_POST_CREATE = "task";
    public static string TASK_GET = "task";
    public static string TASK_POST_SET_STATE = "task/{id}";
    public static string TASK_GETID = "task/{id}";
    public static string TASK_DELETE = "task/";
}


public static class URIs_Log
{
    public static string LOG_POST_CREATE = "log";
    public static string LOG_GET = "log";

    public static string LOG_POST_ID_ENTRY = "log/{id}/entry";
    public static string LOG_GET_ID_ENTRY = "log/{id}/entry";

    public static string LOG_GET_ID = "log/{id}";
    public static string LOG_DELETE = "log/{id}";

    public static string LOG_GET_ID_ENTRY_LIST = "log/{id}/entry-list";
    public static string LOG_GET_ID_CSV = "log/{id}/csv";
}

public static class URIs_Alert
{
    public static string ALERT_CREATE = "alerts";
    public static string ALERT_GET = "alerts";
}

public static class URIs_Message
{
    public static string MESSAGE_CREATE = "message";
}


public static class URIs_ResultFiles
{
    public static string ARTIFACT_POST_CREATE = "artifact";
    public static string ARTIFACT_GET_ARTIFACT = "artifact";
    public static string ARTIFACT_POST_UPLOAD_ARTIFACT = "artifact/log/{id}";
    public static string ARTIFACT_GET_ARTIFACT_ID = "artifact/{id}";
    public static string ARTIFACT_GET_FILE_ARTIFACT_ID = "artifact/{id}/file";
}



