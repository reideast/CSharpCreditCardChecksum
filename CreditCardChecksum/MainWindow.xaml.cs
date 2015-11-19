//credit card validation rules/math from: http://web.eecs.umich.edu/~bartlett/credit_card_number.html

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CreditCardChecksum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TODO: implement data array for all credit cards
        //TODO: create XAML Image controls on-the-fly with that data (that is possible with C#, right??)
        //TODO: put rules into data array via: length lambda function, prefix lambda function!
        private BitmapImage[] visa = { new BitmapImage(new Uri("Card_Visa_active.gif", UriKind.Relative)), new BitmapImage(new Uri("Card_Visa_inactive.gif", UriKind.Relative)) };
        private BitmapImage[] masterCard = { new BitmapImage(new Uri("Card_MasterCard_active.gif", UriKind.Relative)), new BitmapImage(new Uri("Card_MasterCard_inactive.gif", UriKind.Relative)) };
        private BitmapImage[] discover = { new BitmapImage(new Uri("Card_Discover_active.gif", UriKind.Relative)), new BitmapImage(new Uri("Card_Discover_inactive.gif", UriKind.Relative)) };
        private BitmapImage[] americanExpress = { new BitmapImage(new Uri("Card_AmericanExpress_active.gif", UriKind.Relative)), new BitmapImage(new Uri("Card_AmericanExpress_inactive.gif", UriKind.Relative)) };

        private int validCardIndex;

        public MainWindow()
        {
            InitializeComponent();
            CreditCardNumber.TextChanged += CreditCardNumber_TextChanged;

            validCardIndex = -1;
            activateLogo(validCardIndex);
        }

        private void ProcessPaymentClick(object sender, RoutedEventArgs e)
        {
            bool isValidPaymentDetails = true;

            if (string.IsNullOrWhiteSpace(CreditCardNumber.Text)) //if user enters nothing, then clicks submit, this state can be reached
            {
                MessageBox.Show("DEBUG ERROR: Credit card number is blank.");
                isValidPaymentDetails = false;
            }
            else
            {

                //TODO: if (cardTypesArray.All( (cardType) => cardType.Validate(CreditCardNumber.Text) ) ....

                //validCardIndex will be 0-3 for the four credit card types (and therefore MUST be 100% numeric) or -1 for invalid
                if ((validCardIndex == 0 && (CreditCardNumber.Text.Length == 16 || CreditCardNumber.Text.Length == 13))
                 || (validCardIndex == 1 && CreditCardNumber.Text.Length == 16)
                 || (validCardIndex == 2 && CreditCardNumber.Text.Length == 16)
                 || (validCardIndex == 3 && CreditCardNumber.Text.Length == 15))
                {
                    if (!ValidateLuhn(CreditCardNumber.Text)) //perform checksum validation
                    {
                        MessageBox.Show("DEBUG ERROR: Credit card number failed checksum.");
                        isValidPaymentDetails = false;
                    }
                }
                else if (validCardIndex == -1)
                {
                    MessageBox.Show("DEBUG ERROR: Credit card number does not match a valid card type.");
                    isValidPaymentDetails = false;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Card was neither valid nor set to invalid sentinel value.");
                }
            }
            
            if (isValidPaymentDetails)
            {
                MessageBox.Show("DEBUG: Credit card number is valid. Process payment now.");
            }
        }

        private bool ValidateLuhn(string text)
        {
            //put a leading zero on any odd-length string, allowing algorithm to process left-to-right rather than R-to-L
            if (text.Length % 2 == 1)
                text = '0' + text;

            int sum = 0;
            int position = 0;
            foreach(char digit in text)
            {
                if (position++ % 2 == 1)
                {
                    sum += digit - '0';
                }
                else
                {
                    sum += (digit - '0') * 2;
                    if (digit - '0' >= 6) //6 through 9
                        sum -= 9; //see logic below for magic number "9"
                }
                // 6*2=12, 7*2=14, 8*2=16, 9*2=18
                //  6->3   7->5    8->7    9->9
                //  -3      -2      -1      -0
                // digit - (9 - digit) ---> digit - 9 + digit ----> derived formula: 2*digit - 9
                // 9-6=3   9-7=2   9-8=1   9-9=0 //testing ()
                // 12-9=3  14-9=5  16-9=7  18-9=9 //testing
            }
            //MessageBox.Show(sum.ToString()); //4011 == 11

            if (sum % 10 == 0)
                return true;
            else
                return false;
        }

        private void CreditCardNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

            //DEBUG:
            //int value;
            //if (!string.IsNullOrWhiteSpace(CreditCardNumber.Text))
            //{
            //    if (Int32.TryParse(CreditCardNumber.Text, out value))
            //        if (value == 0)
            //            CVC.Text = "zero";
            //        else
            //            CVC.Text = value.ToString();
            //    else
            //        CVC.Text = "parse returned not-an-integer";
            //}
            //else
            //{
            //    CVC.Text = "nullOrWhiteSpace";
            //}


            if (!string.IsNullOrWhiteSpace(CreditCardNumber.Text) && CreditCardNumber.Text.All((digit) => char.IsDigit(digit))) //test if ALL characters of string are numeric
            {
                if (CreditCardNumber.Text.Length >= 1 && CreditCardNumber.Text[0] == '4')
                    validCardIndex = 0;
                else if (CreditCardNumber.Text.Length >= 2 && CreditCardNumber.Text[0] == '5' && CreditCardNumber.Text[1] >= '1' && CreditCardNumber.Text[1] <= '5')
                    validCardIndex = 1;
                else if (CreditCardNumber.Text.Length >= 2 && CreditCardNumber.Text[0] == '3' && (CreditCardNumber.Text[1] == '4' || CreditCardNumber.Text[1] == '7'))
                    validCardIndex = 3;
                else if (CreditCardNumber.Text.Length >= 4 && CreditCardNumber.Text.Substring(0, 4) == "6011")
                    validCardIndex = 2;
                else
                    validCardIndex = -1;
            }
            else
            {
                validCardIndex = -1;
            }

            activateLogo(validCardIndex);
        }

        private void activateLogo(int index)
        {
            LogoVisa.Source = visa[1];
            LogoMasterCard.Source = masterCard[1];
            LogoDiscover.Source = discover[1];
            LogoAmericanExpress.Source = americanExpress[1];
            if (index == 0)
                LogoVisa.Source = visa[0];
            else if (index == 1)
                LogoMasterCard.Source = masterCard[0];
            else if (index == 2)
                LogoDiscover.Source = discover[0];
            else if (index == 3)
                LogoAmericanExpress.Source = americanExpress[0];
        }
    }
}
