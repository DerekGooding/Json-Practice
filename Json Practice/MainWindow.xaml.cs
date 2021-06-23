using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace Json_Practice
{
    public partial class MainWindow : Window
    {
        #region Variables
        readonly DirectoryInfo jsonFolder = new DirectoryInfo(@".\Json Files");
        readonly string jsonExtension = @".json";
        #endregion

        #region Main Members
        public MainWindow()
        {
            InitializeComponent();
            jsonFolder.Create();
            UpdateList();
        }
        private void MainComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => LoadJSON();
        private void MainSaveButton_Click(object sender, RoutedEventArgs e) => SaveJSON();
        private void MainRefreshButton_Click(object sender, RoutedEventArgs e) => UpdateList();
        private void MainDeleteButton_Click(object sender, RoutedEventArgs e) => DeleteJSONFile();
        #endregion

        #region Helper Methods
        private void UpdateList()
        {
            ClearList();
            foreach (var file in jsonFolder.GetFiles())
                if (file.Extension == jsonExtension)
                    mainComboBox.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
        }
        private void ClearList()
        {
            mainComboBox.Items.Clear();
            mainExampleString01.Text = string.Empty;
            mainExampleString02.Text = string.Empty;
            mainExampleInt.SelectedItem = null;
        }
        private string GetTargetFile() => GetTargetFile(mainComboBox.SelectedItem.ToString());
        private string GetTargetFile(string fileName) => Path.Combine(jsonFolder.FullName, fileName + jsonExtension);
        public static bool FileStringIsInvalid(string path)
        {
            System.Console.WriteLine(path);
            System.Console.WriteLine(path.IndexOfAny(Path.GetInvalidPathChars()));
            foreach(var item in Path.GetInvalidPathChars())
                System.Console.WriteLine(item);
            return string.IsNullOrEmpty(path) || path.IndexOfAny(Path.GetInvalidPathChars()) != -1;
        }

        private void DeleteJSONFile()
        {
            if (mainComboBox.SelectedItem == null) return;
            File.Delete(GetTargetFile());
            UpdateList();
        }
        private void LoadJSON()
        {
            if (mainComboBox.SelectedItem == null) return;
            string targetFile = GetTargetFile();
            if (File.Exists(targetFile))
                using (var sr = new StreamReader(targetFile))
                {
                    var serializer = new JsonSerializer();
                    SaveObject loadedObject = (SaveObject)serializer.Deserialize(sr, typeof(SaveObject));
                    LoadObject(loadedObject);
                }
            else
                UpdateList();
        }
        private void SaveJSON()
        {
            if (FileStringIsInvalid(mainExampleString01.Text)) return;
            var saveObject = new SaveObject(mainExampleString01.Text, mainExampleInt.SelectedIndex, mainExampleString02.Text);
            string targetFile = GetTargetFile(mainExampleString01.Text);
            using (var sw = new StreamWriter(targetFile))
                sw.WriteLine(JsonConvert.SerializeObject(saveObject).ToString());
            UpdateList();
        }
        private void LoadObject(SaveObject item)
        {
            mainExampleString01.Text = item.string01;
            mainExampleString02.Text = item.string02;
            mainExampleInt.SelectedIndex = item.int01;
        }
        #endregion
    }
}
