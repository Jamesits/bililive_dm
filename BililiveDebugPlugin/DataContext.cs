﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BilibiliDM_PluginFramework;
using BililiveDebugPlugin.Annotations;

namespace BililiveDebugPlugin
{
    public sealed class MethodToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var methodName = parameter as string;
            if (value == null || methodName == null)
                return null;
            var methodInfo = value.GetType().GetMethod(methodName, new Type[0]);
            if (methodInfo == null)
                return null;
            return methodInfo.Invoke(value, new object[0]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(GetType().Name + " can only be used for one way conversion.");
        }
    }
    public class DMItem
    {
        public string ItemName { get; set; }
        public DanmakuModel Model { get; set; }
        public override string ToString() => ItemName;
    }
    public class PluginDataContext:INotifyPropertyChanged
    {
        private DanmakuModel _selected;
        private ObservableCollection<DMItem> _dataList;
        

        public PluginDataContext()
        {
            this.DataList = new ObservableCollection<DMItem>();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public DMPlugin Plugin { get; set; }
        public ObservableCollection<DMItem> DataList
        {
            get => _dataList;
            set
            {
                if (Equals(value, _dataList)) return;
                _dataList = value;
                OnPropertyChanged();
            }
        }

        public DanmakuModel Selected
        {
            get => _selected;
            set
            {
                if (Equals(value, _selected)) return;
                _selected = value;
                OnPropertyChanged();
            }
        }

        public bool Status
        {
            get => Plugin?.Status==true;
            set
            {
                if(Plugin==null){return;}

                if (value)
                {
                    this.Plugin.Start();
                }
                else
                {
                    this.Plugin.Stop();
                }
                OnPropertyChanged();
            }
        }
    }
}
