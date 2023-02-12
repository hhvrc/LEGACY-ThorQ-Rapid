using ReMod.Core;
using ReMod.Core.Managers;
using ReMod.Core.UI.QuickMenu;
using ReMod.Core.VRChat;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;

namespace ThorQ_Rapid
{
    public class ThorQ : ModComponent
    {
        public static ThorQ Instance { get; private set; }
        ThorQ()
        {
            Instance = this;
        }


        WsClient _wsClient;
        UdpClient _udpClient;
        DeviceController _deviceController;

        ReCategoryPage mainPage;
        ReMenuCategory mainCat;
        ReMenuCategory myIdCat;
        ReMenuCategory myIpCat;
        ReMenuCategory sessionCat;
        ReMenuCategory devicesCat;
        ReMenuButton connectButton;

        string selectedUserID;
        List<ReMenuButton> _deviceList = new List<ReMenuButton>();

        #region CANCER
        string _userId = "null";
        public string UserID
        {
            get { return _userId; }
            set
            {
                _userId = value ?? "null";
                if (myIdCat != null)
                {
                    myIdCat.Title = "ID: " + _userId;
                }
            }
        }
        string _ipAddress = "null";
        public string IPAddress
        {
            get { return _ipAddress; }
            set
            {
                _ipAddress = value ?? "null";
                if (myIpCat != null)
                {
                    myIpCat.Title = "IP: " + _ipAddress;
                }
            }
        }
        #endregion

        void SetupWebSocket()
        {
            _wsClient = new WsClient("wss://api.heavenvr.tech/thorq/rpd/");
            _deviceController = new DeviceController();
            _deviceController.DeviceConnected += DeviceConnected;
            _deviceController.DeviceDisconnected += DeviceDisconnected;
            _deviceController.ActiveDeviceChanged += ActiveDeviceChanged;
        }

        private void DeviceConnected(string deviceName)
        {
            _deviceList.Add(devicesCat.AddButton(deviceName, "", () => _deviceController.SetDevice(deviceName, 9600)));
        }
        private void DeviceDisconnected(string deviceName)
        {
            foreach (var device in _deviceList.Where(d => d.Text == deviceName).ToArray())
            {
                device.Destroy();
                _deviceList.Remove(device);
            }
        }
        private void ActiveDeviceChanged(string deviceName, int baudRate)
        {
            string title = "Devices";
            if (deviceName != null)
            {
                title += $" - Selected {deviceName}@{baudRate}";
            }
            devicesCat.Title = title;
        }

        public void SetUserOnline(string userId)
        {
            if (userId == selectedUserID && connectButton != null)
            {
                connectButton.Active = true;
            }
        }

        public override void OnUiManagerInit(UiManager uiManager)
        {
            mainPage = new ReCategoryPage("ThorQ", true);
            mainPage.GameObject.SetActive(false);

            connectButton = uiManager.TargetMenu.AddButton("ThorQ Connect", "", ConnectToUser);
            ReTabButton.Create("ThorQ", "ThorQ", "ThorQ " + Assembly.GetExecutingAssembly().GetName().Version, null);

            myIdCat = mainPage.AddCategory("ID: " + UserID, false);
            myIpCat = mainPage.AddCategory("IP: " + IPAddress, false);

            mainCat = mainPage.AddCategory("Actions", false);
            mainCat.AddToggle("Discoverable", "Be discoverable for easier connections", OnToggleDiscoverable, false);
            mainCat.AddToggle("P2P", "Enable Peer To Peer communication (Exposes your IP Address to partner)", OnToggleP2P, false);
            mainCat.AddButton("Refresh", "Scan for serial port devices", _deviceController.ScanForDevices);

            sessionCat = mainPage.AddCategory("Session", true);
            sessionCat.AddButton("Disconnect", "Disconnect from current session", OnClickSessionDisconnect);
            sessionCat.AddButton("Test Beep", "Send Beep", OnClickSessionSendBeep);


            devicesCat = mainPage.AddCategory("Devices", true);

            SetupWebSocket();
            _deviceController.ScanForDevices();
        }

        void OnToggleDiscoverable(bool p2pEnabled)
        {
        }
        void OnToggleP2P(bool p2pEnabled)
        {
        }

        void OnClickSessionDisconnect()
        {
            sessionCat.Active = false;
        }
        void OnClickSessionSendBeep()
        {
            _wsClient.Send("BEEP");
        }

        public override void OnSelectUser(VRC.DataModel.IUser user, bool isRemote)
        {
            connectButton.Active = false;

            if (user != null && isRemote)
            {
                _wsClient.Send(MessageFormat.Message.Serialize(new MessageFormat.IsUserOnline(user.prop_String_0)));
            }
        }

        public override void OnAvatarIsReady(VRCPlayer player)
        {
            base.OnAvatarIsReady(player);
        }

        void ConnectToUser()
        {
            var user = QuickMenuEx.SelectedUserLocal.field_Private_IUser_0;
            if (user == null)
                return;

            _wsClient.Send(user.prop_String_0);
            /*
            var player = PlayerManager.field_Private_Static_PlayerManager_0.GetPlayer(user.prop_String_0)._vrcplayer;
            if (player == null)
                return;
            */
        }


        public override void OnApplicationQuit()
        {
            _deviceController.Dispose();
            _wsClient.Dispose();
        }
    }
}
