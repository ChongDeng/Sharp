using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using WebSocketSharp;

namespace CSharpDemo
{  
    [DataContract]
    public class ServerPairMessage
    {
        [DataMember]
        public String Command;

        [DataMember]
        public String MPId;

        [DataMember]
        public String Pin;
    }

    [DataContract]
    public class ServerSignInMessage
    {
        [DataMember]
        public String Command;

        [DataMember]
        public String MPId;

        [DataMember]
        public String UserName;

        [DataMember]
        public String Password;
    }

    [DataContract]
    public class ServerSignOutMessage
    {
        [DataMember]
        public String Command;

        [DataMember]
        public String MPId;

        [DataMember]
        public String SessionKey;
    }

    [DataContract]
    public class MondoPadPairResponse
    {
        [DataMember]
        public String Command;

        [DataMember]
        public String MPId;

        [DataMember]
        public int Result;

        [DataMember]
        public String Msg;
    }

    [DataContract]
    public class MondoPadSignResponse
    {
        [DataMember]
        public String Command;

        [DataMember]
        public String MPId;

        [DataMember]
        public int Result;

        [DataMember]
        public String Msg;

        [DataMember]
        public String SessionKey;
    }



    enum Commands { Pair = 1, SignIn, SignOut, Registration,Attach};
    [DataContract]
    public class MondopadMessage
    {
        [DataMember]
        public int Command;

        [DataMember]
        public String MPId;

        [DataMember]
        public String ReqId;

        [DataMember]
        public String UserName;

        [DataMember]
        public String Password;

        [DataMember]
        public String Pin;

        [DataMember]
        public String SessionKey;

        [DataMember]
        public int MobileReqId;

        [DataMember]
        public int SignInType;
    }

    public class MondopadResponse
    {
        [DataMember]
        public int Result;

        [DataMember]
        public String ReqId;

        [DataMember]
        public String SessionKey;

        [DataMember]
        public String msg;

        [DataMember]
        public int Command;

        [DataMember]
        public String MPId;

        [DataMember]
        public String MobileReqId;
    }

    [DataContract]
    public class SoftwareVersionInfo
    {
        [DataMember]
        public String version_code;

        [DataMember]
        public String version_name;

        [DataMember]
        public String version_desc;

        [DataMember]
        public String version_path;
    }

    class Program
    {
        static string Pin = "1111";
        static string MPId = "5555";

        static string MPIdVal = "5555";
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                MPIdVal = args[0];
                Console.Out.WriteLine("set mondopad id to :" + MPIdVal);               
            }
            //WaitChildThread();
            //Console.Out.WriteLine("child thread exits now!");
            //isProcessExists();


            //MonitoProcess();

            //getCurrentDirectory();

            //getIPAddress();

            //ThreadAndTaskTest();
            //ThreadAndTaskParaTest();
            //TaskReturnValueTest();
            //TaskReturnValueTest2();

            //ThreadPoolTest();

            //SemaphoreTest();

            //TaskCatchExceptionTest();

            //AsyncTaskTest();
            //AsyncTaskTest2();
            //AsyncTaskTest3();
            //AsyncTaskTest4();
            //AsyncTaskTest5();

            //=========================



            //WebSocketClientTest();

            //WebSocketClientQuery();

            //WebSocketConnWithData();

            //MobileSignInWebSocket();

            //MobileAttachWebSocket();

            //MobileSignOutWebSocket();




            //WebSocketConnect();




            //MobileSignIn();

            //MobileSignOut();

            //MobileStatus();

            //MondopadSignOutEvent();

            //===============   test =================
            //MondopadConcurrencyTest();

            //MobileSignInTest();

            //MobileSignOutTest();

            //MobileStatusTest();

            //====================================

            //NamePipeClientTest();

            //JsonToStringTest();

            //String uri = "http://localhost:8080/AutoSignInAgentServer/version.html";
            SoftwareUpdater();

