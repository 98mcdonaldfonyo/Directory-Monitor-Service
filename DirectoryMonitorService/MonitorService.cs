using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryMonitorService
{
    public partial class MonitorService : ServiceBase
    {
        public string AccountNumber;

        public decimal Amount;

        public int CompanyName;

        public int CompnayId;

        public DateTime DateReceived;

        public decimal DepositAmount;

        public int DepositId;

        public string DepositRef;

        public string FirstName;

        public string LastName;

        private string _LOGPATH;

        private string _SOURCE;

        private string _TARGET;

        private FileSystemWatcher _watcher;

        public MonitorService()
        {
            InitializeComponent();
            this._SOURCE = ConfigurationManager.AppSettings["sourcePath"];
            this._TARGET = ConfigurationManager.AppSettings["targetPath"];
            this._LOGPATH = ConfigurationManager.AppSettings["logPath"];
        }

        public bool IncludeSubdirectories { get; set; }

        public void DirectoryChanged(object sender, FileSystemEventArgs e)
        {
            File.AppendAllText($"{_LOGPATH}\\log.txt", $"DirectoryChanged->Start\n");
            //When a new file arrives
            bool processed = false;
            processed = Process();

            if (processed)
            {
                MoveFile();
            }
            //Process();
            //MoveFile();

            var msg = $"{e.ChangeType}-{e.FullPath + "" + DateTime.Now}{System.Environment.NewLine}";
            File.AppendAllText($"{_LOGPATH}\\log.txt", msg);
            File.AppendAllText($"{_LOGPATH}\\log.txt", $"DirectoryChanged->End\n");
        }

        public bool FilesAlreadyInDir()

        {
            File.AppendAllText($"{_LOGPATH}\\log.txt", $"FilesAlreadyInDir->Start\n");
            _watcher = new FileSystemWatcher($"{_SOURCE}");//watches for new files

            if (System.IO.Directory.Exists($"{_SOURCE}"))
            {
                //string[] files = System.IO.Directory.GetFiles($"{_SOURCE}");
                bool processed = false;
                processed = Process();

                if (processed)
                {
                    MoveFile();
                }

                return true;
            }
            else
                // FileWatch(_SOURCE);//executes the new files(triggers Directory Changed) and Directory Chnaged triggers the process and move file methods
                File.AppendAllText($"{_LOGPATH}\\log.txt", $"FilesAlreadyInDir->End\n");
            return false;
            
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        public bool Process()

        {
            string fileName;
            try
            {
                File.AppendAllText($"{_LOGPATH}\\log.txt", $"Process->Start {System.DateTime.Now}\n");
                if (System.IO.Directory.Exists($"{_SOURCE}"))
                {
                    File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->DirExists {System.DateTime.Now}\n");
                    List<string> files = System.IO.Directory.GetFiles($"{_SOURCE}").ToList();
                    File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->Files fileCount: {files.Count}\n");
                    foreach (string s in files)
                    {
                        // fileName = System.IO.Path.GetFileName(s);
                        fileName = s;
                        File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->ForEach s filename: {s}\n" );

                        List<string> lines = System.IO.File.ReadAllLines(fileName).ToList();
                        File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->ForEach s fileLines: {lines.Count}\n");

                        File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->ForEach s fistLine0: {lines[0]}\n");
                        File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->ForEach s fistLine1: {lines[1]}\n");
                        File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->ForEach s fistLine2: {lines[2]}\n");
                        File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->ForEach s fistLine3: {lines[3]}\n");

                        //for (int x = 0; x < lines.Count; x++)
                        //{
                        //    File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->ForEach s fistLine: {lines[x]}\n");
                        //    List<string> entries = lines[x].Split('\t').ToList();

                        //    File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->Entries: AccountNumber {entries[0]}| FirstName {entries[1]}|LastName {entries[2]}|DepositAmount {entries[3]}|DepositId {entries[4]}|DateReceived {entries[5]}|\n");

                        //    string test = "45,66";//entries[3];
                        //    File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->Entries: test {test}\n");

                        //    decimal testd = Convert.ToDecimal(test, CultureInfo.CurrentCulture);
                        //    File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->Entries: testd {testd}\n");

                        //    AccountNumber = entries[0];
                        //    FirstName = entries[1];
                        //    LastName = entries[2];
                        //    DepositAmount = Convert.ToDecimal(entries[3], CultureInfo.CurrentCulture);
                        //    DepositId = (Int16.Parse(entries[4], CultureInfo.CurrentCulture));
                        //    DateReceived = DateTime.Parse(entries[5], CultureInfo.CurrentCulture);
                        //    DepositRef = AccountNumber;

                        //    File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->Entries: AccountNumber {AccountNumber}| FirstName {FirstName}|LastName {LastName}|DepositAmount {DepositAmount}|DepositId {DepositId}|DateReceived {DateReceived}|DepositRef {DepositRef}|\n");

                        //    //File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->Entries: AccountNumber {AccountNumber}| FirstName {FirstName}|LastName {LastName}|DepositAmount {DepositAmount}|DepositId {DepositId}|DateReceived {DateReceived}|DepositRef {DepositRef}|\n");

                        //    var context = new DevMasta();
                        //    var mock = new TMock()
                        //    {
                        //        AccountNumber = AccountNumber,
                        //        FirstName = FirstName,
                        //        LastName = LastName,
                        //        DepositAmount = DepositAmount,
                        //        DepositId = DepositId,
                        //        DateReceived = DateReceived
                        //    };

                        //    var deposit = new TDeposit
                        //    {
                        //        DepositId = DepositId,
                        //        ReferenceUsed = DepositRef,
                        //        DepositDate = DateReceived,
                        //        Amount = DepositAmount,

                        //        //in this case(txt) we have a Company Name :
                        //        //CompanyId=? But we have CompanyName.
                        //        //today!
                        //    };
                        //    try
                        //    {
                        //        if (DepositId.ToString() != null)
                        //        {
                        //            //mock = new TMock();
                        //            context.TMocks.Add(mock);
                        //            context.SaveChanges();
                        //            File.AppendAllText($"{_LOGPATH}\\log.txt", $"File {s} TMock saved DepositId: {mock.DepositId} ");
                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        string sx = ex.Message;
                        //        System.Console.WriteLine(sx);

                        //        File.AppendAllText($"{_LOGPATH}\\log.txt", ex.StackTrace);
                        //        File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->End False {System.DateTime.Now}\n");
                        //        return false;
                        //    }


                        //}

                        foreach (string line in lines)
                        {
                            File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->ForEach s fistLine3: {line}\n");
                            List<string> entries = line.Split('\t').ToList();

                            AccountNumber = entries[0];
                            FirstName = entries[1];
                            LastName = entries[2];
                            DepositAmount = Convert.ToDecimal(entries[3]);
                            DepositId = (Int16.Parse(entries[4]));
                            DateReceived = DateTime.Parse(entries[5]);
                            DepositRef = AccountNumber;

                            File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing->Entries: AccountNumber {AccountNumber}| FirstName {FirstName}|LastName {LastName}|DepositAmount {DepositAmount}|DepositId {DepositId}|DateReceived {DateReceived}|DepositRef {DepositRef}|\n");

                            var context = new DevMasta();
                            var mock = new TMock()
                            {
                                AccountNumber = AccountNumber,
                                FirstName = FirstName,
                                LastName = LastName,
                                DepositAmount = DepositAmount,
                                DepositId = DepositId,
                                DateReceived = DateReceived
                            };

                            var deposit = new TDeposit
                            {
                                DepositId = DepositId,
                                ReferenceUsed = DepositRef,
                                DepositDate = DateReceived,
                                Amount = DepositAmount,

                                //in this case(txt) we have a Company Name :
                                //CompanyId=? But we have CompanyName.
                                //today!
                            };
                            try
                            {
                                if (DepositId.ToString() != null)
                                {
                                    //mock = new TMock();
                                    context.TMocks.Add(mock);
                                    context.SaveChanges();
                                    File.AppendAllText($"{_LOGPATH}\\log.txt", $"File {s} TMock saved DepositId: {mock.DepositId} ");
                                }
                            }
                            catch (Exception ex)
                            {
                                string sx = ex.Message;
                                System.Console.WriteLine(sx);

                                //File.AppendAllText($"{_LOGPATH}\\log.txt Message: {ex.Message} |StackTrace: {ex.StackTrace}\n");
                                File.AppendAllText($"{_LOGPATH}\\log.txt", $"Process->End False  Message: {ex.Message} |StackTrace: {ex.StackTrace} {System.DateTime.Now}\n");
                                return false;
                            }


                        }
                        
                    }
                    File.AppendAllText($"{_LOGPATH}\\log.txt", $"Process->End True {System.DateTime.Now}\n");
                    return true;
                }
                else
                {
                    Console.WriteLine("Source path does not exist!");
                    File.AppendAllText($"{_LOGPATH}\\log.txt", $"Process-> Source path does not exist! {System.DateTime.Now}\n");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                File.AppendAllText($"{_LOGPATH}\\log.txt", $"Processing-> Exception {e.Message}: {System.DateTime.Now} The process failed: {e.StackTrace} \n");
                return false;
            }
            File.AppendAllText($"{_LOGPATH}\\log.txt", $"Process->End True {System.DateTime.Now}\n");
            return true;
        }

        protected override void OnStart(string[] args)
        {
            File.AppendAllText($"{_LOGPATH}\\log.txt", $"OnStart {System.DateTime.Now}");
            FilesAlreadyInDir();
            FileWatch(_SOURCE);
        }

        protected override void OnStop()
        {
            File.AppendAllText($"{_LOGPATH}\\log.txt", $"OnStop {System.DateTime.Now}");
            EventLog.WriteEntry("Directory Monitor Service Stopped");
        }

        private void FileWatch(string source)
        {
            Thread.Sleep(5000);
            File.AppendAllText($"{_LOGPATH}\\log.txt", $"OnFileWatch->Start {System.DateTime.Now}");
            _watcher = new FileSystemWatcher($"{source}")
            {
                Filter = "*.txt",
                EnableRaisingEvents = true,
                IncludeSubdirectories = true
            };

            if (IncludeSubdirectories == true)
            {
                _watcher.Filter = "*.txt";
            }
            _watcher.Created += DirectoryChanged;
            _watcher.Changed += DirectoryChanged;
            //_watcher.Renamed += DirectoryChanged;
            //_watcher.Deleted += DirectoryChanged;
            File.AppendAllText($"{_LOGPATH}\\log.txt", $"OnFileWatch->End {System.DateTime.Now}");

            
        }

        private void MoveFile()
        {
            File.AppendAllText($"{_LOGPATH}\\log.txt", $"OnFileMove->Start {System.DateTime.Now}");
            //Process(); 
            string fileName;
            string destFile;
            try
            {
                if (System.IO.Directory.Exists($"{_SOURCE}"))
                {
                    string[] files = System.IO.Directory.GetFiles($"{_SOURCE}");

                    foreach (string s in files)
                    {
                        fileName = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine($"{_TARGET}", fileName);
                        System.IO.File.Move(s, destFile);
                    }
                }
                else
                {
                    Console.WriteLine("Source path does not exist!");
                    File.AppendAllText($"{_LOGPATH}\\log.txt", "Source path does not exist!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                File.AppendAllText($"{_LOGPATH}\\log.txt", e.ToString());
            }
            File.AppendAllText($"{_LOGPATH}\\log.txt", $"OnFileMove->End {System.DateTime.Now}");
        }
    }
}