using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DUit_Launcher
{
    public class Prog : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }

        private string cmdline;
        public string Cmdline
        {
            get { return this.cmdline; }
            set
            {
                if (this.cmdline != value)
                {
                    this.cmdline = value;
                    this.NotifyPropertyChanged("Cmdline");
                }
            }
        }

        private bool hidewindow;
        public bool Hidewindow
        {
            get { return this.hidewindow; }
            set
            {
                if (this.hidewindow != value)
                {
                    this.hidewindow = value;
                    this.NotifyPropertyChanged("Hidewindow");
                }
            }
        }

        private int? pid;
        public int? Pid
        {
            get { return this.pid; }
            set
            {
                if (this.pid != value)
                {
                    this.pid = value;
                    this.NotifyPropertyChanged("Pid");
                }
            }
        }

        private string arguments;
        public string Arguments
        {
            get { return this.arguments; }
            set
            {
                if (this.arguments != value)
                {
                    this.arguments = value;
                    this.NotifyPropertyChanged("Arguments");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
