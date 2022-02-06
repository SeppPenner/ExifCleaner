// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExifRemover.xaml.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The exif remover form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExifRemove.Gui
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Reflection;
    using System.Windows;

    using ExifRemove.Events;
    using ExifRemove.Implementation;
    using ExifRemove.Interfaces;
    using ExifRemove.Models;

    using Microsoft.Win32;

    /// <summary>
    /// The exif remover form.
    /// </summary>
    public partial class ExifRemover
    {
        /// <summary>
        /// The exif items.
        /// </summary>
        private readonly ObservableCollection<ExifItem> exifItems = new ();

        /// <summary>
        /// The exif cleaner.
        /// </summary>
        private IExifCleaner? cleaner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExifRemover"/> class.
        /// </summary>
        public ExifRemover()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Title = "ExifRemover " + version;
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the exception thrown event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private static void HandleExceptionThrown(object sender, ExceptionThrownEventArgs e)
        {
            MessageBox.Show(e.Message, "Exif remover", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Initializes the handlers.
        /// </summary>
        private void InitializeHandlers()
        {
            if (cleaner is null)
            {
                return;
            }

            this.cleaner.ExifCleanCompleted += this.HandleCleanCompleted;
            this.cleaner.ExifCleanProgress += this.HandleCleanProcess;
            this.cleaner.ExceptionThrown += HandleExceptionThrown;
        }

        /// <summary>
        /// Handles the button to select images.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ButtonSelectImages(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "All images|*.jpg; *.jpeg; *.png; *.gif; *.bmp; *.tif; *.tiff|Jp(e)g images|*.jpg; *.jpeg|Png images|*.png|Gif images|*.gif|Bmp images|*.bmp|Tif(f) images|*.tif; *.tiff",
                Multiselect = true
            };

            var isDialogShown = openFileDialog.ShowDialog();

            if (isDialogShown == null || isDialogShown == false)
            {
                return;
            }

            if (openFileDialog.FileNames.Length > 0)
            {
                this.AddImagesToList(openFileDialog.FileNames);
                this.BtnClearList.IsEnabled = true;
                this.CheckCleanExifEnabled();
            }
            else
            {
                MessageBox.Show("Please select at least one image", "Exif remover", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Adds the images to the list.
        /// </summary>
        /// <param name="fileNames">The file names.</param>
        private void AddImagesToList(string[] fileNames)
        {
            if (fileNames == null)
            {
                throw new ArgumentNullException(nameof(fileNames));
            }

            if (this.exifItems.Count > 0)
            {
                this.exifItems.Clear();
            }

            foreach (var file in fileNames)
            {
                this.exifItems.Add(
                    new ExifItem(
                        Path.GetExtension(file),
                        Path.GetFileNameWithoutExtension(file),
                        Path.GetDirectoryName(file) ?? string.Empty,
                        file));
            }

            this.ExifItemsView.ItemsSource = this.exifItems;
        }

        /// <summary>
        /// Handles the button click to select a folder.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ButtonSelectFolder(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            var isDialogShown = openFileDialog.ShowDialog();

            if (isDialogShown == null || isDialogShown == false)
            {
                return;
            }

            this.TextBoxOutput.Text = Path.GetDirectoryName(openFileDialog.FileName) ?? string.Empty;
            this.CheckCleanExifEnabled();
        }

        /// <summary>
        /// Handles the button click to clear the list.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ButtonClearList(object sender, RoutedEventArgs e)
        {
            this.exifItems.Clear();
            this.BtnClearList.IsEnabled = false;
            this.CheckCleanExifEnabled();
        }

        /// <summary>
        /// Checks whether the clean exif button is enabled or not.
        /// </summary>
        private void CheckCleanExifEnabled()
        {
            if (this.exifItems != null && this.exifItems.Count > 0 && !string.IsNullOrEmpty(this.TextBoxOutput.Text))
            {
                this.ButtonExifClean.IsEnabled = true;
            }
            else
            {
                this.ButtonExifClean.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles the button click to clean the images.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ButtonCleanExif(object sender, RoutedEventArgs e)
        {
            if (this.exifItems.Count <= 0)
            {
                MessageBox.Show("Please select at least one image", "Exif remover", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(this.TextBoxOutput.Text))
            {
                MessageBox.Show("Please select the output folder", "Exif remover", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            this.SetHandlerAndStart();
        }

        /// <summary>
        /// Sets the handler and starts the cleaner.
        /// </summary>
        private void SetHandlerAndStart()
        {
            this.cleaner = new ExifCleaner(this.TextBoxOutput.Text, this.exifItems);
            this.InitializeHandlers();
            this.cleaner.Start();
        }

        /// <summary>
        /// Handles the clean completed event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void HandleCleanCompleted(object sender, ExifCleanCompletedEventArgs e)
        {
            this.ButtonClearList(sender, new RoutedEventArgs());
            this.SetGuiToStop();
            MessageBox.Show(e.Message, "Exif remover", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Handles the clean process event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void HandleCleanProcess(object sender, ExifCleanProgressEventArgs e)
        {
            this.TextBoxFinished.Text = e.Message;
        }

        /// <summary>
        /// Stops the actions enables the UI.
        /// </summary>
        private void SetGuiToStop()
        {
            this.BtnSelectImages.IsEnabled = true;
            this.BtnSelectFolder.IsEnabled = true;
            this.ButtonExifClean.IsEnabled = false;
            this.BtnClearList.IsEnabled = false;
        }
    }
}