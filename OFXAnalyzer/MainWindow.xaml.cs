using Microsoft.Win32;
using System.Windows;
using OFXAnalyzer.Core;
using System.Linq;
using OFXAnalyzer.ViewModels;
using TiqUtils.Serialize;
using TiqUtils.TypeSpecific;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace OFXAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TransactionAnalysisContext _context;
        private readonly Settings _settings;

        public MainWindow()
        {
            this.InitializeComponent();
            this._settings = Settings.Default.Settings;
            this._settings.Init();
            this._context = new TransactionAnalysisContext(this._settings);
            this.DataContext = this._context;
            this._context.Init();
            SetupTheme();
        }

        private static void SetupTheme()
        {
            var paletteHelper = new PaletteHelper();
            var theme = (Theme)paletteHelper.GetTheme();
            theme.ColorAdjustment = new ColorAdjustment()
            {
                DesiredContrastRatio = 4.5f,
                Contrast = Contrast.Medium,
                Colors = ColorSelection.All
            };
            paletteHelper.SetTheme(theme);
        }

        private void LoadOfxButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var parser = new OfxParser();

                var parsedData = parser.ParseFromFile(filePath);

                var allTransactions = parsedData.BankData.BankAccounts.SelectMany(x => x.Statements.Transactions.Transactions);
                this._context.FillTransactions(allTransactions);
                this._context.CalculateBalance();
                this._context.RecalculateGrouping();
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.Save();
        }

        private void CreateNewGroupButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!this.NewGroupName.Text.Empty())
            {
                var newGroup = new TranGroupSettings()
                {
                    Name = this.NewGroupName.Text
                };
                this._settings.Groups!.Add(newGroup);
                this.NewGroupName.Text = "";
                this._context.SelectedGroupForEdit = this._context.AddNewGroup(newGroup);
            }
        }

        private void CreateNewRuleButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedNewRule = this._context.RuleSettings.SelectedRuleToCreate;
            if (selectedNewRule != null)
            {
                TransactionAnalysisContext.CreateNewRule(this._context.SelectedGroupForEdit, selectedNewRule);
            }
        }

        private void RecalculateGrouping_Click(object sender, RoutedEventArgs e)
        {
            this._context.RecalculateGrouping();
        }

        private void CreateNewIgnoreRuleButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedNewRule = this._context.RuleSettings.SelectedRuleToCreateInIgnore;
            if (selectedNewRule != null)
            {
                TransactionAnalysisContext.CreateNewRule(this._context.TransactionIgnoreGroup, selectedNewRule);
            }
        }

        private void ColorPicker_ColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color> e)
        {
            this._context.SelectedGroupForEdit.TriggerGroupRecolorUpdate();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (this._context.SelectedGroupForEdit.UseCustomColor)
            {
                this._context.SelectedGroupForEdit.GroupColor = Color.FromRgb(255, 89, 89);
            }
            else
            {
                this._context.SelectedGroupForEdit.GroupColor = Colors.Transparent;
            }
        }
    }
}
