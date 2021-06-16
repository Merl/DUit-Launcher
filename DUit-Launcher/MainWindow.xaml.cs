using System;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Collections;
using System.Runtime.InteropServices;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System.Windows.Controls;


namespace DUit_Launcher
{
    public partial class DUitLauncher : Window
    {
        private ObservableCollection<Prog> progs = new ObservableCollection<Prog>();
        private ObservableCollection<Prog> mainProgs = new ObservableCollection<Prog>();

        private DataGrid _focusGrid;

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public DUitLauncher()
        {
            InitializeComponent();
            
            string _path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string path = _path + @"\duit.yaml";
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var _allsettings = deserializer.Deserialize<Dictionary<string, object>>(reader);
                    foreach(var _settinggroup in _allsettings) {
                        if (_settinggroup.Key == "Programs")
                        {
                            foreach (Dictionary<object, object> _settings in (List<object>)_settinggroup.Value)
                            {
                                string _name = "";
                                string _cmdline = "";
                                string _arguments = "";
                                bool _hidewindow = false;

                                foreach (var _setting in _settings)
                                {
                                    if (_setting.Value != null)
                                    {
                                        if (_setting.Key.ToString() == "Name")
                                            _name = _setting.Value.ToString();
                                        if (_setting.Key.ToString() == "Cmdline")
                                            _cmdline = _setting.Value.ToString();
                                        if (_setting.Key.ToString() == "Arguments")
                                            _arguments = _setting.Value.ToString();
                                        if (_setting.Key.ToString() == "Hidewindow" && _setting.Value.ToString() == "true")
                                            _hidewindow = true;
                                    }
                                }
                                progs.Add(new Prog() { Name = _name, Cmdline = _cmdline, Arguments = _arguments, Hidewindow = _hidewindow });
                            }
                        }
                        else if (_settinggroup.Key == "MainPrograms")
                        {
                            foreach (Dictionary<object, object> _settings in (List<object>)_settinggroup.Value)
                            {
                                string _name = "";
                                string _cmdline = "";
                                string _arguments = "";
                                bool _hidewindow = false;

                                foreach (var _setting in _settings)
                                {
                                    if (_setting.Value != null)
                                    {
                                        if (_setting.Key.ToString() == "Name")
                                            _name = _setting.Value.ToString();
                                        if (_setting.Key.ToString() == "Cmdline")
                                            _cmdline = _setting.Value.ToString();
                                        if (_setting.Key.ToString() == "Arguments")
                                            _arguments = _setting.Value.ToString();
                                        if (_setting.Key.ToString() == "Hidewindow" && _setting.Value.ToString() == "true")
                                            _hidewindow = true;
                                    }
                                }

                                mainProgs.Add(new Prog() { Name = _name, Cmdline = _cmdline, Arguments = _arguments, Hidewindow = _hidewindow });
                            }
                        } else if (_settinggroup.Key == "Other")
                        {
                            IDictionary _settings = (IDictionary)_settinggroup.Value;

                            foreach (string _setting in _settings.Keys)
                            {
                                if (_setting == "ToTray" && _settings[_setting].ToString() == "True")
                                    chkToTray.IsChecked = true;
                                if (_setting == "QuitWithDU" && _settings[_setting].ToString() == "True")
                                    chkQuitWithDU.IsChecked = true;
                                if (_setting == "MinOnStart" && _settings[_setting].ToString() == "True")
                                    chkMinOnStart.IsChecked = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            dgPrograms.ItemsSource = progs;
            dgMainProgs.ItemsSource = mainProgs;

            if (mainProgs.Where(p => p.Name == "dual-launcher.exe").First().Cmdline == null)
            {
                MessageBox.Show("Please select folder of dual-launcher.exe");
                _focusGrid = dgMainProgs;
                var _progs = dgMainProgs.Items.GetItemAt(0);
                dgMainProgs.SelectedItem = _progs;
                editProgs();
            }

            tbIcon.Visibility = Visibility.Collapsed;
            if ((bool)chkMinOnStart.IsChecked)
            {
                startPrograms();
                //not sure why the OnStateChanged event does not fire here
                
                //this.WindowState = WindowState.Maximized;
                //this.WindowState = WindowState.Minimized;
                if ((bool)chkToTray.IsChecked)
                {  
                    //this.Hide();
                    //tbIcon.Visibility = Visibility.Visible;
                }
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized && (bool)chkToTray.IsChecked)
            {
                this.Hide();
                tbIcon.Visibility = Visibility.Visible;
            }
            
            base.OnStateChanged(e);
        }

        private void btnDualTrayMaximize_Click(object sender, RoutedEventArgs e)
        {
            restoreWindow();
        }

        public void restoreWindow()
        {
            this.Dispatcher.Invoke((System.Action)delegate
            {
                this.Show();
                this.WindowState = WindowState.Normal;
                Process p = Process.GetCurrentProcess();
                SetForegroundWindow(p.MainWindowHandle);
                tbIcon.Visibility = Visibility.Collapsed;
            });
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Prog prog = addProgram();
            if (prog != null)
                progs.Add(prog);
        }

        private Prog addProgram(string filter = "DU Launcher|dual-launcher.exe|Executable|*.exe|CMD|*.cmd|Batch|*.batch")
        {
            OpenFileDialog openFileDlg = new OpenFileDialog
            {
                DefaultExt = ".exe",
                Filter = filter
            };

            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return new Prog() { Name = Path.GetFileName(openFileDlg.FileName), Cmdline = openFileDlg.FileName };
            }
            return null;
            
        }

        private void btnGetProcs_Click(object sender, RoutedEventArgs e)
        {
            var process = Process.GetProcessesByName("Dual").First();
            string path = process.GetMainModuleFileName();
            progs.Add(new Prog() { Name = path });
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgPrograms.SelectedItem != null)
                progs.Remove(dgPrograms.SelectedItem as Prog);
        }

        private void btnDUit_Click(object sender, RoutedEventArgs e)
        {
            startPrograms();
        }

        private void startPrograms()
        {
            foreach (var prog in progs)
            {
                startProgs(prog);
            }

            foreach (var prog in mainProgs)
            {
                startProgs(prog);
            }

            this.WindowState = WindowState.Minimized;
        }

        private void startProgs (Prog prog)
        {
            if (prog.Pid == null)
            {
                string cmdline = prog.Cmdline;
                Process myProc = new Process();

                myProc.StartInfo.FileName = cmdline;
                myProc.StartInfo.WorkingDirectory = Path.GetDirectoryName(cmdline);
                myProc.EnableRaisingEvents = true;
                myProc.Exited += MyProc_Exited;

                myProc.StartInfo.CreateNoWindow = prog.Hidewindow;

                myProc.StartInfo.Arguments = prog.Arguments;
                try
                {
                    myProc.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not start: " + prog.Name + ex.Message);
                }
                prog.Pid = myProc.Id;
            }
        }

        private void MyProc_Exited(object sender, EventArgs e)
        {
            Process proc = (Process)sender;
            if (proc != null)
            {
                string dualName = "";
                string dualCmdline = "";
                int? dualPid = null;
                bool doneit = false;
                Prog tempProg = new Prog();

                foreach (Prog prog in mainProgs)
                {
                    if (prog.Pid == proc.Id)
                    {
                        //clear pID in DataGrid
                        prog.Pid = null;
                        //if the launcher is closed check for running Dual Universe and get pID from "Dual" Process.
                        if (prog.Name == "dual-launcher.exe")
                        {
                            Process[] dualexe = Process.GetProcessesByName("Dual");

                            if (dualexe.Length > 0)
                            {
                                foreach (var _ in dualexe)
                                {
                                    dualName = _.ProcessName;
                                    dualPid = _.Id;
                                    dualCmdline = _.GetMainModuleFileName();

                                    _.EnableRaisingEvents = true;
                                    _.Exited += MyProc_Exited;
                                }
                            }
                            else
                            {
                                doneit = true;
                            }
                        } else if (prog.Name == "Dual")
                        {
                            doneit = true;
                            tempProg = prog;
                        }
                    }
                }

                foreach (Prog prog in progs)
                {
                    if (prog.Pid == proc.Id)
                    {
                        //clear pID in DataGrid
                        prog.Pid = null;
                    }
                }

                if (dualPid != null)
                {
                    this.Dispatcher.Invoke((System.Action)delegate
                    {
                        mainProgs.Add(item: new Prog() { Name = dualName, Cmdline = dualCmdline, Pid = dualPid });
                    });
                }

                if (doneit)
                {
                    //Quit all started programs
                    foreach (Prog prog in progs)
                    {
                        if (prog.Pid != null)
                        {
                            Process p = Process.GetProcessById((int)prog.Pid);
                            //p.Close();
                            if (!p.HasExited)
                            {
                                p.Kill();
                            }
                        }
                    }

                    if (tempProg.Name == "Dual")
                    {
                        this.Dispatcher.Invoke((System.Action)delegate
                        {
                            mainProgs.Remove(tempProg);
                        });
                    }

                    this.Dispatcher.Invoke((System.Action)delegate
                    {
                        if ((bool)chkQuitWithDU.IsChecked)
                            System.Windows.Application.Current.Shutdown();
                        else
                            restoreWindow();
                    });
                }
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            editProgs();
        }

        private void editProgs()
        {
            System.Windows.Controls.DataGrid _progs = _focusGrid;

            if (_progs != null && _progs.SelectedItem != null)
            {
                string _filter;
                if (_progs.Name == "dgMainProgs")
                    _filter = "DU Launcher|dual-launcher.exe";
                else
                    _filter = "Exe|*.exe";
                Prog prog = addProgram(_filter);
                if (prog != null)
                    (_progs.SelectedItem as Prog).Cmdline = prog.Cmdline;
            }
        }

        /*
         * https://stackoverflow.com/a/4428181
         */
        private void dgs_GotFocus(object sender, RoutedEventArgs e)
        {
            _focusGrid = (DataGrid)sender;
        }

        private void saveSettings()
        {
            string _path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string path = _path + @"\duit.yaml";

            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new YamlDotNet.Serialization.Serializer();

                    //Dictionary<string, ObservableCollection<Prog>> _settings = new Dictionary<string, ObservableCollection<Prog>>();
                    Dictionary<string, object> _settings = new Dictionary<string, object>();
                    _settings.Add("Programs", progs);
                    _settings.Add("MainPrograms", mainProgs);
                    _settings.Add("Other", new Dictionary<string, string>() { 
                        ["ToTray"] = chkToTray.IsChecked.ToString(),
                        ["QuitWithDU"] = chkQuitWithDU.IsChecked.ToString(),
                        ["MinOnStart"] = chkMinOnStart.IsChecked.ToString() 
                    });

                    serializer.Serialize(writer, _settings);
                }
            }

            catch (Exception ex)
            {
                // let´s see
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            saveSettings();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if ((bool)chkMinOnStart.IsChecked)
                this.WindowState = WindowState.Minimized;
        }
    }



}