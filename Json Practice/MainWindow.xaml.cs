using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Newtonsoft.Json;

namespace Json_Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        readonly string jsonFolder = "./Json Files";
        #region Main Members
        public MainWindow()
        {
            InitializeComponent();
            VerifyJsonFolder();
            UpdateList();
        }
        private void MainComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => LoadJSON();
        private void MainSaveButton_Click(object sender, RoutedEventArgs e) => SaveJSON();
        private void MainRefreshButton_Click(object sender, RoutedEventArgs e) => UpdateList();
        private void MainDeleteButton_Click(object sender, RoutedEventArgs e) => DeleteJsonFile();
        #endregion

        #region Helper Methods
        private void DeleteJsonFile()
        {
            if (mainComboBox.SelectedItem == null) return;
            string filePath = mainComboBox.SelectedItem.ToString();
            string targetLocation = jsonFolder + "/" + filePath + ".json";
            if (File.Exists(targetLocation))
                File.Delete(targetLocation);
            UpdateList();
        }
        private void VerifyJsonFolder()
        {
            if (!File.Exists(jsonFolder))
                Directory.CreateDirectory(jsonFolder);
        }
        private void UpdateList()
        {
            mainComboBox.Items.Clear();
            mainExampleString01.Text = string.Empty;
            mainExampleString02.Text = string.Empty;
            mainExampleInt.SelectedItem = null;
            var files = Directory.GetFiles(jsonFolder);
            foreach (var file in files)
                if (Path.GetExtension(file) == ".json")
                    mainComboBox.Items.Add(Path.GetFileNameWithoutExtension(file));
        }
        private void SaveJSON()
        {
            if (mainExampleString01.Text == string.Empty) return;
            var saveObject = new Class1(mainExampleString01.Text, mainExampleInt.SelectedIndex, mainExampleString02.Text);
            string JSONresult = JsonConvert.SerializeObject(saveObject);
            string targetLocation = jsonFolder + "/" + mainExampleString01.Text + ".json";
            using(var sw = new StreamWriter(targetLocation))
                sw.WriteLine(JSONresult.ToString());
            UpdateList();
        }
        private void LoadJSON()
        {
            if (mainComboBox.SelectedItem == null) return;
            string filePath = mainComboBox.SelectedItem.ToString();
            string targetLocation = jsonFolder + "/" + filePath + ".json";
            if (!File.Exists(targetLocation))
            {
                Console.WriteLine(targetLocation);
                UpdateList();
                return;
            }
            Class1 loadedObject;
            using (var sr = new StreamReader(targetLocation))
            {
                var serializer = new JsonSerializer();
                loadedObject = (Class1)serializer.Deserialize(sr, typeof(Class1));
            }
            LoadObject(loadedObject);
        }
        private void LoadObject(Class1 item)
        {
            mainExampleString01.Text = item.string01;
            mainExampleString02.Text = item.string02;
            mainExampleInt.SelectedIndex = item.int01;
        }
        #endregion
    }
}
