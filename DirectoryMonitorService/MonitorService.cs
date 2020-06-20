
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryMonitorService
{
    public partial class MonitorService : ServiceBase
    {
        private FileSystemWatcher _watcher;
        public bool IncludeSubdirectories { get; set; }
        private string _SOURCE;
        private string _TARGET;
        private string _LOGPATH;
        public string AccountNumber;
        public string FirstName;
        public string LastName;
        public double DepositAmount;
        public int DepositId;
        public DateTime DateReceived;
        public string DepositRef;
        public int CompnayId;
        public int CompanyName;
        public double Amount;


        public MonitorService()
        {
            InitializeComponent();
            this._SOURCE = ConfigurationManager.AppSettings["sourcePath"];
            this._TARGET = ConfigurationManager.AppSettings["targetPath"];
            this._LOGPATH = ConfigurationManager.AppSettings["logPath"];

        }

        protected override void OnStart(string[] args)
        {

            FilesAlreadyInDir();
            FileWatch(this._SOURCE);

        }
        protected override void OnStop()
        {
            EventLog.WriteEntry("Directory Monitor Service Stopped");
        }

        private void FileWatch(string source)
        {
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
            _watcher.Renamed += DirectoryChanged;
            _watcher.Deleted += DirectoryChanged;

        }
        public void Process()

        {
            string fileName;
            try
            {
                if (System.IO.Directory.Exists($"{_SOURCE}"))
                {
                    List<string> files = System.IO.Directory.GetFiles($"{_SOURCE}").ToList(); 

                    foreach (string s in files)
                    {
                        fileName = System.IO.Path.GetFileName(s);

                        List<string> lines = System.IO.File.ReadAllLines(fileName).ToList();
                        foreach (string line in lines)
                        {
                            List<string> entries = line.Split('\t').ToList();

                            AccountNumber = entries[0];
                            FirstName = entries[1];
                            LastName = entries[2];
                            DepositAmount = Convert.ToDouble(entries[3]);
                            DepositId = (Int16.Parse(entries[4]));
                            DateReceived = DateTime.Parse(entries[5]);
                            DepositRef = AccountNumber;

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
                                if (DepositId.ToString() !=null)
                                {
                                    context.TMocks.Add(mock);
                                    context.SaveChanges();
                                }
                            }
                            catch (Exception ex)
                            {
                                string sx = ex.Message;
                                System.Console.WriteLine(sx);
                            }
                                                      
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Source path does not exist!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
        public bool FilesAlreadyInDir()

        {
            _watcher = new FileSystemWatcher($"{_SOURCE}");//watches for new files

            if (System.IO.Directory.Exists($"{_SOURCE}"))
            {
                //string[] files = System.IO.Directory.GetFiles($"{_SOURCE}");
                Process();
                MoveFile();
                return true;
            }
            else
                FileWatch(_SOURCE);//executes the new files(triggers Directory Changed) and Directory Chnaged triggers the process and move file methods
            return false;
        }

        public void DirectoryChanged(object sender, FileSystemEventArgs e)
        {
            //When a new file arrives 
            Process();
            MoveFile();

            var msg = $"{e.ChangeType}-{e.FullPath + "" + DateTime.Now}{System.Environment.NewLine}";
            File.AppendAllText($"{_LOGPATH}\\log.txt", msg);
        }

      private void MoveFile()
        {
            Process();
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
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
           
        }
    }
}

