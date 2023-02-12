using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace ThorQ_Rapid
{
    internal class DeviceController : IDisposable
    {
        SerialPort _serialPort = null;
        string _portName = "";
        int _baudRate = 0;

        List<string> _connectedPorts = new List<string>();

        public delegate void DeviceDisconnectedEvent(string deviceName);
        DeviceDisconnectedEvent _deviceDisconnectedEvent;
        public event DeviceDisconnectedEvent DeviceDisconnected
        {
            add => _deviceDisconnectedEvent = (DeviceDisconnectedEvent)Delegate.Combine(_deviceDisconnectedEvent, value);
            remove => _deviceDisconnectedEvent = (DeviceDisconnectedEvent)Delegate.Remove(_deviceDisconnectedEvent, value);
        }

        public delegate void DeviceConnectedEvent(string deviceName);
        DeviceConnectedEvent _deviceConnectedEvent;
        public event DeviceConnectedEvent DeviceConnected
        {
            add => _deviceConnectedEvent = (DeviceConnectedEvent)Delegate.Combine(_deviceConnectedEvent, value);
            remove => _deviceConnectedEvent = (DeviceConnectedEvent)Delegate.Remove(_deviceConnectedEvent, value);
        }

        public delegate void ActiveDeviceChangeEvent(string deviceName, int baudRate);
        ActiveDeviceChangeEvent _activeDeviceChangedEvent;
        public event ActiveDeviceChangeEvent ActiveDeviceChanged
        {
            add => _activeDeviceChangedEvent = (ActiveDeviceChangeEvent)Delegate.Combine(_activeDeviceChangedEvent, value);
            remove => _activeDeviceChangedEvent = (ActiveDeviceChangeEvent)Delegate.Remove(_activeDeviceChangedEvent, value);
        }

        public void ScanForDevices()
        {
            var portNames = SerialPort.GetPortNames();

            // Remove old ports
            foreach (var portName in _connectedPorts.Where(pn => !portNames.Contains(pn)).ToArray())
            {
                _deviceDisconnectedEvent.Invoke(portName);
                _connectedPorts.Remove(portName);
            }

            // Add new ports
            foreach (var portName in portNames.Where(p => !_connectedPorts.Contains(p)).ToArray())
            {
                _deviceConnectedEvent.Invoke(portName);
                _connectedPorts.Add(portName);
            }

            if (_connectedPorts.Count == 0)
                SetDevice(null);
        }

        public void SetDevice(string portName)
        {
            SetDevice(portName, _baudRate);
        }
        public void SetDevice(int baudRate)
        {
            SetDevice(_portName, baudRate);
        }
        public void SetDevice(string portName, int baudRate)
        {
            if (_serialPort?.PortName == portName && _serialPort?.BaudRate == baudRate)
                return;

            if (string.IsNullOrEmpty(portName))
            {
                Close();
            }
            else
            {
                try
                {
                    _serialPort = new SerialPort(portName, baudRate);
                    _serialPort.Open();
                }
                catch (Exception)
                {
                    Close();
                }
            }

            _portName = portName;
            _baudRate = baudRate;

            _activeDeviceChangedEvent.Invoke(portName, baudRate);
        }
        public void Close()
        {
            if (_serialPort == null) return;

            try { _serialPort.Close(); } catch (Exception) { }
            try { _serialPort.Dispose(); } catch (Exception) { }
            _serialPort = null;
            _activeDeviceChangedEvent.Invoke(null, 0);
        }

        public void Dispose()
        {
            Close();
        }
    }
}
