using RestSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RandomStrings
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      this.InitializeComponent();
    }

    private void txtNumberStrings_KeyDown(object sender, KeyRoutedEventArgs e)
    {

      // Check if the pressed key is a number or a control key (e.g., Backspace, Delete)
      if (!((e.Key >= VirtualKey.Number0 && e.Key <= VirtualKey.Number9) ||
            (e.Key >= VirtualKey.NumberPad0 && e.Key <= VirtualKey.NumberPad9) ||
            e.Key == VirtualKey.Tab || e.Key == VirtualKey.Enter || e.Key == VirtualKey.Back || e.Key == VirtualKey.Delete))
      {
        // Mark the event as handled to prevent the key from being entered into the TextBox
        e.Handled = true;
      }
    }

    private void btnSubmit_Click(object sender, RoutedEventArgs e)
    {
      string digits =  chkDigits.IsChecked == true ? "on" : "off";
      string upper = chkUpperCase.IsChecked == true ? "on" : "off";
      string lower = chkLowerCase.IsChecked == true ? "on" : "off";
      string symbols = chkSymbols.IsChecked == true ? "on" : "off";
      string unique = chkUnique.IsChecked == true ? "on" : "off";

      txtOutput.Text = "";

      try { 
      int test = int.Parse(txtNumberStrings.Text);
      }
      catch {
        txtOutput.Text = "Invalid input. Please enter a number in the Number of Passwords field.";
        return;
      }
      try
      {
        int test = int.Parse(txtMaxLength.Text);
      }
      catch
      {
        txtOutput.Text = "Invalid input. Please enter a number in the Max Length field.";
        return;
      }


      List<string> passwords = GetRandomStrings(int.Parse(txtNumberStrings.Text), int.Parse(txtMaxLength.Text), digits, upper, lower, unique);
      int counter = 0;
      foreach (string pass in passwords)
      {

        if (symbols == "on")
        {
          string temp = pass;
          temp = InsertRandomChar(pass);
          txtOutput.Text += temp;
        }
        else
        {
          txtOutput.Text += pass;
        }
        if (counter < passwords.Count - 1) { txtOutput.Text += "\r\n"; }
        counter++;


      }


    }
    static string InsertRandomChar(string inputString)
    {
      try
      {
        // Define a string of possible special characters
        string possibleSpecialChars = "!@#$%^&*~?()_+-={}[]|:;<>?";

        // Create a random number generator
        Random random = new Random();

        // Choose a random position to replace the character
        int randomPosition = random.Next(0, inputString.Length - 1);

        // Choose a random special character to replace the existing character
        char randomSpecialChar = possibleSpecialChars[random.Next(possibleSpecialChars.Length)];

        // Replace the character at the random position with the random special character
        char[] charArray = inputString.ToCharArray();
        charArray[randomPosition] = randomSpecialChar;

        return new string(charArray);
      }
      catch
      {
        return "";
      }
    }
    public static async Task<string> GetIntegers(string url)
    {
      var client = new RestClient(url);
      var response = client.Execute(new RestRequest());
      return response.Content;
    }

    public static List<string> GetRandomStrings(int size = 1, int len = 8, string digits = "on", string upperalpha = "on", string loweralpha = "on", string unique = "on")
    {
      if (size > 1)
      {
        var apiCallTask = GetIntegers($"https://www.random.org/strings?num={size}&len={len}&digits={digits}&upperalpha={upperalpha}&loweralpha={loweralpha}&unique={unique}&format=plain&rnd=new");
        var result = apiCallTask.Result;
        string[] lines = result.Split('\n');
        List<string> aList = new List<string>(lines);
        return aList;
      }
      else
      {
        List<string> aList = new List<string>();
        //get the item
        //add to a list
        //return the list
        var apiCallTask = GetIntegers($"https://www.random.org/strings?num=1&len={len}&digits={digits}&upperalpha={upperalpha}&loweralpha={loweralpha}&unique={unique}&format=plain&rnd=new");
        var result = apiCallTask.Result;
        aList.Add(result);
        return aList;
      }
    }
  }
}