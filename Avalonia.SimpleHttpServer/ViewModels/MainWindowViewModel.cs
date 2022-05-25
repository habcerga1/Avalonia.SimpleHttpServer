using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.SimpleHttpServer.Models;
using Avalonia.SimpleHttpServer.Views;
using SimpleHttp.Models;

namespace Avalonia.SimpleHttpServer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private NewServerViewModel addServerWindowViewModel;        // Add new server View Model
        private NewServerWindow addServerWindow;                    // Add new server Window
        
        public BS<int> ResumeBtn { get; set; }                      // Button Resume server handler
        public BS<int> RemoveBtn { get; set; }                      // Button Remove server handler
        public BS<string> AddBtn { get; set; }                      // Button Add server handler
        public BS<ServerViewModel> StartBtn { get; set; }           // Button Start selected server
        public BS<ServerViewModel> StopBtn { get; set; }            // Button Stop server handler
        
        public BS<ServerViewModel> SelectedServer { get; set; }     // Selected server in UI
        public ObservableCollection<ServerViewModel> Servers { get; set; } // List of servers which shows in UI
        
        public MainWindowViewModel()
        {
            Servers = new ObservableCollection<ServerViewModel>();
            AddBtn = new BS<string>(ShowAddServerWindow);
            StopBtn = new BS<ServerViewModel>(StopServer);
            ResumeBtn = new BS<int>(ResumeServer);
            RemoveBtn = new BS<int>(RemoveServer);
            StartBtn = new BS<ServerViewModel>(StartServer);
            SelectedServer = new BS<ServerViewModel>();
            AddDefaultHttpServers();
        }

        private void AddDefaultHttpServers()
        {
             Servers.Add(new ServerViewModel("site1","10100"));
             Servers.Add(new ServerViewModel("site2","10101"));
        }
        
        
        /// <summary>
        /// Create and show Windows for add new server
        /// </summary>
        /// <param name="key"></param>
        private void ShowAddServerWindow(string key)
        {
            addServerWindowViewModel = new NewServerViewModel();
            addServerWindow = new NewServerWindow();
            addServerWindowViewModel.ServerCreated += VmOnServerCreated;
            addServerWindow.DataContext = addServerWindowViewModel;
            addServerWindow.Show();
        }
        
        /// <summary>
        /// Create a ServerViewModel and add it Servers collection
        /// </summary>
        /// <param name="obj"></param>
        private void VmOnServerCreated(ServerSettingsDto obj)
        {
            addServerWindow.Close();
            ServerViewModel vm = new ServerViewModel(obj.Name, obj.Port);
            Servers.Add(vm);
            addServerWindowViewModel.ServerCreated -= VmOnServerCreated;
            addServerWindowViewModel = null;
            addServerWindow = null;
        }
        
        /// <summary>
        /// Start server
        /// </summary>
        /// <param name="server"></param>
        private void StartServer(ServerViewModel server)
        {
            if (SelectedServer.Value != null & SelectedServer.Value.IsRunning == false)
            {
                SelectedServer.Value.StartServer();
            }
        }

        /// <summary>
        /// Stop server
        /// </summary>
        /// <param name="server"></param>
        private void StopServer(ServerViewModel server)
        {
            if (SelectedServer.Value != null)
            {
                SelectedServer.Value.StopServer();
            }
        }
        
        /// <summary>
        /// Resume server
        /// </summary>
        /// <param name="key"></param>
        private void ResumeServer(int key)
        {
            if (SelectedServer.Value != null)
            {
                StartServer(SelectedServer.Value);
            }
        }
        
        /// <summary>
        /// Remove server
        /// </summary>
        /// <param name="key"></param>
        private void RemoveServer(int key)
        {
            if (SelectedServer.Value != null)
            {
                if (SelectedServer.Value.IsRunning == false)
                {
                    Servers.Remove(SelectedServer.Value);
                }
                else
                {
                    StopServer(SelectedServer.Value);
                    Servers.Remove(SelectedServer.Value);
                }
            }
        }
    }
    
}