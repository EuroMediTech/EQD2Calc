using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS
{
  public class Script
  {
    public Script() { }

    public void Execute(ScriptContext context /*, System.Windows.Window window*/ )
    {
      ShowCalcWindow();
    }

    Window MainWindow = new Window();
    TextBox abBox = new TextBox();
    TextBox inputBox = new TextBox();
    TextBox fractionBox = new TextBox();
    TextBox resultBox = new TextBox();
    RadioButton eqdModeButton = new RadioButton();
    RadioButton doseModeButton = new RadioButton();
    bool isAbBoxOK;
    bool isInputBoxOK;
    bool isFractionBoxOK;
    Label inputLabel = new Label();
    Label resultLabel = new Label();

    private void ShowCalcWindow()
    {
      isAbBoxOK = false;
      isInputBoxOK = false;
      isFractionBoxOK = false;

      var mainBorder = new Border();
      mainBorder.Padding = new Thickness(15);

      var mainPanel = new StackPanel();
      mainPanel.Orientation = Orientation.Vertical;

      // Radiobutton box
      var calcMode = new GroupBox();
      calcMode.HorizontalAlignment = HorizontalAlignment.Center;
      calcMode.Margin = new Thickness(10);
      calcMode.Header = "Calculation Mode";
      calcMode.Background = Brushes.AliceBlue;

      var calcModePanel = new StackPanel();
      calcModePanel.Orientation = Orientation.Horizontal;
      calcModePanel.HorizontalAlignment = HorizontalAlignment.Center;

      eqdModeButton.Content = "EQD2を計算";
      eqdModeButton.Margin = new Thickness(50, 10, 20, 10);
      eqdModeButton.IsChecked = true;
      eqdModeButton.Checked += new RoutedEventHandler(eqdMode_checked);

      doseModeButton.Content = "TotalDoseを計算";
      doseModeButton.Margin = new Thickness(20, 10, 50, 10);
      doseModeButton.Checked += new RoutedEventHandler(doseMode_checked);

      doseModeButton.GroupName = "group1";
      eqdModeButton.GroupName = "group1";

      calcModePanel.Children.Add(eqdModeButton);
      calcModePanel.Children.Add(doseModeButton);

      calcMode.Content = calcModePanel;

      // Input box panel
      var inputPanel = new StackPanel();
      inputPanel.Orientation = Orientation.Vertical;
      inputPanel.HorizontalAlignment = HorizontalAlignment.Center;

      var abPanel = new StackPanel();
      abPanel.Orientation = Orientation.Horizontal;
      abPanel.HorizontalAlignment = HorizontalAlignment.Center;

      var abLabel = new Label();
      abLabel.Content = "alpha/beta";
      abLabel.Margin = new Thickness(10);

      abBox.Width = 75;
      abBox.Margin = new Thickness(10);
      abBox.HorizontalAlignment = HorizontalAlignment.Center;
      abBox.TextAlignment = TextAlignment.Center;
      abBox.TextChanged += new TextChangedEventHandler(abBox_changed);

      abPanel.Children.Add(abLabel);
      abPanel.Children.Add(abBox);

      var dosePanel = new StackPanel();
      dosePanel.Orientation = Orientation.Horizontal;
      dosePanel.HorizontalAlignment = HorizontalAlignment.Center;

      var firstPanel = new StackPanel();
      firstPanel.Orientation = Orientation.Vertical;
      firstPanel.HorizontalAlignment = HorizontalAlignment.Center;

      inputLabel.Content = "TotalDose (Gy)";
      inputLabel.HorizontalAlignment = HorizontalAlignment.Center;
      inputLabel.Margin = new Thickness(10, 10, 10, 1);

      inputBox.Width = 120;
      inputBox.Margin = new Thickness(10, 1, 10, 10);
      inputBox.HorizontalAlignment = HorizontalAlignment.Center;
      inputBox.TextAlignment = TextAlignment.Center;
      inputBox.TextChanged += new TextChangedEventHandler(inputBox_changed);

      firstPanel.Children.Add(inputLabel);
      firstPanel.Children.Add(inputBox);

      var secondPanel = new StackPanel();
      secondPanel.Orientation = Orientation.Vertical;
      secondPanel.HorizontalAlignment = HorizontalAlignment.Center;

      var fractionLabel = new Label();
      fractionLabel.Content = "fraction";
      fractionLabel.HorizontalAlignment = HorizontalAlignment.Center;
      fractionLabel.Margin = new Thickness(10, 10, 10, 1);

      fractionBox.Width = 120;
      fractionBox.Margin = new Thickness(10, 1, 10, 10);
      fractionBox.HorizontalAlignment = HorizontalAlignment.Center;
      fractionBox.TextAlignment = TextAlignment.Center;
      fractionBox.TextChanged += new TextChangedEventHandler(fractionBox_changed);

      secondPanel.Children.Add(fractionLabel);
      secondPanel.Children.Add(fractionBox);

      var thirdPanel = new StackPanel();
      thirdPanel.Orientation = Orientation.Vertical;
      thirdPanel.HorizontalAlignment = HorizontalAlignment.Center;

      resultLabel.Content = "EQD2 (Gy)";
      resultLabel.Margin = new Thickness(10, 10, 10, 1);
      resultLabel.HorizontalAlignment = HorizontalAlignment.Center;

      resultBox.Width = 120;
      resultBox.FontWeight = FontWeights.Bold;
      resultBox.Margin = new Thickness(10, 1, 10, 10);
      resultBox.IsReadOnly = true;
      resultBox.HorizontalAlignment = HorizontalAlignment.Center;
      resultBox.TextAlignment = TextAlignment.Center;
      resultBox.Background = Brushes.Snow;

      thirdPanel.Children.Add(resultLabel);
      thirdPanel.Children.Add(resultBox);

      dosePanel.Children.Add(firstPanel);
      dosePanel.Children.Add(secondPanel);
      dosePanel.Children.Add(thirdPanel);

      inputPanel.Children.Add(abPanel);
      inputPanel.Children.Add(dosePanel);

      // calculate button
      var calButton = new Button();
      calButton.Content = "Calculate";
      calButton.HorizontalAlignment = HorizontalAlignment.Right;
      calButton.Padding = new Thickness(10, 5, 10, 5);
      calButton.Margin = new Thickness(10, 10, 10, 30);

      calButton.Click += new RoutedEventHandler(calButton_click);

      mainPanel.Children.Add(calcMode);
      mainPanel.Children.Add(inputPanel);
      mainPanel.Children.Add(calButton);

      mainBorder.Child = mainPanel;

      MainWindow.Title = "EQD2 Calculator";
      MainWindow.Content = mainBorder;
      MainWindow.FontSize = 16;
      MainWindow.SizeToContent = SizeToContent.WidthAndHeight;
      MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      MainWindow.ShowDialog();

    }

    private void calButton_click(object sender, RoutedEventArgs e)
    {
      if (isAbBoxOK && isInputBoxOK && isFractionBoxOK)
      {
        var input = Convert.ToDouble(inputBox.Text);
        var fraction = Convert.ToDouble(fractionBox.Text);
        var ab = Convert.ToDouble(abBox.Text);

        double result;

        if (eqdModeButton.IsChecked == true)
        {
          result = input * (ab + input / fraction) / (ab + 2);

          resultBox.Text = result.ToString("F4");
        }
        if (doseModeButton.IsChecked == true)
        {
          var a2 = 1 / (ab * fraction);
          var c2 = -input * (1 + 2 / (ab));

          result = (Math.Sqrt(1 - 4 * a2 * c2) - 1) / (2 * a2);

          resultBox.Text = result.ToString("F4");
        }
      }
    }

    private void eqdMode_checked(object sender, RoutedEventArgs e)
    {
      inputLabel.Content = "TotalDose (Gy)";
      resultLabel.Content = "EQD2 (Gy)";
      resultBox.Text = "";
    }

    private void doseMode_checked(object sender, RoutedEventArgs e)
    {
      inputLabel.Content = "EQD2 (Gy)";
      resultLabel.Content = "TotalDose (Gy)";
      resultBox.Clear();
    }

    private void abBox_changed(object sender, TextChangedEventArgs e)
    {
      isAbBoxOK = false;

      if (abBox.Text != "")
      {
        double ab = 1;
        try
        {
          ab = Convert.ToDouble(abBox.Text);
        }
        catch (FormatException)
        {
          MessageBox.Show("適切な値を入力してください。");
          abBox.Clear();
        }
        if (ab <= 0)
        {
          MessageBox.Show("適切な値を入力してください。");
        }
        else
        {
          isAbBoxOK = true;
        }
      }
    }

    private void inputBox_changed(object sender, TextChangedEventArgs e)
    {
      isInputBoxOK = false;

      if (inputBox.Text != "")
      {
        double dose = 1;
        try
        {
          dose = Convert.ToDouble(inputBox.Text);
        }
        catch (FormatException)
        {
          MessageBox.Show("適切な値を入力してください。");
          inputBox.Clear();
        }
        if (dose <= 0)
        {
          MessageBox.Show("適切な値を入力してください。");
        }
        else
        {
          isInputBoxOK = true;
        }
      }
    }

    private void fractionBox_changed(object sender, TextChangedEventArgs e)
    {
      isFractionBoxOK = false;

      if (fractionBox.Text != "")
      {
        int fr = 1;
        try
        {
          fr = Convert.ToInt32(fractionBox.Text);
        }
        catch (FormatException)
        {
          MessageBox.Show("適切な値を入力してください。");
          fractionBox.Clear();
        }
        if (fr <= 0)
        {
          MessageBox.Show("適切な値を入力してください。");
        }
        else
        {
          isFractionBoxOK = true;
        }
      }
    }
  }
}