using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CreditCardChecksum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            //BitmapImage visaActive = new BitmapImage(new Uri("Card_Visa_active.gif", UriKind.Relative));
            

            if (string.IsNullOrWhiteSpace(CreditCardNumber.Text))
                throw new inpute
            
            {
                if (CreditCardNumber.Text.Length >= 1 && CreditCardNumber.Text[0] == '4')
                    activateLogo(0);
                else if (CreditCardNumber.Text.Length >= 2 && CreditCardNumber.Text[0] == '5' && CreditCardNumber.Text[1] >= '1' && CreditCardNumber.Text[1] <= '5')
                    activateLogo(1);
                else if (CreditCardNumber.Text.Length >= 2 && CreditCardNumber.Text[0] == '3' && (CreditCardNumber.Text[1] == '4' || CreditCardNumber.Text[1] == '7'))
                    activateLogo(3);
                else if (CreditCardNumber.Text.Length >= 4 && CreditCardNumber.Text.Substring(0, 4) == "6011")
                    activateLogo(2);
                else
                    activateLogo(-1);
            }
            else
            {
                activateLogo(-1);
            }

        }

        private void CreditCardNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (int.Parse(CreditCardNumber.Text) == 0)
            int value;
            //if (int.TryParse(CreditCardNumber.Text, out value))
            if (!string.IsNullOrWhiteSpace(CreditCardNumber.Text))
            {
                if (int.TryParse(CreditCardNumber.Text, out value))
                    if (value == 0)
                        CVC.Text = "zero";
                    else
                        CVC.Text = value.ToString();
                else
                    CVC.Text = "parseFailed";
            }
            else
            {
                CVC.Text = "nullOrWhiteSpace";
            }


            if (!string.IsNullOrWhiteSpace(CreditCardNumber.Text))
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
