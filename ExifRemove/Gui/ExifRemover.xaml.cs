using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using ExifRemove.Events;
using ExifRemove.Implementation;
using ExifRemove.Interfaces;
using ExifRemove.Models;
using MessageBox = System.Windows.MessageBox;

namespace ExifRemove.Gui
{
    public partial class ExifRemover
    {
        private readonly ObservableCollection<ExifItem> _exifItems = new ObservableCollection<ExifItem>();
        private IExifCleaner _cleaner;

        public ExifRemover()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Title = "ExifRemover " + version;
            InitializeComponent();
        }

        private void InitializeHandler()
        {
            _cleaner.ExifCleanCompleted += HandleCleanCompleted;
            _cleaner.ExifCleanProgress += HandleCleanProcess;
            _cleaner.ExceptionThrown += HandleExceptionThrown;
        }

        private void ButtonSelectImages(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = Properties.Resources.FilterImages,
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            if (openFileDialog.FileNames != null && openFileDialog.FileNames.Length > 0)
            {
                AddImagesToList(openFileDialog.FileNames);
                BtnClearList.IsEnabled = true;
                CheckCleanExifEnabled();
            }
            else
            {
                MessageBox.Show("Please select at least one image", "Exif remover", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
        }

        private void AddImagesToList(string[] fileNames)
        {
            if (fileNames == null) throw new ArgumentNullException(nameof(fileNames));
            if (_exifItems.Count > 0)
                _exifItems.Clear();
            foreach (var file in fileNames)
                _exifItems.Add(new ExifItem(Path.GetExtension(file), Path.GetFileNameWithoutExtension(file),
                    Path.GetDirectoryName(file), file));
            ExifItemsView.ItemsSource = _exifItems;
        }

        private void ButtonSelectFolder(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            TextBoxOutput.Text = folderBrowserDialog.SelectedPath;
            CheckCleanExifEnabled();
        }

        private void ButtonClearList(object sender, RoutedEventArgs e)
        {
            _exifItems.Clear();
            BtnClearList.IsEnabled = false;
            CheckCleanExifEnabled();
        }

        private void CheckCleanExifEnabled()
        {
            if (_exifItems != null && _exifItems.Count > 0 && !string.IsNullOrEmpty(TextBoxOutput.Text))
                BtnCleanExif.IsEnabled = true;
            else
                BtnCleanExif.IsEnabled = false;
        }

        private void ButtonCleanExif(object sender, RoutedEventArgs e)
        {
            if (_exifItems.Count <= 0)
            {
                MessageBox.Show("Please select at least one image", "Exif remover", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(TextBoxOutput.Text))
            {
                MessageBox.Show("Please select the output folder", "Exif remover", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return;
            }
            SetHandlerAndStart();
        }

        private void SetHandlerAndStart()
        {
            _cleaner = new ExifCleaner(TextBoxOutput.Text, _exifItems);
            InitializeHandler();
            _cleaner.Start();
        }


        private void HandleCleanCompleted(object sender, ExifCleanCompletedEventArgs e)
        {
            ButtonClearList(sender, new RoutedEventArgs());
            SetGuiToStop();
            MessageBox.Show(e.Text, "Exif remover", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void HandleCleanProcess(object sender, ExifCleanProgressEventArgs e)
        {
            TextBoxFinished.Text = e.Text;
        }

        private void HandleExceptionThrown(object sender, ExceptionThrownEventArgs e)
        {
            MessageBox.Show(e.Message, "Exif remover", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void SetGuiToStop()
        {
            BtnSelectImages.IsEnabled = true;
            BtnSelectFolder.IsEnabled = true;
            BtnCleanExif.IsEnabled = false;
            BtnClearList.IsEnabled = false;
        }
    }
}