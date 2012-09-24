using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace bamsak
{
    class Reflection : IQuickSnapPlugin
    {
        /*
         *  The purpose of this class is to integrate the application's saving mechanism with
         *  DLLs that arbitrarily link to a website.
         */


        Type myType;
        object instance;
        CroppedBitmap bitmap;
        String dll;

        public Reflection()
        {

        }

        public void loadDLL(String dll)
        {
            this.dll = dll;
            Assembly testAssembly = Assembly.LoadFile(dll);
            this.myType = testAssembly.GetType("QuickSnapPlugin.Plugin");
            this.instance = Activator.CreateInstance(myType);
        }

        public void loadDLL(Reflection r)
        {
            this.loadDLL(r.dll);
        }

        public string getUrl()
        {
            return (string)myType.InvokeMember("getUrl",
                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                null, instance, null);

        }

        public string getServiceName()
        {
            return (string)myType.InvokeMember("getServiceName",
                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                null, instance, null);
        }

        public void upload(CroppedBitmap image)
        {
            myType.InvokeMember("upload",
                            BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                            null, instance, new Object[] {image});
        }


        public void setBitmap(CroppedBitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public CroppedBitmap getBitmap()
        {
            return this.bitmap;
        }

        public bool isLocal()
        {
            return (bool)myType.InvokeMember("isLocal",
             BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
             null, instance, null);
        }

        public void setSaveLocation(string p)
        {
            myType.InvokeMember("setSaveLocation",
                            BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                            null, instance, new Object[] { p});
        }
    }
}
