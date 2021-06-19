using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabifier.ViewModel
{
    public class ViewModelLocator
    {

        public ViewModelLocator()
        {

            SimpleIoc.Default.Register <MainWindowViewModel>();
        }


        public ViewModelBase MainWindowVIewModel
        {
            get => SimpleIoc.Default.GetInstance<MainWindowViewModel>();
        }
    }
}
