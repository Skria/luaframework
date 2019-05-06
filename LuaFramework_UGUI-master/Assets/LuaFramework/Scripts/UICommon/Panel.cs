﻿using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LuaFramework
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasScaler))]
    public class Panel : View
    {

        public override void InitView(params object[] args)
        {
            if (moduleName != string.Empty && viewName != string.Empty)
            {
                //确保注册进LuaClassLoader
                App.LuaManager.CallFunction("require", AppConst.LuaGameUIRoot + "/" + moduleName + "/" + viewName);
                handle = App.LuaManager.InitializeLuaObject(viewName);
                if (handle > 0) App.LuaManager.CallObjectFunction(handle, "InitView", gameObject, handle);
                NotifyInitOver();
                OpenView(args);
            }
        }

        public override void OpenView(params object[] args)
        {
            gameObject.SetActive(true);
            StartCoroutine(OnWaitingInitOpen(args));
        }

        IEnumerator OnWaitingInitOpen(params object[] args)
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (isInited) break;
            }
            OnWaitInitOpenViewEnd(args);
        }

        protected void OnWaitInitOpenViewEnd(params object[] args)
        {
            //Debugger.LogError(gameObject.name);
            if (handle > 0)
            {
                //App.LuaManager.CallObjectFunction(handle, "_OpenView");
                App.LuaManager.CallObjectFunction(handle, "OpenView", args[0]);
            }
        }

        public virtual void ReopenView()
        {
            if (handle > 0)
            {
                //App.LuaManager.CallObjectFunction(handle, "_ReopenView");
                App.LuaManager.CallObjectFunction(handle, "ReopenView");
            }
        }

        public override void CloseView()
        {
            if (handle > 0)
            {
                //App.LuaManager.CallObjectFunction(handle, "_CloseView");
                App.LuaManager.CallObjectFunction(handle, "CloseView");
            }
        }

        public override void DestroyView()
        {
            if (handle > 0) App.LuaManager.CallObjectFunction(handle, "DestroyView");
            base.DestroyView();
        }

        private void Start()
        {
            StartCoroutine(StartInit());
        }


        IEnumerator StartInit()
        {
            int[] temp = { 1, 2, 3 };
            yield return new WaitForSeconds(1);
            InitView(temp);
        }

    }
}