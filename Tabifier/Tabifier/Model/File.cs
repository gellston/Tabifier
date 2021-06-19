using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Tabifier.Model
{
    public class File : PropertyChangedBase
    {

        public File()
        {

        }


        private string _FileName = "";
        public string FileName
        {
            get => _FileName;
            set => Set(ref _FileName, value);
        }

        private ObservableCollection<Line> _FileContent = null;
        public ObservableCollection<Line> FileContent
        {
            get
            {
                _FileContent ??= new ObservableCollection<Line>();
                return _FileContent;
            }
        }

        private string _FilePath = "";
        public string FilePath
        {
            get => _FilePath;
            set => Set(ref _FilePath, value);
        }


    }
}
