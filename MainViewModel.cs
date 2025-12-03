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

    //private void BtnConnect_Click(object sender, RoutedEventArgs e)
    //    {
    //        try
    //        {
    //            // 调用Connect方法连接127.0.0.1:502
    //            bool isConnected = _modbusService.Connect();
    //            if (isConnected)
    //            {
    //                TxtResult.Text = "结果：连接成功！";
    //                BtnReadTemp.IsEnabled = true; // 连接成功后启用读取按钮
    //            }
    //            else
    //            {
    //                TxtResult.Text = "结果：连接失败，请检查MThings是否启动";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            TxtResult.Text = $"结果：连接出错 → {ex.Message}";
    //        }
    //    }
    //    // 读取温度按钮点击事件（异步）
    //    private async void BtnReadTemp_Click(object sender, RoutedEventArgs e)
    //    {
    //        try
    //        {
    //            // 调用ReadTemperature读取40001寄存器（slaveId=1）
    //            int tempValue = await _modbusService.ReadTemperature();
    //            TxtResult.Text = $"结果：读取成功！40001寄存器值 = {tempValue}";
    //        }
    //        catch (Exception ex)
    //        {
    //            TxtResult.Text = $"结果：读取出错 → {ex.Message}";
    //        }
    //    }
}

