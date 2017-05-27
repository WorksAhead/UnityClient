using System;
using System.Text;
using System.IO;
using UnityEngine;

namespace ArkCrossEngine
{
    public enum ResUpdateError
    {
        None = 0,

        Create_Assetbundle_Error = 1,
        ClientResVersion_Save_Error,
        Network_Error,

        // RequestServerVersion
        RequestServerVersion_WWW_Error = 100,
        RequestServerVersion_Byte_Error,
        RequestServerVersion_Save_Error,
        RequestServerVersion_Load_Error,

        // RequestClientVersion
        RequestClientVersion_WWW_Error = 200,
        RequestClientVersion_Byte_Error,
        RequestClientVersion_Load_Error,

        // RequestResCache
        RequestResCache_WWW_Error = 300,
        RequestResCache_Byte_Error,
        RequestResCache_Save_Error,
        LoadResCache_WWW_Error,
        LoadResCache_Byte_Error,
        LoadResCache_Save_Exception,
        LoadResCache_Assetbundle_Error,
        LoadResCache_Assetbundle_Load_Error,
        LoadResCache_Collect_Exception,

        // ResDownloader
        ResDownloader_Asset_Error = 400,
        ResDownloader_WWW_Error,
        ResDownloader_Save_Error,

        // ResLevelLoader
        ResLevelLoader_Extract_Error = 500,
        ResLevelLoader_Cache_Error,

        // ResSheetLoader
        RequestResSheet_WWW_Error = 600,
        RequestResSheet_Byte_Error,
        RequestResSheet_Save_Error,
        LoadResSheet_WWW_Error,
        LoadResSheet_Byte_Error,
        LoadResSheet_Save_Error,
        LoadResSheet_Assetbundle_Error,
        LoadResSheet_Extract_Error,

        // ResVersionLoader
        RequestResVersion_WWW_Error = 700,
        RequestResVersion_Byte_Error,
        RequestResVersion_Save_Error,
        LoadResVersion_WWW_Error,
        LoadResVersion_Byte_Error,
        LoadResVersion_Save_Error,
        LoadResVersion_Assetbundle_Error,
        LoadResVersion_Assetbundle_Load_Error,
        LoadResVersion_AsssetEx_Mgr_Error,
        LoadClientResVersion_WWW_Error,
        LoadClientResVersion_Byte_Error,
        LoadClientResVersion_Save_Error,

        // ResZipLoader
        ResZipLoader_WWW_Error = 800,
        ResZipLoader_Save_Error,
        ResZipLoader_Asset_Error,

        // ServerConfig
        RequestServerList_WWW_Error = 900,
        RequestServerList_Byte_Error,
        RequestServerList_Save_Error,

        // NoticeConfig
        RequestNoticeConfig_WWW_Error = 900,
        RequestNoticeConfig_Byte_Error,
        RequestNoticeConfig_Save_Error,
    }
}
