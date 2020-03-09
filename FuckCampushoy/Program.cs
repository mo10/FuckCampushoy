using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuckCampushoy
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            var loadedAssemblies = new Dictionary<string, Assembly>();
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                String resourceName = "FuckCampushoy.Include." +
                new AssemblyName(args.Name).Name + ".dll";

                //Must return the EXACT same assembly, do not reload from a new stream
                if (loadedAssemblies.TryGetValue(resourceName, out Assembly loadedAssembly))
                {
                    return loadedAssembly;
                }

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                        return null;
                    Byte[] assemblyData = new Byte[stream.Length];

                    stream.Read(assemblyData, 0, assemblyData.Length);

                    var assembly = Assembly.Load(assemblyData);
                    loadedAssemblies[resourceName] = assembly;
                    return assembly;
                }
            };
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var cookie = loadCookie();
            if(cookie == null)
            {
                Application.Run(new Login());
            }
            else
            {
                var f = new Notify(null);
                f.Notify_Login(cookie);
                Application.Run(f);
            }
        }
        static void saveCookie(IDictionary<string, Cookie> cookies)
        {
            var savefile = AppDomain.CurrentDomain.BaseDirectory + "cookie.bin";
            var fd = File.OpenWrite(savefile);
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fd, cookies);
            fd.Close();
        }
        static IDictionary<string, Cookie> loadCookie()
        {
            var savefile = AppDomain.CurrentDomain.BaseDirectory + "cookie.bin";
            if (File.Exists(savefile))
            {
                var fd = File.OpenRead(savefile);
                var binaryFormatter = new BinaryFormatter();

                var c =  (IDictionary<string, Cookie>)binaryFormatter.Deserialize(fd);

                fd.Close();
                return c;
            }
            return null;
        }
    }
}
