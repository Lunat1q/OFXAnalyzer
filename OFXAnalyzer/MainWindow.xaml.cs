using Microsoft.Win32;
using System.Windows;
using OFXAnalyzer.Core;
using System.Linq;
using OFXAnalyzer.ViewModels;
using TiqUtils.Serialize;
using TiqUtils.TypeSpecific;

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

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
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
    }
}
