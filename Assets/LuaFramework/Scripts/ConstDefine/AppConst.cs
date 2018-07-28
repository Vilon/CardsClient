using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LuaFramework
{
    public class AppConst
    {
        /// <summary>
        /// 如果不是编辑器,直接返回true
        /// 防止打包时忘记更改
        /// </summary>
        private const bool updateMode = true;                       //更新模式
        public static bool UpdateMode
        {
            get
            {
                if (!Application.isEditor) return true;
                return updateMode;
            }
        }
        public static bool LuaByteMode = true;                       //Lua字节码模式-默认关闭 
        public static bool LuaBundleMode = false;                    //Lua代码AssetBundle模式

        public const int TimerInterval = 1;
        public const int GameFrameRate = 30;                        //游戏帧频

        public const string ResDir = "Assets/Res/";                 //资源目录
        public const string AppName = "LuaFramework";               //应用程序名称
        public const string LuaTempDir = "Lua/";                    //临时目录
        public const string AppPrefix = AppName + "_";              //应用程序前缀
        public const string ExtName = ".unity3d";                   //素材扩展名
        public const string AssetDir = "StreamingAssets";           //素材目录 
        public const string WebUrl = "http://192.168.0.4/";      //测试更新地址

        public static string UserId = string.Empty;                 //用户ID
        public static int SocketPort = 0;                           //Socket服务器端口
        public static string SocketAddress = string.Empty;          //Socket服务器地址

        public static string FrameworkRoot
        {
            get
            {
                return Application.dataPath + "/" + AppName;
            }
        }
    }
}