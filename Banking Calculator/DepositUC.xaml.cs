using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Banking_Calculator
{
    public partial class DepositUC : UserControl
    {
        private double startAmount;
        private double depositPeriod;
        private double interestRate;
        private double compound;
        private bool enableCompound = false;
        bool raiseit = false;

        string errormsg = "Required";

        private event System.EventHandler calculateit;
        private event System.EventHandler calculateno;
        public DepositUC()
        {
            
            InitializeComponent();
        }


        public double StartAmount
        {
            get { return startAmount; }
        }


        public double DepositPeriod
        {
            get { return depositPeriod; }
        }

        public double InterestRate
        {
            get { return interestRate; }
        }

        public double Compound
        {
            get { return compound; }
        }

        public bool EnableCompound
        {
            set { enableCompound=value; }
        }

        public event System.EventHandler CalculateIt
        {
            add { calculateit += value; }
            remove { calculateit -= value; }
        }
        public event System.EventHandler CalculateNo
        {
            add { calculateno += value; }
            remove { calculateno -= value; }
        }
        private void CalculateBtn_Click(object sender, RoutedEventArgs e)
        {
            bool monthOK = false;
            bool yearOK = false;

            if (StartAmtTxtBx.Text == "Enter Start Amount" || StartAmtTxtBx.Text.Equals("") || StartAmtTxtBx.Text == errormsg || !Double.TryParse(StartAmtTxtBx.Text, out startAmount))
            {
                StartAmtTxtBx.Text = errormsg;
                raiseit = false;
            }
            else
            {
                Double.TryParse(StartAmtTxtBx.Text, out startAmount);
                raiseit = true;
            }
            
            if (PeriodMonthsTxtBx.Text==errormsg || PeriodMonthsTxtBx.Text=="Months" || !Double.TryParse(PeriodMonthsTxtBx.Text, out depositPeriod) || PeriodMonthsTxtBx.Text=="")
            {
                monthOK = false;
            }
            else
            {
                monthOK = true;
            }
            if (PeriodYearsTxtBx.Text==errormsg || PeriodYearsTxtBx.Text=="Years" ||!Double.TryParse(PeriodYearsTxtBx.Text, out depositPeriod) || PeriodYearsTxtBx.Text=="")
            {
                yearOK = false;
            }
            else
            {
                yearOK = true;
            }
            if (yearOK == false && monthOK == false)
            {
               PeriodMonthsTxtBx.Text = errormsg;
                PeriodYearsTxtBx.Text= errormsg;
                raiseit = false;
            }
            if (yearOK == true && monthOK == true)
            {
                depositPeriod = 0;
                depositPeriod += Convert.ToDouble(PeriodMonthsTxtBx.Text) + Convert.ToDouble(PeriodYearsTxtBx.Text);
                raiseit = true;
            }
            if (yearOK == true || monthOK == true)
            {
                depositPeriod = 0;
                if (yearOK == true)
                {
                    depositPeriod += Convert.ToDouble(PeriodYearsTxtBx.Text);
                    PeriodMonthsTxtBx.Text = "Months";
                }
                if (monthOK == true)
                {
                    depositPeriod += Convert.ToDouble(PeriodMonthsTxtBx.Text);
                    PeriodYearsTxtBx.Text = "Years";
                }
                raiseit = true;

            }
            if (InterestRateTxtBx.Text == "Annual Interest Rate" || InterestRateTxtBx.Text.Equals("") || InterestRateTxtBx.Text == errormsg || !Double.TryParse(InterestRateTxtBx.Text, out interestRate))
            {
                InterestRateTxtBx.Text = errormsg;
                raiseit = false;
            }
            else
            {
                Double.TryParse(InterestRateTxtBx.Text, out interestRate);
                raiseit = true;
            }
            if (CompoundListPicker.SelectedIndex == 0)
                compound = 12;
            if (CompoundListPicker.SelectedIndex == 1)
                compound = 1;
            if (CompoundListPicker.SelectedIndex == 2)
                compound = 2;
            if (CompoundListPicker.SelectedIndex == 3)
                compound = 4;

            if (raiseit == true)
                if (calculateit != null)
                    calculateit(sender, e);

            if (raiseit == false)
                if (calculateno != null)
                    calculateno(sender, e);

        }

        private void CompoundListPicker_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CompoundListPicker.ListPickerMode == ListPickerMode.Normal)
                CompoundListPicker.Open();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            StartAmtTxtBx.Text = "Enter Start Amount";
            PeriodYearsTxtBx.Text = "Years";
            PeriodMonthsTxtBx.Text = "Months";
            InterestRateTxtBx.Text = "Annual Interest Rate";
            CompoundListPicker.SelectedIndex = 0;
        }

        private void StartAmtTxtBx_GotFocus(object sender, RoutedEventArgs e)
        {
            if (StartAmtTxtBx.Text.Equals("Enter Start Amount") || StartAmtTxtBx.Text.Equals(errormsg))
                StartAmtTxtBx.Text = string.Empty;
        }

        private void StartAmtTxtBx_LostFocus(object sender, RoutedEventArgs e)
        {
            if (StartAmtTxtBx.Text.Equals("") || StartAmtTxtBx.Text.Equals(errormsg))
               StartAmtTxtBx.Text = "Enter Start Amount";
        }


        private void PeriodYearsTxtBx_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PeriodYearsTxtBx.Text.Equals("Years") || PeriodYearsTxtBx.Text.Equals(errormsg))
                PeriodYearsTxtBx.Text = string.Empty;
        }

        private void PeriodYearsTxtBx_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PeriodYearsTxtBx.Text.Equals("") || PeriodYearsTxtBx.Text.Equals(errormsg))
                PeriodYearsTxtBx.Text = "Years";
        }

        private void PeriodMonthsTxtBx_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PeriodMonthsTxtBx.Text.Equals("Months") || PeriodMonthsTxtBx.Text.Equals(errormsg))
                PeriodMonthsTxtBx.Text = string.Empty;
        }

        private void PeriodMonthsTxtBx_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PeriodMonthsTxtBx.Text.Equals("") || PeriodMonthsTxtBx.Text.Equals(errormsg))
                PeriodMonthsTxtBx.Text = "Months";
        }

        private void InterestRateTxtBx_GotFocus(object sender, RoutedEventArgs e)
        {
            if (InterestRateTxtBx.Text.Equals("Annual Interest Rate") || InterestRateTxtBx.Text.Equals(errormsg))
                InterestRateTxtBx.Text = string.Empty;
        }

        private void InterestRateTxtBx_LostFocus(object sender, RoutedEventArgs e)
        {
            if (InterestRateTxtBx.Text.Equals("") || InterestRateTxtBx.Text.Equals(errormsg))
                InterestRateTxtBx.Text = "Annual Interest Rate";
        }

        private void DepositControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (enableCompound == true)
                CompoundListPicker.IsEnabled = false;
            if (enableCompound == false)
                CompoundListPicker.IsEnabled = true;
        }

      
    }
}
