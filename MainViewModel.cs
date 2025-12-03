using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModbusMonitor_Demo1.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ModbusMonitor_Demo1
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _status = "未连接";
        [ObservableProperty]
        private bool _isBtnEnabled = false;

        private readonly QuickModbusService _modbusService;
        public MainViewModel()
        {
            _modbusService = new QuickModbusService();
        }
        [RelayCommand]
        private void Connect()
        {
            try
            {
                bool isConnected = _modbusService.Connect();
                if (isConnected)
                {
                    Status = "结果：连接成功！";
                    IsBtnEnabled = true; // 连接成功后启用读取按钮
                }
                else
                {
                    Status = "连接失败，请检查MThings是否启动";
                }
            }
            catch (Exception ex)
            {
                Status = $"连接出错 → {ex.Message}";
            }
        }
        [RelayCommand]
        private async Task ReadTemperature()
        {
            try
            {
                int tempValue = await _modbusService.ReadTemperature();
                Status = $"读取成功！40001寄存器值 = {tempValue}";
            }
            catch (Exception ex)
            {
                Status = $"读取出错 → {ex.Message}";
            }
        }
    }
}