            Console.ReadLine();
        }

        //static void getWebPageContent()
        //{
        //    try
        //    {
        //        WebClient client = new WebClient();
        //        //string downloadString = client.DownloadString("http://www.gooogle.com");
        //        string VersionInfoStr = client.DownloadString("http://localhost:8080/AutoSignInAgentServer/version.html");

        //        SoftwareVersionInfo VersionInfo = new SoftwareVersionInfo();
        //        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(VersionInfoStr));
        //        DataContractJsonSerializer djs = new DataContractJsonSerializer(VersionInfo.GetType());
        //        VersionInfo = djs.ReadObject(ms) as SoftwareVersionInfo;
        //        ms.Close();

        //        Console.Out.WriteLine("version info: " + VersionInfoStr);

        //        Console.Out.WriteLine("vesion path: " + VersionInfo.version_path);

        //        client.DownloadFile(VersionInfo.version_path, "office365auth.exe");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Out.WriteLine("get exception: " + ex.Message);
        //    }

        //}

        static String FilePathToSave = "";
        private static String ConfigXmlFile = "C:\\Users\\fqyya\\Desktop\\testlog\\Office365AuthConfig.xml";
        static String LocalSoftwareVersion = "";
        static String VersionServerUrl = "";

        static Boolean ReadConfig()
        {
            try
            {
                if (!File.Exists(ConfigXmlFile))
                {
                    Console.Out.WriteLine(ConfigXmlFile + "does not exist!");
                    return false;
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(ConfigXmlFile);

                    XmlNode SavePathNode = doc.DocumentElement.SelectSingleNode("/config/SavePath");
                    if (SavePathNode == null)
                    {
                        Console.Out.WriteLine("No SavePath in " + ConfigXmlFile);
                        return false;
                    }
                    FilePathToSave = SavePathNode.InnerText.Trim();
                    
                    XmlNode CurrentVersionNode = doc.DocumentElement.SelectSingleNode("/config/CurrentVersion");
                    if (CurrentVersionNode == null)
                    {
                        Console.Out.WriteLine("No CurrentVersion in " + ConfigXmlFile);
                        return false;
                    }
                    LocalSoftwareVersion = CurrentVersionNode.InnerText.Trim();

                    XmlNode VersionServerUrlNode = doc.DocumentElement.SelectSingleNode("/config/ServerUrl");
                    if (VersionServerUrlNode == null)
                    {
                        Console.Out.WriteLine("No ServerUrl in " + ConfigXmlFile);
                        return false;
                    }
                    VersionServerUrl = VersionServerUrlNode.InnerText.Trim();
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Exception occured during reading " + ConfigXmlFile + ", exception: " + ex);
                return false;
            }

            return true;
        }
                
        private static Boolean StoreSoftwareVersion(String VersionValue)
        {
            try
            {
                XmlDocument Doc = new XmlDocument();
                Doc.Load(ConfigXmlFile);

                XmlNode CurrentVersionNode = Doc.SelectSingleNode("/config/CurrentVersion");
                CurrentVersionNode.InnerText = VersionValue;
                Doc.Save(ConfigXmlFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        static void SoftwareUpdater()
        {
            if (!ReadConfig())
            {
                Console.Out.WriteLine("Failed to read config file to get SavePath and CurrentVersion");
                return;
            }

            WebClient client = null;
            SoftwareVersionInfo VersionInfo = new SoftwareVersionInfo();
            
            try
            {
                client = new WebClient();
                string VersionInfoStr = client.DownloadString(VersionServerUrl);
                Console.Out.WriteLine("software info: " + VersionInfoStr);

                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(VersionInfoStr));
                DataContractJsonSerializer djs = new DataContractJsonSerializer(VersionInfo.GetType());
                VersionInfo = djs.ReadObject(ms) as SoftwareVersionInfo;
                ms.Close();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("get exception: " + ex.Message);
                VersionInfo.version_path = null;
                VersionInfo.version_code = null;
            }           
            

            if (VersionInfo.version_code != null)
            {
                if(VersionCompare(LocalSoftwareVersion, VersionInfo.version_code) >= 0)
                {
                    Console.Out.WriteLine("You are using latest software version!");
                    return;
                }

                Console.Out.WriteLine("software version: " + VersionInfo.version_code);
                Console.Out.WriteLine("software path: " + VersionInfo.version_path);
                
                Thread th = new Thread(() =>
                {
                    Boolean isDownloadingFinished = false;
                    while (!isDownloadingFinished)
                    {
                        try
                        {
                            Console.Out.WriteLine("begin to download " + VersionInfo.version_path);
                            client.DownloadFile(VersionInfo.version_path, FilePathToSave);                                                   
                            Console.Out.WriteLine("finished downloading " + VersionInfo.version_path + " to " + FilePathToSave);
                            isDownloadingFinished = true;
                            if (!StoreSoftwareVersion(VersionInfo.version_code))
                            {
                                Console.Out.WriteLine("Failed to save new software version");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Out.WriteLine("get exception2: " + ex.Message);
                            Thread.Sleep(2000);
                        }
                    }
                    
                });
                th.IsBackground = true;
                th.Start();
            }
        }
        
        static int VersionCompare(String Version1, String Version2)
        {
            int len1 = Version1.Length;
            int len2 = Version2.Length;

            int pos = 0;
            while (pos < Math.Min(len1, len2))
            {
                if (Version1[pos] == Version2[pos])
                {
                    pos += 1;
                    continue;
                }
                else if (Version1[pos] == '.')
                    return -1;
                else if (Version2[pos] == '.')
                    return 1;
                else if (Version1[pos] < Version2[pos])
                    return -1;
                else
                    return 1;                
            }                

            return 0;
        }

        static void NamePipeClientTest()
        {
            StartServer();
            Task.Delay(1000).Wait();


            //Client
            var client = new NamedPipeClientStream("ScutPipe");
            client.Connect();
            StreamReader reader = new StreamReader(client);
            StreamWriter writer = new StreamWriter(client);

            while (true)
            {
                string input = Console.ReadLine();
                if (String.IsNullOrEmpty(input)) break;
                writer.WriteLine(input);
                writer.Flush();
                Console.WriteLine("get response: " + reader.ReadLine());
            }
        }

        static void StartServer()
        {
            Task.Factory.StartNew(() =>
            {
                var server = new NamedPipeServerStream("ScutPipe");
                server.WaitForConnection();
                StreamReader reader = new StreamReader(server);
                StreamWriter writer = new StreamWriter(server);
                while (true)
                {
                    var line = reader.ReadLine();
                    writer.WriteLine(String.Join("", line.Reverse()));
                    writer.Flush();
                }
            });
        }

        static void TaskCatchExceptionTest()  {
            try
            {
                var task = Task.Run(() => { Do(); });
                task.Wait();  // 在调用了这句话之后，主线程才能捕获task里面的异常

                // 对于有返回值的Task, 我们接收了它的返回值就不需要再调用Wait方法了
                // GetName 里面的异常我们也可以捕获到
                var task2 = Task.Run(() => { return GetName(); });
                var name = task2.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception!");
            }
        }

        static void Do() { throw null; }
        static string GetName() { throw null; }

        // 我们限制能同时访问的线程数量是3
        static SemaphoreSlim _sem = new SemaphoreSlim(3);
        /*信号量：它可以控制对某一段代码或者对某个资源访问的线程的数量，超过这个数量之后，
         * 其它的线程就得等待，只有等现在有线程释放了之后，下面的线程才能访问。
         * 这个跟锁有相似的功能，只不过不是独占的，它允许一定数量的线程同时访问。
         * 
         * demo解释： 在最开始的时候，前3个排队之后就立即进入执行，但是4和5，只有等到有线程退出之后才可以执行
         */
        static void SemaphoreTest() {
            for (int i = 1; i <= 5; i++) new Thread(Enter).Start(i);
            Console.ReadLine();
        }

        static void Enter(object id)
        {
            Console.WriteLine(id + " begin to queque...");
            _sem.Wait();
            Console.WriteLine(id + " begin to execute！");
            Thread.Sleep(1000 * 3);
            Console.WriteLine(id + " finshed!");
            Thread.Sleep(1000 * 3);
            _sem.Release();
        }

        //Thead是不能返回值的，但是作为更高级的Task当然要弥补一下这个功能。
        static void TaskReturnValueTest() {
            Console.WriteLine("I am mian thread：Thread Id {0}", Thread.CurrentThread.ManagedThreadId);
            // GetDayOfThisWeek 运行在另外一个线程中
            var dayName = Task.Run<string>(() => { return GetDayOfThisWeek(); });
            Console.WriteLine("get result from new Task：{0}", dayName.Result);
        }

        static String GetDayOfThisWeek()  {
            Console.WriteLine("new thread：Thread Id {0}", Thread.CurrentThread.ManagedThreadId);
            string result = "yes";
            Thread.Sleep(5000);
            return result;
        }

        static void TaskReturnValueTest2() {
            //1 要在主线程中等待后台线程执行完毕，可以使用Wait方法(会以同步的方式来执行)。
            //2 不用Wait则会以异步的方式来执行
            Console.WriteLine("main thread starts");           
            Task<string> task = Task<string>.Run(() => {
                Thread.Sleep(5000);
                return Thread.CurrentThread.ManagedThreadId.ToString();
            });
            //task.Wait(); 如果用到了这行，就会等待task执行完毕
            //会等到task执行完毕才会输出;
            Console.WriteLine("result: " + task.Result);
            //!!！如果注释掉上面这一行，不会等待task执行完毕
            Console.WriteLine("main thread exits");
            /* 通过task.Result可以取到返回值，若取值的时候，
             * 后台线程还没执行完，则会等待其执行完毕！*/

            //Task任务可以通过CancellationTokenSource类来取消
        }

        static void ThreadAndTaskParaTest() {
            new Thread(Go2).Start("arg1"); // 没有匿名委托之前，我们只能这样传入一个object的参数
            //发现不好使
            new Thread(delegate () {  // 有了匿名委托之后...
                GoGoGo("china", "hubei", "wuhan");
            });
            new Thread(() => {  // 当然,还有 Lambada
                GoGoGo("1", "2", "3");
            }).Start();
            Task.Run(() => {  // Task能这么灵活，也是因为有了Lambda呀。
                GoGoGo("a", "b", "c");
            });
        }
        public static void GoGoGo(string arg1, string arg2, string arg3) {
            Console.Out.WriteLine("first arg: " + arg1 + " second arg: " + arg2 + " thrid arg: " + arg3);
        }

        /* 线程的创建是比较占用资源的一件事情，.NET 为我们提供了线程池来帮助
         * 我们创建和管理线程。Task是默认会直接使用线程池，但是Thread不会。
         * 如果我们不使用Task，又想用线程池的话，可以使用ThreadPool类。
         */
        static void ThreadPoolTest() {
            Console.WriteLine("I am mian thread：Thread Id {0}", Thread.CurrentThread.ManagedThreadId);
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(m =>
                {
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString());
                });
            }
            //从打印结果, 可以看到，虽然执行了10次，但并没有创建10个线程。

            Console.ReadLine();
        }

        public static void Go2(object data)
        {
            Console.WriteLine("I am new thread:Thread Id {0}", Thread.CurrentThread.ManagedThreadId);
        }

        static void ThreadAndTaskTest() {
            new Thread(Go).Start();    // .NET 1.0开始就有的
            Task.Factory.StartNew(Go); // .NET 4.0 引入了 TPL
            Task.Run(new Action(Go));  // .NET 4.5 新增了一个Run的方法
        }
        public static void Go()
        {
            Console.WriteLine("I am a new thread, id: " + Thread.CurrentThread.ManagedThreadId);
        }

        static void MobileStatusTest()
        {
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/AutoSignInAgentServer/registration"))
            using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/AutoSignInAgentServer/registration"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    if (e.IsBinary)
                    {
                        String message = System.Text.Encoding.UTF8.GetString(e.RawData);
                        Console.WriteLine("get message: " + message);
                    }

                    //Console.WriteLine("get message: " + e.Data);                  
                };

                ws.Connect();

                String data = "{\"Command\":5,\"MPId\":\"" + MPIdVal + "\",\"SessionKey\":\"4a1b55c1b870460cbe15a025d1478c66\", \"SignInType\":1, \"MobileReqId\":\"123\"}";
                Console.WriteLine("send data: " + data);
                //ws.Send(data);
                ws.Send(Encoding.UTF8.GetBytes(data));
                Console.ReadLine();
            }
        }

        static void MobileSignOutTest()
        {
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/AutoSignInAgentServer/registration"))
            using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/AutoSignInAgentServer/registration"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    if (e.IsBinary)
                    {
                        String message = System.Text.Encoding.UTF8.GetString(e.RawData);
                        Console.WriteLine("get message: " + message);
                    }

                    //Console.WriteLine("get message: " + e.Data);                  
                };

                ws.Connect();

                String data = "{\"Command\":3,\"MPId\":\"" + MPIdVal + "\",\"SessionKey\":\"36d4b04b69514458a9db6d0bcc02fcc8\", \"SignInType\":1, \"MobileReqId\":\"123\"}";
                Console.WriteLine("send data: " + data);
                //ws.Send(data);
                ws.Send(Encoding.UTF8.GetBytes(data));

                Console.ReadLine();
            }
        }

        static void MobileSignInTest2()
        {
            int ThreadNum = 100;
            for (int i = 0; i < ThreadNum; ++i)
            {
                Thread th = new Thread(() =>
                {
                    Console.Out.WriteLine("launched thread with id: " + Thread.CurrentThread.ManagedThreadId);

                    String ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

                    while (true)
                    {
                        
                    }
                });
                th.IsBackground = true;
                th.Start();
            }
       }

        static void MobileSignInTest()
        {
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/AutoSignInAgentServer/registration"))
            using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/AutoSignInAgentServer/registration"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    if (e.IsBinary)
                    {
                        String message = System.Text.Encoding.UTF8.GetString(e.RawData);
                        Console.WriteLine("get message: " + message);
                    }

                    //Console.WriteLine("get message: " + e.Data);                  
                };

                ws.Connect();

                String data = "{\"Command\":2,\"MPId\":\"" + MPIdVal + "\",\"Pin\":\"1111\",\"UserName\":\"hayward-test@ifc.com\",\"Password\":\"6JRDdkUT8dZRE4fK\", \"MobileReqId\":\"123\",\"SignInType\":1}";
                Console.WriteLine("send data: " + data);
                //ws.Send(data);
                ws.Send(Encoding.UTF8.GetBytes(data));
            }
        }

        static void MondopadConcurrencyTest()
        {
            int ThreadNum = 100;
            for (int i = 0; i < ThreadNum; ++i)
            {
                Thread th = new Thread(() =>
                {
                    Console.Out.WriteLine("launched thread with id: " + Thread.CurrentThread.ManagedThreadId);

                    String ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

                    while (true) {

                        //using (var ws = new WebSocket("ws://10.4.1.255:8080/AutoSignInAgentServer/registration"))
                        using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/AutoSignInAgentServer/registration"))
                        {
                            ws.OnMessage += (sender, e) =>
                            {
                                Console.WriteLine("Thread " + ThreadId + " get message: " + e.Data);

                                String message = null;
                                //if (e.IsText)
                                if (e.IsBinary)
                                {
                                    message = System.Text.Encoding.UTF8.GetString(e.RawData);
                                }
                                else
                                    message = e.Data;

                                MondopadMessage Message = new MondopadMessage();
                                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(message));
                                DataContractJsonSerializer djs = new DataContractJsonSerializer(Message.GetType());
                                Message = djs.ReadObject(ms) as MondopadMessage;
                                ms.Close();

                                MondopadResponse response = new MondopadResponse();
                                response.ReqId = Message.ReqId;
                                response.Command = Message.Command;
                                response.MPId = Message.MPId;

                                Console.WriteLine("Command: " + Message.Command);
                                Console.WriteLine("MPId: " + Message.MPId);
                                Console.WriteLine("ReqId: " + Message.ReqId);
                                Console.WriteLine("UserName: " + Message.UserName);
                                Console.WriteLine("Password: " + Message.Password);
                                Console.WriteLine("Pin: " + Message.Pin);
                                Console.WriteLine("SessionKey: " + Message.SessionKey);
                                Console.WriteLine("SignInType: " + Message.SignInType);
                                Console.WriteLine("MobileReqId: " + Message.MobileReqId);

                                if (Message.Command == (int)Commands.Pair)
                                {
                                    Console.WriteLine("yes: ");

                                    if (Message.Pin != Pin)
                                    {
                                        response.Result = 3;
                                        response.msg = "Invalid Pin";
                                    }
                                    else
                                    {
                                        response.Result = 1;
                                        response.msg = "success";
                                    }
                                }

                                else if (Message.Command == (int)Commands.SignIn)
                                {
                                    Console.WriteLine("sign in: ");

                                    response.Result = 1;
                                    response.msg = "success";
                                    response.SessionKey = "2";
                                    response.MobileReqId = "123";
                                }

                                else if (Message.Command == (int)Commands.SignOut)
                                {
                                    Console.WriteLine("sign out: ");

                                    response.Result = 2;
                                    response.msg = "success";
                                    response.SessionKey = "sign out";
                                    response.MobileReqId = "123";
                                }

                                else if (Message.Command == (int)Commands.Attach)
                                {
                                    Console.WriteLine("attach: ");

                                    response.Result = 1;
                                    response.msg = "attach";
                                    response.SessionKey = "attach";
                                }

                                MemoryStream ResponseStream = new MemoryStream();
                                DataContractJsonSerializer ResponseJs = new DataContractJsonSerializer(typeof(MondopadResponse));
                                ResponseJs.WriteObject(ResponseStream, response);
                                byte[] ResponseJson = ResponseStream.ToArray();
                                ResponseStream.Close();
                                string data = Encoding.UTF8.GetString(ResponseJson, 0, ResponseJson.Length);
                                //ws.Send(data);
                                ws.Send(ResponseJson);
                            };

                            ws.Connect();

                            //ws.Send("{\"Command\":4,\"MPId\":\"654321\"}");

                            //String msg = "{\"Command\":4,\"MPId\":\"1111\"}";

                            String msg = "{\"Command\":4,\"MPId\":\"" + ThreadId + "\"}";
                            Console.Out.WriteLine("Thread " + ThreadId + " send data to fileshare server : " + msg);
                            ws.Send(Encoding.UTF8.GetBytes(msg));

                            //Thread.Sleep(10000);
                            Thread.Sleep(Thread.CurrentThread.ManagedThreadId * 1000 * 2);
                            ws.Close();
                            Console.Out.WriteLine("Thread " + ThreadId + " closed connection!");
                            //Thread.Sleep(10000);
                            Thread.Sleep(Thread.CurrentThread.ManagedThreadId * 1000 * 2);

                            //Console.ReadLine();
                        }

                    }
                });
                th.IsBackground = true;
                th.Start();
            }
        }

        static void WebSocketConnect()
        {
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/AutoSignInAgentServer/registration"))
            using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/AutoSignInAgentServer/registration"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    Console.WriteLine("get message: " + e.Data);

                    String message = null;
                    //if (e.IsText)
                    if (e.IsBinary)
                    {
                        message = System.Text.Encoding.UTF8.GetString(e.RawData);
                    }
                    else
                        message = e.Data;

                    MondopadMessage Message = new MondopadMessage();
                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(message));
                    DataContractJsonSerializer djs = new DataContractJsonSerializer(Message.GetType());
                    Message = djs.ReadObject(ms) as MondopadMessage;
                    ms.Close();

                    MondopadResponse response = new MondopadResponse();
                    response.ReqId = Message.ReqId;
                    response.Command = Message.Command;
                    response.MPId = Message.MPId;

                    Console.WriteLine("Command: " + Message.Command);
                    Console.WriteLine("MPId: " + Message.MPId);
                    Console.WriteLine("ReqId: " + Message.ReqId);
                    Console.WriteLine("UserName: " + Message.UserName);
                    Console.WriteLine("Password: " + Message.Password);
                    Console.WriteLine("Pin: " + Message.Pin);
                    Console.WriteLine("SessionKey: " + Message.SessionKey);
                    Console.WriteLine("SignInType: " + Message.SignInType);
                    Console.WriteLine("MobileReqId: " + Message.MobileReqId);

                    if (Message.Command == (int)Commands.Pair)
                    {
                        Console.WriteLine("yes: ");

                        if (Message.MPId != MPId)
                        {
                            response.Result = 2;
                            response.msg = "Failed to find MP";
                        }
                        else if (Message.Pin != Pin)
                        {
                            response.Result = 3;
                            response.msg = "Invalid Pin";
                        }
                        else
                        {
                            response.Result = 1;
                            response.msg = "success";
                        }
                    }

                    else if (Message.Command == (int)Commands.SignIn)
                    {
                        Console.WriteLine("sign in: ");

                        response.Result = 1;
                        response.msg = "success";
                        response.SessionKey = "2";
                        response.MobileReqId = "123";
                    }

                    else if (Message.Command == (int)Commands.SignOut)
                    {
                        Console.WriteLine("sign out: ");

                        response.Result = 2;
                        response.msg = "success";
                        response.SessionKey = "sign out";
                        response.MobileReqId = "123";
                    }

                    else if (Message.Command == (int)Commands.Attach)
                    {
                        Console.WriteLine("attach: ");

                        response.Result = 1;
                        response.msg = "attach";
                        response.SessionKey = "attach";
                    }

                    MemoryStream ResponseStream = new MemoryStream();
                    DataContractJsonSerializer ResponseJs = new DataContractJsonSerializer(typeof(MondopadResponse));
                    ResponseJs.WriteObject(ResponseStream, response);
                    byte[] ResponseJson = ResponseStream.ToArray();
                    ResponseStream.Close();
                    string data = Encoding.UTF8.GetString(ResponseJson, 0, ResponseJson.Length);
                    //ws.Send(data);
                    ws.Send(ResponseJson);
                };

                ws.Connect();

                //ws.Send("{\"Command\":4,\"MPId\":\"654321\"}");

                //String msg = "{\"Command\":4,\"MPId\":\"1111\"}";

                String msg = "{\"Command\":4,\"MPId\":\"" + MPIdVal + "\"}";
                Console.Out.WriteLine("send data to fileshare server : " + msg);
                ws.Send(Encoding.UTF8.GetBytes(msg));

                Console.ReadLine();
            }
        }

        static void MondopadSignOutEvent()
        {
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/AutoSignInAgentServer/registration"))
            using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/AutoSignInAgentServer/registration"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    if (e.IsBinary)
                    {
                        String message = System.Text.Encoding.UTF8.GetString(e.RawData);
                        Console.WriteLine("get message: " + message);
                    }

                    //Console.WriteLine("get message: " + e.Data);                  
                };

                ws.Connect();

                String data = "{\"Command\":6,\"MPId\":\"1111\",\"SessionKey\":\"9b914d10151c42188c6939ead3421046\", \"SignInType\":1, \"MobileSessionId\":3}";
                Console.WriteLine("send data: " + data);
                //ws.Send(data);
                ws.Send(Encoding.UTF8.GetBytes(data));
                Console.ReadLine();
            }
        }

        static void MobileStatus()
        {
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/AutoSignInAgentServer/registration"))

            using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/AutoSignInAgentServer/registration"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    if (e.IsBinary)
                    {
                        String message = System.Text.Encoding.UTF8.GetString(e.RawData);
                        Console.WriteLine("get message: " + message);
                    }

                    //Console.WriteLine("get message: " + e.Data);                  
                };

                ws.Connect();

                String data = "{\"Command\":5,\"MPId\":\"CBN632554581506\",\"SessionKey\":\"d8e3cbfac0b840108dd80915f7683c3b\", \"SignInType\":1, \"MobileReqId\":\"123\"}";
                Console.WriteLine("send data: " + data);
                //ws.Send(data);
                ws.Send(Encoding.UTF8.GetBytes(data));
                Console.ReadLine();
            }
        }

        static void MobileSignOut()
        {
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/AutoSignInAgentServer/registration"))
            using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/AutoSignInAgentServer/registration"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    if (e.IsBinary)
                    {
                        String message = System.Text.Encoding.UTF8.GetString(e.RawData);
                        Console.WriteLine("get message: " + message);
                    }

                    //Console.WriteLine("get message: " + e.Data);                  
                };

                ws.Connect();

                String data = "{\"Command\":3,\"MPId\":\"CBN632554581506\",\"SessionKey\":\"0e7bba3d89074911b3e0fbdc723266db\", \"SignInType\":1, \"MobileReqId\":\"123\"}";
                Console.WriteLine("send data: " + data);
                //ws.Send(data);
                ws.Send(Encoding.UTF8.GetBytes(data));
                Console.ReadLine();
            }
        }


        static void MobileSignIn()
        {
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/AutoSignInAgentServer/registration"))
            using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/AutoSignInAgentServer/registration"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    if (e.IsBinary)
                    {
                        String message = System.Text.Encoding.UTF8.GetString(e.RawData);
                        Console.WriteLine("get message: " + message);
                    }

                    //Console.WriteLine("get message: " + e.Data);                  
                };

                ws.Connect();

                //String data = "{\"Command\":2,\"MPId\":\"dtb15aa00354201d023000\",\"Pin\":\"7348\",\"UserName\":\"hayward-test@ifc.com\",\"Password\":\"6JRDdkUT8dZRE4fK\", \"MobileReqId\":\"123\",\"SignInType\":1}";

                String data = "{\"Command\":2,\"MPId\":\"CBN632554581506\",\"Pin\":\"8584\",\"UserName\":\"hayward-test@ifc.com\",\"Password\":\"6JRDdkUT8dZRE4fK\", \"MobileReqId\":\"123\",\"SignInType\":1}";
                Console.WriteLine("send data: " + data);
                //ws.Send(data);
                ws.Send(Encoding.UTF8.GetBytes(data));
                Console.ReadLine();
            }
        }



        static void WebSocketClientTest()
        {
            using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign/?name=nobita"))
            {
                ws.OnMessage += (sender, e) =>
                  Console.WriteLine("Laputa says: " + e.Data);

                ws.Connect();
                Console.ReadLine();
            }
        }

        static void MobileSignOutWebSocket()
        {
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign/scut"))
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign?name=scut&pwd=guangzhou"))
            //using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/registration?HostName=scut&MPId=654321"))
            String url = "{\"Command\":3,\"MPId\":\"654321\",\"SessionKey\":\"123\"}";
            url = HttpUtility.UrlEncode(url);
            String uri = "ws://localhost:8080/AutoSignInAgentServer/registration?" + url;
            using (var ws = new WebSocket(uri))
            {
                ws.OnMessage += (sender, e) =>
                {
                    //if (e.IsBinary)
                    //{
                    //    String message = System.Text.Encoding.UTF8.GetString(e.RawData);
                    //    Console.WriteLine("get message: " + message);
                    //}
                    Console.WriteLine("get message: " + e.Data);

                };

                ws.Connect();

                Console.ReadLine();
            }
        }

        static void MobileAttachWebSocket()
        {
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign/scut"))
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign?name=scut&pwd=guangzhou"))
            //using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/registration?HostName=scut&MPId=654321"))
            String url = "{\"Command\":5,\"MPId\":\"654321\",\"SessionKey\":\"123\"}";
            url = HttpUtility.UrlEncode(url);
            String uri = "ws://localhost:8080/AutoSignInAgentServer/registration?" + url;
            using (var ws = new WebSocket(uri))
            {
                ws.OnMessage += (sender, e) =>
                {
                    //if (e.IsBinary)
                    //{
                    //    String message = System.Text.Encoding.UTF8.GetString(e.RawData);
                    //    Console.WriteLine("get message: " + message);
                    //}
                    Console.WriteLine("get message: " + e.Data);

                };

                ws.Connect();

                Console.ReadLine();
            }
        }

        static void MobileSignInWebSocket()
        {
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign/scut"))
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign?name=scut&pwd=guangzhou"))
            //using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/registration?HostName=scut&MPId=654321"))
            String url = "{\"Command\":2,\"MPId\":\"1111\",\"Pin\":\"3434\",\"UserName\":\"hayward-test@ifc.com\",\"Password\":\"6JRDdkUT8dZRE4fK\"}";
            url = HttpUtility.UrlEncode(url);
            String uri = "ws://localhost:8080/AutoSignInAgentServer/registration?" + url;
            using (var ws = new WebSocket(uri))
            {
                ws.OnMessage += (sender, e) =>
                {
                    //if (e.IsBinary)
                    //{
                    //    String message = System.Text.Encoding.UTF8.GetString(e.RawData);
                    //    Console.WriteLine("get message: " + message);
                    //}

                    Console.WriteLine("get message: " + e.Data);

                };

                ws.Connect();                

                Console.ReadLine();
            }
        }

        
        static void WebSocketConnWithData()
        {
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign/scut"))
            //using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign?name=scut&pwd=guangzhou"))
            //using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/registration?HostName=scut&MPId=654321"))
            using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/registration?{\"Command\":4,\"MPId\":\"654321\"}"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    Console.WriteLine("get message: " + e.Data);

                    String message = null;
                    //if (e.IsText)
                    if (e.IsBinary)
                    {
                        message = System.Text.Encoding.UTF8.GetString(e.RawData);
                    }
                    else
                        message = e.Data;

                    MondopadMessage Message = new MondopadMessage();
                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(message));
                    DataContractJsonSerializer djs = new DataContractJsonSerializer(Message.GetType());
                    Message = djs.ReadObject(ms) as MondopadMessage;
                    ms.Close();

                    MondopadResponse response = new MondopadResponse();
                    response.ReqId = Message.ReqId;

                    Console.WriteLine("Command: " + Message.Command);
                    Console.WriteLine("MPId: " + Message.MPId);
                    Console.WriteLine("ReqId: " + Message.ReqId);
                    Console.WriteLine("UserName: " + Message.UserName);
                    Console.WriteLine("Password: " + Message.Password);
                    Console.WriteLine("Pin: " + Message.Pin);
                    Console.WriteLine("SessionKey: " + Message.SessionKey);

                    if (Message.Command == (int)Commands.Pair)
                    {
                        Console.WriteLine("yes: ");

                        if (Message.MPId != MPId)
                        {
                            response.Result = 2;
                            response.msg = "Failed to find MP";
                        }
                        else if (Message.Pin != Pin)
                        {
                            response.Result = 3;
                            response.msg = "Invalid Pin";
                        }
                        else
                        {
                            response.Result = 1;
                            response.msg = "success";
                        }
                    }

                    else if (Message.Command == (int)Commands.SignIn)
                    {
                        Console.WriteLine("sign in: ");

                        response.Result = 1;
                        response.msg = "success";
                        response.SessionKey = "2";
                    }

                    else if (Message.Command == (int)Commands.SignOut)
                    {
                        Console.WriteLine("sign out: ");

                        response.Result = 2;
                        response.msg = "success";
                        response.SessionKey = "sign out";
                    }

                    else if (Message.Command == (int)Commands.Attach)
                    {
                        Console.WriteLine("attach: ");

                        response.Result = 1;
                        response.msg = "attach";
                        response.SessionKey = "attach";
                    }

                    MemoryStream ResponseStream = new MemoryStream();
                    DataContractJsonSerializer ResponseJs = new DataContractJsonSerializer(typeof(MondopadResponse));
                    ResponseJs.WriteObject(ResponseStream, response);
                    byte[] ResponseJson = ResponseStream.ToArray();
                    ResponseStream.Close();
                    string data = Encoding.UTF8.GetString(ResponseJson, 0, ResponseJson.Length);

                    ws.Send(data);
                };

                ws.Connect();

                Console.ReadLine();
            }
        }



        //static void WebSocketConnWithData()
        //{
        //    //using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign/scut"))
        //    //using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign?name=scut&pwd=guangzhou"))
        //    using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/registration?HostName=scut&MPId=654321"))
        //    //using (var ws = new WebSocket("ws://localhost:8080/AutoSignInAgentServer/registration?HostName=scut&MPId=654321&ReqId=12345&UserName=tom&Password=Cat"))
        //    {
        //        ws.OnMessage += (sender, e) =>
        //        {
        //            Console.WriteLine("get message: " + e.Data);

        //            if (e.IsText) {
        //                if (e.Data.Contains("\"Command\":\"pair\"")) {
        //                    ServerPairMessage Message = new ServerPairMessage();
        //                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(e.Data));
        //                    DataContractJsonSerializer djs = new DataContractJsonSerializer(Message.GetType());
        //                    Message = djs.ReadObject(ms) as ServerPairMessage;
        //                    ms.Close();

        //                    Console.WriteLine("MPId: " + Message.MPId);
        //                    Console.WriteLine("Pin: " + Message.Pin);

        //                    MondoPadPairResponse response = new MondoPadPairResponse();
        //                    response.MPId = Message.MPId;
        //                    response.Command = "pair";

        //                    if (Message.MPId != MPId)
        //                    {
        //                        response.Result = 2;
        //                        response.Msg = "Failed to find MP";
        //                    }
        //                    else if (Message.Pin != Pin)
        //                    {
        //                        response.Result = 3;
        //                        response.Msg = "Invalid Pin";
        //                    }
        //                    else
        //                    {
        //                        response.Result = 1;
        //                        response.Msg = "success";
        //                    }

        //                    MemoryStream ResponseStream = new MemoryStream();
        //                    DataContractJsonSerializer ResponseJs = new DataContractJsonSerializer(typeof(MondoPadPairResponse));
        //                    ResponseJs.WriteObject(ResponseStream, response);
        //                    byte[] ResponseJson = ResponseStream.ToArray();
        //                    ResponseStream.Close();
        //                    string data = Encoding.UTF8.GetString(ResponseJson, 0, ResponseJson.Length);

        //                    ws.Send(data);
        //                }
        //                else if (e.Data.Contains("\"Command\":\"SignIn\""))
        //                {
        //                    ServerSignInMessage Message = new ServerSignInMessage();
        //                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(e.Data));
        //                    DataContractJsonSerializer djs = new DataContractJsonSerializer(Message.GetType());
        //                    Message = djs.ReadObject(ms) as ServerSignInMessage;
        //                    ms.Close();

        //                    Console.WriteLine("MPId: " + Message.MPId);
        //                    Console.WriteLine("UserName: " + Message.UserName);
        //                    Console.WriteLine("Password: " + Message.Password);

        //                    MondoPadSignResponse response = new MondoPadSignResponse();
        //                    response.MPId = Message.MPId;
        //                    response.Command = "SignIn";
        //                    response.SessionKey = "abcdefg";

        //                    String res = CheckSignIn(Message.UserName, Message.Password);
        //                    if (res == "success") {
        //                        response.Result = 1;
        //                        response.Msg = "success";
        //                    } 
        //                    else if (res == "wrong password")
        //                    {
        //                        response.Result = 2;
        //                        response.Msg = "Invalid Pin";
        //                    }

        //                    MemoryStream ResponseStream = new MemoryStream();
        //                    DataContractJsonSerializer ResponseJs = new DataContractJsonSerializer(typeof(MondoPadSignResponse));
        //                    ResponseJs.WriteObject(ResponseStream, response);
        //                    byte[] ResponseJson = ResponseStream.ToArray();
        //                    ResponseStream.Close();
        //                    string data = Encoding.UTF8.GetString(ResponseJson, 0, ResponseJson.Length);

        //                    ws.Send(data);
        //                }
        //                else if (e.Data.Contains("\"Command\":\"SignOut\""))
        //                {
        //                    ServerSignOutMessage Message = new ServerSignOutMessage();
        //                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(e.Data));
        //                    DataContractJsonSerializer djs = new DataContractJsonSerializer(Message.GetType());
        //                    Message = djs.ReadObject(ms) as ServerSignOutMessage;
        //                    ms.Close();

        //                    Console.WriteLine("MPId: " + Message.MPId);
        //                    Console.WriteLine("SessionKey : " + Message.SessionKey);

        //                    MondoPadSignResponse response = new MondoPadSignResponse();
        //                    response.MPId = Message.MPId;
        //                    response.Command = "SignOut";
        //                    response.SessionKey = "abc";

        //                    String res = CheckSignOut(Message.MPId, Message.SessionKey);
        //                    if (res == "success")
        //                    {
        //                        response.Result = 1;
        //                        response.Msg = "success";
        //                    }
        //                    else if (res == "wrong id")
        //                    {
        //                        response.Result = 2;
        //                        response.Msg = "Failed to find MP";
        //                    }

        //                    MemoryStream ResponseStream = new MemoryStream();
        //                    DataContractJsonSerializer ResponseJs = new DataContractJsonSerializer(typeof(MondoPadSignResponse));
        //                    ResponseJs.WriteObject(ResponseStream, response);
        //                    byte[] ResponseJson = ResponseStream.ToArray();
        //                    ResponseStream.Close();
        //                    string data = Encoding.UTF8.GetString(ResponseJson, 0, ResponseJson.Length);

        //                    ws.Send(data);
        //                }
        //            }
        //        };                  

        //        ws.Connect();

        //        //MondopadMessage msg = new MondopadMessage();
        //        //msg.Command = "registration";
        //        //msg.ID = "123456";
        //        //msg.HostName = "192.168.1.6";

        //        //MemoryStream stream = new MemoryStream();
        //        //DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(MondopadMessage));
        //        //js.WriteObject(stream, msg);
        //        //byte[] json = stream.ToArray();
        //        //stream.Close();
        //        //string info = Encoding.UTF8.GetString(json, 0, json.Length);

        //        //ws.Send(info);

        //        Console.ReadLine();
        //    }
        //}

        static String CheckSignIn(String UserName, String Password) {
            String res = "success";

            if (UserName != "fqyang") return "wrong name";
            if (Password != "gnayqf") return "wrong password";

            return res;
        }

        static String CheckSignOut(String MPId, String SessionKey)
        {
            String res = "success";

            if (MPId != "654321") return "wrong id";
            if (SessionKey != "abc") return "wrong SessionKey";

            return res;
        }

        //static void WebSocketClientQuery()
        //{
        //    //using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign/name=nobita"))
        //    using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign?name=nobita&pwd=56"))
        //    {
        //        ws.OnMessage += (sender, e) =>
        //          Console.WriteLine("Laputa says: " + e.Data);

        //        ws.Connect();
        //        ws.Send("scut");
        //        Console.ReadKey(true);
        //    }
        //}


        static void WebSocketClientQuery()
        {
            using (var ws = new WebSocket("ws://10.4.1.255:8080/Office365Authenticator/AutoSign"))
            {
                ws.OnMessage += (sender, e) =>
                  Console.WriteLine("Laputa says: " + e.Data);

                ws.Connect();
                ws.Send("hello");
                Console.ReadKey(true);
            }
        }



        static void ShortKeyTest() {

        }
        static void getIPAddress() {
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach(IPAddress addr in ipadrlist) {
                if (addr.AddressFamily == AddressFamily.InterNetwork ||
                    addr.AddressFamily == AddressFamily.InterNetworkV6)
                    Console.Out.WriteLine(addr.ToString());
            }

        }

        static void ToUtcTime(String DumpFile)
        {
            FileInfo DumpFileInfo = new FileInfo(DumpFile);
            Console.WriteLine("ceash time 1: " + DumpFileInfo.LastWriteTime.ToString());
            //String CrashTime = DateTime.SpecifyKind(DumpFileInfo.LastWriteTime, DateTimeKind.Utc).ToString("yyyy-MM-dd-HH:mm:ss");
            String CrashTime = DumpFileInfo.LastWriteTime.ToUniversalTime().ToString("yyyy-MM-dd-HH:mm:ss");
            Console.WriteLine("ceash time 2: " + CrashTime);
        }

        static void getProcessorInfo()
        {
            Console.WriteLine("\n\nDisplaying Processor Name....");

            String ProcessorInfo = "unknown";

            RegistryKey ProcessorKey = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);

            if (ProcessorKey != null)
            {
                if (ProcessorKey.GetValue("ProcessorNameString") != null)
                {
                    ProcessorInfo = ProcessorKey.GetValue("ProcessorNameString").ToString();
                    int index = ProcessorInfo.IndexOf("CPU");
                    if (index != -1)
                    {
                        ProcessorInfo = ProcessorInfo.Substring(0, index - 1);
                    }

                    Console.WriteLine(ProcessorInfo);   //Display processor ingo.
                }
            }
        }

        static void getCurrentDirectory() {
            Console.Out.WriteLine("pwd: " + Environment.CurrentDirectory);
        }
        static void MonitoProcess() {           

            Thread th = new Thread(() =>
            {
                Process p = null;
                if (!isProcessExists())
                {
                    Console.Out.WriteLine("thred id: " + Thread.CurrentThread.ManagedThreadId);
                    p = new Process();
                    // Redirect the error stream of the child process.
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.FileName = "notepad.exe";
                    p.Start();
                    Console.Out.WriteLine("process starts now!");
                }       
          
                p.WaitForExit();
                Console.Out.WriteLine("process exits now!");
                MonitoProcess();
            });
            th.IsBackground = true;
            th.Start();
            
        }

        static bool isProcessExists() {
            Process[] pname = Process.GetProcessesByName("notepad");
            if (pname.Length == 0)
            {
                Console.Out.WriteLine("process is not running!");
                return false;
            }

            else {
                Console.Out.WriteLine("process is running!");
                return true;
            }

            return true;
                
        }


        static void WaitChildThread() {
            Process p = new Process();
            // Redirect the error stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = "notepad.exe";
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected error stream.
            // p.WaitForExit();
            // Read the error stream first and then wait.
            string error = p.StandardError.ReadToEnd();
            p.WaitForExit();
        }

        static void AsyncTaskTest()  {
            ScutAsyncMethod(); 
            Console.WriteLine("main Thread Id :{0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("main Thread exits");
        }
        static async Task ScutAsyncMethod(){//方法名前打async，可用await调用同样打上async的方法
            Console.WriteLine("ScutAsyncMethod start: Thread Id :{0}", Thread.CurrentThread.ManagedThreadId);
            await GetName2();
            Console.WriteLine("ScutAsyncMethod end: Task Id :{0}", Thread.CurrentThread.ManagedThreadId);
        }
        static async Task GetName2() { // 返回值前面加 async 之后，方法里面就可以用await了
            Console.WriteLine("GetName2 start: thread Id :{0}", Thread.CurrentThread.ManagedThreadId);
            await Task.Delay(5000);  
            Console.WriteLine("GetName2 end: Task Id :{0}", Thread.CurrentThread.ManagedThreadId);
        }



        static void AsyncTaskTest2() {
            Console.WriteLine("1, Thread Id: {0}\r\n", Thread.CurrentThread.ManagedThreadId);
            Test();
        }
        static async Task Test() {
            Console.WriteLine("2, Thread Id: {0}\r\n", Thread.CurrentThread.ManagedThreadId);
            var name = GetName3(); //我们这里没有用 await,所以下面的代码可以继续执行
                                   //但如果上面是await GetName()，下面代码就不会立即执行。
            Console.WriteLine("4 End calling GetName.\r\n");
            Console.WriteLine("6 Get result from GetName: {0}", await name);
            Console.WriteLine("7 name is: " + name.Result);
        }
        static async Task<string> GetName3() {           
            Console.WriteLine("3, thread Id: {0}", Thread.CurrentThread.ManagedThreadId);
            return await Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine("5, GetName Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);
                return "Jesse";
            });
        }

        static void AsyncTaskTest3()  {
            Console.WriteLine("1");
            AsyncMethod2();
            Console.WriteLine("2");
        }

        static async Task AsyncMethod2() {
            Task<string> task = Task.Run(() => {
                Thread.Sleep(5000);
                return "3, Hello World";
            });
            string str = await task;  //5 秒之后才会执行这里
            Console.WriteLine(str);
        }

        static async void AsyncTaskTest4()
        {
            Task<int> task = Method1();
            Method2();
            int count = await task;
            Method3(count);
        }

        public static async Task<int> Method1()
        {
            int count = 0;
            await Task.Run(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine(" Method 1");
                    count += 1;
                }
            });
            return count;
        }


        public static void Method2()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(" Method 2");
            }
        }


        public static void Method3(int count)
        {
            Console.WriteLine("Total count is " + count);
        }

        static void AsyncTaskTest5() {
            Task task = new Task(CallMethod);
            task.Start();
            task.Wait();
            Console.ReadLine();
        }
        static async void CallMethod() {
            string filePath = "C:\\Users\\fqyya\\Desktop\\log.txt";
            Task<int> task = ReadFile(filePath);
            Console.WriteLine(" Other Work 1");
            Console.WriteLine(" Other Work 2");

            int length = await task;
            Console.WriteLine(" Total length: " + length);
            Console.WriteLine(" After work 1");
            Console.WriteLine(" After work 2");
        }

        static async Task<int> ReadFile(string file)
        {
            int length = 0;

            Console.WriteLine(" File reading is stating");
            using (StreamReader reader = new StreamReader(file))
            {
                // Reads all characters from the current position to the end of the stream asynchronously  
                // and returns them as one string.  
                string s = await reader.ReadToEndAsync();

                length = s.Length;
            }
            Console.WriteLine(" File reading is completed");
            return length;
        }


        static void JsonToStringTest()
        {
            MondopadResponse response = new MondopadResponse();
            response.ReqId = "123";
            response.Result = 2;
            response.msg = "Failed to find MP";

            MemoryStream ResponseStream = new MemoryStream();
            DataContractJsonSerializer ResponseJs = new DataContractJsonSerializer(typeof(MondopadResponse));
            ResponseJs.WriteObject(ResponseStream, response);

            // wrong method Console.Out.WriteLine("res: " + ResponseStream.ToString());
            
            byte[] ResponseJson = ResponseStream.ToArray();
            ResponseStream.Close();
            string data = Encoding.UTF8.GetString(ResponseJson, 0, ResponseJson.Length);

            Console.Out.WriteLine("res: " + data);
        }
    }    
}
