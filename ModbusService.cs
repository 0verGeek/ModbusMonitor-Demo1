using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
namespace ModbusMonitor_Demo1.Services
{
    internal class ModbusService
    {
        
    }
    // 实现最简Modbus服务（核心代码）
    public class QuickModbusService
    {
        // 类级别变量：持久化连接
        private ModbusIpMaster? _master;
        private TcpClient? _tcpClient;
        
        public bool Connect(string ip = "127.0.0.1", int port = 502)
        {
            try
            {
                // 关闭旧连接
                Disconnect();
                _tcpClient = new TcpClient();
                _tcpClient.Connect(ip, port);
                // 绑定到类变量，避免连接销毁
                _master = ModbusIpMaster.CreateIp(_tcpClient);
                _master.Transport.ReadTimeout = 3000; // 超时3秒
                return _tcpClient.Connected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"连接失败：{ex.Message}");
                return false;
            }
        }
        
        public void Disconnect()
        {
            _master?.Dispose();
            _tcpClient?.Close();
            _tcpClient?.Dispose();
            _master = null;
            _tcpClient = null;
        }

        /// 读取保持寄存器（40001=地址0）
        public async Task<int> ReadTemperature(byte slaveId = 1)
        {
            if (_master == null || _tcpClient == null || !_tcpClient.Connected)
            {
                throw new InvalidOperationException("未连接到从站，请先调用Connect()");
            }
            // 异步读取40001寄存器（地址0，数量1）
            var registers = await _master.ReadHoldingRegistersAsync(slaveId, 0, 1);
            return registers[0];
        }
    }
}
