using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Tabifier.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {

        }





        private ObservableCollection<Model.File> _SourceFileCollection = null;
        public ObservableCollection<Model.File> SourceFileCollection
        {
            get{
                _SourceFileCollection ??= new ObservableCollection<Model.File>();
                return _SourceFileCollection;
            }
        }

        private ObservableCollection<Model.File> _TargetFileCollection = null;
        public ObservableCollection<Model.File> TargetFileCollection
        {
            get
            {
                _TargetFileCollection ??= new ObservableCollection<Model.File>();
                return _TargetFileCollection;
            }
        }

        private int _CurrentSelectedTabIndex = 0;
        public int CurrentSelectedTabIndex
        {
            get => _CurrentSelectedTabIndex;
            set => Set(ref _CurrentSelectedTabIndex, value);
        }

        private bool _IsProgress = false;
        public bool IsProgress
        {
            get => _IsProgress;
            set => Set(ref _IsProgress, value);
        }

        private bool _IsSpaceToTab = false;
        public bool IsSpaceToTab
        {
            get => _IsSpaceToTab;
            set => Set(ref _IsSpaceToTab, value);
        }

        private bool _IsVisible = true;
        public bool IsVisible
        {
            get => _IsVisible;
            set => Set(ref _IsVisible, value);
        }
        public ICommand ClearDataCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.IsProgress == true)
                    return;


                this.IsProgress = false;
                this.IsVisible = true;
                this.TargetFileCollection.Clear();
                this.SourceFileCollection.Clear();

            });
        }
        public ICommand StartSaveCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.IsProgress == true) return;
                this.IsProgress = true;
                

                Task.Run(() =>
                {
                    try
                    {
                        this.IsProgress = true;

                        foreach (var content in this.TargetFileCollection)
                        {
                            string fullContent = "";
                            foreach (var line in content.FileContent)
                            {
                                fullContent += line.Text;
                                fullContent += "\n";
                            }
                            File.WriteAllText(content.FilePath, fullContent, Encoding.UTF8);

                        }

                    }
                    catch (Exception e)
                    {
                        this.IsProgress = false;
                    }

                    this.IsProgress = false;
                });
                
            });
        }

        public ICommand DropFolderCommand
        {
            get => new RelayCommand<DragEventArgs>((arg) =>
            {
                if (arg.Data.GetDataPresent(DataFormats.FileDrop))
                {

                    try
                    {
                        this.IsProgress = true;
                        this.IsVisible = false;
                        this.TargetFileCollection.Clear();
                        this.SourceFileCollection.Clear();
                        // Note that you can have more than one file.
                        string[] directories = (string[])arg.Data.GetData(DataFormats.FileDrop);

                        foreach (var directory in directories)
                        {
                            var files = new List<string>();
                            if (File.GetAttributes(directory).HasFlag(FileAttributes.Directory))
                            {
                                var filePath = Directory.GetFiles(directory, "*.java");
                                files.AddRange(filePath);

                                filePath = Directory.GetFiles(directory, "*.js");
                                files.AddRange(filePath);

                                filePath = Directory.GetFiles(directory, "*.php");
                                files.AddRange(filePath);
                            }

                            foreach (var file in files)
                            {
                                var fileName = Path.GetFileName(file);
                                var lines = File.ReadAllLines(file);
                                var fileContent = new Model.File();
                                var fileTargetContent = new Model.File();


                                fileContent.FileName = fileName;
                                fileContent.FilePath = file;

                                fileTargetContent.FileName = fileName;
                                fileTargetContent.FilePath = file;


                                int lineCount = 1;
                                foreach (string line in lines)
                                {
                                    fileContent.FileContent.Add(new Model.Line()
                                    {
                                        Number = lineCount++,
                                        Text = line
                                    });

                                    fileTargetContent.FileContent.Add(new Model.Line(){
                                        Number = lineCount,
                                        Text = line
                                    });
                                }
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    this.SourceFileCollection.Add(fileContent);
                                    this.TargetFileCollection.Add(fileTargetContent);
                                });
                            }
                        }

                        foreach(var targetFile in TargetFileCollection)
                        {
                            foreach(var content in targetFile.FileContent)
                            {
                                if(this.IsSpaceToTab == true)
                                {
                                    if(content.Text.Contains("    "))
                                    {
                                        content.Text = content.Text.Replace("    ", "\t");
                                        content.IsCorrect = true;
                                    }
                                    
                                }
                                else
                                {
                                    if(content.Text.Contains("\t"))
                                    {
                                        content.Text = content.Text.Replace("\t", "    ");
                                        content.IsCorrect = true;
                                    }

                                }
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        Helper.DialogHelper.ShowErrorMessage(e.Message);
                        
                    }
                    this.IsProgress = false;


                }
            });
        }

    }
}
